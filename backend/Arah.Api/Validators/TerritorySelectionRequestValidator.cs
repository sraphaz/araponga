using Arah.Api.Contracts.Territories;
using FluentValidation;

namespace Arah.Api.Validators;

public sealed class TerritorySelectionRequestValidator : AbstractValidator<TerritorySelectionRequest>
{
    public TerritorySelectionRequestValidator()
    {
        RuleFor(x => x.TerritoryId)
            .NotEmpty().WithMessage("Territory ID is required.");
    }
}
