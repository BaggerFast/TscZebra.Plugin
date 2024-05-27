using TscZebra.Plugin.Abstractions.Enums;

namespace TscZebra.Plugin.Abstractions.Common;

public interface IZplPrinter : IDisposable
{
    public Task ConnectAsync();
    
    public void Disconnect();
    public PrinterStatuses RequestStatus();
}