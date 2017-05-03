using AutoIt;
using System;
using System.ComponentModel;
using System.Media;
using System.Text;
using System.Threading;
//using System.Threading;
using System.Windows.Forms;

namespace Gw2BagOpener
{
	public partial class Form1 : Form
	{
		private bool mLoop = false;
		Random rnd = new Random();
		private BackgroundWorker worker = new BackgroundWorker();

		public Form1()
		{
			InitializeComponent();

			ResourceExtractor.ExtractResourceToFile("Gw2BagOpener.AutoItX3.dll", "AutoItX3.dll");

			worker.DoWork += DoBackgroundWork;
			worker.RunWorkerAsync();
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			SystemSounds.Beep.Play();
			Thread.Sleep(1000);
			mLoop = true;
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			SystemSounds.Hand.Play();
			mLoop = false;
		}

		private void DoBackgroundWork(object sender, DoWorkEventArgs e)
		{
			while(true)
			{
				if(mLoop && GameHasFocus()) {
					int delay = rnd.Next(50, 100);
					Thread.Sleep(delay);

					AutoItX.MouseClick();
				}
			}
		}


		private bool GameHasFocus()
		{
			IntPtr hwnd = Win32.GetForegroundWindow();
			if(hwnd == null || hwnd == IntPtr.Zero)
				return false;

			var sb = new StringBuilder(256);
			Win32.GetClassName(hwnd, sb, sb.Capacity);

			return sb.ToString() == "ArenaNet_Dx_Window_Class";
		}

	}
}
