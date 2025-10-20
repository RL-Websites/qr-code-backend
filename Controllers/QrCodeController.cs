using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace QRCode.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QrCodeController : ControllerBase
    {        
        [HttpPost("GenerateQRCode")]
        public async Task<IActionResult> GenerateQRCode([FromBody] QrCodeGeneratorDto dto)
        {           
            var fileName = $"qr_{DateTime.UtcNow.Ticks}.svg";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "qrcodes", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            await SaveQrCodeToFile(dto, filePath);

            var fileUrl = $"{Request.Scheme}://{Request.Host}/qrcodes/{fileName}";

            return Ok(new { url = fileUrl });
        }

        private async Task SaveQrCodeToFile(QrCodeGeneratorDto dto, string filePath)
        {
            string svg = await GenerateSVG(dto);
            await System.IO.File.WriteAllTextAsync(filePath, svg);
        }

        private Task<string> GenerateSVG(QrCodeGeneratorDto dto)
        {
            using (QRCodeGenerator qrCodeGenerator = new QRCodeGenerator())
            {
                string payload = dto.url ?? string.Empty;
                var errorCorrectionLevel = Math.Clamp(dto.ErrorCorrectionLevel ?? 1, 0, 3);
                var borderWidth = Math.Clamp(dto.BorderWidth ?? 3, 0, 999999);

                using (QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q))
                {
                    using (var code = new SvgQRCode(qrCodeData))
                    {
                        var qr = code.GetGraphic(dto.PixelsPerModule, dto.ForegroundColor, dto.BackgroundColor);
                        return Task.FromResult(qr);
                    }
                }
            }
        }
    }
}
public class QrCodeGeneratorDto
{
    public string url { get; set; }
    public int? ErrorCorrectionLevel { get; set; }
    public int? BorderWidth { get; set; }
    public int PixelsPerModule { get; set; } = 20;
    public string ForegroundColor { get; set; } = "#000000";
    public string BackgroundColor { get; set; } = "#ffffff";
}




