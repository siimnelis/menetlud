using System.Text.Json.Serialization;

namespace Menetlus.External.Contracts.Events;

[JsonDerivedType(typeof(MenetlusLoodudEvent), typeDiscriminator: "menetlusLoodud")]
[JsonDerivedType(typeof(VoetiUlevaatamiseleEvent), typeDiscriminator: "voetiUlevaatamisele")]
[JsonDerivedType(typeof(VoetiMenetlusseEvent), typeDiscriminator: "voetiMenetlusse")]
[JsonDerivedType(typeof(MenetlusLoppesEvent), typeDiscriminator: "menetlusLoppes")]
public abstract record Event
{
    public required int MenetlusId { get; set; }
}