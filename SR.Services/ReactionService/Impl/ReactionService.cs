using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SR.Data;
using SR.Data.Enums;
using SR.Framework.Autofac;
using SR.Services.DiscordServices.DiscordClientService.Options;

namespace SR.Services.ReactionService.Impl
{
    [InjectableService]
    public class ReactionService : IReactionService
    {
        private readonly IOptions<DiscordClientOptions> _options;
        private readonly AppDbContext _db;
        private readonly Random _random = new();

        public ReactionService(IOptions<DiscordClientOptions> options, AppDbContext db)
        {
            _options = options;
            _db = db;
        }

        public async Task UploadImagesFromDiscord(DiscordSocketClient socketClient)
        {
            var guild = socketClient.GetGuild(_options.Value.UploadGuildId);

            foreach (var reaction in Enum.GetValues(typeof(Reaction))
                .Cast<Reaction>())
            {
                var reactionChannel = guild.TextChannels.First(x => x.Name == reaction.Name());
                var messages = await reactionChannel.GetMessagesAsync().FlattenAsync();
                var urls = messages.Select(message => message.Attachments.First().Url);

                await AddReactionUrlsToDb(reaction, urls);
            }
        }

        public async Task<string> GetRandomReactionImage(Reaction type)
        {
            return await _db.Reactions
                .AsQueryable()
                .Where(x => x.Type == type)
                .OrderBy(x => Guid.NewGuid()) // makes order by random, db must have uuid-ossp extension
                .Take(1)
                .Select(x => x.Url)
                .FirstOrDefaultAsync();
        }

        private async Task AddReactionUrlsToDb(Reaction type, IEnumerable<string> urls)
        {
            var newReactions = urls
                .Select(url => new Data.Models.Reaction {Type = type, Url = url})
                .ToArray();

            foreach (var newReaction in newReactions)
            {
                var entity = await _db.Reactions
                    .AsQueryable()
                    .Where(x => x.Type == newReaction.Type && x.Url == newReaction.Url)
                    .SingleOrDefaultAsync();

                if (entity is null)
                {
                    await _db.Reactions.AddAsync(newReaction);
                }
                else
                {
                    entity.UpdatedAt = DateTimeOffset.Now;
                }

                await _db.SaveChangesAsync();
            }
        }
    }
}
