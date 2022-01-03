// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Validation2
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace DoNet.Utility
{
  public class Validation2
  {
    private static readonly Regex RegPhone = new Regex("^[0-9]+[-]?[0-9]+[-]?[0-9]$");
    private static readonly Regex RegNumber = new Regex("^[0-9]+$");
    private static readonly Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
    private static readonly Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
    private static readonly Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$");
    private static readonly Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");
    private static readonly Regex RegCHZN = new Regex("[一-龥]");

    public static bool IsHasCHZN(string inputData) => Validation2.RegCHZN.Match(inputData).Success;

    public static bool IsEmail(string inputData) => Validation2.RegEmail.Match(inputData).Success;

    public static bool IsDateTime(string str)
    {
      try
      {
        if (string.IsNullOrEmpty(str))
          return false;
        DateTime.Parse(str);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static bool IsContainSpecChar(string strInput)
    {
      string[] strArray = new string[2]
      {
        "123456",
        "654321"
      };
      bool flag = false;
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strInput == strArray[index])
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    public static bool IsPhone(string inputData) => Validation2.RegPhone.Match(inputData).Success;

    public static string FetchInputDigit(HttpRequest req, string inputKey, int maxLen)
    {
      string str = string.Empty;
      if (!string.IsNullOrEmpty(inputKey))
      {
        str = req.QueryString[inputKey] ?? req.Form[inputKey];
        if (str != null)
        {
          str = Validation2.SqlText(str, maxLen);
          if (!Validation2.IsNumber(str))
            str = string.Empty;
        }
      }
      if (str == null)
        str = string.Empty;
      return str;
    }

    public static bool IsNumber(string inputData) => Validation2.RegNumber.Match(inputData).Success;

    public static bool IsNumberSign(string inputData) => Validation2.RegNumberSign.Match(inputData).Success;

    public static bool IsDecimal(string inputData) => Validation2.RegDecimal.Match(inputData).Success;

    public static bool IsDecimalSign(string inputData) => Validation2.RegDecimalSign.Match(inputData).Success;

    public static string SqlText(string sqlInput, int maxLength)
    {
      if (sqlInput != null && sqlInput != string.Empty)
      {
        sqlInput = sqlInput.Trim();
        if (sqlInput.Length > maxLength)
          sqlInput = sqlInput.Substring(0, maxLength);
      }
      return sqlInput;
    }

    public static string HtmlEncode(string inputData) => HttpUtility.HtmlEncode(inputData);

    public static void SetLabel(Label lbl, string txtInput) => lbl.Text = Validation2.HtmlEncode(txtInput);

    public static void SetLabel(Label lbl, object inputObj) => Validation2.SetLabel(lbl, inputObj.ToString());

    public static string InputText(string inputString, int maxLength)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (!string.IsNullOrEmpty(inputString))
      {
        inputString = inputString.Trim();
        if (inputString.Length > maxLength)
          inputString = inputString.Substring(0, maxLength);
        for (int index = 0; index < inputString.Length; ++index)
        {
          switch (inputString[index])
          {
            case '"':
              stringBuilder.Append("&quot;");
              break;
            case '<':
              stringBuilder.Append("&lt;");
              break;
            case '>':
              stringBuilder.Append("&gt;");
              break;
            default:
              stringBuilder.Append(inputString[index]);
              break;
          }
        }
        stringBuilder.Replace("'", " ");
      }
      return stringBuilder.ToString();
    }

    public static string Encode(string str)
    {
      str = str.Replace("&", "&amp;");
      str = str.Replace("'", "''");
      str = str.Replace("\"", "&quot;");
      str = str.Replace(" ", "&nbsp;");
      str = str.Replace("<", "&lt;");
      str = str.Replace(">", "&gt;");
      str = str.Replace("\n", "<br>");
      return str;
    }

    public static string Decode(string str)
    {
      str = str.Replace("<br>", "\n");
      str = str.Replace("&gt;", ">");
      str = str.Replace("&lt;", "<");
      str = str.Replace("&nbsp;", " ");
      str = str.Replace("&quot;", "\"");
      return str;
    }

    public static string SqlTextClear(string sqlText)
    {
      switch (sqlText)
      {
        case "":
          return "";
        case null:
          return (string) null;
        default:
          sqlText = sqlText.Replace(",", "");
          sqlText = sqlText.Replace("<", "");
          sqlText = sqlText.Replace(">", "");
          sqlText = sqlText.Replace("--", "");
          sqlText = sqlText.Replace("'", "");
          sqlText = sqlText.Replace("\"", "");
          sqlText = sqlText.Replace("=", "");
          sqlText = sqlText.Replace("%", "");
          sqlText = sqlText.Replace(" ", "");
          return sqlText;
      }
    }

    public static bool IsContainSameChar(string strInput)
    {
      string charInput = string.Empty;
      if (!string.IsNullOrEmpty(strInput))
        charInput = strInput.Substring(0, 1);
      return strInput != null && Validation2.IsContainSameChar(strInput, charInput, strInput.Length);
    }

    public static bool IsContainSameChar(string strInput, string charInput, int lenInput) => !string.IsNullOrEmpty(charInput) && new Regex(string.Format("^([{0}])+$", (object) charInput)).Match(strInput).Success;
  }
}
