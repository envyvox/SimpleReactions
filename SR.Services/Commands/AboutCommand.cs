using System.Globalization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SR.Data.Enums;
using SR.Services.DiscordServices.DiscordGuildService;
using Emote = SR.Data.Enums.Emote;

namespace SR.Services.Commands
{
    public class AboutCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordGuildService _discordGuildService;

        public AboutCommand(IDiscordGuildService discordGuildService)
        {
            _discordGuildService = discordGuildService;
        }

        [Command("about"), Alias("инфо")]
        public async Task AboutTask()
        {
            var language = Language.English;
            var color = "36393F";

            if (Context.Guild is not null)
            {
                language = await _discordGuildService.GetGuildLanguage((long) Context.Guild.Id);
                color = await _discordGuildService.GetGuildColor((long) Context.Guild.Id);
            }

            var embed = new EmbedBuilder()
                .WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl())
                .WithColor(new Color(uint.Parse(color, NumberStyles.HexNumber)))
                .WithDescription(ReplyMessage.AboutDesc.Parse(language: language,
                    Context.Client.CurrentUser.Mention))
                .AddField(ReplyMessage.AboutInviteFieldName.Parse(language: language),
                    ReplyMessage.AboutInviteFieldDesc.Parse(language: language,
                        Emote.PineappleLove.Display()))
                .AddField(ReplyMessage.AboutContactFieldName.Parse(language: language),
                    ReplyMessage.AboutContactFieldDesc.Parse(language: language,
                        Emote.DiscordLogo.Display(), Emote.TwitterLogo.Display()))
                .AddField(ReplyMessage.AboutSupportFieldName.Parse(language: language),
                    ReplyMessage.AboutSupportFieldDesc.Parse(language: language,
                        Emote.PineappleLove.Display(), Emote.Mastercard.Display()));

            await ReplyAsync(null, false, embed.Build());
        }
    }
}
