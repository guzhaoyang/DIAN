// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.MailHelper
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace DoNet.Utility
{
  internal class MailHelper
  {
    public static string SendMail(
      string host,
      int port,
      string from,
      string password,
      string to,
      string subject,
      string body)
    {
      host = "smtp.126.com";
      port = 25;
      from = "st_baby@126.com";
      password = "163_sb@0090";
      to = "stone0090@qq.com";
      subject = "测试邮件";
      body = "测试邮件正文";
      string empty = string.Empty;
      try
      {
        new SmtpClient()
        {
          Host = host,
          Port = port,
          Credentials = ((ICredentialsByHost) new NetworkCredential(from, password))
        }.Send(new MailMessage()
        {
          From = new MailAddress(from),
          BodyEncoding = Encoding.UTF8,
          Subject = subject,
          Body = body,
          To = {
            to
          }
        });
      }
      catch (Exception ex)
      {
        empty = ex.ToString();
      }
      return empty;
    }
  }
}
