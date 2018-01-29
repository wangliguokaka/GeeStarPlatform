using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using CSharpWin_JD.CaptureImage;
using System.Security;
using System.IO;
using System.Drawing.Imaging;
using System.Xml.Serialization;
using System.Xml;

namespace CaptureTool
{
    [Guid("396072D4-A435-495D-ACC9-7D899DE7FD30")]
    [ComVisible(true)]
    public partial class UserControl1 : UserControl, IObjectSafety
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        public string ShowMessage()
        {
            try
            {
                Image image = null;
                CaptureImageTool capture = new CaptureImageTool();
                string curPath = Application.StartupPath;
                if (capture.ShowDialog() == DialogResult.OK)
                {

                    image = capture.Image;
                    //ImageObject ImageO = new ImageObject();
                    //ImageO.img = image;
                    //Bitmap bimmap = new Bitmap(image);
                    //bimmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    //image = bimmap; 
                    ////bimmap.Save(path+ "\\111.jpg");
                    //image.Save(path + "\\111.jpg");
                    //bimmap.Dispose();
                    //image.Dispose();

                    // Bitmap bm1= new Bitmap(image);
                    //  Bitmap bm2=new Bitmap(bm1.Width,bm1.Height);
                    //  Graphics g=Graphics.FromImage(bm2);
                    //   g.DrawImageUnscaled(bm1, 0, 0);
                    //bm2 now contains a non-indexed version of the image.
                    //Now draw the X..
                    //g.DrawLine(blah-blah.);
                    //g.DrawLine(blah-blah.);
                    //get rid of the graphics
                    //g.Dispose();
                    ////and save a new gif
                    //bm2.Save(path + "\\111.jpg", ImageFormat.Gif);
                    byte[] buffer = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, ImageFormat.Bmp);

                        buffer = new byte[ms.Length];
                       
                        //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                        ms.Seek(0, SeekOrigin.Begin);
                        ms.Read(buffer, 0, buffer.Length);
                        image.Dispose();
                    }

                    XmlSerializer xs = new XmlSerializer(typeof(byte[]));
                    MemoryStream ms1 = new MemoryStream();
                    XmlTextWriter xtw = new XmlTextWriter(ms1, System.Text.Encoding.UTF8);
                    xtw.Formatting = Formatting.Indented;
                    xs.Serialize(xtw, buffer);
                    ms1.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(ms1);
                    string str = sr.ReadToEnd();
                    xtw.Close();
                    ms1.Close();
                    return str;


                        //return m.ToArray().ToString();
                        //var img = Image.FromStream(m);

                        ////TEST
                        //img.Save(curPath + "\\111.jpg");
                        //image.Dispose();
                        //GC.Collect();

                  //  }

                }
                else {
                    return null;
                }

               
            }
            catch (Exception ex)
            {
                return ex.Message;
               // return path + "111.jpg" + ex.InnerException;
            }
        }

        #region IObjectSafety 成员

        private const string _IID_IDispatch = "{00020400-0000-0000-C000-000000000046}";
        private const string _IID_IDispatchEx = "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}";
        private const string _IID_IPersistStorage = "{0000010A-0000-0000-C000-000000000046}";
        private const string _IID_IPersistStream = "{00000109-0000-0000-C000-000000000046}";
        private const string _IID_IPersistPropertyBag = "{37D84F60-42CB-11CE-8135-00AA004BB851}";

        private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001;
        private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002;
        private const int S_OK = 0;
        private const int E_FAIL = unchecked((int)0x80004005);
        private const int E_NOINTERFACE = unchecked((int)0x80004002);

        private bool _fSafeForScripting = true;
        private bool _fSafeForInitializing = true;

        public int GetInterfaceSafetyOptions(ref Guid riid, ref int pdwSupportedOptions, ref int pdwEnabledOptions)
        {
            int Rslt = E_FAIL;

            string strGUID = riid.ToString("B");
            pdwSupportedOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForScripting == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForInitializing == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_DATA;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }

        public int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
        {
            int Rslt = E_FAIL;
            string strGUID = riid.ToString("B");
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_CALLER) && (_fSafeForScripting == true))
                        Rslt = S_OK;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_DATA) && (_fSafeForInitializing == true))
                        Rslt = S_OK;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }

        #endregion
    }
    [Serializable]
    public class ImageObject
    {
        public System.Drawing.Image img;
    }
}
