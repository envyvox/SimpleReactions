using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;
using SR.Services.Discord.Guild.Queries;

namespace SR.Services.Embed.Commands
{
    public record FollowupEmbedCommand(
            SocketSlashCommand Command,
            EmbedBuilder EmbedBuilder,
            string Text = null)
        : IRequest;

    public class FollowupEmbedHandler : IRequestHandler<FollowupEmbedCommand>
    {
        private readonly IMediator _mediator;

        public FollowupEmbedHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(FollowupEmbedCommand request, CancellationToken cancellationToken)
        {
            var channel = (SocketTextChannel) request.Command.Channel;
            var guild = await _mediator.Send(new GetDiscordGuildQuery((long) channel.Guild.Id));

            var embed = request.EmbedBuilder
                .WithColor(new Color(uint.Parse(guild.EmbedColor, NumberStyles.HexNumber)))
                .Build();

            await request.Command.FollowupAsync(
                text: request.Text,
                embed: embed);

            return Unit.Value;
        }
    }
}
