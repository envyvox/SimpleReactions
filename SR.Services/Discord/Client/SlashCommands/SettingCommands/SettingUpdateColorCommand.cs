using System.Globalization;
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
    public record SettingUpdateColorCommand(SocketSlashCommand Command) : IRequest;

    public class SettingUpdateColorCommandHandler : IRequestHandler<SettingUpdateColorCommand>
    {
        private readonly IMediator _mediator;

        public SettingUpdateColorCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SettingUpdateColorCommand request, CancellationToken ct)
        {
            var option = ((string) request.Command.Data.Options.First().Options.First().Value).Replace("#", "");
            var color = new Color(uint.Parse(option, NumberStyles.HexNumber));
            var channel = (SocketTextChannel) request.Command.Channel;
            var guild = await _mediator.Send(new GetDiscordGuildQuery((long) channel.Guild.Id));

            await _mediator.Send(new UpdateGuildEmbedColorCommand(guild.Id, option));

            var embed = new EmbedBuilder()
                .WithColor(color)
                .WithDescription(ReplyMessage.SetColorSuccess.Parse(
                    guild.LanguageType, request.Command.User.Mention, option));

            return await _mediator.Send(new FollowupEmbedCommand(request.Command, embed));
        }
    }
}
