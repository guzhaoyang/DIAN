// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.JsonHelper
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DoNet.Utility
{
  public class JsonHelper
  {
    public static string ToJson(DataTable dt)
    {
      if (dt == null)
        throw new ArgumentNullException(nameof (dt));
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("[");
        for (int index1 = 0; index1 < dt.Rows.Count; ++index1)
        {
          stringBuilder.Append("{");
          for (int index2 = 0; index2 < dt.Columns.Count; ++index2)
          {
            stringBuilder.Append("\"");
            stringBuilder.Append(dt.Columns[index2].ColumnName);
            stringBuilder.Append("\":\"");
            stringBuilder.Append(dt.Rows[index1][index2]);
            stringBuilder.Append("\",");
          }
          stringBuilder.Remove(stringBuilder.Length - 1, 1);
          stringBuilder.Append("},");
        }
        if (dt.Rows.Count > 0)
          stringBuilder.Remove(stringBuilder.Length - 1, 1);
        stringBuilder.Append("]");
        return stringBuilder.ToString();
      }
      catch (Exception ex)
      {
        return string.Empty;
      }
    }

    public static string ToJson(DataTable dt, int rowCount)
    {
      if (dt == null)
        throw new ArgumentNullException(nameof (dt));
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("{\"total\":" + (object) rowCount + ",\"rows\":[");
        for (int index1 = 0; index1 < dt.Rows.Count; ++index1)
        {
          stringBuilder.Append("{");
          for (int index2 = 0; index2 < dt.Columns.Count; ++index2)
          {
            stringBuilder.Append("\"");
            stringBuilder.Append(dt.Columns[index2].ColumnName);
            stringBuilder.Append("\":\"");
            stringBuilder.Append(dt.Rows[index1][index2]);
            stringBuilder.Append("\",");
          }
          stringBuilder.Remove(stringBuilder.Length - 1, 1);
          stringBuilder.Append("},");
        }
        if (dt.Rows.Count > 0)
          stringBuilder.Remove(stringBuilder.Length - 1, 1);
        stringBuilder.Append("]}");
        return stringBuilder.ToString();
      }
      catch (Exception ex)
      {
        return string.Empty;
      }
    }

    public static string ToJson(DataTable dt, int rowCount, Dictionary<string, string> dic)
    {
      if (dt == null)
        throw new ArgumentNullException(nameof (dt));
      if (dic == null)
        throw new ArgumentNullException(nameof (rowCount));
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("{\"total\":" + (object) rowCount + ",\"rows\":[");
        for (int index1 = 0; index1 < dt.Rows.Count; ++index1)
        {
          stringBuilder.Append("{");
          for (int index2 = 0; index2 < dt.Columns.Count; ++index2)
          {
            stringBuilder.Append("\"");
            stringBuilder.Append(dt.Columns[index2].ColumnName);
            stringBuilder.Append("\":\"");
            stringBuilder.Append(dt.Rows[index1][index2]);
            stringBuilder.Append("\",");
          }
          foreach (KeyValuePair<string, string> keyValuePair in dic)
          {
            stringBuilder.Append("\"");
            stringBuilder.Append(keyValuePair.Key);
            stringBuilder.Append("\":\"");
            stringBuilder.Append(keyValuePair.Value);
            stringBuilder.Append("\",");
          }
          stringBuilder.Remove(stringBuilder.Length - 1, 1);
          stringBuilder.Append("},");
        }
        if (dt.Rows.Count > 0)
          stringBuilder.Remove(stringBuilder.Length - 1, 1);
        stringBuilder.Append("]}");
        return stringBuilder.ToString();
      }
      catch (Exception ex)
      {
        return string.Empty;
      }
    }

    public static string ToJson(DataSet ds)
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("{");
        foreach (DataTable table in (InternalDataCollectionBase) ds.Tables)
        {
          stringBuilder.Append("\"");
          stringBuilder.Append(table.TableName);
          stringBuilder.Append("\":");
          stringBuilder.Append(JsonHelper.ToJson(table));
          stringBuilder.Append(",");
        }
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        stringBuilder.Append("}");
        return stringBuilder.ToString();
      }
      catch (Exception ex)
      {
        return string.Empty;
      }
    }

    public static string ToJson<T>(IList<T> list)
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("[");
        if (list.Count > 0)
        {
          for (int index1 = 0; index1 < list.Count; ++index1)
          {
            PropertyInfo[] properties = Activator.CreateInstance<T>().GetType().GetProperties();
            stringBuilder.Append("{");
            for (int index2 = 0; index2 < properties.Length; ++index2)
            {
              stringBuilder.Append("\"" + properties[index2].Name + "\":\"" + properties[index2].GetValue((object) list[index1], (object[]) null) + "\"");
              if (index2 < properties.Length - 1)
                stringBuilder.Append(",");
            }
            stringBuilder.Append("}");
            if (index1 < list.Count - 1)
              stringBuilder.Append(",");
          }
        }
        stringBuilder.Append("]");
        return stringBuilder.ToString();
      }
      catch (Exception ex)
      {
        return string.Empty;
      }
    }

    public static string ToJson(IList list)
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("[");
        if (list.Count > 0)
        {
          for (int index1 = 0; index1 < list.Count; ++index1)
          {
            PropertyInfo[] properties = list[index1].GetType().GetProperties();
            stringBuilder.Append("{");
            for (int index2 = 0; index2 < properties.Length; ++index2)
            {
              stringBuilder.Append("\"" + properties[index2].Name + "\":\"" + properties[index2].GetValue(list[index1], (object[]) null) + "\"");
              if (index2 < properties.Length - 1)
                stringBuilder.Append(",");
            }
            stringBuilder.Append("}");
            if (index1 < list.Count - 1)
              stringBuilder.Append(",");
          }
        }
        stringBuilder.Append("]");
        return stringBuilder.ToString();
      }
      catch (Exception ex)
      {
        return string.Empty;
      }
    }

    public static string ToJson(object jsonObject)
    {
      try
      {
        string str1 = "{";
        PropertyInfo[] properties = jsonObject.GetType().GetProperties();
        for (int index = 0; index < properties.Length; ++index)
        {
          object array = properties[index].GetGetMethod().Invoke(jsonObject, (object[]) null);
          string empty = string.Empty;
          int num;
          switch (array)
          {
            case DateTime _:
            case Guid _:
              num = 1;
              break;
            default:
              num = array is TimeSpan ? 1 : 0;
              break;
          }
          string str2;
          if (num != 0)
          {
            str2 = "'" + array + "'";
          }
          else
          {
            switch (array)
            {
              case string _:
                str2 = "'" + JsonHelper.ToJson(array.ToString()) + "'";
                break;
              case IEnumerable _:
                str2 = JsonHelper.ToJson((IEnumerable) array);
                break;
              default:
                str2 = JsonHelper.ToJson(array.ToString());
                break;
            }
          }
          str1 = str1 + "\"" + JsonHelper.ToJson(properties[index].Name) + "\":" + str2 + ",";
        }
        return JsonHelper.DeleteLast(str1) + "}";
      }
      catch (Exception ex)
      {
        return string.Empty;
      }
    }

    public static string ToJson(IEnumerable array)
    {
      try
      {
        string str = "[";
        foreach (object jsonObject in array)
          str = str + JsonHelper.ToJson(jsonObject) + ",";
        return JsonHelper.DeleteLast(str) + "]";
      }
      catch (Exception ex)
      {
        return string.Empty;
      }
    }

    public static string ToJson2(DataTable table)
    {
      try
      {
        string str1 = "[";
        DataRowCollection rows = table.Rows;
        for (int index = 0; index < rows.Count; ++index)
        {
          string str2 = str1 + "{";
          foreach (DataColumn column in (InternalDataCollectionBase) table.Columns)
          {
            str2 = str2 + "\"" + JsonHelper.ToJson(column.ColumnName) + "\":";
            str2 = column.DataType != typeof (DateTime) && column.DataType != typeof (string) ? str2 + JsonHelper.ToJson(rows[index][column.ColumnName].ToString()) + "," : str2 + "\"" + JsonHelper.ToJson(rows[index][column.ColumnName].ToString()) + "\",";
          }
          str1 = JsonHelper.DeleteLast(str2) + "},";
        }
        return JsonHelper.DeleteLast(str1) + "]";
      }
      catch (Exception ex)
      {
        return string.Empty;
      }
    }

    public static string ToJson(DbDataReader dataReader)
    {
      try
      {
        string str1 = "[";
        while (dataReader.Read())
        {
          string str2 = str1 + "{";
          for (int ordinal = 0; ordinal < dataReader.FieldCount; ++ordinal)
          {
            string str3 = str2 + "\"" + JsonHelper.ToJson(dataReader.GetName(ordinal)) + "\":";
            str2 = dataReader.GetFieldType(ordinal) != typeof (DateTime) && dataReader.GetFieldType(ordinal) != typeof (string) ? str3 + JsonHelper.ToJson(dataReader[ordinal].ToString()) + "," : str3 + "\"" + JsonHelper.ToJson(dataReader[ordinal].ToString()) + "\",";
          }
          str1 = JsonHelper.DeleteLast(str2) + "}";
        }
        dataReader.Close();
        return JsonHelper.DeleteLast(str1) + "]";
      }
      catch (Exception ex)
      {
        return string.Empty;
      }
    }

    public static string ToJson2(DataSet dataSet)
    {
      try
      {
        string str1 = "{";
        foreach (DataTable table in (InternalDataCollectionBase) dataSet.Tables)
          str1 = str1 + "\"" + JsonHelper.ToJson(table.TableName) + "\":" + JsonHelper.ToJson(table) + ",";
        string str2;
        return str2 = JsonHelper.DeleteLast(str1) + "}";
      }
      catch (Exception ex)
      {
        return string.Empty;
      }
    }

    public static string ToJson(string value)
    {
      try
      {
        return string.IsNullOrEmpty(value) ? string.Empty : value.Replace("{", "｛").Replace("}", "｝").Replace(":", "：").Replace(",", "，").Replace("[", "【").Replace("]", "】").Replace(";", "；").Replace("\n", "<br/>").Replace("\r", "").Replace("\t", " ").Replace("'", "'").Replace("\\", "\\\\").Replace("\"", "\"\"");
      }
      catch (Exception ex)
      {
        return string.Empty;
      }
    }

    public static string ToJson<T>(T obj)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        new DataContractJsonSerializer(typeof (T)).WriteObject((Stream) memoryStream, (object) obj);
        return Encoding.UTF8.GetString(memoryStream.ToArray());
      }
    }

    public static T ToObject<T>(string json) where T : class
    {
      DataContractJsonSerializer contractJsonSerializer = new DataContractJsonSerializer(typeof (T));
      MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
      T obj = (T) contractJsonSerializer.ReadObject((Stream) memoryStream);
      memoryStream.Close();
      return obj;
    }

    private static string DeleteLast(string str)
    {
      try
      {
        return str.Length > 1 ? str.Substring(0, str.Length - 1) : str;
      }
      catch (Exception ex)
      {
        return string.Empty;
      }
    }
  }
}
