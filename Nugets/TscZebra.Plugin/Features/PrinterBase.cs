using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using TscZebra.Plugin.Abstractions.Common;
using TscZebra.Plugin.Abstractions.Enums;

namespace TscZebra.Plugin.Features;

internal abstract class PrinterBase(IPAddress ip, int port) : IZplPrinter
{
    protected TcpClient TcpClient { get; set; } = new();
    protected PrinterStatuses Status { get; set; } = PrinterStatuses.IsDisabled;

    #region Public

    public async Task ConnectAsync()
    {
        TcpClient.Dispose();
        TcpClient = new() { ReceiveTimeout = 200 };
        await TcpClient.ConnectAsync(ip, port).WaitAsync(TimeSpan.FromMilliseconds(100));
    }
    public void Disconnect() => TcpClient.Dispose();

    public PrinterStatuses RequestStatus()
    {
        throw new NotImplementedException();
    }
    
    public void Dispose() => Disconnect();

    #endregion
}