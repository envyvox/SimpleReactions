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

namespace SR.Services.Discord.Client.SlashCommands
{
    public record HelpCommand(SocketSlashCommand Command) : IRequest;

    public class HelpCommandHandler : IRequestHandler<HelpCommand>
    {
        private readonly IMediator _mediator;

        public HelpCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(HelpCommand request, CancellationToken ct)
        {
            var channel = (SocketTextChannel) request.Command.Channel;
            var guild = await _mediator.Send(new GetDiscordGuildQuery((long) channel.Guild.Id));

            var reactionsString = Enum.GetValues(typeof(ReactionType))
                .Cast<ReactionType>()
                .Aggregate(string.Empty, (s, v) =>
                    s + guild.LanguageType switch
                    {
                        LanguageType.English => $"`/{v.ToString().ToLower()}`, ",
                        LanguageType.Russian => $"`/{v.Name().ToLower()}`, ",
                        _ => throw new ArgumentOutOfRangeException()
                    });

            var embed = new EmbedBuilder()
                .AddField(ReplyMessage.HelpHowToSetupBotFieldName.Parse(guild.LanguageType,
                        EmoteType.PineappleThinking.Display()),
                    ReplyMessage.HelpHowToSetupBotFieldDesc.Parse(guild.LanguageType,
                        EmoteType.PineappleReading.Display(), LanguageType.English.Localize()))
                .AddField(ReplyMessage.HelpHowToUseReactionsFieldName.Parse(guild.LanguageType,
                        EmoteType.PineappleThinking.Display()),
                    ReplyMessage.HelpHowToUseReactionsFieldDesc.Parse(guild.LanguageType,
                        EmoteType.PineappleReading.Display(), reactionsString.Remove(reactionsString.Length - 2)));

            return await _mediator.Send(new FollowupEmbedCommand(request.Command, embed));
        }
    }
}
