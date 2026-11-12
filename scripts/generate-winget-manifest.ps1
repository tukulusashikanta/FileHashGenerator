<#
.SYNOPSIS
  Generate WinGet multi-file manifests for File Hash Generator.

.PARAMETER Version
  Package version (e.g. 1.0.0)

.PARAMETER Sha256
  SHA256 of the installer. If omitted, downloads the GitHub Release asset and hashes it.

.PARAMETER InstallerUrl
  Override installer URL. Defaults to the GitHub Releases asset URL.

.PARAMETER OutputRoot
  Directory that will contain <PackageId>/<Version>/*.yaml
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$Version,

    [string]$Sha256,

    [string]$InstallerUrl,

    [string]$OutputRoot = (Join-Path $PSScriptRoot "..\winget\manifests")
)

$ErrorActionPreference = "Stop"

$PackageId = "SashikantaPanda.FileHashGenerator"
$Publisher = "Sashikanta Panda"
$PackageName = "File Hash Generator"
$RepoUrl = "https://github.com/tukulusashikanta/FileHashGenerator"
$License = "MIT"
$ProductCode = "{B8E4A12F-6C3D-4F91-9A2E-7D5C1B0E8F34}_is1"
$ManifestVersion = "1.6.0"

$Version = $Version.TrimStart("v")
if (-not $InstallerUrl) {
    $InstallerUrl = "$RepoUrl/releases/download/v$Version/FileHashGenerator-Setup-$Version.exe"
}

if (-not $Sha256) {
    Write-Host "Downloading installer to compute SHA256..."
    $temp = Join-Path ([System.IO.Path]::GetTempPath()) ("FileHashGenerator-Setup-$Version.exe")
    Invoke-WebRequest -Uri $InstallerUrl -OutFile $temp
    $Sha256 = (Get-FileHash -Algorithm SHA256 -Path $temp).Hash.ToUpperInvariant()
    Remove-Item $temp -Force
}

$Sha256 = $Sha256.ToUpperInvariant()
$outDir = Join-Path $OutputRoot "$PackageId\$Version"
New-Item -ItemType Directory -Force -Path $outDir | Out-Null

$locale = @"
# Created with scripts/generate-winget-manifest.ps1
# yaml-language-server: `$schema=https://aka.ms/winget-manifest.defaultLocale.1.6.0.schema.json

PackageIdentifier: $PackageId
PackageVersion: $Version
PackageLocale: en-US
Publisher: $Publisher
PublisherUrl: $RepoUrl
PublisherSupportUrl: $RepoUrl/issues
Author: $Publisher
PackageName: $PackageName
PackageUrl: $RepoUrl
License: $License
LicenseUrl: $RepoUrl/blob/main/LICENSE
Copyright: Copyright (c) 2026 Sashikanta Panda
ShortDescription: Calculate SHA256, MD5, and SHA1 hashes for files and encode file contents as Base64.
Description: |-
  File Hash Generator is a lightweight Windows desktop utility by Sashikanta Panda.
  Browse to any local file, compute SHA256, MD5, or SHA1 hashes, and convert file contents to Base64.
Moniker: filehashgenerator
Tags:
- hash
- sha256
- md5
- sha1
- checksum
- base64
- cryptography
- utility
ReleaseNotesUrl: $RepoUrl/releases/tag/v$Version
ManifestType: defaultLocale
ManifestVersion: $ManifestVersion
"@

$installer = @"
# Created with scripts/generate-winget-manifest.ps1
# yaml-language-server: `$schema=https://aka.ms/winget-manifest.installer.1.6.0.schema.json

PackageIdentifier: $PackageId
PackageVersion: $Version
Platform:
- Windows.Desktop
MinimumOSVersion: 10.0.0.0
InstallerType: inno
Scope: machine
InstallModes:
- interactive
- silent
- silentWithProgress
UpgradeBehavior: install
ReleaseDate: $(Get-Date -Format "yyyy-MM-dd")
Installers:
- Architecture: x86
  InstallerUrl: $InstallerUrl
  InstallerSha256: $Sha256
  ProductCode: '$ProductCode'
  AppsAndFeaturesEntries:
  - DisplayName: $PackageName
    Publisher: $Publisher
    ProductCode: '$ProductCode'
ManifestType: installer
ManifestVersion: $ManifestVersion
"@

$versionManifest = @"
# Created with scripts/generate-winget-manifest.ps1
# yaml-language-server: `$schema=https://aka.ms/winget-manifest.version.1.6.0.schema.json

PackageIdentifier: $PackageId
PackageVersion: $Version
DefaultLocale: en-US
ManifestType: version
ManifestVersion: $ManifestVersion
"@

$localePath = Join-Path $outDir "$PackageId.locale.en-US.yaml"
$installerPath = Join-Path $outDir "$PackageId.installer.yaml"
$versionPath = Join-Path $outDir "$PackageId.yaml"

Set-Content -Path $localePath -Value $locale.TrimEnd() -Encoding utf8
Set-Content -Path $installerPath -Value $installer.TrimEnd() -Encoding utf8
Set-Content -Path $versionPath -Value $versionManifest.TrimEnd() -Encoding utf8

Write-Host "Generated WinGet manifests in: $outDir"
Write-Host "InstallerSha256: $Sha256"
Write-Host "InstallerUrl: $InstallerUrl"
Write-Host ""
Write-Host "Validate with:"
Write-Host "  winget validate --manifest `"$outDir`""
