---
title: Releasing a version
---

# Developing a new version
- Ensure changes have accompanying in-line XML docs
- Consider adding a docs page for the changes
- Consider adding tests for the changes

# Preparaing to release a new version
- Update the version info in [Version.props](gitfile://eng/Version.props)
    - Increment `TyneMajorVersion`/`TyneMinorVersion`/`TynePatchVersion`/`TynePreReleaseTag` as appropriate
    - `TynePreReleaseTag` should be set to `rc.N` (e.g. `rc.1`) for RC versions, or not set for full release versions
- Ensure you have a changes doc under [/docs/docs/changes/](gitfile://docs/docs/changes)
    - You should have a page per release version, e.g. `v3.2.0.md` includes changes from `v3.2.0-rc.1`, `v3.2.0-rc.2`, and `v3.2.0`)
    - If the version is not a full release, include a warning at the top of the doc page:\
    \> [!WARNING]\
    \> This version is still in active development.
    - Ensure the docs has a links footer which is up-to-date:\
    \## Links\
    \- \[GitHub release\]\(https://github.com/alexnoddings/Tyne/releases/tag/vX.Y.Z)\
    \- \[GitHub milestone\]\(https://github.com/alexnoddings/Tyne/milestone/MM?closed=1)\
    \- \[GitHub change log\]\(https://github.com/alexnoddings/Tyne/compare/vX.Y.Z...vX.W.Z)
- For a full-release, ensure the version's milestone is completed
- Ensure your changes are merged into `main`

# Releasing a new version
- On GitHub, draft a [new release](https://github.com/alexnoddings/Tyne/releases/new)
- Create a new tag based on the version (e.g. `v3.2.0`)
    - Note that this tag must identically match the assembly version, otherwise the publish pipeline will fail
- Include the changes in this version
    - See a previous versions release for what the description should look like
- Mark the release as a pre-release if it is an RC
- Click 'Publish release'
    - This will kick off the publishing pipeline
