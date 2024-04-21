---
title: Event IDs
---

# Event IDs

## Intro
Tyne messages logged to an `ILogger` each have a unique [EventId](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.eventid).
Each Tyne library has a distinct range of `EventId`s.

## Format
Tyne's `EventId`s are formatted as `101_###_###`, where:
- `101` is the Tyne identifier
- the first `###` block is allocated to the project
- the second `###` block identifies the event

If searching the source code for `EventId` `101abcdef`, the `LoggerMessage` will be defined as `101_abc_def`.

## Ranges
| Package                                   | EventId Minimum | EventId Maximum |
| ----------------------------------------- | --------------- | --------------- |
| Tyne.Blazor                               | 101_001_000     | 101_001_999     |
| Tyne.HttpMediator.Client                  | 101_002_000     | 101_002_999     |
| Tyne.HttpMediator.Server                  | 101_003_000     | 101_003_999     |
| Tyne.HttpMediator.Client.FluentValidation | 101_004_000     | 101_004_999     |

Unlisted packages don't currently contain any logging.

## Unit tests
The [EventIdsInRangeTests](gitfile://test/EventIdTests/EventIdsInRangeTests.cs) are
used to ensure a project's `EventId`s stay within the project's allocated bounds.

The [EventIdsAreUnique](gitfile://test/EventIdTests/EventIdsAreUnique.cs) are
used to ensure a project's `EventId`s are unique within the project.
