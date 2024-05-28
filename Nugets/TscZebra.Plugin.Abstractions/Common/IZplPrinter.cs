using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Abstractions.Exceptions;

namespace TscZebra.Plugin.Abstractions.Common;

public interface IZplPrinter : IDisposable
{
    #region Connect
    
    /// <summary>
    /// Asynchronously establishes a tcp connection to the printer.
    /// </summary>
    /// <returns>A task that represents the asynchronous connect operation.</returns>
    public Task ConnectAsync();
    
    public void Disconnect();

    #endregion
    
    #region Polling status

    public void StartStatusPolling(short secs = 30);
    public void StopStatusPolling();

    #endregion

    #region Commands

    
    /// <summary>
    /// Asynchronously sends ZPL (Zebra Programming Language) code to the printer for printing
    /// </summary>
    /// <param name="zpl">The ZPL code to be printed.</param>
    /// <returns>A task that represents the asynchronous print operation.</returns>
    /// <exception cref="PrinterCommandBodyException">Thrown when the provided ZPL code is invalid.</exception>
    /// <exception cref="PrinterStatusException">Thrown when the printer status is not ready or busy</exception>
    /// <exception cref="PrinterConnectionException">Thrown when the connection to the printer is lost.</exception>
    public Task PrintZplAsync(string zpl);

    
    public Task<PrinterStatuses> RequestStatusAsync();

    #endregion
}