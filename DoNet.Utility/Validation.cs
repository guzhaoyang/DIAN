// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.Validation
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Collections.Generic;

namespace DoNet.Utility
{
  public static class Validation
  {
    public static ValidationHelper<T> InitValidation<T>(
      this T value,
      string name,
      List<string> errorList)
    {
      return new ValidationHelper<T>(value, name, errorList);
    }

    public static ValidationHelper<string> NotEmpty(
      this ValidationHelper<string> current)
    {
      if (string.IsNullOrEmpty(current.Value))
        current.ErrorList.Add(string.Format("{0}不能为空！", (object) current.Name));
      return current;
    }

    public static ValidationHelper<string> LongerThan(
      this ValidationHelper<string> current,
      int length)
    {
      if (string.IsNullOrEmpty(current.Value) && current.Value.Length < length)
        current.ErrorList.Add(string.Format("{0}的长度不可小于{1}！", (object) current.Name, (object) length));
      return current;
    }

    public static ValidationHelper<string> ShorterThan(
      this ValidationHelper<string> current,
      int length)
    {
      if (string.IsNullOrEmpty(current.Value) && current.Value.Length > length)
        current.ErrorList.Add(string.Format("{0}的长度不可超过{1}！", (object) current.Name, (object) length));
      return current;
    }

    public static ValidationHelper<string> LengthBetween(
      this ValidationHelper<string> current,
      int minLength,
      int maxLength)
    {
      if (string.IsNullOrEmpty(current.Value) && (current.Value.Length < minLength || current.Value.Length > maxLength))
        current.ErrorList.Add(string.Format("{0}的长度必须在{1}和{2}之间！", (object) current.Name, (object) minLength, (object) maxLength));
      return current;
    }

    public static ValidationHelper<int> IsNum(this ValidationHelper<string> current)
    {
      int result;
      if (!int.TryParse(current.Value, out result))
        current.ErrorList.Add(string.Format("{0}必须为数字！", (object) current.Name));
      return new ValidationHelper<int>(result, current.Name, current.ErrorList);
    }

    public static ValidationHelper<int> LargerThan(
      this ValidationHelper<int> current,
      int num)
    {
      if (current.Value < num)
        current.ErrorList.Add(string.Format("{0}的值不可小于{1}！", (object) current.Name, (object) num));
      return current;
    }

    public static ValidationHelper<int> SmallerThan(
      this ValidationHelper<int> current,
      int num)
    {
      if (current.Value > num)
        current.ErrorList.Add(string.Format("{0}的值不可大于{1}！", (object) current.Name, (object) num));
      return current;
    }

    public static ValidationHelper<DateTime> IsDateTime(
      this ValidationHelper<string> current)
    {
      DateTime result;
      if (!DateTime.TryParse(current.Value, out result))
        current.ErrorList.Add(string.Format("{0}必须为时间！", (object) current.Name));
      return new ValidationHelper<DateTime>(result, current.Name, current.ErrorList);
    }

    public static ValidationHelper<DateTime> LargerThan(
      this ValidationHelper<DateTime> current,
      DateTime time)
    {
      if (current.Value < time)
        current.ErrorList.Add(string.Format("{0}不可小于{1}！", (object) current.Name, (object) time));
      return current;
    }

    public static ValidationHelper<DateTime> SmallerThan(
      this ValidationHelper<DateTime> current,
      DateTime time)
    {
      if (current.Value > time)
        current.ErrorList.Add(string.Format("{0}不可大于{1}！", (object) current.Name, (object) time));
      return current;
    }
  }
}
