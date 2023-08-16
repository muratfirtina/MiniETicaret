using MiniETicaret.Application.Abstractions.Services;
using QRCoder;

namespace MiniETicaret.Infrastructure.Services;

public class QrCodeService : IQrCodeService
{
    public byte[] GenerateQrCode(string text)
    {
        QRCodeGenerator generator = new();
        QRCodeData data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode qrCode = new(data);
        byte[] byteGraphic = qrCode.GetGraphic(10, new byte[] { 84, 99, 71 }, new byte[] { 240, 240, 240 });
        return byteGraphic;
    }
}