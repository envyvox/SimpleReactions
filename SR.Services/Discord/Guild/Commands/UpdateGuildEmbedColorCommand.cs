using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SR.Data;
using SR.Data.Extensions;

namespace SR.Services.Discord.Guild.Commands
{
    public record UpdateGuildEmbedColorCommand(long GuildId, string EmbedColor) : IRequest;

    public class UpdateGuildEmbedHandler : IRequestHandler<UpdateGuildEmbedColorCommand>
    {
        private readonly AppDbContext _db;

        public UpdateGuildEmbedHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(UpdateGuildEmbedColorCommand request, CancellationToken ct)
        {
            var entity = await _db.DiscordGuilds
                .SingleOrDefaultAsync(x => x.Id == request.GuildId);

            entity.EmbedColor = request.EmbedColor;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}
