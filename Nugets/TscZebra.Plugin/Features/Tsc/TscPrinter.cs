using System.Net;
using System.Threading.Tasks;
using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Features.Tsc.Commands;

namespace TscZebra.Plugin.Features.Tsc;

internal sealed class TscPrinter(IPAddress ip, int port) : PrinterBase(ip, port)
{
    public override async Task<PrinterStatuses> RequestStatusAsync()
    {
        if (Status is PrinterStatuses.IsDisconnected)
            return Status;
        PrinterStatuses stat = await new TscGetStatusCmd().RequestAsync(TcpClient.GetStream());
        SetStatus(stat);
        return stat;
    }
};