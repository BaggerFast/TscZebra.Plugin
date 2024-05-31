using System.Net.Sockets;
using System.Text;

namespace TscZebra.Plugin.Common;

internal abstract class BaseCommand<T> (string command)
{
    public async Task<T> RequestAsync(NetworkStream stream)
    {
        byte[] commandBytes = Encoding.UTF8.GetBytes(command);
        await stream.WriteAsync(commandBytes);
        return await ResponseAsync(stream);
    }
    
    protected abstract Task<T> ResponseAsync(NetworkStream stream);
}