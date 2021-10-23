using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SR.Data;
using SR.Data.Enums;
using SR.Data.Extensions;

namespace SR.Services.Reaction.Commands
{
    public record CreateReactionCommand(ReactionType Type, string Url) : IRequest;

    public class CreateReactionHandler : IRequestHandler<CreateReactionCommand>
    {
        private readonly AppDbContext _db;

        public CreateReactionHandler(DbContextOptions options)
        {
            _db = new AppDbContext(options);
        }

        public async Task<Unit> Handle(CreateReactionCommand request, CancellationToken ct)
        {
            var exist = await _db.Reactions
                .AnyAsync(x =>
                    x.Type == request.Type &&
                    x.Url == request.Url);

            if (exist) return Unit.Value;

            await _db.CreateEntity(new Data.Entities.Reaction
            {
                Id = Guid.NewGuid(),
                Type = request.Type,
                Url = request.Url
            });

            return Unit.Value;
        }
    }
}
