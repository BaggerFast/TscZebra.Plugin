using System.Net;
using TscZebra.Plugin.Abstractions;
using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Abstractions.Exceptions;
using TscZebra.Plugin.Common;
using TscZebra.Plugin.Misc;
using TscZebra.Plugin.Shared.Commands;

namespace TscZebra.Plugin.Features;

internal abstract partial class PrinterBase(IPAddress ip, int port) : IZplPrinter
{
    public event EventHandler<PrinterStatus>? OnStatusChanged;
    
    #region Connect
    
    public async Task ConnectAsync()
    {
        try
        {
            Disconnect();

            using CancellationTokenSource cancellationTokenSource = new(TimeSpan.FromMilliseconds(500));
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            
            TcpClient = new() { ReceiveTimeout = 500, SendTimeout = 500 };
            await TcpClient.ConnectAsync(ip, port, cancellationToken);

            SetStatus(PrinterStatus.Ready);
        }
        catch (Exception)
        {
            throw new PrinterConnectionException();
        }
    }

    public void Disconnect()
    {
        SetStatus(PrinterStatus.Disconnected);
        StopStatusPolling();
        
        TcpClient.Dispose();
    }
    
    public void Dispose() => Disconnect();
    
    #endregion

    #region StatusPolling

    public void StartStatusPolling(ushort secs = 5)
    {
        if (secs < 3)
            throw new ArgumentOutOfRangeException(nameof(secs), "The polling interval must be at least 5 seconds.");
        
        TimeSpan interval = TimeSpan.FromSeconds(secs);

        StatusTimer = new(Callback, null, TimeSpan.Zero, interval);
        return;

        async void Callback(object? _)
        {
            try
            {
                if (Status is PrinterStatus.Disconnected)
                    StopStatusPolling();
                else
                    await RequestStatusAsync();
            }
            catch
            {
                StopStatusPolling();
            }
        }
    }

    public void StopStatusPolling()
    {
        StatusTimer?.Dispose();
        StatusTimer = null;
    }

    #endregion

    #region Commands
    
    public async Task PrintZplAsync(string zpl)
    { 
        await RequestStatusAsync();
        
        if (Status is not (PrinterStatus.Ready or PrinterStatus.Busy))
            throw new PrinterStatusException();
        
        Command<VoidType> printCmd = new PrintCommand(zpl); 
        await ExecuteCommand(printCmd);
    }

    public async Task<PrinterStatus> RequestStatusAsync()
    {
        if (Status is PrinterStatus.Disconnected)
            return Status;
        
        PrinterStatus stat = await ExecuteCommand(GetStatusCommand);
        SetStatus(stat);
        return Status;
    }

    #endregion
}