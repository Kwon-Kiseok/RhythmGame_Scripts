using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

/// <summary>
/// Dictionary�� ���� Ÿ���� �ƴ϶� ����ȭ�� �Ǿ����� �ʱ� ������ �ν����� �󿡼� ���̷��� 
/// ISerializationCallbackReceiver�� ���� ���� �����ؾ� ��
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
        // Ű�� ����� ī��Ʈ ���ϰ˻�
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
        // �ν����� Ű ��� ����Ʈ �ʱ�ȭ
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
        //��ųʸ� Ű ��� ����Ʈ �ʱ�ȭ
        foreach (var key in Keys.ToList())
        {
            base.Remove(key);
        }

        for (int i = 0; i < keys.Count; i++)
        {
            //�ߺ��� Ű�� �ִٸ� ���� ���
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