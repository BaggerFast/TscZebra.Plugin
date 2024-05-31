using System.Net;
using TscZebra.Plugin.Abstractions.Common;
using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Features.Tsc;
using TscZebra.Plugin.Features.Zebra;

namespace TscZebra.Plugin;

/// <summary>
/// Factory class for creating instances of IZplPrinter
/// </summary>
public static class PrinterFactory
{
    /// <summary>
    /// Creates an instance of a printer based on the specified IP address, port, and printer type.
    /// </summary>
    /// <param name="ip">The IP address of the printer.</param>
    /// <param name="port">The port number used to connect to the printer.</param>
    /// <param name="types">The type of printer to create. This can be either Tsc or Zebra.</param>
    /// <returns>An instance of a class that implements the IZplPrinter interface.</returns>
    /// <exception cref="ArgumentException">Thrown when the specified printer type is not supported.</exception>
    public static IZplPrinter Create(IPAddress ip, int port, PrinterTypes types) =>
        types switch
        {
            PrinterTypes.Tsc => new TscPrinter(ip, port),
            PrinterTypes.Zebra => new ZebraPrinter(ip, port),
            _ => throw new ArgumentException("Unsupported printer type", nameof(types))
        };
}