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
    public class HelpCommand : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordGuildService _discordGuildService;

        public HelpCommand(IDiscordGuildService discordGuildService)
        {
            _discordGuildService = discordGuildService;
        }

        [Command("help"), Alias("помощь")]
        public async Task HelpTask()
        {
            var embed = new EmbedBuilder();
            var reactions = Enum.GetValues(typeof(Reaction)).Cast<Reaction>();
            var language = Language.English;
            var prefix = "..";
            var color = "36393F";

            if (Context.Guild is not null)
            {
                language = await _discordGuildService.GetGuildLanguage((long) Context.Guild.Id);
                prefix = await _discordGuildService.GetGuildPrefix((long) Context.Guild.Id);
                color = await _discordGuildService.GetGuildColor((long) Context.Guild.Id);
            }
            else
            {
                embed.WithFooter(ReplyMessage.HelpFooter.Parse(language: language));
            }

            var availableReactions = reactions.Aggregate(string.Empty,
                (current, reaction) =>
                    current +
                    $"{(language == Language.English ? reaction.ToString().ToLower() : reaction.Name())}, ");

            embed
                .WithColor(new Color(uint.Parse(color, NumberStyles.HexNumber)))
                .AddField(ReplyMessage.HelpHowToEditBotSettingsFieldName.Parse(language: language,
                        Emote.PineappleThinking.Display()),
                    ReplyMessage.HelpHowToEditBotSettingsFieldDesc.Parse(language: language,
                        Emote.PineappleReading.Display(), prefix,
                        language == Language.English ? "settings" : "настройки") +
                    $"\n{Emote.Blank.Display()}")
                .AddField(ReplyMessage.HelpHowToSendReactionsFieldName.Parse(language: language,
                        Emote.PineappleThinking.Display()),
                    ReplyMessage.HelpHowToSendReactionsFieldDesc.Parse(language: language,
                        Emote.PineappleReading.Display(), prefix,
                        language == Language.English ? "reaction name" : "название реакции",
                        availableReactions.Remove(availableReactions.Length - 2)) +
                    $"\n{Emote.Blank.Display()}")
                .AddField(ReplyMessage.HelpHowToInviteBotFieldName.Parse(language: language,
                        Emote.PineappleThinking.Display()),
                    ReplyMessage.HelpHowToInviteBotFieldDesc.Parse(language: language,
                        Emote.PineappleLove.Display()));

            await ReplyAsync(null, false, embed.Build());
        }
    }
}
