using IronOcr;
using Patagames.Ocr;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRTestingFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            //var bitmap = Image.FromFile("cvs-health-financials-adjusted-earnings-per-share-guidance-unaudited-table-image.png");
            //var resizedImage = ResizeImage(bitmap, 499, 499);
            //resizedImage.Save("image-1.png");
            //OcrApi.LicenseKey = "42433553494d500328bd021c3fd13c295f0f22d28e311f9cfb5937fbbb94fda3eb077f57b681c1c01742a0e23ccb0e7c52ccd8ea1674baf7473992f5043a4c77d2dc5ba756cd558d2555tdc333e90a2a036c1ec4eb88edda821c1e6d2f5a9517d7d092574ec50a75aaae3b461c62b70";
            //using (var api = OcrApi.Create())
            //{
            //    api.Init(Patagames.Ocr.Enums.Languages.English);
            //    var text = api.GetTextFromImage("cvs-health-financials-adjusted-earnings-per-share-guidance-unaudited-table-image.png");
            //    Console.WriteLine(text);
            //}

            var Ocr = new AutoOcr();
            var Result = Ocr.Read("image.png");
            Console.WriteLine(Result.Text);
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
