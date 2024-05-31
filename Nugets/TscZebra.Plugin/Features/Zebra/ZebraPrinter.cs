using System.Net;
using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Features.Zebra.Commands.Status;
using TscZebra.Plugin.Validators.State;

namespace TscZebra.Plugin.Features.Zebra;

internal sealed class ZebraPrinter(IPAddress ip, int port) : PrinterBase(ip, port)
{
    public override async Task<PrinterStatuses> RequestStatusAsync()
    {
        if (Status == PrinterStatuses.IsDisconnected) return PrinterStatuses.IsDisconnected;
        PrinterStatuses stat = await ExecuteCommand(new ZebraGetStatusCmd(), new IsPrinterConnected());
        SetStatus(stat);
        return Status;
    }
}