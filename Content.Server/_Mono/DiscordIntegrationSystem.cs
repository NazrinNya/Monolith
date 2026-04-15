namespace Content.Server._Mono;

/// <summary>
/// This handles...
/// </summary>
public sealed class DiscordIntegrationSystem : EntitySystem
{
    /// <inheritdoc/>
    public override void Initialize()
    {
        Mono.DiscordIntegration.Program.InitServer(EntityManager);
    }

    public override void Update(float frameTime)
    {
        Mono.DiscordIntegration.Program.Update(frameTime);
    }
}
