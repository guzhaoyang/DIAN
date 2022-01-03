// Decompiled with JetBrains decompiler
// Type: DoNet.Utility.DataTableHelper
// Assembly: DoNet.Utility, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD5DA16A-6F4C-4458-AC23-48055BBFFF84
// Assembly location: D:\Dian.Web\bin\DoNet.Utility.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace DoNet.Utility
{
  public static class DataTableHelper
  {
    public static T DataRowToEntity<T>(DataRow tableRow) where T : new()
    {
      Type type = typeof (T);
      T entity = new T();
      foreach (DataColumn column in (InternalDataCollectionBase) tableRow.Table.Columns)
      {
        string columnName = column.ColumnName;
        PropertyInfo property = type.GetProperty(columnName.ToLower(), BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
        if (property != null)
        {
          object obj = tableRow[columnName];
          if (Nullable.GetUnderlyingType(property.PropertyType) != null)
          {
            obj = !(obj is DBNull) ? Convert.ChangeType(obj, Nullable.GetUnderlyingType(property.PropertyType)) : (object) null;
          }
          else
          {
            try
            {
              obj = Convert.ChangeType(obj, property.PropertyType);
            }
            catch (Exception ex)
            {
            }
          }
          try
          {
            property.SetValue((object) entity, obj, (object[]) null);
          }
          catch (Exception ex)
          {
          }
        }
      }
      return entity;
    }

    public static List<T> DataTableToList<T>(DataTable table) where T : new()
    {
      List<T> list = new List<T>();
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
      {
        Type type = typeof (T);
        T obj1 = new T();
        foreach (DataColumn column in (InternalDataCollectionBase) table.Columns)
        {
          string columnName = column.ColumnName;
          PropertyInfo property = type.GetProperty(columnName.ToLower(), BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
          if (property != null)
          {
            object obj2 = row[columnName];
            object obj3 = Nullable.GetUnderlyingType(property.PropertyType) == null ? Convert.ChangeType(obj2, property.PropertyType) : (!(obj2 is DBNull) ? Convert.ChangeType(obj2, Nullable.GetUnderlyingType(property.PropertyType)) : (object) null);
            property.SetValue((object) obj1, obj3, (object[]) null);
          }
        }
        list.Add(obj1);
      }
      return list;
    }

    public static DataTable GetDataTableSchema<T>()
    {
      PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof (T));
      DataTable dataTableSchema = new DataTable();
      for (int index = 0; index < properties.Count; ++index)
      {
        PropertyDescriptor propertyDescriptor = properties[index];
        Type type = propertyDescriptor.PropertyType;
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>))
          type = Nullable.GetUnderlyingType(type);
        dataTableSchema.Columns.Add(propertyDescriptor.Name, type);
      }
      return dataTableSchema;
    }

    public static DataTable ListToDataTable<T>(List<T> data)
    {
      PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof (T));
      DataTable dataTableSchema = DataTableHelper.GetDataTableSchema<T>();
      object[] objArray = new object[properties.Count];
      foreach (T component in data)
      {
        for (int index = 0; index < objArray.Length; ++index)
          objArray[index] = properties[index].GetValue((object) component);
        dataTableSchema.Rows.Add(objArray);
      }
      dataTableSchema.AcceptChanges();
      return dataTableSchema;
    }

    public static void RemoveDbNullColumn<T>(T customRow) where T : class
    {
      foreach (PropertyInfo property in typeof (T).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty))
      {
        try
        {
          property.GetValue((object) customRow, (object[]) null);
        }
        catch (Exception ex)
        {
          property.SetValue((object) customRow, DataTableHelper.GetDefaultValue(property.PropertyType.FullName.ToLower()), (object[]) null);
        }
      }
    }

    public static void RemoveDbNullColumn<T>(T newCustomRow, T oldCustomRow) where T : class
    {
      foreach (PropertyInfo property in typeof (T).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty))
      {
        try
        {
          property.GetValue((object) newCustomRow, (object[]) null);
        }
        catch (Exception ex1)
        {
          try
          {
            property.SetValue((object) newCustomRow, property.GetValue((object) oldCustomRow, (object[]) null), (object[]) null);
          }
          catch (Exception ex2)
          {
            property.SetValue((object) newCustomRow, DataTableHelper.GetDefaultValue(property.PropertyType.FullName.ToLower()), (object[]) null);
          }
        }
      }
    }

    private static object GetDefaultValue(string type)
    {
      object defaultValue;
      switch (type)
      {
        case "system.boolean":
          defaultValue = (object) false;
          break;
        case "system.char":
          defaultValue = (object) char.MinValue;
          break;
        case "system.datetime":
          defaultValue = (object) DateTime.Parse("1900-01-01");
          break;
        case "system.double":
          defaultValue = (object) double.MinValue;
          break;
        case "system.guid":
          defaultValue = (object) Guid.Empty;
          break;
        case "system.int16":
        case "system.int32":
        case "system.int64":
          defaultValue = (object) int.MinValue;
          break;
        case "system.single":
          defaultValue = (object) float.MinValue;
          break;
        case "system.string":
          defaultValue = (object) string.Empty;
          break;
        default:
          defaultValue = new object();
          break;
      }
      return defaultValue;
    }
  }
}
