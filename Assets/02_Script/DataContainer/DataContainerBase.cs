using System;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// offline file caching을 위한 base class  
/// </summary>
public class DataContainerBase
{
	readonly bool compression;
	[NonSerialized]
	protected TLog myLog;
	public bool IsDataReady { get; protected set; }

	[NonSerialized]
	readonly string FileName;

	protected DataContainerBase(bool bCompression, string fileName)
	{
		myLog = new TLog(this.GetType().ToString());
		IsDataReady = false;
		compression = bCompression;

		FileName = fileName;
	}

	/// <summary>
	/// 모든 정보를 로컬에 파일로 저장
	/// </summary>
	public virtual void SaveDataToTextFile()
	{
		string path = System.IO.Path.Combine(Application.persistentDataPath, FileName);
		string text = JsonUtility.ToJson(this);
		// if (compression)
			// ZipUtil.CompressBytesToFile(Encoding.UTF8.GetBytes(text), path);
		// else
			File.WriteAllText(path, text);

#if UNITY_EDITOR
		UnityEditor.AssetDatabase.Refresh();
#endif
	}
}