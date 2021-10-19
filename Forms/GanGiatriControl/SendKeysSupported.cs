using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BMS
{
	public class SendKeysSupported
	{
		public SendKeysSupported()
		{
		}

		// Get a handle to an application window.
		[DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
		public static extern IntPtr FindWindow(string lpClassName,
			string lpWindowName);

		// Activate an application window.
		[DllImport("USER32.DLL")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);


		/// <summary>
		/// gans giá trị lên PM control
		/// </summary>
		/// <param name="processName">tên PM</param>
		/// <param name="processID">ID</param>
		/// <param name="msg"></param>
		public static void SendWait(string processName, string processID, string msg)
		{
			if (!string.IsNullOrWhiteSpace(processName))
			{
				IntPtr windowHandle = new IntPtr();
				int processIDNumber;
				//------------------------------------------------------------
				// Lấy danh sách process có tên trùng với processName
				//------------------------------------------------------------
				Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(processName));
				if (processes.Count() < 1)
				{
					MessageBox.Show("Không thể tìm process");
					return;
				}
				//------------------------------------------------------------
				// Lấy ID của process nếu có
				//------------------------------------------------------------
				try
				{
					processIDNumber = int.Parse(processID);
				}
				catch
				{
					MessageBox.Show("Sai ProcessID");
					return;
				}
				//------------------------------------------------------------
				// Lấy Process Window handle theo ID, hoặc theo số thứ tự 0 nếu process ID nhỏ hơn 1
				//------------------------------------------------------------
				if (processIDNumber < 1)
				{
					windowHandle = processes[0].MainWindowHandle;
				}
				else
				{
					foreach (Process item in processes)
					{
						if (item.Id == processIDNumber)
						{
							windowHandle = item.MainWindowHandle;
						}
					}
				}
				//------------------------------------------------------------
				// Nếu không thể tìm thấy Window handle => 
				//------------------------------------------------------------
				if (windowHandle == null)
				{
					MessageBox.Show("Không thể tìm window");
					return;
				}
				//------------------------------------------------------------
				// Điền dữ liệu vào window tìm được
				//------------------------------------------------------------
				SetForegroundWindow(windowHandle);
				System.Windows.Forms.SendKeys.SendWait(msg);
			}
		}
	}
}
