using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SR.Data;
using SR.Data.Extensions;

namespace SR.Services.Discord.Guild.Commands
{
    public record DeleteDiscordGuildCommand(long GuildId) : IRequest;

    public class DeleteDiscordGuildHandler : IRequestHandler<DeleteDiscordGuildCommand>
    {
        private readonly AppDbContext _db;

        public DeleteDiscordGuildHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(DeleteDiscordGuildCommand request, CancellationToken ct)
        {
            var entity = await _db.DiscordGuilds
                .SingleOrDefaultAsync(x => x.Id == request.GuildId);

            if (entity is null)
            {
                throw new Exception($"discord guild with id {request.GuildId} not found");
            }

            await _db.DeleteEntity(entity);

            return Unit.Value;
        }
    }
}
