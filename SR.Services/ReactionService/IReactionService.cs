using System.Threading.Tasks;
using Discord.WebSocket;
using SR.Data.Enums;

namespace SR.Services.ReactionService
{
    public interface IReactionService
    {
        Task UploadImagesFromDiscord(DiscordSocketClient socketClient);
        Task<string> GetRandomReactionImage(Reaction type);
    }
}
