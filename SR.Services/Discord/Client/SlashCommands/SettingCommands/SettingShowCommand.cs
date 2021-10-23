using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;
using SR.Data;
using SR.Data.Enums;
using SR.Services.Discord.Guild.Queries;
using SR.Services.Embed.Commands;

namespace SR.Services.Discord.Client.SlashCommands.SettingCommands
{
    public record SettingShowCommand(SocketSlashCommand Command) : IRequest;

    public class SettingShowCommandHandler : IRequestHandler<SettingShowCommand>
    {
        private readonly IMediator _mediator;

        public SettingShowCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SettingShowCommand request, CancellationToken ct)
        {
            var channel = (SocketTextChannel) request.Command.Channel;
            var guild = await _mediator.Send(new GetDiscordGuildQuery((long) channel.Guild.Id));

            var embed = new EmbedBuilder()
                .AddField(ReplyMessage.SettingsLanguageFieldName.Parse(guild.LanguageType, EmoteType.List.Display()),
                    ReplyMessage.SettingsLanguageFieldDesc.Parse(guild.LanguageType, guild.LanguageType.Localize()) +
                    $"\n{PresetStrings.EmptyChar}")
                .AddField(ReplyMessage.SettingsEmbedColorFieldName.Parse(guild.LanguageType, EmoteType.List.Display()),
                    ReplyMessage.SettingsEmbedColorFieldDesc.Parse(guild.LanguageType, guild.EmbedColor));

            return await _mediator.Send(new FollowupEmbedCommand(request.Command, embed));
        }
    }
}
