﻿using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Abstractions.Exceptions;

namespace TscZebra.Plugin.Abstractions;

/// <summary>
/// Represents a ZPL printer interface.
/// </summary>
public interface IZplPrinter : IDisposable
{
    #region Connect
    
    /// <summary>
    /// Asynchronously establishes a TCP connection to the printer.
    /// </summary>
    /// <exception cref="PrinterConnectionException">Thrown when the connection to the printer is lost.</exception>
    public Task ConnectAsync();
    
    /// <summary>
    /// Disconnects the TCP connection from the printer.
    /// </summary>
    public void Disconnect();

    #endregion
    
    #region Polling status

    /// <summary>
    /// Starts polling the printer's status at regular intervals.
    /// </summary>
    /// <param name="secs">The interval, in seconds, for polling the printer's status. The default is 5 seconds.</param>
    /// <remarks>
    /// Subscribes to the <see cref="OnStatusChanged"/> event to receive status updates.
    /// Call <see cref="StopStatusPolling"/> when status monitoring is no longer needed.
    /// If the printer is initially in a "Disabled" state, polling will automatically stop.
    /// </remarks>
    public void StartStatusPolling(ushort secs = 5);

    /// <summary>
    /// Stops polling the printer's status.
    /// </summary>
    /// <remarks>
    /// Stops the periodic status checks initiated by <see cref="StartStatusPolling"/>.
    /// </remarks>
    public void StopStatusPolling();
    
    #endregion

    #region Commands
    
    /// <summary>
    /// Asynchronously sends ZPL (Zebra Programming Language) code to the printer for printing.
    /// </summary>
    /// <param name="zpl">The ZPL code to be printed.</param>
    /// <returns>A task representing the asynchronous print operation.</returns>
    /// <exception cref="PrinterCommandBodyException">Thrown when the provided ZPL code is invalid.</exception>
    /// <exception cref="PrinterStatusException">Thrown when the printer status is not ready or busy.</exception>
    /// <exception cref="PrinterConnectionException">Thrown when the connection to the printer is lost.</exception>
    public Task PrintZplAsync(string zpl);

    /// <summary>
    /// Asynchronously requests the current status of the printer.
    /// </summary>
    /// <returns>
    /// A task with the printer's current status as a <see cref="PrinterStatus"/>.
    /// </returns>
    /// <exception cref="PrinterConnectionException">
    /// Thrown when there is an error retrieving the printer's status.
    /// </exception>
    /// <remarks>
    /// Invokes the <see cref="OnStatusChanged"/> event to receive status updates.
    /// </remarks>
    public Task<PrinterStatus> RequestStatusAsync();

    #endregion
    
    #region Events
    
    /// <summary>
    /// Subscribe to this event to receive notifications about the status of the Printer.
    /// </summary>
    public event EventHandler<PrinterStatus> OnStatusChanged;
    
    #endregion
}