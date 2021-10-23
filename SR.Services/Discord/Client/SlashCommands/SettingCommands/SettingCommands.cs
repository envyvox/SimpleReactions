using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;
using SR.Data.Enums;
using SR.Services.Discord.Guild.Queries;
using SR.Services.Embed.Commands;

namespace SR.Services.Discord.Client.SlashCommands.SettingCommands
{
    public record SettingCommands(SocketSlashCommand Command) : IRequest;

    public class SettingCommandsHandler : IRequestHandler<SettingCommands>
    {
        private readonly IMediator _mediator;

        public SettingCommandsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SettingCommands request, CancellationToken ct)
        {
            var channel = (SocketTextChannel) request.Command.Channel;
            var guild = await _mediator.Send(new GetDiscordGuildQuery((long) channel.Guild.Id));
            var guildUser = channel.Guild.GetUser(request.Command.User.Id);

            if (guildUser.GuildPermissions.Administrator is false)
            {
                return await _mediator.Send(new FollowupEmbedCommand(request.Command,
                    new EmbedBuilder()
                        .WithDescription(ReplyMessage.CommandRequireAdministratorPermissions.Parse(
                            guild.LanguageType, request.Command.User.Mention))));
            }

            return request.Command.Data.Options.First().Name switch
            {
                "show" => await _mediator.Send(new SettingShowCommand(request.Command)),
                "sync-commands" => await _mediator.Send(new SettingSyncCommandsCommand(request.Command)),
                "update-color" => await _mediator.Send(new SettingUpdateColorCommand(request.Command)),
                "update-language" => await _mediator.Send(new SettingUpdateLanguageCommand(request.Command)),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
