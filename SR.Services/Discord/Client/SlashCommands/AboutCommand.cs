using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;
using SR.Data;
using SR.Data.Enums;
using SR.Services.Discord.Guild.Queries;
using SR.Services.Embed.Commands;

namespace SR.Services.Discord.Client.SlashCommands
{
    public record AboutCommand(SocketSlashCommand Command) : IRequest;

    public class AboutCommandHandler : IRequestHandler<AboutCommand>
    {
        private readonly IMediator _mediator;
        private readonly IDiscordClientService _discordClientService;

        public AboutCommandHandler(
            IMediator mediator,
            IDiscordClientService discordClientService)
        {
            _mediator = mediator;
            _discordClientService = discordClientService;
        }

        public async Task<Unit> Handle(AboutCommand request, CancellationToken ct)
        {
            var client = await _discordClientService.GetSocketClient();
            var channel = (SocketTextChannel) request.Command.Channel;
            var guild = await _mediator.Send(new GetDiscordGuildQuery((long) channel.Guild.Id));

            var embed = new EmbedBuilder()
                .WithThumbnailUrl(client.CurrentUser.GetAvatarUrl())
                .WithDescription(ReplyMessage.AboutDesc.Parse(guild.LanguageType, client.CurrentUser.Mention))
                .AddField(ReplyMessage.AboutJoinSupportServerFieldName.Parse(guild.LanguageType),
                    ReplyMessage.AboutJoinSupportServerFieldDesc.Parse(guild.LanguageType,
                        EmoteType.PineappleLove.Display(), PresetStrings.SupportServerInviteLink))
                .AddField(ReplyMessage.AboutInviteBotFieldName.Parse(guild.LanguageType),
                    ReplyMessage.AboutInviteBotFieldDesc.Parse(guild.LanguageType,
                        EmoteType.PineappleLove.Display(), PresetStrings.BotInviteLink))
                .AddField(ReplyMessage.AboutVoteFieldName.Parse(guild.LanguageType),
                    ReplyMessage.AboutVoteFieldDesc.Parse(guild.LanguageType,
                        EmoteType.PineappleLove.Display(), PresetStrings.VoteLink))
                .AddField(ReplyMessage.AboutContactsFieldName.Parse(guild.LanguageType),
                    ReplyMessage.AboutContactsFieldDesc.Parse(guild.LanguageType,
                        EmoteType.DiscordLogo.Display(), EmoteType.TwitterLogo.Display()))
                .AddField(ReplyMessage.AboutSupportFieldName.Parse(guild.LanguageType),
                    ReplyMessage.AboutSupportFieldDesc.Parse(guild.LanguageType,
                        EmoteType.PineappleLove.Display(), EmoteType.Mastercard.Display()));

            return await _mediator.Send(new FollowupEmbedCommand(request.Command, embed));
        }
    }
}
