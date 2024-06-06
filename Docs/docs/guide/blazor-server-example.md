# Examples
To see how the library works, you can run the blazor application from the official [repository](https://github.com/BaggerFast/TscZebra.Plugin/tree/main/Clients/PrinterUI)

```csharp
@page "/"

@using System.Net
@using TscZebra.Plugin
@using TscZebra.Plugin.Abstractions
@using TscZebra.Plugin.Abstractions.Enums
@using TscZebra.Plugin.Abstractions.Exceptions
@implements IDisposable

<div class="w-full pt-4 flex flex-col items-center justify-center">
  <p class="text-5xl">TscZebra.WebUI</p>
</div>

<div class="w-full flex gap-2 pt-2 items-center justify-center">
  <Button OnClick="@Connect">
    Connect
  </Button>
  <Button OnClick="@Disconnect">
    Disconnect
  </Button>
  <Button OnClick="@GetStatus">
    GetStatusByHand
  </Button>
  <Button OnClick="@Print">
    Print
  </Button>
</div>
<div class="w-full pt-4 flex flex-col items-center justify-center">
  <p class="text-2xl">StatusByEvent = @Status</p>
  <p class="text-2xl">StatusByHande = @StatusByHand</p>
</div>



@code {
  IZplPrinter Printer { get; set; } = PrinterFactory.Create(IPAddress.Parse("10.0.22.71"), 9100, PrinterTypes.Tsc);
  PrinterStatus Status { get; set; }= PrinterStatus.Disconnected;
  PrinterStatus StatusByHand { get; set; }= PrinterStatus.Disconnected;
  static string Zpl => "^XA\n^FO 0,10\n^GB632,0,2^FS\n^FO0,25\n^FB632,1,0,C,0\n^ASN,70,70\n^FDWAR INC.^FS\n^FO0," +
                       "100\n^GB632,0,2^FS\n^FO0,120\n^FB632,1,0,C,0\n^ASN,60,60\n^FDGoose^FS\n^FO0,180\n^FB632,1," +
                       "0,C,0\n^ASN,60,60\n^FDWild^FS\n^FO0,240\n^GB632,0,2^FS\n^FO120,260\n^BY2\n^BCN,70,N,N,N\n^" +
                       "FDSECRECTCODE^FS - \n^XZ";
  
  protected override void OnAfterRender(bool firstRender)
  {
    if (firstRender)
      Printer.OnStatusChanged += ReceiveOnStatus;
    base.OnAfterRender(firstRender);
  }
  
  private async Task Connect()
  {
    try
    {
      await Printer.ConnectAsync();
      Printer.StartStatusPolling();
    } catch (PrinterConnectionException)
    {
      
    } 
  }

  private void Disconnect() => Printer.Disconnect();
  
  private async Task GetStatus()
  {
    try
    {
      StatusByHand = await Printer.RequestStatusAsync();
    } catch (PrinterConnectionException)
    {
      
    } 

  }
  
  private async Task Print()
  {
    try
    {
      await Printer.PrintZplAsync(Zpl);
    } catch (Exception e)
    {
      
    } 
  }

  private void ReceiveOnStatus(object? sender, PrinterStatus status)
  {
      Status = status;
      InvokeAsync(StateHasChanged);
  }

  public void Dispose()
  {
    Printer.Dispose();
    Printer.OnStatusChanged -= ReceiveOnStatus;
  }
}
```