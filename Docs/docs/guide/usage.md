#  Usage

## Setup printer
This code creates IZplPrinter Instance without connecting

```csharp{5}
using TscZebra.Plugin;
using TscZebra.Plugin.Abstractions;

IZplPrinter Printer = 
    PrinterFactory.Create(IPAddress.Parse("127.0.0.1"), 9100, PrinterTypes.Tsc);
```

## Connect

```csharp{3}
try
{
  await Printer.ConnectAsync();
} catch (PrinterConnectionException)
{
  // connection cannot be established
} 
```

## Get status

### By polling
This method request printer for status, every interval seconds.
Invokes event **OnStatusChanged**.

Method auto disabled when printer disabled (always when IZplPrinter throws PrinterConnectionException).
Use after ConnectAsync method

```csharp{1}
  Printer.StartStatusPolling(5);
  
  Printer.OnStatusChanged += Receive;

  private void Receive(object? sender, PrinterStatus status)
  {
    // Your logic here
  }
  
  Printer.StopStatusPolling();
  Printer.OnStatusChanged -= Receive;
```

### By hand

This method also invokes event **OnStatusChanged**

```csharp
  PrinterStatuses StatusByHand = await Printer.RequestStatusAsync();
```

## Print zpl
Before printing, the printer requests its status and triggers all status events.
```csharp
  try {
    Printer.PrintZplAsync(string zpl);
  } catch {
      
  }
```

This method validates the ZPL code for printing. If the ZPL code is not valid, it throws a **PrinterCommandBodyException**
```csharp
   public async Task PrintZplAsync(string zpl)
    {
        if (!(zpl.StartsWith("^XA") && zpl.EndsWith("^XZ")))
            throw new PrinterCommandBodyException();
    }
```

If the printer is successfully connected but cannot print a label (e.g., the head is open), it throws a **PrinterStatusException**
```csharp
    public async Task PrintZplAsync(string zpl)
    {
        if (Status is not (PrinterStatuses.Ready or PrinterStatuses.Busy))
            throw new PrinterStatusException();
    }
```

## Disconnect
```csharp
Printer.Disconnect();
```
or 
```csharp
Printer.Dispose();
```

## Other Commands
You can find comprehensive documentation for IZplPrinter in the [code](https://github.com/BaggerFast/TscZebra.Plugin/blob/main/Nugets/TscZebra.Plugin.Abstractions/IZplPrinter.cs).