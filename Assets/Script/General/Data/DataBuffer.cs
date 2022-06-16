using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataBuffer
{
    public static DataBuffer Instance { get; } = new();

    // current data
    // <int> "level"
    // <LevelInfo>

    private Dictionary<(Type dataType, string name), object> buffer;

    private DataBuffer() { buffer = new(); }

    // will overwrite if data exists
    public void Add(object data, string name = null)
    {
        buffer[(data.GetType(), name)] = data;
    }

    public T Get<T>(string name = null)
    {
        object data;
        if (!buffer.TryGetValue((typeof(T), name), out data))
        {
            Debug.Log(string.Format(
                "Data of type {0}, name {1} not found in buffer"
                , typeof(T).ToString(), name)
            );
            return default;
        }
        return (T)data;
    }

    public void Remove(Type dataType, string name = null)
    {
        buffer.Remove((dataType, name));
    }

    // Remove all matching data
    public void Remove(object data)
    {
        foreach (var item in buffer.Where(kvp => kvp.Value == data).ToList())
        {
            buffer.Remove(item.Key);
        }
    }

}
