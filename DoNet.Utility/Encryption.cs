// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Encryption
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DoNet.Utility
{
  public static class Encryption
  {
    private static readonly byte[] StrDesKey = new byte[8]
    {
      (byte) 46,
      (byte) 63,
      (byte) 131,
      (byte) 201,
      (byte) 34,
      (byte) 142,
      (byte) 146,
      (byte) 136
    };
    private static readonly byte[] StrDesIv = new byte[8]
    {
      (byte) 195,
      (byte) 34,
      (byte) 6,
      (byte) 154,
      (byte) 59,
      (byte) 82,
      (byte) 146,
      (byte) 245
    };

    public static bool IsEncrypted(string val) => Regex.IsMatch(val, "^[0-9,a-f,A-F]{2,}$") && (uint) (val.Length % 2) <= 0U;

    public static string Encrypt(string val)
    {
      if (string.IsNullOrEmpty(val))
        return "";
      byte[] numArray = Encryption.DesEncrypt(Encoding.UTF8.GetBytes(val), Encryption.StrDesKey, Encryption.StrDesIv);
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < numArray.Length; ++index)
        stringBuilder.Append(numArray[index].ToString("x2"));
      return stringBuilder.ToString();
    }

    public static string Decrypt(string val)
    {
      if (string.IsNullOrEmpty(val) || !Regex.IsMatch(val, "^[0-9,a-f,A-F]{2,}$"))
        return "";
      using (MemoryStream memoryStream = new MemoryStream(val.Length))
      {
        for (int startIndex = 0; startIndex < val.Length - 1; startIndex += 2)
          memoryStream.WriteByte(byte.Parse(val.Substring(startIndex, 2), NumberStyles.HexNumber));
        return Encoding.UTF8.GetString(Encryption.DesDecrypt(memoryStream.ToArray(), Encryption.StrDesKey, Encryption.StrDesIv));
      }
    }

    private static byte[] DesEncrypt(byte[] srcData, byte[] key, byte[] iv)
    {
      using (DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider())
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, cryptoServiceProvider.CreateEncryptor(key, iv), CryptoStreamMode.Write))
          {
            cryptoStream.Write(srcData, 0, srcData.Length);
            return memoryStream.ToArray();
          }
        }
      }
    }

    private static byte[] DesDecrypt(byte[] cipherData, byte[] key, byte[] iv)
    {
      using (DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider())
      {
        using (MemoryStream memoryStream1 = new MemoryStream(cipherData))
        {
          using (MemoryStream memoryStream2 = new MemoryStream())
          {
            byte[] buffer = new byte[512];
            using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream1, cryptoServiceProvider.CreateDecryptor(key, iv), CryptoStreamMode.Read))
            {
              int count;
              while ((count = cryptoStream.Read(buffer, 0, 512)) > 0)
                memoryStream2.Write(buffer, 0, count);
              return memoryStream2.ToArray();
            }
          }
        }
      }
    }
  }
}
