using System;
using System.Collections.Generic;
using OxyPlot;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using MathCore_2_0;

namespace Degree_Work
{
    static class Extensions
    {
        public static DataPoint ComplexToDataPoint(this complex c)
        {
            return new DataPoint(c.Re, c.Im);
        }
        public static double Abs(this DataPoint p)
        {
            return Math.Sqrt(p.X * p.X + p.Y * p.Y);
        }
        public static complex DataPointToComplex(this DataPoint p)
        {
            return new complex(p.X, p.Y);
        }
        public static bool IsAllThreadsCompleted(this List<IAsyncResult> l)
        {
            foreach (IAsyncResult i in l)
            {
                if (!i.IsCompleted) { return false; }
            }
            return true;
        }
        public static void SaveJPG100(this BitmapSource bmp, string filename)
        {
            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            bmp.GetBitmap().Save(filename, GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg), encoderParameters);
        }
        public static Bitmap GetBitmap(this BitmapSource source)
        {
            Bitmap bmp = new Bitmap(
              source.PixelWidth,
              source.PixelHeight,
              System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            BitmapData data = bmp.LockBits(
              new Rectangle(System.Drawing.Point.Empty, bmp.Size),
              ImageLockMode.WriteOnly,
              System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            source.CopyPixels(
              Int32Rect.Empty,
              data.Scan0,
              data.Height * data.Stride,
              data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }
        static ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
