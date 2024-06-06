using System.Net;
using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Common;
using TscZebra.Plugin.Features.Tsc.Commands;

namespace TscZebra.Plugin.Features.Tsc;

internal sealed class TscPrinter(IPAddress ip, int port) : PrinterBase(ip, port)
{
    protected override Command<PrinterStatus> GetStatusCommand => new TscGetStatusCommand();
}