using System.Diagnostics.CodeAnalysis;

// This assembly is just testing utils, it shouldn't be included in final code coverage stats.
// (eg there are some argument-null branches which we, by design, shouldn't hit in this library.
[assembly: ExcludeFromCodeCoverage(Justification = "Assembly is testing utilities, not shipped code.")]
