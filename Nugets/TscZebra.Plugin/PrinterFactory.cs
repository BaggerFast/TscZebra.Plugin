using System.Net;
using TscZebra.Plugin.Abstractions.Common;
using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Features.Tsc;
using TscZebra.Plugin.Features.Zebra;

namespace TscZebra.Plugin;

public static class PrinterFactory
{
    public static IZplPrinter Create(IPAddress ip, int port, PrinterTypes types) =>
        types switch
        {
            PrinterTypes.Tsc => new TscPrinter(ip, port),
            PrinterTypes.Zebra => new ZebraPrinter(ip, port),
            _ => new TscPrinter(ip, port)
        };
}