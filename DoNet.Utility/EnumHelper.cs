// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.EnumHelper
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Collections.Generic;
using System.Data;

namespace DoNet.Utility
{
  public class EnumHelper
  {
    public static Dictionary<int, string> ToDictionary<T>()
    {
      Type enumType = typeof (T);
      Dictionary<int, string> dictionary = new Dictionary<int, string>();
      foreach (int key in Enum.GetValues(enumType))
        dictionary.Add(key, Enum.GetName(enumType, (object) key));
      return dictionary;
    }

    public static DataTable ToDataTable<T>()
    {
      Type enumType = typeof (T);
      DataTable dataTable = new DataTable();
      dataTable.Columns.Add(new DataColumn("value"));
      dataTable.Columns.Add(new DataColumn("name"));
      foreach (int num in Enum.GetValues(enumType))
      {
        DataRow row = dataTable.NewRow();
        row["value"] = (object) num;
        row["name"] = (object) Enum.GetName(enumType, (object) num);
        dataTable.Rows.Add(row);
      }
      return dataTable;
    }

    public static DataTable ToDataTable<T>(
      EnumHelper.GetDescriptionDelegate<T> getDescription)
    {
      Type enumType = typeof (T);
      DataTable dataTable = new DataTable();
      dataTable.Columns.Add(new DataColumn("value"));
      dataTable.Columns.Add(new DataColumn("name"));
      dataTable.Columns.Add(new DataColumn("description"));
      foreach (int num in Enum.GetValues(enumType))
      {
        DataRow row = dataTable.NewRow();
        row["value"] = (object) num;
        row["name"] = (object) Enum.GetName(enumType, (object) num);
        row["description"] = (object) getDescription((T) Enum.Parse(enumType, num.ToString()));
        dataTable.Rows.Add(row);
      }
      return dataTable;
    }

    public delegate string GetDescriptionDelegate<in T>(T enumValue);
  }
}
