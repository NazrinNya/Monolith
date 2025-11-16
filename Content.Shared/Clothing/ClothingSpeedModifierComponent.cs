using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Clothing;

/// <summary>
/// Modifies speed when worn and activated.
/// Supports <c>ItemToggleComponent</c>.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(ClothingSpeedModifierSystem))]
public sealed partial class ClothingSpeedModifierComponent : Component
{
    [DataField]
    public float WalkModifier = 1.0f;

    [DataField]
    public float SprintModifier = 1.0f;

    /// <summary>
    /// Mono edit - This is used to mark if toggle state of ItemToggle component affects ClothingSpeedComponent or not.
    /// </summary>
    [DataField]
    public bool ItemToggleAffected = true;
}

[Serializable, NetSerializable]
public sealed class ClothingSpeedModifierComponentState : ComponentState
{
    public float WalkModifier;
    public float SprintModifier;

    public ClothingSpeedModifierComponentState(float walkModifier, float sprintModifier)
    {
        WalkModifier = walkModifier;
        SprintModifier = sprintModifier;
    }
}
