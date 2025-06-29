// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Sbom.Contracts;
using Microsoft.Sbom.Extensions.Entities;

namespace Microsoft.Sbom.Extensions;

/// <summary>
/// Recorder and validator for items discovered during SBOM creation.
/// </summary>
public interface ISbomPackageDetailsRecorder
{
    /// <summary>
    /// Record a fileId that is included in this SBOM.
    /// </summary>
    /// <param name="fileId"></param>
    public void RecordFileId(string fileId);

    /// <summary>
    /// Record a fileId for SPDX files that are referenced in the SBOM.
    /// </summary>
    public void RecordSPDXFileId(string spdxFileId);

    /// <summary>
    /// Record a packageId and dependon package that is included in this SBOM.
    /// </summary>
    /// <param name="packageId"></param>
    /// <param name="dependOn"></param>
    public void RecordPackageId(string packageId, string dependOn);

    /// <summary>
    /// Record a externalDocumentReference Id that is included in this SBOM.
    /// </summary>
    public void RecordExternalDocumentReferenceIdAndRootElement(string externalDocumentReferenceId, string rootElement);

    /// <summary>
    /// Gets SBOM generation data.
    /// </summary>
    public GenerationData GetGenerationData();

    /// <summary>
    /// Record the SHA1 hash for the file.
    /// </summary>
    public void RecordChecksumForFile(Checksum[] checksums);

    /// <summary>
    /// Record ID of the root package.
    /// </summary>
    public void RecordRootPackageId(string rootPackageId);

    /// <summary>
    /// Record Document ID.
    /// </summary>
    public void RecordDocumentId(string documentId);
}
