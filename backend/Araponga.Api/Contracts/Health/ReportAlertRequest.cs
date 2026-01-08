namespace Araponga.Api.Contracts.Health;

public sealed record ReportAlertRequest(
    string Title,
    string Description
);
