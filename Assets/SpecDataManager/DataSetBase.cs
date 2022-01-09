using System.Collections.Generic;
using UnityEngine;


public class DataFieldBase<T> : ScriptableObject
{
	public T fieldID;
}


public class DataSetBase<TKey, TValue> : ScriptableObject where TValue : DataFieldBase<TKey>
{
	[SerializeField]
	private List<TValue> values = new List<TValue>();

	private Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();

	public void UpdateDic()
	{
		if (dict != null && dict.Count > 0) 
			return;
        
		dict = new Dictionary<TKey, TValue>();

		if (values == null)
		{
			values = new List<TValue>();
			return;
		}

		foreach (var v in values)
		{
			dict[v.fieldID] = v;
		}
	}

	public TValue this[TKey key]
	{
		get
		{
			UpdateDic();
			return dict[key];
		}
	}

	public ICollection<TKey> Keys
	{
		get
		{
			UpdateDic();
			return dict.Keys;
		}
	}

	public ICollection<TValue> Values
	{
		get
		{
			UpdateDic();
			return dict.Values;
		}
	}

	public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
	{
		UpdateDic();
		return dict.GetEnumerator();
	}
    
	public void Add(TKey key, TValue value)
	{
		if (dict == null)
			dict = new Dictionary<TKey, TValue>();
        
		if (values == null)
			values = new List<TValue>();

		values.Add(value);
		dict.Add(key, value);
	}
    
	public int Count
	{
		get
		{
			UpdateDic();
			return dict?.Count ?? 0;
		}
	}

	public bool ContainsKey(TKey key)
	{
		UpdateDic();
		return dict.ContainsKey(key);
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		UpdateDic();
		if (dict.ContainsKey(key))
		{
			value = dict[key];
			return true;
		}

		value = null;
		return false;
	}
}