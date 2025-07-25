// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Sbom.Api.Entities;
using Microsoft.Sbom.Api.Manifest;
using Microsoft.Sbom.Contracts;
using Microsoft.Sbom.Extensions;
using Serilog;

namespace Microsoft.Sbom.Api.Executors;

/// <summary>
/// Uses the <see cref="IManifestGenerator"/> to write a json object that contains
/// a format specific representation of the <see cref="PackageInfo"/>.
/// </summary>
public class PackageInfoJsonWriter
{
    private readonly IManifestGeneratorProvider manifestGeneratorProvider;
    private readonly ILogger log;

    public PackageInfoJsonWriter(
        IManifestGeneratorProvider manifestGeneratorProvider,
        ILogger log)
    {
        if (manifestGeneratorProvider is null)
        {
            throw new ArgumentNullException(nameof(manifestGeneratorProvider));
        }

        this.manifestGeneratorProvider = manifestGeneratorProvider;
        this.log = log ?? throw new ArgumentNullException(nameof(log));
    }

    public (ChannelReader<JsonDocWithSerializer> result, ChannelReader<FileValidationResult> errors) Write(ChannelReader<SbomPackage> packageInfos, IList<ISbomConfig> packagesArraySupportingConfigs)
    {
        var errors = Channel.CreateUnbounded<FileValidationResult>();
        var result = Channel.CreateUnbounded<JsonDocWithSerializer>();

        Task.Run(async () =>
        {
            await foreach (var packageInfo in packageInfos.ReadAllAsync())
            {
                await GenerateJson(packagesArraySupportingConfigs, packageInfo, result, errors);
            }

            errors.Writer.Complete();
            result.Writer.Complete();
        });

        return (result, errors);
    }

    internal async Task GenerateJson(
        IList<ISbomConfig> packagesArraySupportingConfigs,
        SbomPackage packageInfo,
        Channel<JsonDocWithSerializer> result,
        Channel<FileValidationResult> errors)
    {
        try
        {
            foreach (var sbomConfig in packagesArraySupportingConfigs)
            {
                var generationResult =
                    manifestGeneratorProvider.Get(sbomConfig.ManifestInfo).GenerateJsonDocument(packageInfo);

                var recordedAnyDependencies = false;

                if (generationResult?.ResultMetadata?.DependOn != null)
                {
                    foreach (var dependency in generationResult?.ResultMetadata?.DependOn)
                    {
                        sbomConfig.Recorder.RecordPackageId(generationResult?.ResultMetadata?.EntityId, dependency);
                        recordedAnyDependencies = true;
                    }
                }

                if (!recordedAnyDependencies)
                {
                    sbomConfig.Recorder.RecordPackageId(generationResult?.ResultMetadata?.EntityId, null);
                }

                await result.Writer.WriteAsync((generationResult?.Document, sbomConfig.JsonSerializer));
            }
        }
        catch (Exception e)
        {
            log.Warning($"Encountered an error while generating json for packageInfo {packageInfo}: {e.Message}");
            await errors.Writer.WriteAsync(new FileValidationResult
            {
                ErrorType = ErrorType.JsonSerializationError,
                Path = packageInfo.PackageName
            });
        }
    }
}
