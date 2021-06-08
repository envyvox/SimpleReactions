using System.Threading.Tasks;
using Discord.Commands;
using SR.Data.Enums;
using SR.Services.DiscordServices.DiscordEmbedService;
using SR.Services.DiscordServices.DiscordGuildService;
using SR.Services.ReactionService;

namespace SR.Services.Commands
{
    [RequireContext(ContextType.Guild)]
    public class ReactionCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IReactionService _reactionService;
        private readonly IDiscordGuildService _discordGuildService;

        public ReactionCommands(IDiscordEmbedService discordEmbedService, IReactionService reactionService,
            IDiscordGuildService discordGuildService)
        {
            _discordEmbedService = discordEmbedService;
            _reactionService = reactionService;
            _discordGuildService = discordGuildService;
        }

        [Command("shoot"), Alias("выстрелить")]
        public async Task ReactionShootTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Shoot, userInput);

        [Command("bite"), Alias("укусить")]
        public async Task ReactionBiteTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Bite, userInput);

        [Command("hug"), Alias("обнять")]
        public async Task ReactionHugTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Hug, userInput);

        [Command("kiss"), Alias("поцеловать")]
        public async Task ReactionKissTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Kiss, userInput);

        [Command("sleep"), Alias("спатки")]
        public async Task ReactionSleepTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Sleep, userInput);

        [Command("knock"), Alias("стукнуть")]
        public async Task ReactionKnockTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Knock, userInput);

        [Command("pat"), Alias("погладить")]
        public async Task ReactionPatTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Pat, userInput);

        [Command("hello"), Alias("приветствовать")]
        public async Task ReactionHelloTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Hello, userInput);

        [Command("poke"), Alias("тыкнуть")]
        public async Task ReactionPokeTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Poke, userInput);

        [Command("drink"), Alias("выпить")]
        public async Task ReactionDrinkTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Drink, userInput);

        [Command("love"), Alias("люблю")]
        public async Task ReactionLoveTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Love, userInput);

        [Command("play"), Alias("играть")]
        public async Task ReactionPlayTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Play, userInput);

        [Command("angry"), Alias("злюсь")]
        public async Task ReactionAngryTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Angry, userInput);

        [Command("blush"), Alias("краснею")]
        public async Task ReactionBlushTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Blush, userInput);

        [Command("laugh"), Alias("смеюсь")]
        public async Task ReactionLaughTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Laugh, userInput);

        [Command("sad"), Alias("грущу")]
        public async Task ReactionSadTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Sad, userInput);

        [Command("cry"), Alias("плачу")]
        public async Task ReactionCryTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Cry, userInput);

        [Command("shock"), Alias("шок")]
        public async Task ReactionShockTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Shock, userInput);

        [Command("happy"), Alias("радуюсь")]
        public async Task ReactionHappyTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Happy, userInput);

        [Command("facepalm"), Alias("фейспалм")]
        public async Task ReactionFacepalmTask([Remainder] string userInput = "") =>
            await SendReactionEmbed(Reaction.Facepalm, userInput);

        private async Task SendReactionEmbed(Reaction reaction, string userInput = "")
        {
            var guildColor = await _discordGuildService.GetGuildColor((long) Context.Guild.Id);
            var guildLanguage = await _discordGuildService.GetGuildLanguage((long) Context.Guild.Id);
            var randomImage = await _reactionService.GetRandomReactionImage(reaction);
            var message = reaction.Message(guildLanguage, Context.User.Mention, userInput);
            var embed = _discordEmbedService.BuildReactionEmbed(guildColor, message, randomImage);
            await ReplyAsync(null, false, embed);
        }
    }
}
