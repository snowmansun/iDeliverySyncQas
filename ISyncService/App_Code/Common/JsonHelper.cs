﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

/// <summary>
/// JsonHelper 的摘要说明
/// </summary>
public class JsonHelper
{
	public JsonHelper()
	{
		//
		// TODO: 在此处添加构造函数逻辑
		//
	}

    public static string GetJsonByObject(Object obj)
    {
        //实例化DataContractJsonSerializer对象，需要待序列化的对象类型
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        //实例化一个内存流，用于存放序列化后的数据
        MemoryStream stream = new MemoryStream();
        //使用WriteObject序列化对象
        serializer.WriteObject(stream, obj);
        //写入内存流中
        byte[] dataBytes = new byte[stream.Length];
        stream.Position = 0;
        stream.Read(dataBytes, 0, (int)stream.Length);
        //通过UTF8格式转换为字符串
        return Encoding.UTF8.GetString(dataBytes);
    }

    public static Object GetObjectByJson(string jsonString, Object obj)
    {
        //实例化DataContractJsonSerializer对象，需要待序列化的对象类型
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        //把Json传入内存流中保存
        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
        // 使用ReadObject方法反序列化成对象
        return serializer.ReadObject(stream);
    }

    public static string GetJsonByResult(string result)
    {
        string json = "";

        json = result.Substring(result.IndexOf("{"), result.LastIndexOf("}") - result.IndexOf("{") + 1);

        return json;
    }
}