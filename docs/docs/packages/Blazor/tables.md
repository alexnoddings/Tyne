---
title: Tables
---

# Tables
Tyne's tables are based on Tyne's [filtering context](./filtering/intro.md).

## Column headers controllers
Tyne provides the following controllers designed for use in column headers.
These usually inherit from a regular filter component and wrap it in a [TyneColumnPopoverHeader](xref:Tyne.Blazor.Tables.Columns.TyneColumnPopoverHeader`1).

| Controller | Usage | Inherits from |
| ---------- | ----- | ------------- |
| [`TyneStringColumnHeader`](xref:Tyne.Blazor.Tables.Columns.TyneStringColumnHeader`2) | Provides a simple text input box for use with string values | [`TyneStringFilterController`](xref:Tyne.Blazor.Filtering.Controllers.TyneStringFilterController`1) |
| [`TyneDateRangeColumnHeader`](xref:Tyne.Blazor.Tables.Columns.TyneDateRangeColumnHeader`2) | Provides a date range picker for min/max `DateTime`s | [`TyneDateRangeFilterController`](xref:Tyne.Blazor.Filtering.Controllers.TyneDateRangeFilterController`1) |
| [`TyneSingleSelectBoxColumnHeader`](xref:Tyne.Blazor.Tables.Columns.TyneSingleSelectBoxColumnHeader`3) | Provides a dropdown selection box for single select values | [`TyneSingleSelectBoxFilterController`](xref:Tyne.Blazor.Filtering.Controllers.TyneSingleSelectBoxFilterController`2) |
| [`TyneSingleSelectRadioColumnHeader`](xref:Tyne.Blazor.Tables.Columns.TyneSingleSelectRadioColumnHeader`3) | Provides radio buttons for single select values | [`TyneSingleSelectRadioFilterController`](xref:Tyne.Blazor.Filtering.Controllers.TyneSingleSelectRadioFilterController`2) |
| [`TyneMultiSelectBoxColumnHeader`](xref:Tyne.Blazor.Tables.Columns.TyneMultiSelectBoxColumnHeader`3) | Provides a dropdown selection box for multi select values | [`TyneMultiSelectBoxFilterController`](xref:Tyne.Blazor.Filtering.Controllers.TyneMultiSelectBoxFilterController`2) |
| [`TyneMultiSelectCheckboxColumnHeader`](xref:Tyne.Blazor.Tables.Columns.TyneMultiSelectCheckboxColumnHeader`3) | Provides checkboxes for multi select values | [`TyneMultiSelectCheckboxFilterController`](xref:Tyne.Blazor.Filtering.Controllers.TyneMultiSelectCheckboxFilterController`2) |

See the [filtering page](./filtering/intro.md) for more information about controllers provided for use in filtering contexts.
