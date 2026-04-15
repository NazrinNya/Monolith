using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Robust.Server;
using Robust.Shared.GameObjects;
using Robust.Shared.Log;

namespace Content.Mono.DiscordIntegration;

public sealed class Program
{
    private static DiscordSocketClient _client = null!;
    private static EntityManager _entityManager = null!;
    private static HashSet<SocketSlashCommand> _commandQueue = new();

    public static void InitServer(EntityManager entityManager)
    {
        _entityManager = entityManager;
        Main();
    }

    private static async Task Main()
    {
        _client = new DiscordSocketClient();

        _client.Log += Log;
        _client.SlashCommandExecuted += SlashCommandHandler;
        
        await _client.StartAsync();

        await Task.Delay(-1);

        Task SlashCommandHandler(SocketSlashCommand command)
        {
            _commandQueue.Add(command);

            return Task.CompletedTask;
        }

        async Task CreateCommands()
        {
            var globalCommand = new SlashCommandBuilder();
            globalCommand.WithName("get_entity_name");
            globalCommand.WithDescription("65");
            globalCommand.AddOption("uid", ApplicationCommandOptionType.Integer, "uid of entity");

            try
            {
                await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
            }
            catch
            {
                Console.WriteLine("real");
            }
        }

        Task Log(Discord.LogMessage msg)
        {
            Logger.Info(msg.ToString());
            return Task.CompletedTask;
        }
    }

    public static void Update(float frameTime)
    {
        foreach (var command in _commandQueue)
        {
            Logger.Info("command!");

            NetEntity.TryParse(command.Data.Options.First().Value.ToString(), out var net);

            command.RespondAsync(_entityManager.MetaQuery.Get(_entityManager.GetEntity(net)).Comp.EntityName);
        }

        _commandQueue.Clear();
    }
}

