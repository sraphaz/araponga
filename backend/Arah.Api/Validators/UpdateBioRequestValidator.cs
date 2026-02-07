using Arah.Api.Contracts.Users;
using FluentValidation;

namespace Arah.Api.Validators;

public sealed class UpdateBioRequestValidator : AbstractValidator<UpdateBioRequest>
{
    public UpdateBioRequestValidator()
    {
        RuleFor(x => x.Bio)
            .MaximumLength(500)
            .WithMessage("Bio deve ter no máximo 500 caracteres.");
    }
}
