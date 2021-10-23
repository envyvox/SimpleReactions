using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;
using SR.Data.Enums;
using SR.Services.Discord.Guild.Queries;
using SR.Services.Reaction.Queries;

namespace SR.Services.Discord.Client.SlashCommands
{
    public record ReactionCommand(SocketSlashCommand Command) : IRequest;

    public class ReactionCommandHandler : IRequestHandler<ReactionCommand>
    {
        private readonly IMediator _mediator;

        public ReactionCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ReactionCommand request, CancellationToken ct)
        {
            var reaction = request.Command.Data.Name switch
            {
                "shoot" or "выстрелить" => ReactionType.Shoot,
                "bite" or "укусить" => ReactionType.Bite,
                "hug" or "обнять" => ReactionType.Hug,
                "kiss" or "поцеловать" => ReactionType.Kiss,
                "sleep" or "спатки" => ReactionType.Sleep,
                "knock" or "стукнуть" => ReactionType.Knock,
                "pat" or "погладить" => ReactionType.Pat,
                "hello" or "приветствовать" => ReactionType.Hello,
                "poke" or "тыкнуть" => ReactionType.Poke,
                "drink" or "выпить" => ReactionType.Drink,
                "love" or "люблю" => ReactionType.Love,
                "play" or "играть" => ReactionType.Play,
                "angry" or "злюсь" => ReactionType.Angry,
                "blush" or "краснею" => ReactionType.Blush,
                "laugh" or "смеюсь" => ReactionType.Laugh,
                "sad" or "грущу" => ReactionType.Sad,
                "cry" or "плачу" => ReactionType.Cry,
                "shock" or "шок" => ReactionType.Shock,
                "happy" or "радуюсь" => ReactionType.Happy,
                "facepalm" or "фейспалм" => ReactionType.Facepalm,
                _ => throw new ArgumentOutOfRangeException()
            };
            var args = request.Command.Data.Options.FirstOrDefault();

            var randomImageUrl = await _mediator.Send(new GetRandomReactionImageUrlQuery(reaction));
            var channel = (SocketTextChannel) request.Command.Channel;
            var guild = await _mediator.Send(new GetDiscordGuildQuery((long) channel.Guild.Id));

            var embed = new EmbedBuilder()
                .WithColor(new Color(uint.Parse(guild.EmbedColor, NumberStyles.HexNumber)))
                .WithDescription(
                    reaction.Message(guild.LanguageType, request.Command.User.Mention,
                        args is null
                            ? ""
                            : (string) args.Value))
                .WithImageUrl(randomImageUrl)
                .Build();

            await request.Command.FollowupAsync(embed: embed);

            return Unit.Value;
        }
    }
}
