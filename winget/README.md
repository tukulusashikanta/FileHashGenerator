# WinGet manifests

Package identifier: **SashikantaPanda.FileHashGenerator**  
Publisher / Author: **Sashikanta Panda**

Manifests are generated per release by:

```powershell
.\scripts\generate-winget-manifest.ps1 -Version 1.0.0
```

The GitHub Actions release workflow also emits a `winget-manifests-<version>` artifact containing ready-to-submit YAML files (with the correct installer URL and SHA256).

See [docs/WINGET.md](../docs/WINGET.md) for submission steps to `microsoft/winget-pkgs`.
