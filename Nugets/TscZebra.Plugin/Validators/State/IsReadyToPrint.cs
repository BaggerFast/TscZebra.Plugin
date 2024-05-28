using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Common;

namespace TscZebra.Plugin.Validators.State;

public class IsPrinterPrintReady : BaseValidator<PrinterStatuses>
{
    public override bool Validate(PrinterStatuses item) => item is PrinterStatuses.Ready or PrinterStatuses.Busy;
}