using FluentValidation;

namespace Tyne.HttpMediator;

public class ValidatedRequestValidator : AbstractValidator<ValidatedRequest>
{
    public ValidatedRequestValidator()
    {
        _ = RuleFor(m => m.Message).Equal(ValidatedRequest.ValidMessage);
    }
}
