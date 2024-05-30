using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using TscZebra.Plugin.Abstractions.Common;
using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Abstractions.Exceptions;
using TscZebra.Plugin.Abstractions.Messages;
using TscZebra.Plugin.Common;
using TscZebra.Plugin.Validators.State;

namespace TscZebra.Plugin.Features;

internal abstract class PrinterBase(IPAddress ip, int port) : IZplPrinter
{
    private Timer? StatusTimer { get; set; }
    protected TcpClient TcpClient { get; private set; } = new();
    protected PrinterStatuses Status { get; set; } = PrinterStatuses.IsDisconnected;

    #region Public
    
    #region Connect

    public async Task ConnectAsync()
    {
        try
        {
            Disconnect();
            TcpClient = new() { ReceiveTimeout = 200 };

            Task connectTask = TcpClient.ConnectAsync(ip, port);
            Task timeoutTask = Task.Delay(200);

            if (await Task.WhenAny(connectTask, timeoutTask) == timeoutTask)
            {
                connectTask.Dispose();
                throw new PrinterConnectionException();
            }
            
            await connectTask;
            Status = PrinterStatuses.Ready;
            await RequestStatusAsync();
        }
        catch
        {
            Disconnect();
            throw new PrinterConnectionException();
        }
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

        async void Callback(object? _)
        {
            try
            {
                if (Status == PrinterStatuses.IsDisconnected)
                {
                    StopStatusPolling();
                    return;
                }
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

    public abstract Task<PrinterStatuses> RequestStatusAsync();
    
    public async Task PrintZplAsync(string zpl)
    {
        if (!new IsPrinterPrintReady().Validate(Status))
            throw new PrinterStatusException();
        
        if (!zpl.StartsWith("^XA") || !zpl.EndsWith("^XZ"))
            throw new PrinterCommandBodyException();

        try
        {    
            Stream stream = TcpClient.GetStream();
            byte[] commandBytes = Encoding.UTF8.GetBytes(zpl);
            await stream.WriteAsync(commandBytes);
        }
        catch
        {
            Disconnect();
            throw new PrinterConnectionException();
        }
    }

    #endregion
    
    public void Dispose() => Disconnect();

    #endregion

    #region Private

    protected void SetStatus(PrinterStatuses state)
    {
        Status = state;
        WeakReferenceMessenger.Default.Send(new PrinterStatusMsg(Status));
    }
    
    protected async Task<T> ExecuteCommand<T>(BaseCommand<T> command, BaseValidator<PrinterStatuses> stateValidator)
    {
        if (!stateValidator.Validate(Status))
            throw new PrinterStatusException();
        try
        {
            return await command.RequestAsync(TcpClient.GetStream());
        }
        catch (Exception)
        {
            Disconnect();
            throw new PrinterConnectionException();
        }
    }

    
    #endregion
}