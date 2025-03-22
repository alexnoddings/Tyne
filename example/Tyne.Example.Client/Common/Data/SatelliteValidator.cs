using FluentValidation;

namespace Tyne.Example.Client.Common.Data;

public class SatelliteValidator : AbstractValidator<Satellite>
{
    public SatelliteValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(50);

        RuleFor(x => x.ApogeeKm).GreaterThanOrEqualTo(100).LessThanOrEqualTo(60_000);
        RuleFor(x => x.PerigeeKm).GreaterThanOrEqualTo(100).LessThanOrEqualTo(60_000);
        RuleFor(x => x.PerigeeKm).LessThanOrEqualTo(s => s.ApogeeKm).WithMessage("Periapsis cannot be above apoapsis.");
    }
}
