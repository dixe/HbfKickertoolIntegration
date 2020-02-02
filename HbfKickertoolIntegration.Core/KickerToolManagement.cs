using System.Diagnostics;
using System.Linq;

namespace HbfKickertoolIntegration.Core
{
	public static class KickerToolManagement
	{
		public static Process EnsureKickertoolRunning()
		{
			var process = Process.GetProcesses().FirstOrDefault(x => x.ProcessName.ToLower() == "kickertool");
			if (null == process)
			{
				process = Process.Start("C:\\Program Files (x86)\\Kickertool\\Kickertool.exe");
			}
			return process;
		}

		public static bool IsKickertoolRunning()
		{
			return null != Process.GetProcesses().FirstOrDefault(x => x.ProcessName.ToLower() == "kickertool");
		}		
	}
}
