// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Sbom.Contracts;
using Microsoft.Sbom.Extensions.Entities;

namespace Microsoft.Sbom.Api.Utils;

/// <summary>
/// Extension methods to convert SBOM format specificaitons from multiple formats.
/// </summary>
public static class SbomFormatExtensions
{
    /// <summary>
    /// Converts a <see cref="SbomSpecification"/> to a <see cref="ManifestInfo"/> object.
    /// </summary>
    /// <param name="specification"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static ManifestInfo ToManifestInfo(this SbomSpecification specification)
    {
        if (specification is null)
        {
            throw new ArgumentNullException(nameof(specification));
        }

        return new ManifestInfo
        {
            Name = specification.Name,
            Version = specification.Version
        };
    }
}
