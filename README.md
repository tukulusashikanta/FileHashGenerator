# File Hash Generator

A lightweight Windows desktop utility that calculates cryptographic hashes (SHA256, MD5, SHA1) for any file and can encode file contents as Base64.

**Author:** Sashikanta Panda  
**Publisher:** Sashikanta Panda  
**License:** [MIT](LICENSE)

## Features

- Select a file via browse dialog or drag-and-drop
- Compute **SHA256**, **MD5**, or **SHA1** hashes
- Convert the selected file to a **Base64** string
- Copy hash / Base64 results in one click
- Responsive WPF UI for Windows 10/11

## Requirements

- Windows 10 or later
- [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472) (usually already installed on modern Windows)

## Install

### WinGet (recommended)

```powershell
winget install SashikantaPanda.FileHashGenerator
```

### GitHub Releases

Download the latest installer from the [Releases](https://github.com/tukulusashikanta/FileHashGenerator/releases) page and run:

`FileHashGenerator-Setup-<version>.exe`

## Build from source

### Prerequisites

- Visual Studio 2022 or later with **.NET desktop development** workload
- [.NET Framework 4.7.2 Developer Pack](https://dotnet.microsoft.com/download/dotnet-framework/net472)
- Optional: [Inno Setup 6](https://jrsoftware.org/isinfo.php) to build the installer locally

### Compile

```powershell
msbuild FileHashgenerator\FileHashgenerator.csproj /p:Configuration=Release /p:Platform=AnyCPU /restore
```

Output: `FileHashgenerator\bin\Release\FileHashgenerator.exe`

### Build installer (local)

```powershell
& "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" installer\FileHashGenerator.iss /DMyAppVersion=1.0.0
```

Installer output: `installer\output\FileHashGenerator-Setup-1.0.0.exe`

## Release process

Releases are automated with GitHub Actions.

1. Update `CHANGELOG.md` with notes for the new version.
2. Update the version in `FileHashgenerator\Properties\AssemblyInfo.cs` if needed.
3. Commit and push to `main`.
4. Create and push a version tag:

```powershell
git tag v1.0.1
git push origin v1.0.1
```

The [release workflow](.github/workflows/release.yml) will:

- Build the Release binary
- Compile the Inno Setup installer
- Compute the SHA256 checksum
- Create a GitHub Release with the installer, checksum file, and release notes
- Generate WinGet manifest files as workflow artifacts

See [docs/WINGET.md](docs/WINGET.md) for submitting or updating the package in the official WinGet repository.

## Project structure

```
FileHashgenerator/          # WPF application source
installer/                  # Inno Setup script
.github/workflows/          # CI/CD (build + release)
winget/                     # WinGet manifest templates
docs/                       # Packaging and WinGet guides
scripts/                    # Helper scripts for manifests / checksums
```

## Author

**Sashikanta Panda**  
GitHub: [@tukulusashikanta](https://github.com/tukulusashikanta)

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.
