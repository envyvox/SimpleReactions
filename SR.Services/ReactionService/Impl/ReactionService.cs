using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using SR.Data.Enums;
using SR.Framework.Autofac;
using SR.Framework.Database;
using SR.Services.DiscordServices.DiscordClientService.Options;

namespace SR.Services.ReactionService.Impl
{
    [InjectableService]
    public class ReactionService : IReactionService
    {
        private readonly IConnectionManager _con;
        private readonly IOptions<DiscordClientOptions> _options;

        public ReactionService(IConnectionManager con, IOptions<DiscordClientOptions> options)
        {
            _con = con;
            _options = options;
        }

        public async Task UploadImagesFromDiscord(DiscordSocketClient socketClient)
        {
            var guild = socketClient.GetGuild(_options.Value.UploadGuildId);

            foreach (var reaction in Enum.GetValues(typeof(Reaction))
                .Cast<Reaction>())
            {
                var reactionChannel = guild.TextChannels.First(x => x.Name == reaction.Name());
                var messages = await reactionChannel.GetMessagesAsync().FlattenAsync();
                var urls = messages.Select(message => message.Attachments.First().Url).ToArray();

                await AddReactionUrlsToDb(reaction, urls);
            }
        }

        public async Task<string> GetRandomReactionImage(Reaction type)
        {
            return await _con.GetConnection()
                .QueryFirstOrDefaultAsync<string>(@"
                    select url from reactions
                    where type = @type
                    order by random()
                    limit 1",
                    new {type});
        }

        private async Task AddReactionUrlsToDb(Reaction type, string[] urls)
        {
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into reactions(type, url)
                    values (@type, unnest(array[@urls]))
                    on conflict (type, url) do update
                        set updated_at = now()",
                    new {type, urls});
        }
    }
}
