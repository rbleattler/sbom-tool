// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Sbom.Api;
using Microsoft.Sbom.Api.Config;
using Microsoft.Sbom.Api.Config.Args;
using Microsoft.Sbom.Api.Config.Extensions;
using Microsoft.Sbom.Api.Exceptions;
using Microsoft.Sbom.Extensions.DependencyInjection;
using PowerArgs;

namespace Microsoft.Sbom.Tool;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var result = await Args.InvokeActionAsync<SbomToolCmdRunner>(args);
        if (result.HandledException != null || (result.ActionArgs is not CommonArgs))
        {
            Environment.ExitCode = (int)ExitCode.GeneralError;
            return;
        }

        try
        {
            await Host.CreateDefaultBuilder(args)
                .ConfigureServices((host, services) =>
                {
                    services = result.ActionArgs switch
                    {
                        ValidationArgs => services.AddHostedService<ValidationService>(),
                        GenerationArgs => services.AddHostedService<GenerationService>(),
                        AggregationArgs => services.AddHostedService<AggregationService>(),
                        RedactArgs => services.AddHostedService<RedactService>(),
                        FormatValidationArgs => services.AddHostedService<FormatValidationService>(),
                        _ => services
                    };

                    services
                        .AddTransient<ConfigFileParser>()
                        .AddSingleton(typeof(IConfigurationBuilder<>), typeof(ConfigurationBuilder<>))
                        .AddSingleton(x =>
                        {
                            var validationConfigurationBuilder = x.GetService<IConfigurationBuilder<ValidationArgs>>();
                            var generationConfigurationBuilder = x.GetService<IConfigurationBuilder<GenerationArgs>>();
                            var aggregationConfigurationBuilder = x.GetService<IConfigurationBuilder<AggregationArgs>>();
                            var redactConfigurationBuilder = x.GetService<IConfigurationBuilder<RedactArgs>>();
                            var formatValidationConfigurationBuilder = x.GetService<IConfigurationBuilder<FormatValidationArgs>>();
                            var inputConfiguration = result.ActionArgs switch
                            {
                                ValidationArgs v => validationConfigurationBuilder.GetConfiguration(v).GetAwaiter().GetResult(),
                                GenerationArgs g => generationConfigurationBuilder.GetConfiguration(g).GetAwaiter().GetResult(),
                                AggregationArgs c => aggregationConfigurationBuilder.GetConfiguration(c).GetAwaiter().GetResult(),
                                RedactArgs r => redactConfigurationBuilder.GetConfiguration(r).GetAwaiter().GetResult(),
                                FormatValidationArgs f => formatValidationConfigurationBuilder.GetConfiguration(f).GetAwaiter().GetResult(),
                                _ => default
                            };

                            inputConfiguration.ToConfiguration();
                            return inputConfiguration;
                        })

                        .AddSbomTool();
                })
                .RunConsoleAsync(x => x.SuppressStatusMessages = true);
            Environment.ExitCode = (int)ExitCode.Success;
        }
        catch (AccessDeniedValidationArgException e)
        {
            Console.WriteLine(e.Message);
            Environment.ExitCode = (int)ExitCode.WriteAccessError;
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            Environment.ExitCode = (int)ExitCode.GeneralError;
        }
    }
}
