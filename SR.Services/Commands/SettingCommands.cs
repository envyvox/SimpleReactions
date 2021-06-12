using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SR.Data.Enums;
using SR.Services.DiscordServices.DiscordGuildService;
using Emote = SR.Data.Enums.Emote;

namespace SR.Services.Commands
{
    [Group("settings"), Alias("настройки")]
    [RequireContext(ContextType.Guild), RequireUserPermission(GuildPermission.Administrator)]
    public class SettingCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordGuildService _discordGuildService;

        public SettingCommands(IDiscordGuildService discordGuildService)
        {
            _discordGuildService = discordGuildService;
        }

        [Command]
        public async Task SettingsTask()
        {
            var guildPrefix = await _discordGuildService.GetGuildPrefix((long) Context.Guild.Id);
            var guildColor = await _discordGuildService.GetGuildColor((long) Context.Guild.Id);
            var guildLanguage = await _discordGuildService.GetGuildLanguage((long) Context.Guild.Id);
            var availableLanguages = Enum.GetValues(typeof(Language))
                .Cast<Language>()
                .Aggregate(string.Empty, (current, language) => current + $"> `{language.GetHashCode()}` - {language.Localize()}\n");

            var embed = new EmbedBuilder()
                .WithImageUrl("https://cdn.discordapp.com/attachments/842067362139209778/848897929641328660/ColorPicker.png")
                .WithAuthor(Context.Guild.Name)
                .WithColor(new Color(uint.Parse(guildColor, NumberStyles.HexNumber)))
                .AddField(ReplyMessage.SettingsPrefixFieldName.Parse(language: guildLanguage, Emote.List.Display()),
                    ReplyMessage.SettingsPrefixFieldDesc.Parse(language: guildLanguage, guildPrefix) +
                    $"\n{Emote.Blank.Display()}")
                .AddField(ReplyMessage.SettingsEmbedColorFieldName.Parse(language: guildLanguage, Emote.List.Display()),
                    ReplyMessage.SettingsEmbedColorFieldDesc.Parse(language: guildLanguage,
                        guildColor, guildPrefix) +
                    $"\n{Emote.Blank.Display()}")
                .AddField(ReplyMessage.SettingsLanguageFieldName.Parse(language: guildLanguage, Emote.List.Display()),
                    ReplyMessage.SettingsLanguageFieldDesc.Parse(language: guildLanguage,
                        guildLanguage.Localize(), guildPrefix, availableLanguages));

            await ReplyAsync(null, false, embed.Build());
        }

        [Command("prefix"), Alias("префикс")]
        public async Task SetPrefixTask(string newPrefix)
        {
            var guildLanguage = await _discordGuildService.GetGuildLanguage((long) Context.Guild.Id);

            await _discordGuildService.UpdateGuildPrefix((long) Context.Guild.Id, newPrefix);
            await ReplyAsync(ReplyMessage.SetPrefixSuccess.Parse(language: guildLanguage,
                Context.User.Mention, newPrefix));
        }

        [Command("language"), Alias("язык")]
        public async Task SetLanguageTask(Language newLanguage)
        {
            await _discordGuildService.UpdateGuildLanguage((long) Context.Guild.Id, newLanguage);
            await ReplyAsync(ReplyMessage.SetLanguageSuccess.Parse(language: newLanguage,
                Context.User.Mention, newLanguage.Localize()));
        }

        [Command("color"), Alias("цвет")]
        public async Task SetColorTask(string newColor)
        {
            var guildLanguage = await _discordGuildService.GetGuildLanguage((long) Context.Guild.Id);

            if (newColor.StartsWith("#")) newColor = newColor.Remove(0, 1);

            await _discordGuildService.UpdateGuildColor((long) Context.Guild.Id, newColor);
            await ReplyAsync(ReplyMessage.SetColorSuccess.Parse(language: guildLanguage,
                Context.User.Mention, newColor));
        }
    }
}
