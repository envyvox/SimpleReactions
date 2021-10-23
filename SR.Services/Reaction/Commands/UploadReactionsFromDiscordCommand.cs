using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using MediatR;
using Microsoft.Extensions.Options;
using SR.Data.Enums;
using SR.Services.Discord.Client;
using SR.Services.Discord.Client.Extensions;

namespace SR.Services.Reaction.Commands
{
    public record UploadReactionsFromDiscordCommand : IRequest;

    public class UploadReactionsFromDiscordHandler : IRequestHandler<UploadReactionsFromDiscordCommand>
    {
        private readonly DiscordClientOptions _options;
        private readonly IMediator _mediator;
        private readonly IDiscordClientService _discordClientService;

        public UploadReactionsFromDiscordHandler(
            IOptions<DiscordClientOptions> options,
            IMediator mediator,
            IDiscordClientService discordClientService)
        {
            _options = options.Value;
            _mediator = mediator;
            _discordClientService = discordClientService;
        }

        public async Task<Unit> Handle(UploadReactionsFromDiscordCommand request, CancellationToken ct)
        {
            var client = await _discordClientService.GetSocketClient();
            var guild = client.GetGuild(_options.UploadGuildId);

            foreach (var type in Enum.GetValues(typeof(ReactionType))
                .Cast<ReactionType>())
            {
                var reactionChannel = guild.TextChannels.First(x => x.Name == type.Name());
                var messages = await reactionChannel.GetMessagesAsync().FlattenAsync();
                var urls = messages.Select(message => message.Attachments.First().Url);

                foreach (var url in urls)
                {
                    await _mediator.Send(new CreateReactionCommand(type, url));
                }
            }

            return Unit.Value;
        }
    }
}
