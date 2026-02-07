using Arah.Api.Contracts.Users;
using FluentValidation;

namespace Arah.Api.Validators;

public sealed class UpdateAvatarRequestValidator : AbstractValidator<UpdateAvatarRequest>
{
    public UpdateAvatarRequestValidator()
    {
        RuleFor(x => x.MediaAssetId)
            .NotEmpty()
            .WithMessage("MediaAssetId é obrigatório.");
    }
}
