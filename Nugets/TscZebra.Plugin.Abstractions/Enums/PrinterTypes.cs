using System.ComponentModel;

namespace TscZebra.Plugin.Abstractions.Enums;

public enum PrinterTypes
{
    [Description("PrinterTsc")]
    Tsc,
    [Description("PrinterZebra")]
    Zebra
}