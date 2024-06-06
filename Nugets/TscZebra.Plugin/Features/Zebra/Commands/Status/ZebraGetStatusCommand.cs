using System.Net.Sockets;
using System.Text;
using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Common;
using TscZebra.Plugin.Features.Zebra.Constants;

namespace TscZebra.Plugin.Features.Zebra.Commands.Status;

internal sealed class ZebraGetStatusCommand() : Command<PrinterStatus>(ZebraCommandConsts.GetStatus)
{
    private static readonly char[] Separators = [',', '\n', '\r'];
   
    protected override async Task<PrinterStatus> ResponseAsync(NetworkStream stream, CancellationToken token)
    {
        byte[] buffer = new byte[76];
        int read = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length), token);
        return ParseBytesToStatus(read, buffer);
    }
    
    #region Private

    private static PrinterStatus ParseBytesToStatus(int readBytes, byte[] bytes)
    {
        string strStatus = Encoding.UTF8.GetString(bytes, 0, bytes.Length).Replace("\"", "");
        string[] arrayStatus = strStatus.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
        
        if (arrayStatus.Length != ZebraBaseConsts.StatusStrLen || readBytes != bytes.Length)
            return PrinterStatus.Unsupported;

        try {

            List<KeyValuePair<bool, PrinterStatus>> statuses =
            [
                new(StateParse(arrayStatus, ZebraStatusIndexes.PaperOut), PrinterStatus.PaperOut),
                new(StateParse(arrayStatus, ZebraStatusIndexes.HeadOpen), PrinterStatus.HeadOpen),
                new(StateParse(arrayStatus, ZebraStatusIndexes.Pause), PrinterStatus.Paused),
                new(StateParse(arrayStatus, ZebraStatusIndexes.RibbonOut), PrinterStatus.RibbonOut),
                new(StateParse(arrayStatus, ZebraStatusIndexes.HeightTemp), PrinterStatus.Unsupported),
                new(StateParse(arrayStatus, ZebraStatusIndexes.BufferFull), PrinterStatus.Unsupported)
            ];

            foreach (KeyValuePair<bool, PrinterStatus> kvp in statuses.Where(kvp => kvp.Key))
                return kvp.Value;

            return PrinterStatus.Ready;
        }
        catch (Exception)
        {
            return PrinterStatus.Unsupported;
        }
    }
    
    private static bool StateParse(IReadOnlyList<string> array, ZebraStatusIndexes index)
    {
        if ((int)index >= array.Count) throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range");
        return array[(int)index] == "1";
    }

    #endregion
}