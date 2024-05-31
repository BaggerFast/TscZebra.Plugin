using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Common;

namespace TscZebra.Plugin.Validators.State;

internal class IsPrinterPrintReady : BaseValidator<PrinterStatuses>
{
    public override bool Validate(PrinterStatuses item) => item is PrinterStatuses.Ready or PrinterStatuses.Busy;
}