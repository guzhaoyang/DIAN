﻿// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.HttpProc.UploadEventArgs
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;

namespace DoNet.Utility.HttpProc
{
  public class UploadEventArgs : EventArgs
  {
    public int BytesSent { get; set; }

    public int TotalBytes { get; set; }
  }
}
