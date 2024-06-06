using System.Net;
using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Common;
using TscZebra.Plugin.Features.Zebra.Commands.Status;

namespace TscZebra.Plugin.Features.Zebra;

internal sealed class ZebraPrinter(IPAddress ip, int port) : PrinterBase(ip, port)
{
    protected override Command<PrinterStatus> GetStatusCommand => new ZebraGetStatusCommand();
}