// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.HttpProc.FileUpload
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace DoNet.Utility.HttpProc
{
  public class FileUpload
  {
    public FileUpload(string userName, string password)
    {
      this.FileServerName = userName;
      this.FileServerPassword = password;
    }

    public string FileServerName { get; set; }

    public string FileServerPassword { get; set; }

    public string[] GetFiles(string filePathUrl)
    {
      if (string.IsNullOrEmpty(filePathUrl))
        return (string[]) null;
      using (WebClient webClient = this.GetWebClient())
      {
        MatchCollection matchCollection = Regex.Matches(Encoding.UTF8.GetString(webClient.DownloadData(filePathUrl)).ToLower(), "href=\"([^\\.><]{1,}\\.[^\\.><]{1,})\"");
        List<string> stringList = new List<string>(1024);
        if (matchCollection.Count > 0)
        {
          for (int i = 0; i < matchCollection.Count; ++i)
          {
            string str = matchCollection[i].Groups[1].Value;
            int num = str.LastIndexOf("/", StringComparison.Ordinal);
            if (num >= 0)
              str = str.Substring(num + 1);
            stringList.Add(str);
          }
        }
        return stringList.Count == 0 ? (string[]) null : stringList.ToArray();
      }
    }

    public byte[] ReadFileFromServer(string fileNameUrl)
    {
      if (string.IsNullOrEmpty(fileNameUrl))
        return (byte[]) null;
      try
      {
        using (WebClient webClient = this.GetWebClient())
          return webClient.DownloadData(fileNameUrl);
      }
      catch (Exception ex)
      {
        return (byte[]) null;
      }
    }

    public void UploadFileToServer(string fileNameUrl, byte[] fileData)
    {
      this.CheckCreatFolder(this.GetFatherFolder(fileNameUrl));
      using (WebClient webClient = this.GetWebClient())
        webClient.UploadData(fileNameUrl, "PUT", fileData);
    }

    private WebClient GetWebClient() => new WebClient()
    {
      Credentials = (ICredentials) new NetworkCredential(this.FileServerName, this.FileServerPassword)
    };

    private void CheckCreatFolder(string url)
    {
      Stack<string> stringStack = new Stack<string>(16);
      if (url.EndsWith("/"))
        url = url.Remove(url.Length - 1, 1);
      int length;
      for (; !this.CheckFolderExist(url); url = url.Substring(0, length))
      {
        stringStack.Push(url);
        length = url.LastIndexOf("/");
      }
      while (stringStack.Count > 0)
        this.CreatFolder(stringStack.Pop());
    }

    private bool CheckFolderExist(string url)
    {
      try
      {
        using (WebClient webClient = this.GetWebClient())
          webClient.DownloadData(url);
        return true;
      }
      catch
      {
        return false;
      }
    }

    private string GetFatherFolder(string url)
    {
      int num = url.LastIndexOf(".");
      int length = url.LastIndexOf("/");
      return num < 0 || num < length ? url : url.Substring(0, length);
    }

    private void CreatFolder(string url)
    {
      try
      {
        using (WebClient webClient = this.GetWebClient())
          webClient.UploadData(url, "MKCOL", new byte[0]);
      }
      catch
      {
      }
    }
  }
}
