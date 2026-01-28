using System.Text.Json;
using Araponga.Domain.Subscriptions;
using Araponga.Modules.Subscriptions.Infrastructure.Postgres.Entities;

namespace Araponga.Modules.Subscriptions.Infrastructure.Postgres;

public static class SubscriptionsMappers
{
    public static SubscriptionPlanRecord ToRecord(this SubscriptionPlan plan)
    {
        return new SubscriptionPlanRecord
        {
            Id = plan.Id,
            Name = plan.Name,
            Description = plan.Description,
            Tier = (int)plan.Tier,
            Scope = (int)plan.Scope,
            TerritoryId = plan.TerritoryId,
            PricePerCycle = plan.PricePerCycle,
            BillingCycle = plan.BillingCycle.HasValue ? (int)plan.BillingCycle.Value : null,
            CapabilitiesJson = JsonSerializer.Serialize(plan.Capabilities),
            LimitsJson = plan.Limits != null ? JsonSerializer.Serialize(plan.Limits) : null,
            IsDefault = plan.IsDefault,
            TrialDays = plan.TrialDays,
            IsActive = plan.IsActive,
            CreatedByUserId = plan.CreatedByUserId,
            StripePriceId = plan.StripePriceId,
            StripeProductId = plan.StripeProductId,
            CreatedAtUtc = plan.CreatedAtUtc,
            UpdatedAtUtc = plan.UpdatedAtUtc
        };
    }

    public static SubscriptionPlan ToDomain(this SubscriptionPlanRecord record)
    {
        var capabilities = JsonSerializer.Deserialize<List<FeatureCapability>>(record.CapabilitiesJson) ?? new List<FeatureCapability>();
        Dictionary<string, object>? limits = null;
        if (!string.IsNullOrWhiteSpace(record.LimitsJson))
        {
            try
            {
                limits = JsonSerializer.Deserialize<Dictionary<string, object>>(record.LimitsJson);
            }
            catch
            {
                // Se falhar, limits permanece null
            }
        }

        return new SubscriptionPlan(
            record.Id,
            record.Name,
            record.Description,
            (SubscriptionPlanTier)record.Tier,
            (PlanScope)record.Scope,
            record.TerritoryId,
            record.PricePerCycle,
            record.BillingCycle.HasValue ? (SubscriptionBillingCycle)record.BillingCycle.Value : null,
            capabilities,
            limits,
            record.IsDefault,
            record.TrialDays,
            record.CreatedByUserId,
            record.StripePriceId,
            record.StripeProductId);
    }

    public static SubscriptionRecord ToRecord(this Subscription subscription)
    {
        return new SubscriptionRecord
        {
            Id = subscription.Id,
            UserId = subscription.UserId,
            TerritoryId = subscription.TerritoryId,
            PlanId = subscription.PlanId,
            Status = (int)subscription.Status,
            CurrentPeriodStart = subscription.CurrentPeriodStart,
            CurrentPeriodEnd = subscription.CurrentPeriodEnd,
            TrialStart = subscription.TrialStart,
            TrialEnd = subscription.TrialEnd,
            CanceledAt = subscription.CanceledAt,
            CancelAtPeriodEnd = subscription.CancelAtPeriodEnd,
            StripeSubscriptionId = subscription.StripeSubscriptionId,
            StripeCustomerId = subscription.StripeCustomerId,
            CreatedAtUtc = subscription.CreatedAtUtc,
            UpdatedAtUtc = subscription.UpdatedAtUtc
        };
    }

    public static Subscription ToDomain(this SubscriptionRecord record)
    {
        return new Subscription(
            record.Id,
            record.UserId,
            record.TerritoryId,
            record.PlanId,
            (SubscriptionStatus)record.Status,
            record.CurrentPeriodStart,
            record.CurrentPeriodEnd,
            record.TrialStart,
            record.TrialEnd,
            record.StripeSubscriptionId,
            record.StripeCustomerId);
    }

    public static SubscriptionPaymentRecord ToRecord(this SubscriptionPayment payment)
    {
        return new SubscriptionPaymentRecord
        {
            Id = payment.Id,
            SubscriptionId = payment.SubscriptionId,
            Amount = payment.Amount,
            Currency = payment.Currency,
            Status = (int)payment.Status,
            PaymentDate = payment.PaymentDate,
            PeriodStart = payment.PeriodStart,
            PeriodEnd = payment.PeriodEnd,
            StripeInvoiceId = payment.StripeInvoiceId,
            StripePaymentIntentId = payment.StripePaymentIntentId,
            FailureReason = payment.FailureReason,
            CreatedAtUtc = payment.CreatedAtUtc,
            UpdatedAtUtc = payment.UpdatedAtUtc
        };
    }

    public static SubscriptionPayment ToDomain(this SubscriptionPaymentRecord record)
    {
        return new SubscriptionPayment(
            record.Id,
            record.SubscriptionId,
            record.Amount,
            record.Currency,
            (SubscriptionPaymentStatus)record.Status,
            record.PaymentDate,
            record.PeriodStart,
            record.PeriodEnd,
            record.StripeInvoiceId,
            record.StripePaymentIntentId,
            record.FailureReason);
    }
}
