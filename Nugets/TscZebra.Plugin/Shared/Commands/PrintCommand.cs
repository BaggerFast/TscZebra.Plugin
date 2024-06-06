using System.Net.Sockets;
using TscZebra.Plugin.Abstractions.Exceptions;
using TscZebra.Plugin.Common;
using TscZebra.Plugin.Misc;

namespace TscZebra.Plugin.Shared.Commands;

internal sealed class PrintCommand : Command<VoidType>
{
    public PrintCommand(string zpl) : base(zpl)
    {
        if (!(zpl.StartsWith("^XA") && zpl.EndsWith("^XZ")))
            throw new PrinterCommandBodyException();
    }

    protected override Task<VoidType> ResponseAsync(NetworkStream stream, CancellationToken token) 
        => Task.FromResult(new VoidType());
}