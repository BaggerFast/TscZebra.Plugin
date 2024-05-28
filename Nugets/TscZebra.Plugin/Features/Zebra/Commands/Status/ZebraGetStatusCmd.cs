using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TscZebra.Plugin.Abstractions.Enums;
using TscZebra.Plugin.Common;
using TscZebra.Plugin.Features.Zebra.Constants;

namespace TscZebra.Plugin.Features.Zebra.Commands.Status;

internal sealed class ZebraGetStatusCmd() : BaseCommand<PrinterStatuses>(ZebraCommandConsts.GetStatus)
{
    private static readonly char[] Separators = [',', '\n', '\r'];

    protected override async Task<PrinterStatuses> ResponseAsync(NetworkStream stream)
    {
        byte[] buffer = new byte[76];
        int read = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length));
        return ParseBytesToStatus(read, buffer);
    }
    
    #region Private

    private static PrinterStatuses ParseBytesToStatus(int readBytes, byte[] bytes)
    {
        string strStatus = Encoding.UTF8.GetString(bytes, 0, bytes.Length).Replace("\"", "");
        string[] arrayStatus = strStatus.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
        
        if (arrayStatus.Length != ZebraBaseConsts.StatusStrLen || readBytes != bytes.Length)
            return PrinterStatuses.Unsupported;

        try {

            List<KeyValuePair<bool, PrinterStatuses>> statuses =
            [
                new(StateParse(arrayStatus, ZebraStatusIndexes.PaperOut), PrinterStatuses.PaperOut),
                new(StateParse(arrayStatus, ZebraStatusIndexes.HeadOpen), PrinterStatuses.HeadOpen),
                new(StateParse(arrayStatus, ZebraStatusIndexes.Pause), PrinterStatuses.Paused),
                new(StateParse(arrayStatus, ZebraStatusIndexes.RibbonOut), PrinterStatuses.RibbonOut),
                new(StateParse(arrayStatus, ZebraStatusIndexes.HeightTemp), PrinterStatuses.Unsupported),
                new(StateParse(arrayStatus, ZebraStatusIndexes.BufferFull), PrinterStatuses.Unsupported)
            ];

            foreach (KeyValuePair<bool, PrinterStatuses> kvp in statuses.Where(kvp => kvp.Key))
                return kvp.Value;

            return PrinterStatuses.Ready;
        }
        catch (Exception)
        {
            return PrinterStatuses.Unsupported;
        }
    }
    
    private static bool StateParse(IReadOnlyList<string> array, ZebraStatusIndexes index)
    {
        if ((int)index >= array.Count) throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range");
        return array[(int)index] == "1";
    }

    #endregion
}