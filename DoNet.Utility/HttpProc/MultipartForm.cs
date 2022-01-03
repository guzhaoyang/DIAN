// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.HttpProc.MultipartForm
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using Microsoft.Win32;
using System;
using System.IO;
using System.Text;

namespace DoNet.Utility.HttpProc
{
  public class MultipartForm
  {
    private readonly string _boundary;
    private readonly MemoryStream _ms;
    private byte[] _formData;

    public MultipartForm()
    {
      this._boundary = string.Format("--{0}--", (object) Guid.NewGuid());
      this._ms = new MemoryStream();
      this.StringEncoding = Encoding.Default;
    }

    public byte[] FormData
    {
      get
      {
        if (this._formData == null)
        {
          byte[] bytes = this.StringEncoding.GetBytes("--" + this._boundary + "--\r\n");
          this._ms.Write(bytes, 0, bytes.Length);
          this._formData = this._ms.ToArray();
        }
        return this._formData;
      }
    }

    public string ContentType => string.Format("multipart/form-data; boundary={0}", (object) this._boundary);

    public Encoding StringEncoding { set; get; }

    public void AddFlie(string name, string filename)
    {
      if (!File.Exists(filename))
        throw new FileNotFoundException("尝试添加不存在的文件。", filename);
      using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        byte[] numArray = new byte[fileStream.Length];
        fileStream.Read(numArray, 0, numArray.Length);
        this.AddFlie(name, Path.GetFileName(filename), numArray, numArray.Length);
      }
    }

    public void AddFlie(string name, string filename, byte[] fileData, int dataLength)
    {
      if (dataLength <= 0 || dataLength > fileData.Length)
        dataLength = fileData.Length;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("--{0}\r\n", (object) this._boundary);
      stringBuilder.AppendFormat("Content-Disposition: form-data; name=\"{0}\";filename=\"{1}\"\r\n", (object) name, (object) filename);
      stringBuilder.AppendFormat("Content-Type: {0}\r\n", (object) this.GetContentType(filename));
      stringBuilder.Append("\r\n");
      byte[] bytes1 = this.StringEncoding.GetBytes(stringBuilder.ToString());
      this._ms.Write(bytes1, 0, bytes1.Length);
      this._ms.Write(fileData, 0, dataLength);
      byte[] bytes2 = this.StringEncoding.GetBytes("\r\n");
      this._ms.Write(bytes2, 0, bytes2.Length);
    }

    public void AddString(string name, string value)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("--{0}\r\n", (object) this._boundary);
      stringBuilder.AppendFormat("Content-Disposition: form-data; name=\"{0}\"\r\n", (object) name);
      stringBuilder.Append("\r\n");
      stringBuilder.AppendFormat("{0}\r\n", (object) value);
      byte[] bytes = this.StringEncoding.GetBytes(stringBuilder.ToString());
      this._ms.Write(bytes, 0, bytes.Length);
    }

    private string GetContentType(string filename)
    {
      string defaultValue = "application/stream";
      if (filename == null)
        return defaultValue;
      using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(Path.GetExtension(filename)))
      {
        if (registryKey != null)
          defaultValue = registryKey.GetValue("Content Type", (object) defaultValue).ToString();
      }
      return defaultValue;
    }
  }
}
