using System.Net;

namespace TscZebra.Plugin.Features.Zebra;

internal sealed class ZebraPrinter(IPAddress ip, int port) : PrinterBase(ip, port);