using Arah.Api.Contracts.Alerts;
using FluentValidation;

namespace Arah.Api.Validators;

public sealed class ReportAlertRequestValidator : AbstractValidator<ReportAlertRequest>
{
    public ReportAlertRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");
    }
}
