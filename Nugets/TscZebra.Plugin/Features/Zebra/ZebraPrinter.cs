using System.Net;

namespace TscZebra.Plugin.Features.Zebra;

internal class ZebraPrinter(IPAddress ip, int port) : PrinterBase(ip, port);