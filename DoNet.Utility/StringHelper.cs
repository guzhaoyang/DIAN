// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.StringHelper
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System.Collections.Generic;
using System.Text;

namespace DoNet.Utility
{
  public class StringHelper
  {
    public static List<string> GetStrArray(string str, char speater, bool toLower)
    {
      List<string> strArray = new List<string>();
      string str1 = str;
      char[] chArray = new char[1]{ speater };
      foreach (string str2 in str1.Split(chArray))
      {
        if (!string.IsNullOrEmpty(str2) && str2 != speater.ToString())
        {
          string str3 = str2;
          if (toLower)
            str3 = str2.ToLower();
          strArray.Add(str3);
        }
      }
      return strArray;
    }

    public static string[] GetStrArray(string str) => str.Split(new char[44]);

    public static string GetArrayStr(List<string> list, string speater)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < list.Count; ++index)
      {
        if (index == list.Count - 1)
        {
          stringBuilder.Append(list[index]);
        }
        else
        {
          stringBuilder.Append(list[index]);
          stringBuilder.Append(speater);
        }
      }
      return stringBuilder.ToString();
    }

    public static string DelLastComma(string str) => str.Substring(0, str.LastIndexOf(","));

    public static string DelLastChar(string str, string strchar) => str.Substring(0, str.LastIndexOf(strchar));

    public static string ToSBC(string input)
    {
      char[] charArray = input.ToCharArray();
      for (int index = 0; index < charArray.Length; ++index)
      {
        if (charArray[index] == ' ')
          charArray[index] = '　';
        else if (charArray[index] < '\u007F')
          charArray[index] = (char) ((uint) charArray[index] + 65248U);
      }
      return new string(charArray);
    }

    public static string ToDBC(string input)
    {
      char[] charArray = input.ToCharArray();
      for (int index = 0; index < charArray.Length; ++index)
      {
        if (charArray[index] == '　')
          charArray[index] = ' ';
        else if (charArray[index] > '\uFF00' && charArray[index] < '｟')
          charArray[index] = (char) ((uint) charArray[index] - 65248U);
      }
      return new string(charArray);
    }
  }
}
