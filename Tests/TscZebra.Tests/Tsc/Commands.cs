// using System.Net.Sockets;
// using FluentAssertions;
// using TscZebra.Plugin.Abstractions.Enums;
// using TscZebra.Plugin.Features.Tsc.Commands;
//
// namespace TscZebra.Tests.Tsc;
//
// public class Commands
// {
//     [Theory]
//     [InlineData(0x00, PrinterStatuses.Ready)]
//     [InlineData(0x01, PrinterStatuses.HeadOpen)]
//     [InlineData(0x02, PrinterStatuses.PaperJam)]
//     [InlineData(0x04, PrinterStatuses.PaperOut)]
//     [InlineData(0x08,  PrinterStatuses.RibbonOut)]
//     [InlineData(0x10,  PrinterStatuses.Paused)]
//     [InlineData(0x20,  PrinterStatuses.Busy)]
//     [InlineData(0x30,  PrinterStatuses.Unsupported)]
//     [InlineData(0x25,  PrinterStatuses.Unsupported)]
//     public async Task Test_Status_By_Byte(byte input, PrinterStatuses expected)
//     {
//         byte[] data = [input];
//         Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//         NetworkStream stream = new(socket);
//         await stream.WriteAsync(data);
//         PrinterStatuses status = await new TscGetStatusCmd().RequestAsync(stream);
//         status.Should().Be(expected);
//     }
// }