# Publishing File Hash Generator to WinGet

This guide explains how to submit or update **SashikantaPanda.FileHashGenerator** in the [Windows Package Manager Community Repository](https://github.com/microsoft/winget-pkgs).

**Author / Publisher:** Sashikanta Panda

## Package identity

| Field | Value |
| --- | --- |
| PackageIdentifier | `SashikantaPanda.FileHashGenerator` |
| PackageName | File Hash Generator |
| Publisher | Sashikanta Panda |
| Installer type | Inno Setup (`inno`) |
| Installer URL | `https://github.com/tukulusashikanta/FileHashGenerator/releases/download/v{VERSION}/FileHashGenerator-Setup-{VERSION}.exe` |
| ProductCode | `{B8E4A12F-6C3D-4F91-9A2E-7D5C1B0E8F34}_is1` |

## Prerequisites

1. A public GitHub Release with the installer `.exe` asset (created by the release workflow).
2. The installer SHA256 checksum (uploaded as `FileHashGenerator-Setup-{VERSION}.exe.sha256` and shown in the workflow summary).
3. [WinGet Create](https://github.com/microsoft/winget-create) (`wingetcreate`) installed, or a fork of `microsoft/winget-pkgs`.

```powershell
winget install Microsoft.WingetCreate
```

## First-time submission

After `v1.0.0` (or your first public release) is published:

```powershell
.\scripts\generate-winget-manifest.ps1 -Version 1.0.0
```

Or with WinGet Create:

```powershell
wingetcreate new https://github.com/tukulusashikanta/FileHashGenerator/releases/download/v1.0.0/FileHashGenerator-Setup-1.0.0.exe
```

Fill in:

- **Publisher:** Sashikanta Panda  
- **Package identifier:** `SashikantaPanda.FileHashGenerator`  
- **License:** MIT  
- **InstallerType:** `inno`  
- **PackageUrl / PublisherUrl:** repository URL above  

Then open a pull request against `microsoft/winget-pkgs` under:

`manifests/s/SashikantaPanda/FileHashGenerator/<version>/`

Validate locally before submitting:

```powershell
winget validate --manifest .\winget\manifests\SashikantaPanda.FileHashGenerator\1.0.0
winget install --manifest .\winget\manifests\SashikantaPanda.FileHashGenerator\1.0.0
```

## Updating an existing version

After each new tagged release:

```powershell
wingetcreate update SashikantaPanda.FileHashGenerator `
  --urls https://github.com/tukulusashikanta/FileHashGenerator/releases/download/v1.0.1/FileHashGenerator-Setup-1.0.1.exe `
  --version 1.0.1 `
  --submit
```

`--submit` opens the PR automatically when authenticated (`wingetcreate token` / GitHub login).

You can also regenerate YAML files with:

```powershell
.\scripts\generate-winget-manifest.ps1 -Version 1.0.1
```

The release workflow attaches generated manifests as the `winget-manifests` artifact for convenience.

## Manifest guidelines checklist

Follow Microsoft’s [manifest](https://learn.microsoft.com/en-us/windows/package-manager/package/manifest) and [submission](https://github.com/microsoft/winget-pkgs/blob/master/doc/README.md) docs:

- [ ] Installer URL is a direct HTTPS download (GitHub Releases asset)
- [ ] `InstallerSha256` matches the released `.exe`
- [ ] `InstallerType` is `inno` (silent switches are implied)
- [ ] `PackageIdentifier` / version match the folder path
- [ ] Publisher and author metadata credit **Sashikanta Panda**
- [ ] `winget validate` and a local test install succeed
- [ ] PR targets `microsoft/winget-pkgs` with the required checklist completed

## After approval

Users can install with:

```powershell
winget install SashikantaPanda.FileHashGenerator
```
