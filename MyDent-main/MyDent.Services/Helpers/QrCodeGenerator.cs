using MessagingToolkit.QRCode.Codec;
using MyDent.Services.Abstractions;
using System.Drawing;

namespace MyDent.Services.Helpers
{
    public class QrCodeGenerator : IQrCodeGenerator
    {
        public Bitmap GenerateQRCode(string content)
        {
            QRCodeEncoder encoder = new QRCodeEncoder();
            encoder.QRCodeErrorCorrect=QRCodeEncoder.ERROR_CORRECTION.H;
            encoder.QRCodeEncodeMode=QRCodeEncoder.ENCODE_MODE.BYTE;
            encoder.QRCodeScale=10;
            Bitmap qrcode = encoder.Encode(content);
            return qrcode;
        }
    }
}
