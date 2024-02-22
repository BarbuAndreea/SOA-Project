using System.Drawing;

namespace MyDent.Services.Abstractions
{
    public interface IQrCodeGenerator
    {
        public Bitmap GenerateQRCode(string content);
    }
}
