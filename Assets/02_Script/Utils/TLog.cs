using UnityEngine;

/// <summary>
/// 태그별 로그 분류용 자체 로그
/// </summary>
public class TLog
{
	private ILogger myLogger;

	string sTag = "";
	bool isLogEnabled = false;

	public TLog(string sTag = null, bool logEnable = true)
	{
		myLogger = Debug.unityLogger;
		this.sTag = string.IsNullOrEmpty(sTag) ? this.GetType().ToString() : sTag;
		this.isLogEnabled = logEnable;
		myLogger.logEnabled = this.isLogEnabled;// Debug.isDebugBuild;
	}

	[System.Diagnostics.Conditional("__DEV")]
	public void Log(string msg)
	{
		if (myLogger != null && isLogEnabled)
			myLogger.Log(sTag, msg);
	}

	[System.Diagnostics.Conditional("__DEV")]
	public void Log(string format, params object[] args)
	{
		if (myLogger != null && isLogEnabled)
			myLogger.Log(sTag, string.Format(format, args));
	}

	public void LogError(string msg
		//            , [System.Runtime.CompilerServices.CallerMemberName] string memberName = ""
		//            , [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = ""
		//            , [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
	)
	{
		//            Debug.LogFormat(context, "!!! {0}, {1}, {2}!!!", memberName, sourceFilePath, sourceLineNumber);

		if (myLogger != null)// && isLogEnabled)
			myLogger.LogError(sTag, msg);
	}
	public void LogError(string format, params object[] args)
	{
		if (myLogger != null)// && isLogEnabled)
			myLogger.LogError(sTag, string.Format(format, args));
	}

}