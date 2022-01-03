// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.ValidationHelper`1
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Collections.Generic;

namespace DoNet.Utility
{
  public class ValidationHelper<T>
  {
    public ValidationHelper(T value, string name, List<string> errorList)
    {
      this.Value = value;
      this.Name = name;
      this.ErrorList = errorList ?? new List<string>();
    }

    public T Value { get; }

    public string Name { get; }

    public List<string> ErrorList { get; set; }

    public bool NotDefault() => !this.Value.Equals((object) default (T));

    public ValidationHelper<T> CustomRule(Action<T, string> rule)
    {
      rule(this.Value, this.Name);
      return this;
    }
  }
}
