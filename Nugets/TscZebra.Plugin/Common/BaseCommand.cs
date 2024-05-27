using System.Net.Sockets;
using System.Text;

namespace TscZebra.Plugin.Common;

public abstract class BaseCommand<T> (string command)
{
    public virtual T Request(NetworkStream stream)
    {
        byte[] commandBytes = Encoding.UTF8.GetBytes(command);
        stream.Write(commandBytes, 0, commandBytes.Length);
        return Response(stream);
    }
    protected abstract T Response(NetworkStream stream);
}