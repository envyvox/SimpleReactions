using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SR.Data;
using SR.Data.Enums;
using SR.Data.Extensions;

namespace SR.Services.Discord.Guild.Commands
{
    public record UpdateGuildLanguageCommand(long GuildId, LanguageType LanguageType) : IRequest;

    public class UpdateGuildLanguageHandler : IRequestHandler<UpdateGuildLanguageCommand>
    {
        private readonly AppDbContext _db;

        public UpdateGuildLanguageHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(UpdateGuildLanguageCommand request, CancellationToken ct)
        {
            var entity = await _db.DiscordGuilds
                .SingleOrDefaultAsync(x => x.Id == request.GuildId);

            entity.LanguageType = request.LanguageType;

            await _db.UpdateEntity(entity);

            return Unit.Value;
        }
    }
}
