using System.Net;
using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Features.Tsc.Commands;
using TscZebra.Plugin.Validators.State;

namespace TscZebra.Plugin.Features.Tsc;

internal sealed class TscPrinter(IPAddress ip, int port) : PrinterBase(ip, port)
{
    public override async Task<PrinterStatuses> RequestStatusAsync()
    {
        if (Status == PrinterStatuses.IsDisconnected) return PrinterStatuses.IsDisconnected;
        PrinterStatuses stat = await ExecuteCommand(new TscGetStatusCmd(), new IsPrinterConnected());
        SetStatus(stat);
        return Status;
    }
};