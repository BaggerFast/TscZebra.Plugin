using System.Net;
using System.Threading.Tasks;
using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Features.Zebra.Commands.Status;

namespace TscZebra.Plugin.Features.Zebra;

internal sealed class ZebraPrinter(IPAddress ip, int port) : PrinterBase(ip, port)
{
    public override async Task<PrinterStatuses> RequestStatusAsync()
    {
        return await new ZebraGetStatusCmd().RequestAsync(TcpClient.GetStream());
    }
}