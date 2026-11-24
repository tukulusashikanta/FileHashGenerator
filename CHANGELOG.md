# Changelog

All notable changes to **File Hash Generator** are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.0] - 2026-11-24

### Added
- Drag-and-drop file selection.
- One-click **Copy** for hash and Base64 results.
- Clear action to reset the workspace.
- Status bar with live operation feedback.
- File name and size summary in the selection panel.

### Changed
- Redesigned the desktop UI with a clearer layout, algorithm chips, and monospace result fields.
- Hash and Base64 work now run asynchronously so the window stays responsive.

## [1.0.0] - 2026-11-12

### Added
- Initial public release of File Hash Generator by **Sashikanta Panda**.
- Browse and select any local file for hashing.
- Calculate file hashes using **SHA256**, **MD5**, or **SHA1**.
- Convert selected file contents to **Base64**.
- Windows installer built with **Inno Setup**.
- GitHub Actions workflow for automated build, installer packaging, release publishing, and SHA256 checksum generation.
- WinGet package manifest templates for publishing to the Windows Package Manager Community Repository.

[1.1.0]: https://github.com/tukulusashikanta/FileHashGenerator/releases/tag/v1.1.0
[1.0.0]: https://github.com/tukulusashikanta/FileHashGenerator/releases/tag/v1.0.0
