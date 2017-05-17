using System;
using System.Collections.Generic;
using OxyPlot;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media.Imaging;
using Degree_Work.Mathematical_Sources.Complex;
using static Degree_Work.Mathematical_Sources.Functions.ElementaryFunctions;
using static Degree_Work.Mathematical_Sources.Functions.SpecialFunctions;

namespace Degree_Work
{
    /// <summary>
    /// Методы расширения
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// Выполнение операции комплексного сопряжения над точкой как над комплексным числом
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static DataPoint Conjugate(this DataPoint p)
        {
            p.Y = -p.Y;
            return p;
        }

        /// <summary>
        /// Вычисление модуля точки как модуля комплексного числа
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static double Abs(this DataPoint p)
        {
            return Math.Sqrt(p.X * p.X + p.Y * p.Y);
        }

        /// <summary>
        /// Обращение строки
        /// </summary>
        /// <param name="s">Строка, которую нужно обратить</param>
        /// <returns>Обращенная строка</returns>
        public static string Reverse(this string s)
        {
            char[] tmp = s.ToCharArray();
            Array.Reverse(tmp);
            return new string(tmp);
        }

        /// <summary>
        /// Выполняет проверку, завершено ли выполнение всех асинхронных делегатов, полученных в виде списка IAsyncResult
        /// </summary>
        /// <param name="l">Список возвращаемых методом BeginInvoke экземпляра делегата интерфейсных ссылок</param>
        /// <returns></returns>
        public static bool IsAllThreadsCompleted(this List<IAsyncResult> l)
        {
            foreach (IAsyncResult i in l)
            {
                if (!i.IsCompleted) { return false; }
            }
            return true;
        }

        /// <summary>
        /// Сохранение битового рисунка в JPG
        /// </summary>
        /// <param name="bmp">Объект BitmapSource</param>
        /// <param name="filename">Имя конечного файла</param>
        public static void SaveJPG100(this BitmapSource bmp, string filename)
        {
            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
            bmp.GetBitmap().Save(filename, GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg), encoderParameters);
        }
        
        /// <summary>
        /// Получение объекта Bitmap из BitmapSource
        /// </summary>
        /// <param name="source">Объект BitmapSource</param>
        /// <returns></returns>
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
