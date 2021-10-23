using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;
using SR.Data.Enums;
using SR.Services.Discord.Guild.Commands;
using SR.Services.Discord.Guild.Queries;
using SR.Services.Embed.Commands;

namespace SR.Services.Discord.Client.SlashCommands.SettingCommands
{
    public record SettingUpdateLanguageCommand(SocketSlashCommand Command) : IRequest;

    public class SettingUpdateLanguageCommandHandler : IRequestHandler<SettingUpdateLanguageCommand>
    {
        private readonly IMediator _mediator;

        public SettingUpdateLanguageCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SettingUpdateLanguageCommand request, CancellationToken ct)
        {
            var language = (LanguageType) (long) request.Command.Data.Options.First().Options.First().Value;
            var channel = (SocketTextChannel) request.Command.Channel;
            var guild = await _mediator.Send(new GetDiscordGuildQuery((long) channel.Guild.Id));

            await _mediator.Send(new UpdateGuildLanguageCommand(guild.Id, language));

            var embed = new EmbedBuilder()
                .WithDescription(ReplyMessage.SetLanguageSuccess.Parse(
                    language, request.Command.User.Mention, language.Localize()));

            return await _mediator.Send(new FollowupEmbedCommand(request.Command, embed));
        }
    }
}
