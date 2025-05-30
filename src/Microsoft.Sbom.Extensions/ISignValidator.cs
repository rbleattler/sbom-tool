// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.Sbom.Extensions;

/// <summary>
/// Validates the given manifest.json using the platform specific sign verification mechanism.
/// </summary>
public interface ISignValidator
{
    /// <summary>
    /// Gets the OS Platform that this validator supports, ex. Windows or Linux.
    /// </summary>
    public OSPlatform SupportedPlatform { get; }

    /// <summary>
    /// Validates the given manifest.json using the platform specific sign verification mechanism.
    /// </summary>
    /// <param name="additionalTelemetry">Property bag where the validation can add additional telemetry info</param>
    /// <returns>true if valid, false otherwise.</returns>
    public bool Validate(IDictionary<string, string> additionalTelemetry);
}
