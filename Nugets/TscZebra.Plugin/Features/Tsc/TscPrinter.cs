using System.Net;

namespace TscZebra.Plugin.Features.Tsc;

internal class TscPrinter(IPAddress ip, int port) : PrinterBase(ip, port);