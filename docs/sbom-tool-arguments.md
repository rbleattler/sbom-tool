# Sbom tool arguments

```powershell
dotnet run -p src/Microsoft.Sbom.Tool generate -- -h
```

```text
The Sbom tool generates a SBOM for any build artifact.

Usage - Microsoft.Sbom.Tool <action> -options

GlobalOption    Description
Help (-?, -h)   Prints this help message

Actions

  Validate -options - Validate a build artifact using the manifest. Optionally also verify the signing certificate of the manifest.

    Option                   Description
    BuildDropPath (-b)       Specifies the root folder of the drop directory containing the final build artifacts (binaries and executables) for which the SBOM file will be validated. This is the directory
                             where the completed build output is stored.
    ManifestDirPath (-m)     The path of the directory where the manifest will be validated. If this parameter is not specified, the manifest will be validated in {BuildDropPath}/_manifest directory.
    OutputPath (-o)          The path where the output json should be written. ex: Path/output.json
    CatalogFilePath (-C)     This parameter is deprecated and will not be used, we will automatically detect the catalog file using our standard directory structure. The path of signed catalog file that is used
                             to verify the signature of the manifest json file.
    ValidateSignature (-s)   If set, will validate the manifest using the signed catalog file.
    IgnoreMissing (-im)      If set, will not fail validation on the files presented in Manifest but missing on the disk.
    FailIfNoPackages (-n)    If set, validation will fail if there are no packages detected in the sbom.
    RootPathFilter (-r)      If you're downloading only a part of the drop using the '-r' or 'root' parameter in the drop client, specify the same string value here in order to skip validating paths that are
                             not downloaded.
    HashAlgorithm (-Ha)      The Hash algorithm to use while verifying or generating the hash value of a file
    Conformance (-cs)        The conformance to validate against
    Parallelism (-P)         The number of parallel threads to use for the workflows.
    FollowSymlinks (-F)      If set to false, we will not follow symlinks while traversing the build drop folder. Default is set to 'true'.
    ManifestInfo (-mi)       A list of the name and version of the manifest format that we are using.
    ConfigFilePath (-Co)     The json file that contains the configuration for the DropValidator.
    TelemetryFilePath (-t)   Specify a file where we should write detailed telemetry for the workflow.
    Verbosity (-V)           Display this amount of detail in the logging output.
                             Verbose
                             Debug
                             Information
                             Warning
                             Error
                             Fatal

  Generate -options - Generate a SBOM for all the files in the given build drop folder, and the packages in the components path.

    Option                                      Description
    BuildDropPath (-b)                          Specifies the root folder of the drop directory containing the final build artifacts (binaries and executables) for which the SBOM file will be generated. This is
                                                the directory where the completed build output is stored.
    BuildComponentPath (-bc)                    Specifies the folder containing the source code and components used to build the binary. This is where the tool will look for the individual components and
                                                packages that are part of the build process.
    BuildListFile (-bl)                         The file path to a file containing a list of files one file per line for which the SBOM file will be generated. Only files listed in the file will be included in
                                                the generated SBOM.
    ManifestDirPath (-m)                        The path of the directory where the generated SBOM files will be placed. A folder named '_manifest' will be created at this location, where all generated SBOMs
                                                will be placed. If this parameter is not specified, the files will be placed in {BuildDropPath}/_manifest directory.
    PackageName (-pn)                           The name of the package this SBOM represents. If this is not provided, we will try to infer this name from the build that generated this package, if that also
                                                fails, the SBOM generation fails.
    PackageVersion (-pv)                        The version of the package this SBOM represents. If this is not provided, we will try to infer the version from the build that generated this package, if that
                                                also fails, the SBOM generation fails.
    PackageSupplier (-ps)                       Supplier of the package that this SBOM represents.
    DockerImagesToScan (-di)                    Comma separated list of docker image names or hashes to be scanned for packages, ex: ubuntu:16.04, 56bab49eef2ef07505f6a1b0d5bd3a601dfc3c76ad4460f24c91d6fa298369ab.
    AdditionalComponentDetectorArgs (-cd)       Additional set of arguments for Component Detector.  An appropriate usage of this would be a space-delimited list of `--key value` pairs, representing
                                                command-line arguments. See the component-detection repository for a list of valid arguments (https://github.com/microsoft/component-detection/blob/main/docs/detector-arguments.md).
    ExternalDocumentReferenceListFile (-er)     The path to a file containing a list of external SBOMs that will be included as external document reference in the output SBOM. SPDX 2.2 is the only supported
                                                format for now.
    NamespaceUriUniquePart (-nsu)               A unique valid URI part that will be appended to the SPDX SBOM namespace URI. This value should be globally unique.
    NamespaceUriBase (-nsb)                     The base path of the SBOM namespace URI.
    GenerationTimestamp (-gt)                   A timestamp in the format 'yyyy-MM-ddTHH:mm:ssZ' that will be used as the generated timestamp for the SBOM.
    DeleteManifestDirIfPresent (-D)             If set to true, we will delete any previous manifest directories that are already present in the ManifestDirPath without asking the user for confirmation. The new
                                                manifest directory will then be created at this location and the generated SBOM will be stored there.
    FetchLicenseInformation (-li)               If set to true, we will attempt to fetch license information of packages detected in the SBOM from the ClearlyDefinedApi.
    LicenseInformationTimeoutInSeconds (-lto)   Specifies the timeout in seconds for fetching the license information. Defaults to 30 seconds. Has no effect if the FetchLicenseInformation (li) argument is false
                                                or not provided. A negative value correspondsto an infinite timeout.
    EnablePackageMetadataParsing (-pm)          If set to true, we will attempt to parse license and supplier info from the packages metadata file.
    Parallelism (-P)                            The number of parallel threads to use for the workflows.
    FollowSymlinks (-F)                         If set to false, we will not follow symlinks while traversing the build drop folder. Default is set to 'true'.
    ManifestInfo (-mi)                          A list of the name and version of the manifest format that we are using.
    ConfigFilePath (-C)                         The json file that contains the configuration for the DropValidator.
    TelemetryFilePath (-t)                      Specify a file where we should write detailed telemetry for the workflow.
    Verbosity (-V)                              Display this amount of detail in the logging output.
                                                Verbose
                                                Debug
                                                Information
                                                Warning
                                                Error
                                                Fatal

  Redact -options - Redact file information from given SBOM(s).

    Option            Description
    SbomPath (-sp)    The file path of the SBOM to redact.
    SbomDir (-sd)     The directory containing the sbom(s) to redact.
    OutputPath (-o)   Gets or sets the directory where the redacted SBOM file(s) will be generated.
    Verbosity (-V)    Display this amount of detail in the logging output.
                      Verbose
                      Debug
                      Information
                      Warning
                      Error
                      Fatal

  Aggregate -options - Aggregate multiple SBOMs into a single SBOM.

    Option                   Description
    ConfigFilePath (-C)      The json file that contains the configuration for the DropValidator.
    TelemetryFilePath (-t)   Specify a file where we should write detailed telemetry for the workflow.
    Verbosity (-V)           Display this amount of detail in the logging output.
                             Verbose
                             Debug
                             Information
                             Warning
                             Error
                             Fatal

  Version  - Displays the version of the tool being used. Can be used as '--version'
```

# Configuration Files

## Generation

The configuration file provided via the `--ConfigFilePath` argument should be a json file with all the relevant arguments. Note that environment variables can be included and will be expanding during the config file parsing. See the following example of config file content for the generate command:

```json
{
    "PackageSupplier": "PackageSupplier",
    "PackageName": "PackageName",
    "PackageVersion": "1.0.0",
    "BuildDropPath": "$(BUILD_DROP_PATH_ENV_VAR)",
    "BuildComponentPath": "$(BUILD_COMPONENT_PATH_ENV_VAR)"
}
```

## Aggregation

The configuration file provided via the `--ConfigFilePath` argument should be a json file with all the relevant arguments. Note that environment variables can be included and will be expanding during the config file parsing. See the following example of config file content for the aggregate command

Important: Aggregation currently only supports SPDX 2.2 SBOMs. SPDX 3.0 SBOMs will be added in the future.

```json
{
  "ArtifactInfoMap": {
    "C:\\Artifact1\\Bin\\Release\\net8.0": { },
    "C:\\Artifact2\\Bin": {
      "ExternalManifestDir": "C:\\Artifact2\\_manifest"
    }
  },
  "ManifestDirPath": "$(UserProfile)\\testing",
  "PackageName": "CombinedPackageName",
  "PackageVersion": "$(CombinedPackagedVersion)",
  "PackageSupplier": "$(CombinedPackageSupplier)"
}
```

Aggregation config parameters are as follows:

Parameter | Description
--- | ---
ArtifactInfoMap | Each key in the ArtifactInfoMap identifies where to locate the files artifactt to validate. By default, the SBOMs will be expected to be in the `./_manifest/spdx_2.2` relative path to the artifact directory. The `ExternalManifestDir` property can be used to specify a different location for the SBOMs (replacing just the `_manifest` part of the path). The `ManifestDirPath` property specifies where to place the aggregated SBOM. The `PackageName`, `PackageVersion`, and `PackageSupplier` properties are used to set the metadata for the aggregated SBOM.
ManifestDirPath | Where to locate the aggregated SBOM. The aggregated files will be placed in the `./spdx_2.2` relative path to this directory.
PackageName | The name of the package the aggregated SBOM represents.
PackageVersion | The version of the package the aggregated SBOM represents.
PackageSupplier | The supplier of the package the aggregated SBOM represents.

