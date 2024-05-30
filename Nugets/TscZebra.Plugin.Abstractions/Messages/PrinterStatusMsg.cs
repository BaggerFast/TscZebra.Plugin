using TscZebra.Plugin.Abstractions.Enums;

namespace TscZebra.Plugin.Abstractions.Messages;

public class PrinterStatusMsg(PrinterStatuses status)
{
    public PrinterStatuses Status { get; } = status;
}