using System.Security.Cryptography;
using System.Text;
using Araponga.Application.Common;
using Araponga.Application.Interfaces;
using Araponga.Application.Models;
using Microsoft.Extensions.Logging;

namespace Araponga.Infrastructure.Email;

public sealed class LoggingEmailSender : IEmailSender
{
    private readonly ILogger<LoggingEmailSender> _logger;

    public LoggingEmailSender(ILogger<LoggingEmailSender> logger)
    {
        _logger = logger;
    }

    public Task<OperationResult> SendEmailAsync(
        string to,
        string subject,
        string body,
        bool isHtml,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Email enviado (simulado). To={To} Subject={Subject} IsHtml={IsHtml} BodyLength={BodyLength}",
            MaskEmail(to),
            subject,
            isHtml,
            body?.Length ?? 0);

        return Task.FromResult(OperationResult.Success());
    }

    public Task<OperationResult> SendEmailAsync(
        string to,
        string subject,
        string templateName,
        object templateData,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Email enviado (simulado) com template. To={To} Subject={Subject} Template={Template}",
            MaskEmail(to),
            subject,
            templateName);

        return Task.FromResult(OperationResult.Success());
    }

    public Task<OperationResult> SendEmailAsync(
        EmailMessage message,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Email enviado (simulado) via EmailMessage. To={To} Subject={Subject} IsHtml={IsHtml} BodyLength={BodyLength}",
            MaskEmail(message.To),
            message.Subject,
            message.IsHtml,
            message.Body?.Length ?? 0);

        return Task.FromResult(OperationResult.Success());
    }

    private static string MaskEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return "***";

        // Use a non-reversible hash of the email to prevent log forging and PII exposure
        // Remove control characters first to prevent log injection
        var sanitized = new string(email.Where(c => !char.IsControl(c)).ToArray()).Trim();
        
        if (string.IsNullOrEmpty(sanitized))
            return "***";

        // Generate SHA-256 hash of the email
        var emailBytes = Encoding.UTF8.GetBytes(sanitized);
        var hashBytes = SHA256.HashData(emailBytes);
        var hashHex = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

        // Return hash prefix for correlation without exposing the email
        return $"email#{hashHex.Substring(0, 8)}";
    }
}
