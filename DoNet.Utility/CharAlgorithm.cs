// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.CharAlgorithm
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Collections.Generic;

namespace DoNet.Utility
{
  public sealed class CharAlgorithm
  {
    public string Random(List<char> listChar, int count)
    {
      string str = "";
      int num = -1;
      System.Random random = new System.Random();
      for (int index1 = 0; index1 < count; ++index1)
      {
        if (num != -1)
          random = new System.Random(num * index1 * (int) DateTime.Now.Ticks);
        int index2 = random.Next(listChar.Count - 1);
        while (num == index2)
          index2 = random.Next(listChar.Count - 1);
        num = index2;
        str += listChar[index2].ToString();
      }
      return str;
    }

    public List<string> Permute(
      List<char> listChar,
      int count,
      string temp = null,
      List<string> result = null)
    {
      if (temp == null)
        temp = string.Empty;
      if (result == null)
        result = new List<string>();
      if (listChar == null || listChar.Count <= 0)
        return result;
      foreach (char ch in listChar)
      {
        string temp1 = temp + ch.ToString();
        if (count == 1)
          result.Add(temp1);
        else
          this.Permute(listChar, --count, temp1, result);
      }
      return result;
    }
  }
}
