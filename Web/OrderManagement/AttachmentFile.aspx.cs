using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using D2012.Common.DbCommon;
using D2012.Domain.Services;
using D2012.Common;
using D2012.Domain.Entities;
using System.Drawing;
using System.Drawing.Drawing2D;
using GeeStar.Workflow.Common;
using System.Drawing.Imaging;
public partial class OrderManagement_AttachmentFile : PageBase
{
    protected string picID;
    protected void Page_Load(object sender, EventArgs e)
    {
        //try {
            picID = yeyRequest.Params("fileid");
            HttpPostedFile file = Request.Files["txtshareF"];

            //如果读取不到文件对象
            if (file == null)
            {
                if (!String.IsNullOrEmpty(Request["picID"]))
                {
                    Response.Clear();
                    Response.Write("上传失败！");
                    Response.End();
                }
            }
            else
            {


                picID = Request["picID"];
                //获取文件夹路径
                string path = Server.MapPath("~" + SaveFilePath); //网站中有一个 uploadedFiles 文件夹，存储上传来的图片

                //生成文件名（系统要重新生成一个文件名，但注意扩展名要相同。千万不要用中文名称！！！）
                string originalFileName = file.FileName;
                string fileExtension = originalFileName.Substring(originalFileName.LastIndexOf('.'), originalFileName.Length - originalFileName.LastIndexOf('.'));
                string currentFileName = picID + fileExtension;  //例如：可使用“随机数+扩展名”生成文件名
                //生成文件路径
                string imageUrl = path + currentFileName;
                //保存文件

                file.SaveAs(imageUrl);

                using (System.Drawing.Image image = System.Drawing.Image.FromFile(imageUrl))
                {
                    int width = image.Width;
                    int height = image.Height;
                    int picWidth = 0;
                    int picHeight = 0;
                    if (width >= height)
                    {
                        picWidth = 400;
                        picHeight = Convert.ToInt32(((400 * 1.0 / width) * height));
                    }
                    else
                    {
                        picHeight = 400;
                        picWidth = Convert.ToInt32(((400 * 1.0 / height) * width));
                    }
                    Image ImageUpload = Image.FromStream(file.InputStream, true, true);
                    ImageFormat imageFormat = ImageUpload.RawFormat;
                    Image imageFile = resizeImage(ImageUpload, new Size { Height = picHeight, Width = picWidth });
                    string newFileName = picID + "resize" + fileExtension;

                    ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo ici = null;

                    //foreach (ImageCodecInfo i in icis)
                    //{
                    //    if (i.MimeType == "image/jpeg" || i.MimeType == "image/jpg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                    //    {
                    //        ici = i;
                    //    }
                    //}
                    EncoderParameters ep = new EncoderParameters(1);
                    ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);

                    if (fileExtension.ToLower().IndexOf("jpg") > -1 || fileExtension.ToLower().IndexOf("jpeg") > -1)
                    {

                        imageFile.Save(path + newFileName, icis[1], ep);
                    }
                    else if (fileExtension.IndexOf("png") > -1)
                    {
                        imageFile.Save(path + newFileName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    else
                    {
                        imageFile.Save(path + newFileName);
                    }
                   
                    file.InputStream.Close();
                    imageFile.Dispose();
                    image.Dispose();

                    string url = Request.Url.GetLeftPart(UriPartial.Authority) + SaveFilePath + newFileName;
                    Response.Clear();
                    Response.Write(url + "," + picWidth.ToString() + "," + picHeight.ToString());
                }
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                Response.End();
                //-----------------获取文件url-----------------
                //获取img的url地址（如果 file.FileName 有汉字，要进行url编码）
                //string url = Request.Url.GetLeftPart(UriPartial.Authority) + "/uploadedFiles/" + currentFileName;
                //此时 result 已被赋值为“<iframe src="http://localhost:8080/wangEditor_uploadImg_assist.html#ok|图片url地址"></iframe>”
            }
        //}
        //catch (Exception ex)
        //{
        //    Log.LogError(ex.Message, ex);
        //    Response.End();
        //}
        //finally
        //{
           
        //}
    }

    private static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, System.Drawing.Size size)
    {
        int sourceWidth = imgToResize.Width;
        int sourceHeight = imgToResize.Height;

        float nPercent = 0;
        float nPercentW = 0;
        float nPercentH = 0;

        nPercentW = ((float)size.Width / (float)sourceWidth);
        nPercentH = ((float)size.Height / (float)sourceHeight);

        if (nPercentH < nPercentW)
            nPercent = nPercentH;
        else
            nPercent = nPercentW;

        int destWidth = (int)(sourceWidth * nPercent);
        int destHeight = (int)(sourceHeight * nPercent);

        Bitmap b = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);

        Graphics g = Graphics.FromImage((System.Drawing.Image)b);
        //Bitmap b = new Bitmap(imgToResize);
       
        //Graphics g = Graphics.FromImage(imgToResize);
        
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        g.CompositingQuality = CompositingQuality.HighQuality;

        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;

        Rectangle fromr = new Rectangle(0, 0, sourceWidth, sourceWidth);
        Rectangle tor = new Rectangle(0, 0, destWidth, destHeight);
        g.DrawImage(imgToResize, 0, 0, destWidth,destHeight);
        g.Dispose();
        imgToResize.Dispose();
       
        return (System.Drawing.Image)b;

       
        //return imgToResize;
    }
}
