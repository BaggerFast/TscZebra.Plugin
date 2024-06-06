using System.Net.Sockets;
using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Common;
using TscZebra.Plugin.Features.Tsc.Constants;

namespace TscZebra.Plugin.Features.Tsc.Commands;

internal sealed class TscGetStatusCommand() : Command<PrinterStatus>(TscCommandConsts.GetStatus)
{
    protected override async Task<PrinterStatus> ResponseAsync(NetworkStream stream, CancellationToken token)
    {
        byte[] buffer = new byte[1];
        int bytesRead = await stream.ReadAsync(buffer.AsMemory(0, 1), token);

        if (bytesRead == 0)
            return PrinterStatus.Unsupported;

        return buffer[0] switch
        {
            0x00 => PrinterStatus.Ready,
            0x01 => PrinterStatus.HeadOpen,
            0x02 => PrinterStatus.PaperJam,
            0x04 => PrinterStatus.PaperOut,
            0x08 => PrinterStatus.RibbonOut,
            0x10 => PrinterStatus.Paused,
            0x20 => PrinterStatus.Busy,
            _ => PrinterStatus.Unsupported
        };
    }
}