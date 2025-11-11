using System.Globalization;
using Content.Shared.Atmos.Reactions;
using Content.Shared.GameTicking;
using Robust.Shared.Random;

namespace Content.Server.Atmos.EntitySystems;

/// <summary>
/// This handles...
/// </summary>
public sealed class GasReactionAmplitudeSystem : EntitySystem
{
    /// <inheritdoc/>
    [Dependency] private readonly AtmosphereSystem _atmosphereSystem = default!;
    [Dependency] private readonly ILogManager _logManager = default!;

    private ISawmill _sawmill = default!;
    private readonly Random _random = Random.Shared;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<RoundStartedEvent>(OnRoundStart);

        _sawmill = _logManager.GetSawmill("deb");
    }

    private void OnRoundStart(RoundStartedEvent ev)
    {
        foreach (var reaction in _atmosphereSystem.GasReactions)
        {
            var minTAmplitude = reaction.MinimumTemperatureAmplitude;
            var maxTAmplitude = reaction.MaximumTemperatureAmplitude;

            reaction.CurrentMinimumTemperatureRequirement = reaction.MinimumTemperatureRequirement + _random.NextFloat(-minTAmplitude, minTAmplitude);
            reaction.CurrentMaximumTemperatureRequirement = reaction.MaximumTemperatureAmplitude + _random.NextFloat(-maxTAmplitude, maxTAmplitude);
        }
    }
}
