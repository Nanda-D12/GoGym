using System.Text.Json.Serialization;

namespace GoGym.Contracts.Subscriptions;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SubscriptionType
{
    Free,
    Starter,
    Pro
}