using System.Net.Sockets;
using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Abstractions.Exceptions;
using TscZebra.Plugin.Common;

namespace TscZebra.Plugin.Features;

internal abstract partial class PrinterBase
{
    private Timer? StatusTimer { get; set; }
    private TcpClient TcpClient { get; set; } = new();
    private PrinterStatus Status { get; set; } = PrinterStatus.Disconnected;

    private void SetStatus(PrinterStatus status)
    {
        if (status == Status) return;
        Status = status;
        OnStatusChanged?.Invoke(this, Status);
    }

    private async Task<T> ExecuteCommand<T>(Command<T> command)
    {
        if (TcpClient.Connected == false)
            throw new PrinterConnectionException();
        
        try
        {
            using CancellationTokenSource cancellationTokenSource = new(TimeSpan.FromMilliseconds(1000));
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            
            return await command.RequestAsync(TcpClient.GetStream(), cancellationToken);
        }
        catch (Exception)
        {
            Disconnect();
            throw new PrinterConnectionException();
        }
    }
    
    protected abstract Command<PrinterStatus> GetStatusCommand { get; }
}