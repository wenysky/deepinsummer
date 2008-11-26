using System.IO;
using System.Net;
using System.Text;

using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System;


namespace Natsuhime
{
    /// <summary>
    /// 对Http协议的封装（post,get）,提供同步和异步调用的方法
    /// </summary>
    public class NewHttper:Component
    {
        #region 属性
        private string m_Url;
        private string m_PostData = "";
        private int m_Timeout = 100000;
        private string m_ContentType = "application/x-www-form-urlencoded";
        private string m_UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
        private string m_Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/xaml+xml, application/vnd.ms-xpsdocument, application/x-ms-xbap, application/x-ms-application, */*";
        private string m_Referer = "";
        private string m_X_FORWARDED_FOR = "";
        private string m_Charset = "UTF-8";
        private WebProxy m_Proxy;
        private CookieContainer m_Cookie;

        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
        }
        public string PostData
        {
            get { return m_PostData; }
            set { m_PostData = value; }
        }
        public int Timeout
        {
            get { return m_Timeout; }
            set { m_Timeout = value; }
        }
        public string ContentType
        {
            get { return m_ContentType; }
            set { m_ContentType = value; }
        }
        public string UserAgent
        {
            get { return m_UserAgent; }
            set { m_UserAgent = value; }
        }
        public string Accept
        {
            get { return m_Accept; }
            set { m_Accept = value; }
        }
        public string Referer
        {
            get { return m_Referer; }
            set { m_Referer = value; }
        }
        public string X_FORWARDED_FOR
        {
            get { return m_X_FORWARDED_FOR.Trim(); }
            set { m_X_FORWARDED_FOR = value; }
        }
        public string Charset
        {
            get { return m_Charset; }
            set { m_Charset = value; }
        }
        public WebProxy Proxy
        {
            get { return m_Proxy; }
            set { m_Proxy = value; }
        }
        public CookieContainer Cookie
        {
            get { return m_Cookie; }
            set { m_Cookie = value; }
        }
        #endregion

       
        public delegate void RequestDataCompletedEventHandler(
            object sender,
            RequestDataCompletedEventArgs e);
        public delegate void RequestStringCompleteEventHandler(
            object sender,
            RequestStringCompletedEventArgs e
            );

       
        public event RequestDataCompletedEventHandler RequestDataCompleted;
        public event RequestStringCompleteEventHandler RequestStringCompleted;
        

     
        private SendOrPostCallback onRequestDataCompletedDelegate;
        private SendOrPostCallback onRequestStringCompletedDelegate;

        private delegate void WorkerEventHandler(
            EnumRequestMethod requestMethod,
            AsyncOperation asyncOp);
        private HybridDictionary userStateToLifetime =
            new HybridDictionary();

        private System.ComponentModel.Container components = null;

         /////////////////////////////////////////////////////////////
        #region Construction and destruction

        public NewHttper(IContainer container)
        {   
            container.Add(this);
            InitializeComponent();
            InitializeDelegates();
        }

        public NewHttper()
        {   
            InitializeComponent();
            InitializeDelegates();
        }

        protected virtual void InitializeDelegates()
        {
            
            onRequestDataCompletedDelegate =
                new SendOrPostCallback(ReportRequestDataCompleted);
            onRequestStringCompletedDelegate =
            new SendOrPostCallback(ReportRequestStringCompleted);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion // Construction and destruction

        /////////////////////////////////////////////////////////////
        #region 异步的模式的实现
        /// <summary>
        /// Http异步请求，事件中返回byte数组
        /// </summary>
        /// <param name="requestMethod">http请求的方法</param>
        /// <returns>任务Id,唯一标识一次任务</returns>
        public virtual Guid RequestDataAsync(EnumRequestMethod requestMethod)
        {
            Guid taskId=Guid.NewGuid();
            RequestDataAsync(requestMethod, taskId);
            return taskId;

        }

        // This method starts an asynchronous calculation. 
        // First, it checks the supplied task ID for uniqueness.
        // If taskId is unique, it creates a new WorkerEventHandler 
        // and calls its BeginInvoke method to start the calculation.
        public virtual void RequestDataAsync(
            EnumRequestMethod requestMethod,
            object taskId)
        {
            // Create an AsyncOperation for taskId.
           
            AsyncOperation asyncOp =
                AsyncOperationManager.CreateOperation(taskId);

            // Multiple threads will access the task dictionary,
            // so it must be locked to serialize access.
            lock (userStateToLifetime.SyncRoot)
            {
                if (userStateToLifetime.Contains(taskId))
                {
                    throw new ArgumentException(
                        "Task ID parameter must be unique",
                        "taskId");
                }

                userStateToLifetime[taskId] = asyncOp;
            }

            // Start the asynchronous operation.
            WorkerEventHandler workerDelegate = new WorkerEventHandler(RequestDataWorker);
            workerDelegate.BeginInvoke(
                requestMethod,
                asyncOp,
                null,
                null);
           
        }

        public virtual Guid RequestStringAsync(EnumRequestMethod requestMethod)
        {
            Guid taskId = Guid.NewGuid();
            RequestStringAsync(requestMethod, taskId);
            return taskId;
        }

        public virtual void RequestStringAsync(
            EnumRequestMethod requestMethod,
            object taskId)
        {
            // Create an AsyncOperation for taskId.

            AsyncOperation asyncOp =
                AsyncOperationManager.CreateOperation(taskId);

            // Multiple threads will access the task dictionary,
            // so it must be locked to serialize access.
            lock (userStateToLifetime.SyncRoot)
            {
                if (userStateToLifetime.Contains(taskId))
                {
                    throw new ArgumentException(
                        "Task ID parameter must be unique",
                        "taskId");
                }

                userStateToLifetime[taskId] = asyncOp;
            }

            // Start the asynchronous operation.
            WorkerEventHandler workerDelegate = new WorkerEventHandler(RequestStringWorker);
            workerDelegate.BeginInvoke(
                requestMethod,
                asyncOp,
                null,
                null);

        }

        // 这个方法开始真正的请求过程
        // 运行在单独的线程中
        private void RequestStringWorker(
            EnumRequestMethod requestMethod,
            AsyncOperation asyncOp)
        {

            Stream responseStream;
            string responseString = string.Empty;
            Exception e = null;

            // Check that the task is still active.
            // The operation may have been canceled before
            // the thread was scheduled.
            if (!TaskCanceled(asyncOp.UserSuppliedState))
            {
                try
                {
                    switch (requestMethod)
                    {
                        case EnumRequestMethod.GET:
                            responseStream = this.HttpGetStream();
                            break;
                        case EnumRequestMethod.POST:
                            responseStream = this.HttpPostStream();
                            break;
                        default: //默认是POST
                            responseStream = this.HttpPostStream();
                            break;
                    }
                    responseString = GetResponseString(responseStream);
                }
                catch (Exception ex)
                {
                    e = ex;
                }
            }

            this.CompletionMethod(
                responseString,
                e,
                TaskCanceled(asyncOp.UserSuppliedState),
                asyncOp);

            //completionMethodDelegate(calcState);
        }

        // 这个方法开始真正的请求过程
        // 运行在单独的线程中
        private void RequestDataWorker(
            EnumRequestMethod requestMethod,
            AsyncOperation asyncOp)
        {

            byte[] responseData=null;
            HttpWebResponse response=null;
            Exception e = null;
            if (!TaskCanceled(asyncOp.UserSuppliedState))
            {
                try
                {
                    switch (requestMethod)
                    {
                        case EnumRequestMethod.GET:
                            response = this.HttpGetMethod();
                            break;
                        case EnumRequestMethod.POST:
                            response = this.HttpPostMethod();
                            break;
                        default: //默认是POST
                            response = this.HttpPostMethod();
                            break;
                    }
                    responseData = GetResponseData(response);
                }
                catch (Exception ex)
                {
                    e = ex;
                }
            }

            this.CompletionMethod(
                responseData,
                e,
                TaskCanceled(asyncOp.UserSuppliedState),
                asyncOp);

            //completionMethodDelegate(calcState);
        }

        

        // This is the method that the underlying, free-threaded 
        // asynchronous behavior will invoke.  This will happen on
        // an arbitrary thread.
        private void CompletionMethod(
            string responseString,
            Exception exception,
            bool canceled,
            AsyncOperation asyncOp)
        {
            
            // If the task was not previously canceled,
            // remove the task from the lifetime collection.
            if (!canceled)
            {
                lock (userStateToLifetime.SyncRoot)
                {
                    userStateToLifetime.Remove(asyncOp.UserSuppliedState);
                }
            }
            
            RequestStringCompletedEventArgs e =
                new RequestStringCompletedEventArgs(
                responseString,
                exception,
                canceled,
                asyncOp.UserSuppliedState);

            // End the task. The asyncOp object is responsible 
            // for marshaling the call.
            asyncOp.PostOperationCompleted(onRequestStringCompletedDelegate, e);

            // Note that after the call to OperationCompleted, 
            // asyncOp is no longer usable, and any attempt to use it
            // will cause an exception to be thrown.
        }

        private void CompletionMethod(
           byte[] responseData,
           Exception exception,
           bool canceled,
           AsyncOperation asyncOp)
        {

            // If the task was not previously canceled,
            // remove the task from the lifetime collection.
            if (!canceled)
            {
                lock (userStateToLifetime.SyncRoot)
                {
                    userStateToLifetime.Remove(asyncOp.UserSuppliedState);
                }
            }

            RequestDataCompletedEventArgs e =
                new RequestDataCompletedEventArgs(
                responseData,
                exception,
                canceled,
                asyncOp.UserSuppliedState);

            // End the task. The asyncOp object is responsible 
            // for marshaling the call.
            asyncOp.PostOperationCompleted(onRequestDataCompletedDelegate, e);

            // Note that after the call to OperationCompleted, 
            // asyncOp is no longer usable, and any attempt to use it
            // will cause an exception to be thrown.
        }

        // This method is invoked via the AsyncOperation object,
        // so it is guaranteed to be executed on the correct thread.
        private void ReportRequestDataCompleted(object operationState)
        {
            RequestDataCompletedEventArgs e =
                operationState as RequestDataCompletedEventArgs;

            OnRequestDataCompleted(e);
        }

        private void ReportRequestStringCompleted(object operationState)
        {
            RequestStringCompletedEventArgs e =
                operationState as RequestStringCompletedEventArgs;

            OnRequestStringCompleted(e);
        }

        private void OnRequestStringCompleted(RequestStringCompletedEventArgs e)
        {
            if (RequestStringCompleted != null)
            {
                RequestStringCompleted(this, e);
            }
        }

       

        protected void OnRequestDataCompleted(
            RequestDataCompletedEventArgs e)
        {
            if (RequestDataCompleted != null)
            {
                RequestDataCompleted(this, e);
            }
        }

       

        /// <summary>
        /// 判断一次请求任务是否被取消
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns>是否被取消</returns>
        private bool TaskCanceled(object taskId)
        {
            return (userStateToLifetime[taskId] == null);
        }

        /// <summary>
        /// 取消一个任务的执行
        /// </summary>
        /// <param name="taskId">任务ID</param>
        public void CancelAsync(object taskId)
        {
            AsyncOperation asyncOp = userStateToLifetime[taskId] as AsyncOperation;
            if (asyncOp != null)
            {
                lock (userStateToLifetime.SyncRoot)
                {
                    userStateToLifetime.Remove(taskId);
                }
            }
        }

        #endregion 异步的模式的实现

        private string GetResponseString(Stream responseStream)
        {
            StreamReader sr = new StreamReader(responseStream, Encoding.GetEncoding(Charset));
            string Content = string.Empty;
            try
            {
                Content = sr.ReadToEnd();
            }
            finally
            {
                if(null!=sr)
                {
                    sr.Close();
                }
                if(null!=responseStream)
                {
                    responseStream.Close();
                }
            }
            return Content;

        }

        private byte[] GetResponseData(HttpWebResponse response)
        {

          
            long contentLength = response.ContentLength;
            Stream readStream = response.GetResponseStream();
            byte[] resultBytes=null;
            try
            {  
  
                if(null!=readStream)
                {

                    resultBytes = ReadFully(readStream, (int)contentLength);
                    
                }
                    

            }
            finally
            {
                if (null != readStream)
                {
                    readStream.Close();
                }
                
            }
            return resultBytes;
        }

        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        private static byte[] ReadFully(Stream stream, int initialLength)
        {
            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }


        /// <summary>
        /// 暂时不要用这个方法.
        /// </summary>
        /// <param name="PostParamName"></param>
        /// <param name="PostParamValue"></param>
        public void AddPostParam(string PostParamName, string PostParamValue)
        {
            PostData += string.Format("&{0}={1}", PostParamName, System.Web.HttpUtility.UrlEncode(PostParamValue));
        }
        /// <summary>
        /// 取得页面编码
        /// </summary>
        /// <returns></returns>
        public string GetPageLanguageCode()
        {
            string pageLanguageCode = HttpGet();
            return RegexUtility.GetMatch(pageLanguageCode, "charset=(.*)\"");
        }
        public string HttpGet()
        {

            Stream s = HttpGetStream();
            StreamReader sr = new StreamReader(s, Encoding.GetEncoding(Charset));
            string Content = sr.ReadToEnd();
            //s.Close();
            sr.Close();
            s.Close();
            return Content;
        }

        private HttpWebResponse HttpGetMethod()
        {
            HttpWebRequest objHWR = (HttpWebRequest)HttpWebRequest.Create(Url);
            objHWR.Timeout = Timeout;
            //objHWR.ContentType = ContentType;
            objHWR.UserAgent = UserAgent;
            objHWR.Accept = Accept;
            objHWR.Referer = Referer;
            objHWR.Method = "GET";
            if (X_FORWARDED_FOR != string.Empty)
            {
                objHWR.Headers.Set("X_FORWARDED_FOR", X_FORWARDED_FOR);
            }

            if (Proxy != null)
            {
                objHWR.Proxy = Proxy;
            }
            if (Cookie != null)
            {
                objHWR.CookieContainer = Cookie;
            }
            HttpWebResponse objResponse = (HttpWebResponse)objHWR.GetResponse();
            return objResponse;

        }

        public Stream HttpGetStream()
        {
            HttpWebResponse response = HttpGetMethod();
            Stream s=null;
            if(null!=response)
            {
                s = response.GetResponseStream();
            }
            return s;
        }
        public string HttpPost()
        {
            
            Stream s = HttpPostStream();
            StreamReader sr = new StreamReader(s, Encoding.GetEncoding(Charset));
            string Content = sr.ReadToEnd();
            sr.Close();
            s.Close();
            return Content;
        }
        /// <summary>
        /// Post实现
        /// </summary>
        /// <returns>Response对象</returns>
        private HttpWebResponse HttpPostMethod()
        {
            HttpWebRequest objHWR = (HttpWebRequest)HttpWebRequest.Create(Url);
            objHWR.Timeout = Timeout;
            objHWR.ContentType = ContentType;
            objHWR.UserAgent = UserAgent;
            objHWR.Accept = Accept;
            objHWR.Referer = Referer;
            objHWR.Method = "POST";
            if (X_FORWARDED_FOR != string.Empty)
            {
                objHWR.Headers.Set("X_FORWARDED_FOR", X_FORWARDED_FOR);
            }

            if (Proxy != null)
            {
                objHWR.Proxy = Proxy;
            }
            if (Cookie != null)
            {
                objHWR.CookieContainer = Cookie;
            }

            byte[] byteData = Encoding.ASCII.GetBytes(PostData);
            objHWR.ContentLength = byteData.Length;
            Stream newStream = objHWR.GetRequestStream();

            // Send the data.
            newStream.Write(byteData, 0, byteData.Length);
            newStream.Close();


            HttpWebResponse objResponse = (HttpWebResponse)objHWR.GetResponse();

            return objResponse;

        }

        /// <summary>
        /// HttpPost()重载返回流的方法
        /// </summary>
        /// <returns></returns>
        public Stream HttpPostStream()
        {

            HttpWebResponse response = HttpPostMethod();
            Stream s = null;
            if (null != response)
            {
                s = response.GetResponseStream();
            }
            return s;
        }

       
        //////////////////////////////////////////////////////////////////////// 
        #region Component Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion
    }

    

    #region RequestCompletedEventArgs 参数类

    public class RequestStringCompletedEventArgs :
        AsyncCompletedEventArgs
    {

      
        private string responseStringValue = string.Empty;

        public RequestStringCompletedEventArgs(
            string responseString,
            Exception e,
            bool canceled,
            object state)
            : base(e, canceled, state)
        {

            this.responseStringValue = responseString;

        }

       

        public string ResponseString
        {
            get
            {
                // Raise an exception if the operation failed or 
                // was canceled.
                RaiseExceptionIfNecessary();

                // If the operation was successful, return the 
                // property value.
                return responseStringValue;
            }
        }


    }

    public class RequestDataCompletedEventArgs :
       AsyncCompletedEventArgs
    {
        private byte[] responseDataValue;
        public RequestDataCompletedEventArgs(
            byte[] responseData,
            Exception e,
            bool canceled,
            object state)
            : base(e, canceled, state)
        {
            this.responseDataValue = responseData;
        }



        public byte[] ResponseData
        {
            get
            {
                
                RaiseExceptionIfNecessary();
                return responseDataValue;
            }
        }


    }
    #endregion RequestStringCompletedEventArgs 参数类

    public enum EnumRequestMethod
    {
        GET=0,
        POST=1
    }


}
