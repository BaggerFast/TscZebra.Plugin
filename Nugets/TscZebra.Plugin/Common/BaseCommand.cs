using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TscZebra.Plugin.Common;

public abstract class BaseCommand<T> (string command)
{
    public async Task<T> RequestAsync(NetworkStream stream)
    {
        byte[] commandBytes = Encoding.UTF8.GetBytes(command);
        await stream.WriteAsync(commandBytes);
        return await ResponseAsync(stream);
    }
    protected abstract Task<T> ResponseAsync(NetworkStream stream);
}