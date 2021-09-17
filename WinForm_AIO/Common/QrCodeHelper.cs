using System.Drawing;
using ZXing;
using ZXing.QrCode;

namespace WinForm_AIO.Common
{
    public static class QrCodeHelper
    {
        public static Bitmap CreateQrCode(string value,int width,int height)
        {
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            QrCodeEncodingOptions options = new QrCodeEncodingOptions()
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = width,
                Height = height,
                Margin = 1
            };

            writer.Options = options;
            Bitmap map = writer.Write(value);
            return map;
        }
    }
}
