---
title: Package upgrades
---

# Package upgrades

Common packages have their versions managed by [Directory.Build.targets](gitfile://Directory.Build.targets).
This is the preferred mechanism for specifying package versions as it ensures consistency across the solution.
To upgrade a common package defined in the targets file, update the `Version="x.y.z"` attribute.
