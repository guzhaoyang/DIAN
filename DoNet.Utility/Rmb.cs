// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Rmb
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;

namespace DoNet.Utility
{
  public class Rmb
  {
    public static string CmycurD(Decimal num)
    {
      string str1 = "零壹贰叁肆伍陆柒捌玖";
      string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分";
      string str3 = "";
      string str4 = "";
      int num1 = 0;
      num = Math.Round(Math.Abs(num), 2);
      string str5 = ((long) (num * 100M)).ToString();
      int length = str5.Length;
      if (length > 15)
        return "溢出";
      string str6 = str2.Substring(15 - length);
      for (int startIndex = 0; startIndex < length; ++startIndex)
      {
        string str7 = str5.Substring(startIndex, 1);
        int int32 = Convert.ToInt32(str7);
        string str8;
        if (startIndex != length - 3 && startIndex != length - 7 && startIndex != length - 11 && startIndex != length - 15)
        {
          if (str7 == "0")
          {
            str8 = "";
            str4 = "";
            ++num1;
          }
          else if (str7 != "0" && (uint) num1 > 0U)
          {
            str8 = "零" + str1.Substring(int32, 1);
            str4 = str6.Substring(startIndex, 1);
            num1 = 0;
          }
          else
          {
            str8 = str1.Substring(int32, 1);
            str4 = str6.Substring(startIndex, 1);
            num1 = 0;
          }
        }
        else if (str7 != "0" && (uint) num1 > 0U)
        {
          str8 = "零" + str1.Substring(int32, 1);
          str4 = str6.Substring(startIndex, 1);
          num1 = 0;
        }
        else if (str7 != "0" && num1 == 0)
        {
          str8 = str1.Substring(int32, 1);
          str4 = str6.Substring(startIndex, 1);
          num1 = 0;
        }
        else if (str7 == "0" && num1 >= 3)
        {
          str8 = "";
          str4 = "";
          ++num1;
        }
        else if (length >= 11)
        {
          str8 = "";
          ++num1;
        }
        else
        {
          str8 = "";
          str4 = str6.Substring(startIndex, 1);
          ++num1;
        }
        if (startIndex == length - 11 || startIndex == length - 3)
          str4 = str6.Substring(startIndex, 1);
        str3 = str3 + str8 + str4;
        if (startIndex == length - 1 && str7 == "0")
          str3 += "整";
      }
      if (num == 0M)
        str3 = "零元整";
      return str3;
    }

    public static string CmycurD(string num)
    {
      try
      {
        return Rmb.CmycurD(Convert.ToDecimal(num));
      }
      catch
      {
        return "非数字形式！";
      }
    }
  }
}
