// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.HttpProc.MyWebClient
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DoNet.Utility.HttpProc
{
  public class MyWebClient
  {
    private static CookieContainer _cc;

    static MyWebClient() => MyWebClient.LoadCookiesFromDisk();

    public MyWebClient()
    {
      this.RequestHeaders = new WebHeaderCollection();
      this.ResponseHeaders = new WebHeaderCollection();
    }

    public int BufferSize { get; set; } = 15240;

    public WebHeaderCollection ResponseHeaders { get; private set; }

    public WebHeaderCollection RequestHeaders { get; }

    public WebProxy Proxy { get; set; }

    public Encoding Encoding { get; set; } = Encoding.Default;

    public string RespHtml { get; set; } = "";

    public CookieContainer CookieContainer
    {
      get => MyWebClient._cc;
      set => MyWebClient._cc = value;
    }

    public event EventHandler<UploadEventArgs> UploadProgressChanged;

    public event EventHandler<DownloadEventArgs> DownloadProgressChanged;

    public string GetHtml(string url)
    {
      this.RespHtml = this.Encoding.GetString(this.GetData(this.CreateRequest(url, "GET")));
      return this.RespHtml;
    }

    public void DownloadFile(string url, string filename)
    {
      FileStream fileStream = (FileStream) null;
      try
      {
        byte[] data = this.GetData(this.CreateRequest(url, "GET"));
        fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
        fileStream.Write(data, 0, data.Length);
      }
      finally
      {
        fileStream?.Close();
      }
    }

    public byte[] GetData(string url) => this.GetData(this.CreateRequest(url, "GET"));

    public string Post(string url, string postData)
    {
      byte[] bytes = this.Encoding.GetBytes(postData);
      return this.Post(url, bytes);
    }

    public string Post(string url, byte[] postData)
    {
      HttpWebRequest request = this.CreateRequest(url, "POST");
      request.ContentType = "application/x-www-form-urlencoded";
      request.ContentLength = (long) postData.Length;
      request.KeepAlive = true;
      this.PostData(request, postData);
      this.RespHtml = this.Encoding.GetString(this.GetData(request));
      return this.RespHtml;
    }

    public string Post(string url, MultipartForm mulitpartForm)
    {
      HttpWebRequest request = this.CreateRequest(url, "POST");
      request.ContentType = mulitpartForm.ContentType;
      request.ContentLength = (long) mulitpartForm.FormData.Length;
      request.KeepAlive = true;
      this.PostData(request, mulitpartForm.FormData);
      this.RespHtml = this.Encoding.GetString(this.GetData(request));
      return this.RespHtml;
    }

    private byte[] GetData(HttpWebRequest request)
    {
      HttpWebResponse response = (HttpWebResponse) request.GetResponse();
      Stream responseStream = response.GetResponseStream();
      this.ResponseHeaders = response.Headers;
      DownloadEventArgs e = new DownloadEventArgs();
      if (this.ResponseHeaders[HttpResponseHeader.ContentLength] != null)
        e.TotalBytes = Convert.ToInt32(this.ResponseHeaders[HttpResponseHeader.ContentLength]);
      MemoryStream memoryStream1 = new MemoryStream();
      int num = 0;
      byte[] numArray = new byte[this.BufferSize];
      int length;
      while ((length = responseStream.Read(numArray, 0, numArray.Length)) > 0)
      {
        memoryStream1.Write(numArray, 0, length);
        if (this.DownloadProgressChanged != null)
        {
          e.BytesReceived += length;
          e.ReceivedData = new byte[length];
          Array.Copy((Array) numArray, (Array) e.ReceivedData, length);
          this.DownloadProgressChanged((object) this, e);
        }
      }
      responseStream.Close();
      if (this.ResponseHeaders[HttpResponseHeader.ContentEncoding] != null)
      {
        MemoryStream memoryStream2 = new MemoryStream();
        num = 0;
        byte[] buffer = new byte[100];
        string lower = this.ResponseHeaders[HttpResponseHeader.ContentEncoding].ToLower();
        if (!(lower == "gzip"))
        {
          if (lower == "deflate")
          {
            DeflateStream deflateStream = new DeflateStream((Stream) memoryStream1, CompressionMode.Decompress);
            int count;
            while ((count = deflateStream.Read(buffer, 0, buffer.Length)) > 0)
              memoryStream2.Write(buffer, 0, count);
            return memoryStream2.ToArray();
          }
        }
        else
        {
          GZipStream gzipStream = new GZipStream((Stream) memoryStream1, CompressionMode.Decompress);
          int count;
          while ((count = gzipStream.Read(buffer, 0, buffer.Length)) > 0)
            memoryStream2.Write(buffer, 0, count);
          return memoryStream2.ToArray();
        }
      }
      return memoryStream1.ToArray();
    }

    private void PostData(HttpWebRequest request, byte[] postData)
    {
      int offset = 0;
      int count = this.BufferSize;
      Stream requestStream = request.GetRequestStream();
      UploadEventArgs e = new UploadEventArgs();
      e.TotalBytes = postData.Length;
      int num;
      while ((num = postData.Length - offset) > 0)
      {
        if (count > num)
          count = num;
        requestStream.Write(postData, offset, count);
        offset += count;
        if (this.UploadProgressChanged != null)
        {
          e.BytesSent = offset;
          this.UploadProgressChanged((object) this, e);
        }
      }
      requestStream.Close();
    }

    private HttpWebRequest CreateRequest(string url, string method)
    {
      Uri requestUri = new Uri(url);
      if (requestUri.Scheme == "https")
        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
      HttpWebRequest.DefaultCachePolicy = (RequestCachePolicy) new HttpRequestCachePolicy(HttpRequestCacheLevel.Revalidate);
      HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);
      request.AllowAutoRedirect = false;
      request.AllowWriteStreamBuffering = false;
      request.Method = method;
      if (this.Proxy != null)
        request.Proxy = (IWebProxy) this.Proxy;
      request.CookieContainer = MyWebClient._cc;
      foreach (string key in this.RequestHeaders.Keys)
        request.Headers.Add(key, this.RequestHeaders[key]);
      this.RequestHeaders.Clear();
      return request;
    }

    private bool CheckValidationResult(
      object sender,
      X509Certificate certificate,
      X509Chain chain,
      SslPolicyErrors errors)
    {
      return true;
    }

    private static void SaveCookiesToDisk()
    {
      string path = Environment.GetFolderPath(Environment.SpecialFolder.Cookies) + "\\webclient.cookie";
      FileStream serializationStream = (FileStream) null;
      try
      {
        serializationStream = new FileStream(path, FileMode.Create);
        new BinaryFormatter().Serialize((Stream) serializationStream, (object) MyWebClient._cc);
      }
      finally
      {
        serializationStream?.Close();
      }
    }

    private static void LoadCookiesFromDisk()
    {
      MyWebClient._cc = new CookieContainer();
      string path = Environment.GetFolderPath(Environment.SpecialFolder.Cookies) + "\\webclient.cookie";
      if (!System.IO.File.Exists(path))
        return;
      FileStream serializationStream = (FileStream) null;
      try
      {
        serializationStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        MyWebClient._cc = (CookieContainer) new BinaryFormatter().Deserialize((Stream) serializationStream);
      }
      finally
      {
        serializationStream?.Close();
      }
    }
  }
}
