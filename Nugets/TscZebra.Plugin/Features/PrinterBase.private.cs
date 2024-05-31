using System.Net.Sockets;
using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Abstractions.Exceptions;
using TscZebra.Plugin.Common;

namespace TscZebra.Plugin.Features;

internal abstract partial class PrinterBase
{
    private Timer? StatusTimer { get; set; }
    private TcpClient TcpClient { get; set; } = new();
    protected PrinterStatuses Status { get; set; } = PrinterStatuses.IsDisconnected;
    
    public abstract Task<PrinterStatuses> RequestStatusAsync();
    
    public event EventHandler<PrinterStatuses>? PrinterStatusChanged;
    
    protected void SetStatus(PrinterStatuses state)
    {
        Status = state;
        PrinterStatusChanged?.Invoke(this, Status);
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

    public void Dispose() => Disconnect();
}