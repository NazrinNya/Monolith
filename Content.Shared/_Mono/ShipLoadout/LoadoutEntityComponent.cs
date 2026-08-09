namespace Content.Shared._Mono.ShipLoadout;

/// <summary>
/// This is used for...
/// </summary>
[RegisterComponent]
public sealed partial class LoadoutEntityComponent : Component
{
    [DataField]
    public string LoadoutName;

    [DataField]
    public int LoadoutIndex;
}
