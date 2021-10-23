using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;
using SR.Data.Enums;
using SR.Services.Discord.Client.Commands;
using SR.Services.Discord.Guild.Queries;
using SR.Services.Embed.Commands;

namespace SR.Services.Discord.Client.SlashCommands.SettingCommands
{
    public record SettingSyncCommandsCommand(SocketSlashCommand Command) : IRequest;

    public class SettingSyncCommandsCommandHandler : IRequestHandler<SettingSyncCommandsCommand>
    {
        private readonly IMediator _mediator;
        private readonly IDiscordClientService _discordClientService;

        public SettingSyncCommandsCommandHandler(
            IMediator mediator,
            IDiscordClientService discordClientService)
        {
            _mediator = mediator;
            _discordClientService = discordClientService;
        }

        public async Task<Unit> Handle(SettingSyncCommandsCommand request, CancellationToken ct)
        {
            var channel = (SocketTextChannel) request.Command.Channel;
            var guild = await _mediator.Send(new GetDiscordGuildQuery((long) channel.Guild.Id));
            var client = await _discordClientService.GetSocketClient();

            var reactionCommands = Enum
                .GetValues(typeof(ReactionType))
                .Cast<ReactionType>()
                .Select(reaction =>
                    new SlashCommandBuilder()
                        .WithName(guild.LanguageType switch
                        {
                            LanguageType.English =>
                                reaction.ToString().ToLower(),
                            LanguageType.Russian =>
                                reaction.Name().ToLower(),
                            _ => throw new ArgumentOutOfRangeException()
                        })
                        .WithDescription(guild.LanguageType switch
                        {
                            LanguageType.English =>
                                $"Use '{reaction.ToString().ToLower()}' reaction",
                            LanguageType.Russian =>
                                $"Использовать реакцию '{reaction.Name().ToLower()}'",
                            _ => throw new ArgumentOutOfRangeException()
                        })
                        .AddOption(new SlashCommandOptionBuilder().WithType(ApplicationCommandOptionType.String)
                            .WithRequired(false)
                            .WithName(guild.LanguageType switch
                            {
                                LanguageType.English =>
                                    "text",
                                LanguageType.Russian =>
                                    "текст",
                                _ => throw new ArgumentOutOfRangeException()
                            })
                            .WithDescription(guild.LanguageType switch
                            {
                                LanguageType.English =>
                                    "Text or mention to be added to the reaction message",
                                LanguageType.Russian =>
                                    "Текст или упоминание которое будет добавлено к сообщению реакции",
                                _ => throw new ArgumentOutOfRangeException()
                            }))
                        .Build())
                .Cast<ApplicationCommandProperties>()
                .ToArray();

            await _mediator.Send(new BulkOverwriteGuildCommandsCommand(
                client.Rest, reactionCommands, channel.Guild.Id));

            var embed = new EmbedBuilder()
                .WithDescription(ReplyMessage.SyncCommandsSuccess.Parse(
                    guild.LanguageType, request.Command.User.Mention));

            return await _mediator.Send(new FollowupEmbedCommand(request.Command, embed));
        }
    }
}
