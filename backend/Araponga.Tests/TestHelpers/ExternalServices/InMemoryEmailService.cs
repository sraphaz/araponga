using System.Collections.Concurrent;
using Araponga.Application.Common;
using Araponga.Application.Interfaces;
using Araponga.Application.Models;

namespace Araponga.Tests.TestHelpers.ExternalServices;

/// <summary>
/// Implementação in-memory de IEmailSender para testes.
/// Armazena emails enviados em memória e suporta reset e configuração de comportamento.
/// </summary>
public sealed class InMemoryEmailService : IEmailSender, ITestExternalService
{
    private readonly ConcurrentBag<EmailMessage> _sentEmails = new();
    private readonly Dictionary<string, object?> _behaviors = new();

    public Task<OperationResult> SendEmailAsync(
        string to,
        string subject,
        string body,
        bool isHtml,
        CancellationToken cancellationToken)
    {
        if (_behaviors.TryGetValue("throw_exception", out var throwException) && throwException != null)
        {
            throw new InvalidOperationException("Configured to throw exception");
        }

        if (_behaviors.TryGetValue("return_failure", out var returnFailure) && returnFailure != null)
        {
            return Task.FromResult(OperationResult.Failure("Configured to return failure"));
        }

        var message = new EmailMessage
        {
            To = to,
            Subject = subject,
            Body = body,
            IsHtml = isHtml
        };

        _sentEmails.Add(message);
        return Task.FromResult(OperationResult.Success());
    }

    public Task<OperationResult> SendEmailAsync(
        string to,
        string subject,
        string templateName,
        object templateData,
        CancellationToken cancellationToken)
    {
        if (_behaviors.TryGetValue("throw_exception", out var throwException) && throwException != null)
        {
            throw new InvalidOperationException("Configured to throw exception");
        }

        if (_behaviors.TryGetValue("return_failure", out var returnFailure) && returnFailure != null)
        {
            return Task.FromResult(OperationResult.Failure("Configured to return failure"));
        }

        var message = new EmailMessage
        {
            To = to,
            Subject = subject,
            TemplateName = templateName,
            TemplateData = templateData
        };

        _sentEmails.Add(message);
        return Task.FromResult(OperationResult.Success());
    }

    public Task<OperationResult> SendEmailAsync(
        EmailMessage message,
        CancellationToken cancellationToken)
    {
        if (_behaviors.TryGetValue("throw_exception", out var throwException) && throwException != null)
        {
            throw new InvalidOperationException("Configured to throw exception");
        }

        if (_behaviors.TryGetValue("return_failure", out var returnFailure) && returnFailure != null)
        {
            return Task.FromResult(OperationResult.Failure("Configured to return failure"));
        }

        _sentEmails.Add(message);
        return Task.FromResult(OperationResult.Success());
    }

    public void Reset()
    {
        _sentEmails.Clear();
        _behaviors.Clear();
    }

    public void ConfigureBehavior(string behavior, object? result = null)
    {
        _behaviors[behavior] = result;
    }

    /// <summary>
    /// Obtém todos os emails enviados durante o teste.
    /// </summary>
    public IReadOnlyList<EmailMessage> GetSentEmails() => _sentEmails.ToList();

    /// <summary>
    /// Verifica se um email foi enviado para um destinatário específico.
    /// </summary>
    public bool WasEmailSentTo(string to) => _sentEmails.Any(e => e.To == to);
}
