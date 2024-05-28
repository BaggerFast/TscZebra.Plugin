using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Common;
using TscZebra.Plugin.Features.Tsc.Constants;

namespace TscZebra.Plugin.Features.Tsc.Commands;

internal sealed class TscGetStatusCmd() : BaseCommand<PrinterStatuses>(TscCommandConsts.GetStatus)
{
    protected override async Task<PrinterStatuses> ResponseAsync(NetworkStream stream)
    {
        byte[] buffer = new byte[1];
        int bytesRead = await stream.ReadAsync(buffer.AsMemory(0, 1));

        if (bytesRead == 0)
            return PrinterStatuses.Unsupported;

        return buffer[0] switch
        {
            0x00 => PrinterStatuses.Ready,
            0x01 => PrinterStatuses.HeadOpen,
            0x02 => PrinterStatuses.PaperJam,
            0x04 => PrinterStatuses.PaperOut,
            0x08 => PrinterStatuses.RibbonOut,
            0x10 => PrinterStatuses.Paused,
            0x20 => PrinterStatuses.Busy,
            _ => PrinterStatuses.Unsupported
        };
    }
}