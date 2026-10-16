using System.Text.Json.Serialization;

namespace GoGym.Contracts.Profiles;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProfileType { Admin, Trainer, Participant };