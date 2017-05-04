using AutoIt;
using System;
using System.ComponentModel;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Gw2BagOpener
{
	public partial class Form1 : Form
	{
		private bool mOpenBags = false;
		private bool mPrevOpenBags = false;
		private bool mKeepRunning = true;
		Random rnd = new Random();
		private BackgroundWorker worker = new BackgroundWorker();

		public Form1()
		{
			InitializeComponent();

			ResourceExtractor.ExtractResourceToFile("Gw2BagOpener.AutoItX3.dll", "AutoItX3.dll");
			ResourceExtractor.ExtractResourceToFile("Gw2BagOpener.AutoItX3.Assembly.dll", "AutoItX3.Assembly.dll");

			worker.DoWork += DoBackgroundWork;
			worker.RunWorkerAsync();
		}
		
		private void DoBackgroundWork(object sender, DoWorkEventArgs e)
		{
			while(mKeepRunning)
			{
				if(mOpenBags && !mPrevOpenBags)
					Thread.Sleep(1000);
				mPrevOpenBags = mOpenBags;

				bool openBags = mOpenBags || Win32.KeyDown((int)VirtualKeyStates.VK_DIVIDE);

				if(GameHasFocus() && openBags) {
					int delay = rnd.Next(40, 70);
					Thread.Sleep(delay);
					AutoItX.MouseClick();
				}
				else
					Thread.Sleep(10);
			}
		}
		
		private void btnStartStop_Click(object sender, EventArgs e)
		{
			mOpenBags = !mOpenBags;

			if(mOpenBags) {
				SystemSounds.Beep.Play();
				btnStartStop.Text = "Stop Opening";
				SetGameFocus();
            }
			else {
				SystemSounds.Hand.Play();
				btnStartStop.Text = "Start Opening";
			}
        }

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			mKeepRunning = false;
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

		private void SetGameFocus()
		{
			IntPtr hwnd = Win32.FindWindow("ArenaNet_Dx_Window_Class", null);
			if(hwnd == null || hwnd == IntPtr.Zero)
				return;

			Win32.SetForegroundWindow(hwnd);
		}
	}
}
