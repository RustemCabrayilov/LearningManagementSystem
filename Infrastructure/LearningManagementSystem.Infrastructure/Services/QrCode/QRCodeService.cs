using LearningManagementSystem.Application.Abstractions.Services.Aws;
using LearningManagementSystem.Application.Abstractions.Services.Document;
using LearningManagementSystem.Application.Abstractions.Services.QRCode;
using Microsoft.Extensions.Options;
using QRCoder;

namespace LearningManagementSystem.Infrastructure.Services.QrCode;

public class QRCodeService(IDocumentService _documentService) : IQRCodeService
{
    public async Task<byte[]> GenerateQrCode(Guid documentId)
    {
        QRCodeGenerator generator = new();
        var documentResponse = await _documentService.GetByOwnerId(documentId);
        QRCodeData data = generator.CreateQrCode(documentResponse.Path, QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode qrCode = new(data);
        byte[] byteGraphic = qrCode.GetGraphic(10, new byte[] { 84, 99, 71 }, new byte[] { 240, 240, 240 });
        return byteGraphic;
    }
}