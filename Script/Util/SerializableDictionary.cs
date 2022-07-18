using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

/// <summary>
/// Dictionary는 원소 타입이 아니라 직렬화가 되어있지 않기 때문에 인스펙터 상에서 보이려면 
/// ISerializationCallbackReceiver를 통해 직접 구현해야 함
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys;
    [SerializeField]
    private List<TValue> values;

    public SerializableDictionary()
    {
        keys = new List<TKey>();
        values = new List<TValue>();
        SyncInspectorFromDictionary();
    }

    public void OnAfterDeserialize()
    {
        // 키와 밸류의 카운트 동일검사
        if(keys.Count != values.Count)
        {
            throw new System.Exception(string.Format("Dictionary must be have key and value with same count"));
        }
        else
        {
            SyncDictionaryFromInspector();
        }
    }

    public void OnBeforeSerialize()
    {
    }

    public new void Add(TKey key, TValue value)
    {
        base.Add(key, value);
        SyncInspectorFromDictionary();
    }

    public new void Remove(TKey key)
    {
        base.Remove(key);
        SyncInspectorFromDictionary();
    }

    private void SyncInspectorFromDictionary()
    {
        // 인스펙터 키 밸류 리스트 초기화
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void SyncDictionaryFromInspector()
    {
        //딕셔너리 키 밸류 리스트 초기화
        foreach (var key in Keys.ToList())
        {
            base.Remove(key);
        }

        for (int i = 0; i < keys.Count; i++)
        {
            //중복된 키가 있다면 에러 출력
            if (this.ContainsKey(keys[i]))
            {
                Debug.LogError("There are duplicate keys.");
                break;
            }
            base.Add(keys[i], values[i]);
        }
    }
}

[System.Serializable]
public class SerializeDicPanels : SerializableDictionary<string, GameObject>
{
}