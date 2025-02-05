using LearningManagementSystem.Application.Abstractions.Services.QRCode;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class QRCodeController(IQRCodeService _qrCodeService) : ControllerBase
{
    [HttpPost]
    public  async Task<IActionResult> Post(Guid documentId)
    {
        var response= await _qrCodeService.GenerateQrCode(documentId);
        return File(response,"image/png");
    }
}