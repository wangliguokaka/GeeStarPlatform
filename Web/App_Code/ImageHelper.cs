using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

/// <summary>
/// Summary description for ImageHelper
/// </summary>
public class ImageHelper
{
	public ImageHelper()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static string CropImage(string originamImgPath, string imgPath, int width, int height, int x, int y)
   {
       string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
       byte[] CropImage = Crop(originamImgPath, width, height, x, y);
       using (MemoryStream ms = new MemoryStream(CropImage, 0, CropImage.Length))
       {
           ms.Write(CropImage, 0, CropImage.Length);
           using (System.Drawing.Image CroppedImage = System.Drawing.Image.FromStream(ms, true))
           {
               string SaveTo = imgPath + filename;
               CroppedImage.Save(SaveTo, CroppedImage.RawFormat);
           }
       }
        return filename;
   }
    private static byte[] Crop(string Img, int Width, int Height, int X, int Y)
   {
       try
       {
           if (Width == 0 || Height == 0)
           {
               Width = 400;
               Height = 400;
               X = 0;
               Y = 0;
           }
           using (Image OriginalImage = Image.FromFile(Img))
           {
               using (Bitmap bmp = new Bitmap(Width, Height, OriginalImage.PixelFormat))
               {
                   bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);
                   using (Graphics Graphic = Graphics.FromImage(bmp))
                   {
                       Graphic.Clear(Color.White);
                       Graphic.SmoothingMode = SmoothingMode.AntiAlias;
                       Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                       Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                       Graphic.DrawImage(OriginalImage, new Rectangle(0, 0, Width, Height), X, Y, Width, Height, GraphicsUnit.Pixel);
                       MemoryStream ms = new MemoryStream();
                       bmp.Save(ms, OriginalImage.RawFormat);
                       return ms.GetBuffer();
                   }
               }
           }
       }
       catch (Exception Ex)
       {
           throw (Ex);
       }
   }
}