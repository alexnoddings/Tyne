---
title: Filtering intro
---

# Filtering intro
Tyne.Blazor provides a set of components for rich, interactive data filtering.

> [!NOTE]
> The demo app contains [companion examples](/Tyne/demo/examples/filtering/intro).

## Overview
Filtering components come in one of two flavours:
[value](xref:Tyne.Blazor.Filtering.Values.IFilterValue`1) or
[controller](xref:Tyne.Blazor.Filtering.Controllers.IFilterController`1),
which operate in an [`IFilterContext<TRequest>`](xref:Tyne.Blazor.Filtering.Context.IFilterContext`1).
Filter values store a single value associated with a single property on `TRequest`,
while controllers allow users to interct with filter values.

### Filter context
The filter context orchestrates filtering, facilitating communication between values and controllers.
When a `TRequest` is being made, the context is responsible for delegating request configuration to attached values.

#### Attaching handles
Filtering involves a lot of bi-directional communication, for example:
- A controller needs to be able to update a value
- A value needs to notify controllers of changes to the value
- The context needs to delegate request configuration to values

Because of this complexity, values and controllers do not interact directly with the context.
Instead, they attach themselves to one or more properties on the context using a [`TyneKey`s](xref:Tyne.Blazor.TyneKey).
[Attaching a value](xref:Tyne.Blazor.Filtering.Context.IFilterContext`1.AttachValue*)
will return an
[`IFilterValueHandle`](xref:Tyne.Blazor.Filtering.Values.IFilterValueHandle`1),
and [attaching a controller](xref:Tyne.Blazor.Filtering.Context.IFilterContext`1.AttachController*)
will return an
[`IFilterControllerHandle`](xref:Tyne.Blazor.Filtering.Controllers.IFilterControllerHandle`1).
These handles are used to communicate with the context (such as reloading data or updating a value),
and once disposed of will detach the component from the context.
Handles *must be disposed of once out of scope*. This ensures filtering resources are cleaned up properly.

Values must be attached to a filter context before it is initialised.
Controllers must be attached after their respective value, otherwise they have nothing to attach to.
While they _can_ be attached after context initialisation, it is good practice to ensure they are attached as early as possible.

#### Built-in filter context
Tyne provides an `IFilterContext` implementation through [`TyneFilterContext<TRequest>`](xref:Tyne.Blazor.Filtering.Context.TyneFilterContext`1).
This implementation is used extensively by Tyne's own systems (e.g. the Table), and handles the filtering lifecycle for you.

### Filter values
Filter values store state, such as a string which a name property is being filtered by.
They operate as the backend of the filtering system - they do not render any UI, nor do users interact with them directly.
Instead, it is responsible soley for storing state and configuring requests.
Users interact with controllers, which in turn communicate with values.

Each property on `TRequest` can have zero or one values attached. See [below](#separation-of-value-and-controller) for why only one.

### Filter controllers
Filter controllers are how users interact with filter values.
They should not store any state; instead, they act as outlets for users to interact with filter values.

Any number of controllers may be attached to the same value.
One controller may also register to more than one value, such as a controller which manages a min/max date range.
This is done by attaching itself multiple times, with a handle for each different value.

### Separation of value and controller
Values and controllers are strictly separated. This single-responsibility helps keep implementations of both relatively simple.
Values are only concerned with state management, and don't care what happens with the UI.
Controllers are only concerned with showing the value, not what else happens with it.
Rolling both into one implementation is very simple for very simple scenarios, but quickly becomes a mess for anything more complicated.

For example, you may have a table with a fine-grained 'created at' control, and also an easy 'created in last 7 days' button.
Currently, this would be implemented with one value and two controllers. Combining both introduces a mess where both components fight over which is the correct source of truth.

Additionally, range controllers currently interact with two values.
Combining values and controllers would mean a component needs to re-implement the wheel as it manages state for two values, not just one.

## See also
- The [diagram page](./diagrams.md) contains technical diagrams about the lifecycle of filtering
- The [components page](./components.md) contains more details on the component implementations provided
- The [persistence page](./persistence.md) contains more details on value persistence
