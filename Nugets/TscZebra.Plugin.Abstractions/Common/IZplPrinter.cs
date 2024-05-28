using TscZebra.Plugin.Abstractions.Enums;

namespace TscZebra.Plugin.Abstractions.Common;

public interface IZplPrinter : IDisposable
{
    #region Connect
    
    public Task ConnectAsync();
    public void Disconnect();

    #endregion
    
    #region Polling status

    public void StartStatusPolling(short secs = 30);
    public void StopStatusPolling();

    #endregion

    #region Commands

    public Task<PrinterStatuses> RequestStatusAsync();

    #endregion
}