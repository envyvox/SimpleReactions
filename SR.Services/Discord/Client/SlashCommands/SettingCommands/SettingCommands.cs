using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using MediatR;

namespace SR.Services.Discord.Client.SlashCommands.SettingCommands
{
    public record SettingCommands(SocketSlashCommand Command) : IRequest;

    public class SettingCommandsHandler : IRequestHandler<SettingCommands>
    {
        private readonly IMediator _mediator;

        public SettingCommandsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SettingCommands request, CancellationToken ct)
        {
            return request.Command.Data.Options.First().Name switch
            {
                "show" => await _mediator.Send(new SettingShowCommand(request.Command)),
                "sync-commands" => await _mediator.Send(new SettingSyncCommandsCommand(request.Command)),
                "update-color" => await _mediator.Send(new SettingUpdateColorCommand(request.Command)),
                "update-language" => await _mediator.Send(new SettingUpdateLanguageCommand(request.Command)),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
