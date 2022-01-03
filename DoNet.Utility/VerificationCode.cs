// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.VerificationCode
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Threading;

namespace DoNet.Utility
{
  public class VerificationCode
  {
    public static string Number(int length) => VerificationCode.Number(length, false);

    public static string Number(int length, bool sleep)
    {
      if (sleep)
        Thread.Sleep(3);
      string str = "";
      Random random = new Random();
      for (int index = 0; index < length; ++index)
        str += random.Next(10).ToString();
      return str;
    }

    public static string Str(int length) => VerificationCode.Str(length, false);

    public static string Str(int length, bool sleep)
    {
      if (sleep)
        Thread.Sleep(3);
      char[] chArray = new char[36]
      {
        '0',
        '1',
        '2',
        '3',
        '4',
        '5',
        '6',
        '7',
        '8',
        '9',
        'A',
        'B',
        'C',
        'D',
        'E',
        'F',
        'G',
        'H',
        'I',
        'J',
        'K',
        'L',
        'M',
        'N',
        'O',
        'P',
        'Q',
        'R',
        'S',
        'T',
        'U',
        'V',
        'W',
        'X',
        'Y',
        'Z'
      };
      string str = "";
      int length1 = chArray.Length;
      Random random = new Random(~(int) DateTime.Now.Ticks);
      for (int index1 = 0; index1 < length; ++index1)
      {
        int index2 = random.Next(0, length1);
        str += chArray[index2].ToString();
      }
      return str;
    }

    public static string Str_char(int length) => VerificationCode.Str_char(length, false);

    public static string Str_char(int length, bool sleep)
    {
      if (sleep)
        Thread.Sleep(3);
      char[] chArray = new char[26]
      {
        'A',
        'B',
        'C',
        'D',
        'E',
        'F',
        'G',
        'H',
        'I',
        'J',
        'K',
        'L',
        'M',
        'N',
        'O',
        'P',
        'Q',
        'R',
        'S',
        'T',
        'U',
        'V',
        'W',
        'X',
        'Y',
        'Z'
      };
      string str = "";
      int length1 = chArray.Length;
      Random random = new Random(~(int) DateTime.Now.Ticks);
      for (int index1 = 0; index1 < length; ++index1)
      {
        int index2 = random.Next(0, length1);
        str += chArray[index2].ToString();
      }
      return str;
    }
  }
}
