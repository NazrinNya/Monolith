using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Shared._Mono.ShipLoadout;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class ShipLoadoutComponent : Component
{
    public List<LoadoutData> LoadoutData;
}

[DataDefinition]
public sealed partial class LoadoutData
{
    [DataField]
    public string LoadoutName;

    [DataField]
    public EntityData[] EntityData;
}

[DataDefinition]
public sealed partial class EntityData
{
    [DataField]
    public EntProtoId Id;

    [DataField]
    public EntityCoordinates Coordinates;
}
