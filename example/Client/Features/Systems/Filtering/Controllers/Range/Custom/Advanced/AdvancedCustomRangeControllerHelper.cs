using System.Globalization;

namespace Tyne.Aerospace.Client.Features.Systems.Filtering.Controllers.Range.Custom.Advanced;

internal static class AdvancedCustomRangeControllerHelper
{
    public static string GetOrbitalPeriod(int minMeters, int maxMeters)
    {
        // Orbital period T (seconds) can be found with
        //      T = 2pi * sqrt(a^3 / GM)
        // Where a is the semi-major axis
        //      a = (radiusMax + radiusMin) / 2
        // Where radius is that of the elliptical orbit from the centre of the earth

        var earthRadiusMeters = 6_378_000;
        var earthMassKg = 5.9722 * Math.Pow(10, 24);
        var gravitationalConstant = 6.6743 * Math.Pow(10, -11);

        var minFromCenterMeters = earthRadiusMeters + minMeters;
        var maxFromCenterMeters = earthRadiusMeters + maxMeters;

        var semiMajorAxis = (maxFromCenterMeters + minFromCenterMeters) / 2;

        var sqrt = Math.Pow(semiMajorAxis, 3) / (gravitationalConstant * earthMassKg);

        var timeSeconds = 2 * Math.PI * Math.Sqrt(sqrt);

        var orbitalPeriod = TimeSpan.FromSeconds(timeSeconds);

        var format =
            orbitalPeriod.TotalDays >= 1
            ? "d'.'hh':'mm':'ss"
            : "hh':'mm':'ss";

        return orbitalPeriod.ToString(format, CultureInfo.InvariantCulture);
    }
}
