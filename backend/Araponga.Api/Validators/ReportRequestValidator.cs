using Araponga.Api.Contracts.Moderation;
using FluentValidation;

namespace Araponga.Api.Validators;

public sealed class ReportRequestValidator : AbstractValidator<ReportRequest>
{
    public ReportRequestValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason is required.")
            .MaximumLength(300).WithMessage("Reason must not exceed 300 characters.");

        When(x => !string.IsNullOrWhiteSpace(x.Details), () =>
        {
            RuleFor(x => x.Details)
                .MaximumLength(2000).WithMessage("Details must not exceed 2000 characters.");
        });
    }
}
