using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SR.Framework.Autofac;
using SR.Services.DiscordServices.DiscordClientService.Options;
using SR.Services.DiscordServices.DiscordGuildService;
using SR.Services.ReactionService;

namespace SR.Services.DiscordServices.DiscordClientService.Impl
{
    [InjectableService(IsSingletone = true)]
    public class DiscordClientService : IDiscordClientService
    {
        private readonly IOptions<DiscordClientOptions> _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly CommandService _commandService;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly ILogger<DiscordClientService> _logger;
        private readonly IReactionService _reactionService;
        private readonly IDiscordGuildService _discordGuildService;

        private DiscordSocketClient _socketClient;

        public DiscordClientService(IOptions<DiscordClientOptions> options, IServiceProvider serviceProvider,
            CommandService commandService, IHostApplicationLifetime lifetime, ILogger<DiscordClientService> logger,
            IReactionService reactionService, IDiscordGuildService discordGuildService)
        {
            _options = options;
            _serviceProvider = serviceProvider;
            _commandService = commandService;
            _lifetime = lifetime;
            _logger = logger;
            _reactionService = reactionService;
            _discordGuildService = discordGuildService;
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
            await _commandService.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);
            await _socketClient.LoginAsync(TokenType.Bot, _options.Value.Token);
            await _socketClient.StartAsync();

            _commandService.CommandExecuted += CommandServiceOnCommandExecuted;
            _socketClient.Log += SocketClientOnLog;
            _socketClient.Ready += SocketClientOnReady;
            _socketClient.MessageReceived += SocketClientOnMessageReceived;
            _socketClient.JoinedGuild += SocketClientOnJoinedGuild;
            _socketClient.LeftGuild += SocketClientOnLeftGuild;
        }

        private static async Task CommandServiceOnCommandExecuted(Optional<CommandInfo> command,
            ICommandContext context,
            IResult result)
        {
            if (!string.IsNullOrEmpty(result?.ErrorReason) && result.Error is not CommandError.UnknownCommand)
                await context.Channel.SendMessageAsync($"{context.User.Mention}, {result.ErrorReason}");
        }

        private static Task SocketClientOnLog(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        private async Task SocketClientOnReady()
        {
            try
            {
                await _socketClient.SetGameAsync("..help ..about", null, ActivityType.Watching);
                await _reactionService.UploadImagesFromDiscord(_socketClient);

                _logger.LogInformation("Bot started");
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Unable to startup the bot. Application will now exit");
                _lifetime.StopApplication();
            }
        }

        private async Task SocketClientOnMessageReceived(SocketMessage socketMessage)
        {
            if (socketMessage is not SocketUserMessage message) return;

            var prefix = "..";

            if (socketMessage.Channel is SocketGuildChannel guildChannel)
                prefix = await _discordGuildService.GetGuildPrefix((long) guildChannel.Guild.Id);

            var argPos = 0;
            if (!(message.HasStringPrefix(prefix, ref argPos) ||
                  message.HasMentionPrefix(_socketClient.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_socketClient, message);
            var result = await _commandService.ExecuteAsync(context, argPos, _serviceProvider);

            if (result.IsSuccess && message.Channel is SocketGuildChannel)
            {
                await Task.Delay(1000);
                await context.Message.DeleteAsync();
            }
        }

        private async Task SocketClientOnJoinedGuild(SocketGuild socketGuild) =>
            await _discordGuildService.AddDiscordGuildToDb((long) socketGuild.Id);

        private async Task SocketClientOnLeftGuild(SocketGuild socketGuild) =>
            await _discordGuildService.DeleteDiscordGuildFromDb((long) socketGuild.Id);
    }
}
