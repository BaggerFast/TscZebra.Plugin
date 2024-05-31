namespace TscZebra.Plugin.Abstractions.Enums;

/// <summary>
/// Represents the possible statuses of a printer.
/// </summary>
public enum PrinterStatuses
{
    /// <summary>
    /// The printer has unsupported status by developer.
    /// </summary>
    Unsupported,

    /// <summary>
    /// The printer is paused.
    /// </summary>
    Paused,

    /// <summary>
    /// The printer is ready.
    /// </summary>
    Ready,

    /// <summary>
    /// The printer's head is open.
    /// </summary>
    HeadOpen,

    /// <summary>
    /// The printer is out of paper.
    /// </summary>
    PaperOut,

    /// <summary>
    /// The printer has a paper jam.
    /// </summary>
    PaperJam,

    /// <summary>
    /// The printer is busy.
    /// </summary>
    Busy,

    /// <summary>
    /// The printer is out of ribbon.
    /// </summary>
    RibbonOut,

    /// <summary>
    /// The printer is disconnected.
    /// </summary>
    IsDisconnected,
}
