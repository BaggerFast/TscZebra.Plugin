using System.Net.Sockets;
using System.Text;

namespace TscZebra.Plugin.Common;

internal abstract class Command<T> (string command)
{
    public async Task<T> RequestAsync(NetworkStream stream, CancellationToken token)
    {
        byte[] commandBytes = Encoding.UTF8.GetBytes(command);
        await stream.WriteAsync(commandBytes, token);
        return await ResponseAsync(stream, token);
    }
    
    protected abstract Task<T> ResponseAsync(NetworkStream stream, CancellationToken token);
}