using System;
using System.Collections.Generic;
using System.Web;
using System.Threading;
using System.Web.SessionState;

/// <summary>
/// Summary description for AsnyHandler
/// </summary>
public class AsnyHandler : IHttpAsyncHandler
{
	public AsnyHandler()
	{
	}

    public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
    {
        try
        {
            // http://localhost:58826/App_Code/AsnyHandler.cs
            //myAsynResult为实现了IAsyncResult接口的类，当不调用cb的回调函数时，该请求不会返回到给客户端，会一直处于连接状态
            myAsynResult asyncResult = new myAsynResult(context, cb, extraData);

            String userName = context.Request.Params["userName"];
            String orderNumber = context.Request.Params["orderNumber"];
            String content = context.Request.Params["content"];
            String BelongFactory = context.Request.Params["BelongFactory"];
            //context.Session["BelongFactory"] = BelongFactory;
            //向Message类中添加该消息
            Messages.Instance().AddMessage(orderNumber, BelongFactory,userName, content, asyncResult);
            return asyncResult;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    #region 不必理会
    
    public void EndProcessRequest(IAsyncResult result)
    {
      
    }

    public bool IsReusable
    {
        get { return false; ; }
    }

    public void ProcessRequest(HttpContext context)
    {
    }
    #endregion
}
public class myAsynResult : IAsyncResult
{
    bool _IsCompleted = false;
    private HttpContext context;
    private AsyncCallback cb;
    private object extraData;
    public myAsynResult(HttpContext context, AsyncCallback cb, object extraData)
    {
        this.context = context;
        this.cb = cb;
        this.extraData = extraData;
    }
    private string _content;
    public string Content { 
        get {return _content;}
        set{_content=value;} 
    }

    #region IAsyncResult接口
    public object AsyncState
    {
        get { return null; }
    }

    public System.Threading.WaitHandle AsyncWaitHandle
    {
        get { return null; }
    }

    public bool CompletedSynchronously
    {
        get { return false; }
    }
    public bool IsCompleted
    {
        get { return _IsCompleted; }
    }
    #endregion

    //在Message类中的添加消息方法中，调用该方法，将消息输入到客户端，从而实现广播的功能
    public void Send(object data)
    {
            context.Response.Write(this.Content);
            if (cb!=null)
            {
                cb(this);
            }
            _IsCompleted = true; ;
    }
}
