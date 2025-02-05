namespace LearningManagementSystem.Application.Abstractions.Services.QRCode;

public interface IQRCodeService
{
    Task<byte[]> GenerateQrCode(Guid documentId);
}