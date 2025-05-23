// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Sbom.Api.Config.Extensions;
using Microsoft.Sbom.Api.Utils;
using Microsoft.Sbom.Common.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Sbom.Api.Tests.Config;

[TestClass]
public class ConfigurationCLITests
{
    private Mock<IConfiguration> mockConfiguration;

    [TestInitialize]
    public void Setup()
    {
        mockConfiguration = new Mock<IConfiguration>();
    }

    [TestMethod]
    public void Configuration_CommandLineParams()
    {
        // A property that is not a ComponentDetectorArgument
        mockConfiguration.SetupProperty(c => c.BuildComponentPath, new ConfigurationSetting<string> { Value = "build_component_path" });

        // A named ComponentDetectorArgument
        mockConfiguration.SetupProperty(c => c.DockerImagesToScan, new ConfigurationSetting<string> { Value = "the_docker_image" });

        // An unnamed ComponentDetectorArgument
        mockConfiguration.SetupProperty(c => c.AdditionalComponentDetectorArgs, new ConfigurationSetting<string> { Value = "--arg1 val1 --arg2 val2" });

        var config = mockConfiguration.Object;

        var argBuilder = new ComponentDetectionCliArgumentBuilder()
            .SourceDirectory("X:/")
            .AddArg("defaultArg1", "val1")
            .AddArg("defaultArg2", "val2");

        var commandLineParams = config.ToComponentDetectorCommandLineParams(argBuilder);

        Assert.AreEqual("--SourceDirectory X:/ --DetectorArgs Timeout=900 --defaultArg1 val1 --defaultArg2 val2 --DockerImagesToScan the_docker_image --arg1 val1 --arg2 val2", string.Join(' ', commandLineParams));
    }

    [TestMethod]
    public void Configuration_CommandLineParams_DefaultArgsOnly()
    {
        var config = mockConfiguration.Object;

        var argBuilder = new ComponentDetectionCliArgumentBuilder()
            .SourceDirectory("X:/")
            .AddArg("defaultArg1", "val1")
            .AddArg("defaultArg2", "val2");

        var commandLineParams = config.ToComponentDetectorCommandLineParams(argBuilder);

        Assert.AreEqual("--SourceDirectory X:/ --DetectorArgs Timeout=900 --defaultArg1 val1 --defaultArg2 val2", string.Join(' ', commandLineParams));
    }
}
