using System;
using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SR.Data.Enums;
using SR.Services.Discord.Client.Extensions;
using SR.Services.Discord.Client.SlashCommands;
using SR.Services.Discord.Client.SlashCommands.SettingCommands;
using SR.Services.Discord.Guild.Commands;
using SR.Services.Reaction.Commands;
using IRequest = MediatR.IRequest;

namespace SR.Services.Discord.Client.Impl
{
    public class DiscordClientService : IDiscordClientService
    {
        private readonly IOptions<DiscordClientOptions> _options;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly ILogger<DiscordClientService> _logger;
        private readonly IMediator _mediator;
        private readonly DiscordSocketClient _socketClient;

        public DiscordClientService(
            IOptions<DiscordClientOptions> options,
            IHostApplicationLifetime lifetime,
            ILogger<DiscordClientService> logger,
            IMediator mediator)
        {
            _options = options;
            _lifetime = lifetime;
            _logger = logger;
            _mediator = mediator;
            _socketClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 100,
                AlwaysDownloadUsers = true,
                GatewayIntents = GatewayIntents.Guilds |
                                 GatewayIntents.GuildMembers |
                                 GatewayIntents.GuildMessageReactions |
                                 GatewayIntents.GuildMessages |
                                 GatewayIntents.GuildVoiceStates |
                                 GatewayIntents.DirectMessages
            });
        }

        public async Task Start()
        {
            await _socketClient.LoginAsync(TokenType.Bot, _options.Value.Token);
            await _socketClient.StartAsync();

            _socketClient.Ready += SocketClientOnReady;
            _socketClient.JoinedGuild += SocketClientOnJoinedGuild;
            _socketClient.LeftGuild += SocketClientOnLeftGuild;
            _socketClient.InteractionCreated += SocketClientOnInteractionCreated;
        }

        public async Task<DiscordSocketClient> GetSocketClient()
        {
            return await Task.FromResult(_socketClient);
        }

        private async Task SocketClientOnReady()
        {
            ApplicationCommandProperties[] commands =
            {
                new SlashCommandBuilder()
                    .WithName("about")
                    .WithDescription("Bot information")
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("help")
                    .WithDescription("Information on how to use bot")
                    .Build(),

                new SlashCommandBuilder()
                    .WithName("settings")
                    .WithDescription("Bot settings")
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithName("show")
                        .WithDescription("Show current bot settings"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithName("sync-commands")
                        .WithDescription(
                            "Run this command to create reaction commands with current language"))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithName("update-color")
                        .WithDescription("Change bot embed color")
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.String)
                            .WithRequired(true)
                            .WithName("color")
                            .WithDescription("New embed color (HEX)")))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithType(ApplicationCommandOptionType.SubCommand)
                        .WithName("update-language")
                        .WithDescription("Change bot language")
                        .AddOption(new SlashCommandOptionBuilder()
                            .WithType(ApplicationCommandOptionType.Integer)
                            .WithRequired(true)
                            .WithName("language")
                            .WithDescription("New bot language")
                            .AddChoice(LanguageType.English.ToString(), LanguageType.English.GetHashCode())
                            .AddChoice(LanguageType.Russian.ToString(), LanguageType.Russian.GetHashCode())))
                    .Build()
            };

            try
            {
                await _socketClient.Rest.BulkOverwriteGlobalCommands(commands);
                await _mediator.Send(new UploadReactionsFromDiscordCommand());
                await UpdateClientStatus();

                _logger.LogInformation("Bot started");
            }
            catch (ApplicationCommandException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Error, Formatting.Indented);
                Console.WriteLine(json);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Unable to startup the bot. Application will now exit");
                _lifetime.StopApplication();
            }
        }

        private async Task SocketClientOnJoinedGuild(SocketGuild socketGuild)
        {
            await UpdateClientStatus();
            await _mediator.Send(new CreateDiscordGuildCommand((long) socketGuild.Id));
        }

        private async Task SocketClientOnLeftGuild(SocketGuild socketGuild)
        {
            await UpdateClientStatus();
            await _mediator.Send(new DeleteDiscordGuildCommand((long) socketGuild.Id));
        }

        private async Task<Unit> SocketClientOnInteractionCreated(SocketInteraction interaction)
        {
            try
            {
                return interaction switch
                {
                    SocketSlashCommand command => command.Data.Name switch
                    {
                        "about" => await HandleInteraction(interaction, new AboutCommand(command), true),
                        "help" => await HandleInteraction(interaction, new HelpCommand(command), true),
                        "settings" => await HandleInteraction(interaction, new SettingCommands(command), true),
                        _ => await HandleInteraction(interaction, new ReactionCommand(command), false)
                    },
                    _ => Unit.Value
                };
            }
            catch (Exception e)
            {
                var embed = new EmbedBuilder()
                    .WithColor(new Color(uint.Parse("36393F", NumberStyles.HexNumber)))
                    .WithDescription(
                        $"{interaction.User.Mention}, {e.Message}.")
                    .Build();

                await interaction.FollowupAsync(embed: embed, ephemeral: true);
            }

            return Unit.Value;
        }

        private async Task<Unit> HandleInteraction<T>(SocketInteraction interaction, T implementation, bool ephemeral)
            where T : IRequest
        {
            await interaction.DeferAsync(ephemeral, new RequestOptions
            {
                RetryMode = RetryMode.Retry502,
                Timeout = 10000
            });

            return await _mediator.Send(implementation);
        }

        private async Task UpdateClientStatus()
        {
            await _socketClient.SetGameAsync(
                name: $"/help /about | {_socketClient.Guilds.Count} servers",
                type: ActivityType.Watching);
        }
    }
}
