// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.OtherHelper
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DoNet.Utility
{
  internal class OtherHelper
  {
    public static byte[] Serialize(object data)
    {
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      MemoryStream serializationStream = new MemoryStream();
      binaryFormatter.Serialize((Stream) serializationStream, data);
      return serializationStream.GetBuffer();
    }

    public static object Deserialize(byte[] data) => new BinaryFormatter().Deserialize((Stream) new MemoryStream(data));
  }
}
