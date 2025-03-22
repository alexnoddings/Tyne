using System.Diagnostics.CodeAnalysis;

namespace Tyne.Example.Client.Common.Data;

public static class Satellites
{
    [SuppressMessage("Design", "CA1002: Do not expose generic lists.", Justification = "Mock data-source used for the app.")]
    public static readonly List<Satellite> Data =
    [
        new()
        {
            Name = "Sputnik 1",
            PerigeeKm = 215,
            ApogeeKm = 939,
            IsAlive = false
        },
        new()
        {
            Name = "Hubble",
            PerigeeKm = 537,
            ApogeeKm = 541,
            IsAlive = true
        },
        new()
        {
            Name = "Chandra",
            PerigeeKm = 14_308,
            ApogeeKm = 134_528,
            IsAlive = true
        },
        new()
        {
            Name = "Fermi",
            PerigeeKm = 526,
            ApogeeKm = 544,
            IsAlive = true
        },
        new()
        {
            Name = "JWST",
            PerigeeKm = 250_000,
            ApogeeKm = 832_000,
            IsAlive = true
        },
    ];
}
