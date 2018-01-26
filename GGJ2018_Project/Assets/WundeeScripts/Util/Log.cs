namespace Wundee
{
    public static class Logger
	{
		private const string LOG_TEXT = "<i>{0} (line <b>{1}</b>):</i> \n{2}";

		public static void Print(System.Object message)
		{
			UnityEngine.Debug.Log(message);
		}

		public static void Log(System.Object message, int StackTraceFrameOffset = 0)
		{
			var stackTraceFrame = new System.Diagnostics.StackTrace(true).GetFrame(1 + StackTraceFrameOffset);
			
			var line = stackTraceFrame.GetFileLineNumber();
			var method = stackTraceFrame.GetMethod();
			
			UnityEngine.Debug.Log(string.Format(LOG_TEXT, method, line, message));
		}

		public static void Warning (System.Object message, int StackTraceFrameOffset = 0)
		{
			var stackTraceFrame = new System.Diagnostics.StackTrace(true).GetFrame(1 + StackTraceFrameOffset);

			var line = stackTraceFrame.GetFileLineNumber();
			var method = stackTraceFrame.GetMethod();

			UnityEngine.Debug.LogWarning(string.Format(LOG_TEXT, method, line, message));
		}

		public static void Error (System.Object message, int StackTraceFrameOffset = 0)
		{
			var stackTraceFrame = new System.Diagnostics.StackTrace(true).GetFrame(1 + StackTraceFrameOffset);

			var line = stackTraceFrame.GetFileLineNumber();
			var method = stackTraceFrame.GetMethod();

			UnityEngine.Debug.LogError(string.Format(LOG_TEXT, method, line, message));
		}
	}

}