using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Common;

namespace TscZebra.Plugin.Validators.State;

public class IsPrinterConnected : BaseValidator<PrinterStatuses>
{
    public override bool Validate(PrinterStatuses item) => item is not PrinterStatuses.IsDisconnected;
}