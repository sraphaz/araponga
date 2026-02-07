using Arah.Api.Contracts.Map;
using FluentValidation;

namespace Arah.Api.Validators;

public sealed class SuggestMapEntityRequestValidator : AbstractValidator<SuggestMapEntityRequest>
{
    public SuggestMapEntityRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmptyWithMaxLength(200);

        RuleFor(x => x.Category)
            .NotEmptyWithMaxLength(100);

        RuleFor(x => x.Latitude)
            .Latitude();

        RuleFor(x => x.Longitude)
            .Longitude();
    }
}
