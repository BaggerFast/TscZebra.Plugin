using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using TscZebra.Plugin.Abstractions.Common;
using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Messages;

namespace TscZebra.Plugin.Features;

internal abstract class PrinterBase(IPAddress ip, int port) : IZplPrinter
{
    private Timer? StatusTimer { get; set; }
    protected TcpClient TcpClient { get; private set; } = new();
    protected PrinterStatuses Status { get; private set; } = PrinterStatuses.IsDisconnected;

    #region Public
    
    #region Connect

    public async Task ConnectAsync()
    {
        TcpClient.Dispose();
        TcpClient = new() { ReceiveTimeout = 200 };
        await TcpClient.ConnectAsync(ip, port).WaitAsync(TimeSpan.FromMilliseconds(100));
        SetStatus(PrinterStatuses.Ready);
    }

    public void Disconnect()
    {
        TcpClient.Dispose();
        StopStatusPolling();
        SetStatus(PrinterStatuses.IsDisconnected);
    }
    
    #endregion

    #region StatusPolling

    public void StartStatusPolling(short secs = 30)
    {
        TimeSpan interval = TimeSpan.FromSeconds(secs);

        StatusTimer = new(Callback, null, TimeSpan.Zero, interval);
        return;

        async void Callback(object? _) => await RequestStatusAsync();
    }

    public void StopStatusPolling()
    {
        StatusTimer?.Dispose();
        StatusTimer = null;
    }

    #endregion

    #region Commands

    public abstract Task<PrinterStatuses> RequestStatusAsync();

    #endregion
    
    public void Dispose() => Disconnect();

    #endregion

    #region Private

    protected void SetStatus(PrinterStatuses state)
    {
        Status = state;
        WeakReferenceMessenger.Default.Send(new PrinterStatusMsg(Status));
    }
    
    // protected async Task<TResult?> ExecuteCmd<TResult>(Func<Task<TResult>> command) where TResult : class
    // {
    //     if (Status is PrinterStatuses.IsDisconnected) return null;
    //     
    //     try
    //     {
    //         await command();
    //     }
    //     catch (Exception)
    //     {
    //         SetStatus(Prit.IsForceDisconnected);
    //     }
    // }

    
    #endregion
}