using CSharpWin_JD.CaptureImage;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MyCsActiveXControl
{
    [Guid("7CDF344D-A42D-44e0-A5B5-4562288B6A37")]
    [ComVisible(true)]
    public partial class UserControl1 : UserControl, IObjectSafety
    {
        public UserControl1()
        {
            InitializeComponent();
        }
        public string Message { get; set; }
        //public mshtml.HTMLWindow2Class Html { private get; set; }

        #region IObjectSafety 成员
        public void GetInterfacceSafyOptions(int riid, out int pdwSupportedOptions, out int pdwEnabledOptions)
        {
            pdwSupportedOptions = 1;
            pdwEnabledOptions = 2;
        }
        public void SetInterfaceSafetyOptions(int riid, int dwOptionsSetMask, int dwEnabledOptions)
        {
            throw new NotImplementedException();
        }
        #endregion

        public void ShowMessage()
        {
            MessageBox.Show(Message);
        }

        private void MessageButton_Click(object sender, EventArgs e)
        {
           
           
            //ShowMessage();
        }

        private void JsButton_Click(object sender, EventArgs e)
        {
            ////Html.execScript(string.Format("document.getElementById('numberText').value='{0}';", DateTime.Now), "javascript");
        }

       
    }
}
