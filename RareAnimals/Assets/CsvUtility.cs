using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

public class CsvUtility
{
    /// <summary>
    /// Csv内容的分隔符
    /// </summary>
    public const char DEFAULD_FIELD_SEPARATOR = ',';

    public delegate T Parser<T>(List<string> lineFields);

    public static List<T> DoParseByName<T>(string cfgName, string[] colNames)
    {
        //string path = UnityEngine.Application.dataPath + "\\Resources\\Texts\\" + cfgName;
        string path = UnityEngine.Application.streamingAssetsPath + "\\Configs\\csv\\" + cfgName;
        if(!File.Exists(path))
        {
            //Logger.Log(path + " is not exist!!!");
            return null;
        }
        else
        {
            return DoParseByFilePath<T>(path, colNames);
        }
    }

    public static List<T> DoParseByFilePath<T>(string path, string[] colNames)
    {
        return DoParseByReader<T>(typeof(T), colNames, File.OpenText(path));
    }

    /// <summary>
    /// 转换
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="colNames"></param>
    /// <param name="fieldValues"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static T ReflectionParser<T>(string[] colNames, string[] fieldValues, Type type)
    {
        //使用指定类型的默认构造函数来创建该类型的实例
        T t = (T)System.Activator.CreateInstance(type);
        for (int i = 0; i < colNames.Length; i++)
        {
            //#if UNITY_EDITOR
            try
            {
                //#endif
                if (colNames[i] == null)
                {
                    continue;
                }
                PropertyInfo property = t.GetType().GetProperty(colNames[i]);
                if (string.IsNullOrEmpty(fieldValues[i]))
                {
                    Type propertyType = property.PropertyType;
                    if (propertyType == typeof(string))
                    {
                        property.SetValue(t, "", null);
                    }
                    else if (propertyType == typeof(int) || propertyType == typeof(short) ||
                              propertyType == typeof(byte) || propertyType == typeof(float) ||
                              propertyType == typeof(double))
                    {
                        property.SetValue(t, 0, null);
                    }
                    else
                    {
                        UnityEngine.Debug.Log("Can not found default value for " + propertyType);
                    }
                }
                else
                {
                    property.SetValue(t, Convert.ChangeType(fieldValues[i], property.PropertyType), null);
                }
                //#if UNITY_EDITOR
            }
            catch (Exception e)
            {
                //Logger.Error("Error at parse for `" + typeof(T) + "` column index=" + i + ", column name=" + colNames[i] + ", value=" + fieldValues[i] + " value in #0 column=" + fieldValues[0] + ", error=" + e);
            }
            //#endif
        }
        return t;
    }

    private static List<T> DoParseByReader<T>(Type type, string[] colNames, StreamReader reader)
    {
        return DoParse<T>(type, colNames, DEFAULD_FIELD_SEPARATOR, reader);
    }

    private static List<T> DoParseByReader<T>(char fieldSeparator, StreamReader reader, Parser<T> parser)
    {
        string line = null;
        List<T> list = new List<T>();
        // 去掉第一行数据
        line = reader.ReadLine();

        while ((line = reader.ReadLine()) != null)
        {
            list.Add(processLine(fieldSeparator, line, parser));
        }
        return list;
    }

    private static T processLine<T>(char fieldSeparator, string line, Parser<T> parser)
    {
        List<string> lineFields = new List<string>();
        lineFields.AddRange(line.Split(fieldSeparator));
        return parser(lineFields);
    }
    private static List<T> DoParse<T>(Type type, string[] colNames, char fieldSeparator, StreamReader reader)
    {
        return DoParseByReader(fieldSeparator, reader, delegate (List<string> lineFields) {
            return ReflectionParser<T>(colNames, lineFields.ToArray(), type);
        });
    }

}