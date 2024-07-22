using ETicaretAPI.Application.Abstractions.Services;
using QRCoder;

namespace ETicaretAPI.Infrastructure.Services
{
    public class QRCodeService : IQRCodeService
    {
        public byte[] GenerateQRCode(string text)
        {
            QRCodeGenerator generator = new();
            var data = generator.CreateQrCode("laylaylom galiba sana göre sevmeler", QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new(data);
            byte[] byteGraphic = qrCode.GetGraphic(10, [84, 99, 71], [240, 240, 240]);
            return byteGraphic;
        }
    }
}
