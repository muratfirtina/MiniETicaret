namespace MiniETicaret.Application.Abstractions.Services;

public interface IQrCodeService
{
    byte[] GenerateQrCode(string text);
}