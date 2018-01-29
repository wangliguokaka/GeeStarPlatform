<%@ WebHandler Language="C#" Class="ValidateCode" %>

using System;
using System.Web;
using System.Drawing;
using System.Web.SessionState;

public class ValidateCode : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {

        string checkCode = CreateRandomCode(4);
        context.Session["CheckCode"] = checkCode;
        CreateImage(context,checkCode);
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    private string CreateRandomCode(int codeCount)
    {
        string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,W,X,Y,Z";
        //天,地,不,仁,以,万,物,为,刍,狗,圣,人,不,仁,以,百,姓,为,刍,狗,这,句,经,常,出,现,在,控,诉,暴,君,暴,政,上,地,残,暴,不,仁,把,万,物,都,当,成,低,贱,的,猪,狗,来,看,待,而,那,些,高,高,在,上,的,所,谓,圣,人,们,也,没,两,样,还,不,是,把,我,们,老,百,姓,也,当,成,猪,狗,不,如,的,东,西,但,实,在,正,取,的,解,读,是,地,不,情,感,用,事,对,万,物,一,视,同,仁,圣,人,不,情,感,用,事,对,百,姓,一,视,同,仁,执,子,之,手,与,子,偕,老,当,男,女,主,人,公,含,情,脉,脉,看,着,对,方,说,了,句,执,子,之,手,与,子,偕,老,女,方,泪,眼,朦,胧,含,羞,地,回,一,句,讨,厌,啦,这,样,的,情,节,我,们,是,不,是,见,过,很,多,但,是,我,们,来,看,看
        string[] allCharArray = allChar.Split(',');
        string randomCode = "";
        int temp = -1;

        Random rand = new Random();
        for (int i = 0; i < codeCount; i++)
        {
            if (temp != -1)
            {
                rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
            }
            int t = rand.Next(35);
            if (temp == t)
            {
                return CreateRandomCode(codeCount);
            }
            temp = t;
            randomCode += allCharArray[t];
        }
        return randomCode;
    }

    private void CreateImage(HttpContext context,string checkCode)
    {
        int iwidth = (int)(checkCode.Length * 18);
        System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, 25);
        Graphics g = Graphics.FromImage(image);
        Font f = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
        Brush b = new System.Drawing.SolidBrush(Color.Black);
        //g.FillRectangle(new System.Drawing.SolidBrush(Color.Blue),0,0,image.Width, image.Height);
        g.Clear(Color.FromArgb(238, 238, 238));
        g.DrawString(checkCode, f, b, 3, 3);

        Pen blackPen = new Pen(Color.Black, 0);
        Random rand = new Random();
        for (int i = 0; i < 3; i++)
        {
            int y = rand.Next(image.Height);
            g.DrawLine(blackPen, 0, y, image.Width, y);
        }

        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        context.Response.ClearContent();
        context.Response.ContentType = "image/Jpeg";
        context.Response.BinaryWrite(ms.ToArray());
        g.Dispose();
        image.Dispose();
    }
}