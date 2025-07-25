// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Sbom.Common.Utils;
using Microsoft.Sbom.Contracts;
using Microsoft.Sbom.Contracts.Enums;
using Microsoft.Sbom.Extensions.Exceptions;
using Microsoft.Sbom.Parsers.Spdx22SbomParser.Entities;
using Microsoft.Sbom.Parsers.Spdx22SbomParser.Entities.Enums;

namespace Microsoft.Sbom.Parsers.Spdx22SbomParser.Utils;

/// <summary>
/// Provides extensions to SPDX objects.
/// </summary>
public static class SPDXExtensions
{
    /// <summary>
    /// Using a <see cref="PackageInfo"/> object, add package urls to the spdxPackage.
    /// </summary>
    /// <param name="spdxPackage">The object to add the external reference to.</param>
    /// <param name="packageInfo">The packageInfo object to use for source data.</param>
    public static void AddPackageUrls(this SPDXPackage spdxPackage, SbomPackage packageInfo)
    {
        if (spdxPackage is null)
        {
            throw new ArgumentNullException(nameof(spdxPackage));
        }

        if (packageInfo is null)
        {
            return;
        }

        // Add purl information if available.
        if (packageInfo.PackageUrl != null)
        {
            if (spdxPackage.ExternalReferences == null)
            {
                spdxPackage.ExternalReferences = new List<ExternalReference>();
            }

            // Create a new PURL external reference.
            var extRef = new ExternalReference
            {
                ReferenceCategory = ReferenceCategory.PACKAGE_MANAGER.ToNormalizedString(),
                Type = ExternalRepositoryType.purl.ToString(),
                Locator = packageInfo.PackageUrl,
            };

            spdxPackage.ExternalReferences.Add(extRef);
        }
    }

    /// <summary>
    /// Adds a SPDXID property to the given package. The id of the package should be the same
    /// for any build as long as the contents of the package haven't changed.
    /// </summary>
    /// <param name="spdxPackage"></param>
    /// <param name="packageInfo"></param>
    public static string AddSpdxId(this SPDXPackage spdxPackage, SbomPackage packageInfo)
    {
        if (spdxPackage is null)
        {
            throw new ArgumentNullException(nameof(spdxPackage));
        }

        spdxPackage.SpdxId = CommonSPDXUtils.GenerateSpdxPackageId(packageInfo);
        return spdxPackage.SpdxId;
    }

    /// <summary>
    /// Adds a SPDXID property to the given file. The id of the file should be the same
    /// for any build as long as the contents of the file haven't changed.
    /// </summary>
    /// <param name="spdxFile"></param>
    /// <param name="fileName"></param>
    /// <param name="checksums"></param>
    public static string AddSpdxId(this SPDXFile spdxFile, string fileName, IEnumerable<Contracts.Checksum> checksums)
    {
        if (spdxFile is null)
        {
            throw new ArgumentNullException(nameof(spdxFile));
        }

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException($"'{nameof(fileName)}' cannot be null or empty.", nameof(fileName));
        }

        if (checksums is null || !checksums.Any(c => c.Algorithm == AlgorithmName.SHA1))
        {
            throw new MissingHashValueException($"The file {fileName} is missing the {HashAlgorithmName.SHA1} hash value."); // CodeQL [SM02196] Sha1 is required per the SPDX spec.
        }

        // Get the SHA1 for this file.
        var sha1Value = checksums.Where(c => c.Algorithm == AlgorithmName.SHA1)
            .Select(s => s.ChecksumValue)
            .FirstOrDefault();

        spdxFile.SPDXId = CommonSPDXUtils.GenerateSpdxFileId(fileName, sha1Value);
        return spdxFile.SPDXId;
    }

    /// <summary>
    /// Adds externalReferenceId property to the SPDXExternalDocumentReference based on name and checksum information.
    /// </summary>
    public static string AddExternalReferenceSpdxId(this SpdxExternalDocumentReference reference, string name, IEnumerable<Contracts.Checksum> checksums)
    {
        if (reference is null)
        {
            throw new ArgumentNullException(nameof(reference));
        }

        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        if (checksums is null || !checksums.Any(c => c.Algorithm == AlgorithmName.SHA1))
        {
            throw new MissingHashValueException($"The external reference {name} is missing the {HashAlgorithmName.SHA1} hash value."); // CodeQL [SM02196] Sha1 is required per the SPDX spec.
        }

        // Get the SHA1 for this file.
        var sha1Value = checksums.Where(c => c.Algorithm == AlgorithmName.SHA1)
            .Select(s => s.ChecksumValue)
            .FirstOrDefault();

        reference.ExternalDocumentId = CommonSPDXUtils.GenerateSpdxExternalDocumentId(name, sha1Value);
        return reference.ExternalDocumentId;
    }

    /// <summary>
    /// Extension method to normalize the string value of <see cref="ReferenceCategory"/> to be compliant with SPDX 2.2 spec.
    /// </summary>
    /// <param name="referenceCategory"></param>
    /// <returns>A reference category value complaint with the SPDX 2.2 spec.</returns>
    public static string ToNormalizedString(this ReferenceCategory referenceCategory)
    {
        return referenceCategory.ToString().Replace('_', '-');
    }
}
