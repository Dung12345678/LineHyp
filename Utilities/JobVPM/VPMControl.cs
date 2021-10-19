using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BMS
{
	public class VPMControl
	{
		/// <summary>
		/// Kiểm tra trạng thái Online/Offline của VPM
		/// </summary>
		/// <returns></returns>
		public bool CheckOnlineStatus(string ipAddress, int port)
		{
			string cmd = "cmd";
			string resultStr = "";
			byte[] data;
			byte[] dataRev;

			// Kết nối đến địa chỉ để lấy giá trị Online
			TcpClient sendTCPClient;
			sendTCPClient = new TcpClient();
			try
			{
				sendTCPClient.Connect(ipAddress, port);
			}
			catch { }
			if (sendTCPClient.Connected == false)
			{
				sendTCPClient.Close();
				return false;
			}
			Console.WriteLine("Đã kết nối thành công");

			// Kiểm tra Online
			using (Stream stream = sendTCPClient.GetStream())
			{
				// Gửi lệnh lấy tín hiệu Online/Offline
				cmd = "cmd IsOnline\r\n";
				data = Encoding.UTF8.GetBytes(cmd);
				stream.WriteTimeout = 1000;
				try
				{
					stream.Write(data, 0, data.Length);
				}
				catch
				{
					Console.WriteLine("Không gửi được sang VPM");
					sendTCPClient.Close();
					return false;
				}
				dataRev = new byte[1024];
				stream.ReadTimeout = 1000;

				// Đọc tín hiệu trả về
				try
				{
					stream.Read(dataRev, 0, 1024);
				}
				catch
				{
					Console.WriteLine("Không nhận được trả về từ VPM");
					sendTCPClient.Close();
					return false;
				}

				// Đóng kết nối
				sendTCPClient.Close();

				// So sánh, thay đổi trạng thái báo Online/Offline
				resultStr = Encoding.UTF8.GetString(dataRev);
				if (resultStr.Contains("True"))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		/// <summary>
		/// Điều khiển VPM Online
		/// </summary>
		/// <param name="ipAddress"></param>
		/// <param name="port"></param>
		/// <returns></returns>
		public bool ForceOnline(string ipAddress, int port)
		{
			string cmd = "cmd";
			byte[] data;

			// Kết nối đến địa chỉ để lấy giá trị Online
			TcpClient sendTCPClient;
			sendTCPClient = new TcpClient();
			try
			{
				sendTCPClient.Connect(ipAddress, port);
			}
			catch { }
			if (sendTCPClient.Connected == false)
			{
				sendTCPClient.Close();
				return false;
			}
			Console.WriteLine("Đã kết nối thành công");

			// Điều khiển VPM Online
			using (Stream stream = sendTCPClient.GetStream())
			{
				// Gửi lệnh Online
				cmd = "cmd Online\r\n";
				data = Encoding.UTF8.GetBytes(cmd);
				stream.WriteTimeout = 1000;
				try
				{
					stream.Write(data, 0, data.Length);
				}
				catch
				{
					Console.WriteLine("Không gửi được sang VPM");
					sendTCPClient.Close();
					return false;
				}

				// Đóng kết nối
				sendTCPClient.Close();

				// Trả kết quả dựa theo trạng thái Online đã được set hay chưa
				if (CheckOnlineStatus(ipAddress, port) == true) return true;
				return false;
			}
		}
		/// <summary>
		/// Điều khiển VPM Offline
		/// </summary>
		/// <param name="ipAddress"></param>
		/// <param name="port"></param>
		/// <returns></returns>
		public bool ForceOffline(string ipAddress, int port)
		{
			string cmd = "cmd";
			byte[] data;

			// Kết nối đến địa chỉ để lấy giá trị Online
			TcpClient sendTCPClient;
			sendTCPClient = new TcpClient();
			try
			{
				sendTCPClient.Connect(ipAddress, port);
			}
			catch { }
			if (sendTCPClient.Connected == false)
			{
				sendTCPClient.Close();
				return false;
			}
			Console.WriteLine("Đã kết nối thành công");

			// Điều khiển VPM Online
			using (Stream stream = sendTCPClient.GetStream())
			{
				// Gửi lệnh Online
				cmd = "cmd Offline\r\n";
				data = Encoding.UTF8.GetBytes(cmd);
				stream.WriteTimeout = 1000;
				try
				{
					stream.Write(data, 0, data.Length);
				}
				catch
				{
					Console.WriteLine("Không gửi được sang VPM");
					sendTCPClient.Close();
					return false;
				}

				// Đóng kết nối
				sendTCPClient.Close();

				// Trả kết quả dựa theo trạng thái Online đã được set hay chưa
				if (CheckOnlineStatus(ipAddress, port) == false) return true;
				return false;
			}
		}
		/// <summary>
		/// Lấy mã job hiện tại đang lưu trong VPM
		/// </summary>
		/// <param name="ipAddress"></param>
		/// <param name="port"></param>
		/// <returns></returns>
		public string GetCurrentJob(string ipAddress, int port)
		{
			string cmd = "cmd";
			string resultStr = "";
			byte[] data;
			byte[] dataRev;

			// Kết nối đến địa chỉ để lấy giá trị Online
			TcpClient sendTCPClient;
			sendTCPClient = new TcpClient();
			try
			{

				var result = sendTCPClient.BeginConnect(ipAddress, port, null, null);
				var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
				if (!success)
				{
					throw new Exception("kết nối không thành công");
					//kết nối k thành công
				}
				sendTCPClient.EndConnect(result);
				
			}
			catch { }
			if (sendTCPClient.Connected == false)
			{
				sendTCPClient.Close();
				return "";
			}
			Console.WriteLine("Đã kết nối thành công");

			// Điều khiển VPM Online
			using (Stream stream = sendTCPClient.GetStream())
			{
				// Gửi lệnh Online
				cmd = "cmd ListLoadedVPFiles\r\n";
				data = Encoding.UTF8.GetBytes(cmd);
				stream.WriteTimeout = 1000;
				try
				{
					stream.Write(data, 0, data.Length);
				}
				catch
				{
					Console.WriteLine("Không gửi được sang VPM");
					sendTCPClient.Close();
					return "";
				}
				dataRev = new byte[1024];
				stream.ReadTimeout = 1000;

				// Đọc tín hiệu trả về
				try
				{
					stream.Read(dataRev, 0, 1024);
				}
				catch
				{
					Console.WriteLine("Không nhận được trả về từ VPM");
					sendTCPClient.Close();
					return "";
				}

				// Đóng kết nối
				sendTCPClient.Close();

				// Trả kết quả
				resultStr = Encoding.UTF8.GetString(dataRev);
				return resultStr;
			}
		}
		/// <summary>
		/// Load job với tên tương ứng
		/// </summary>
		/// <param name="ipAddress">192.168.125.129</param>
		/// <param name="port">20000</param>
		/// <param name="programFileName">Tên job tương ứng với order</param>
		/// <returns></returns>
		public bool ForceLoadJob(string ipAddress, int port, string programFileName)
		{
			string cmd = "cmd";
			byte[] data;

			// Kết nối đến địa chỉ để lấy giá trị Online
			TcpClient sendTCPClient;
			sendTCPClient = new TcpClient();
			try
			{
				var result = sendTCPClient.BeginConnect(ipAddress, port, null, null);
				var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
				if (!success)
				{
					throw new Exception("kết nối không thành công");
					//kết nối k thành công
				}
				sendTCPClient.EndConnect(result);
			}
			catch { }
			if (sendTCPClient.Connected == false)
			{
				sendTCPClient.Close();
				return false;
			}
			Console.WriteLine("Đã kết nối thành công");

			// Điều khiển VPM Online
			using (Stream stream = sendTCPClient.GetStream())
			{
				// Gửi lệnh Online
				cmd = $"cmd loadvpfile {programFileName}\r\n";
				data = Encoding.UTF8.GetBytes(cmd);
				stream.WriteTimeout = 1000;
				try
				{
					stream.Write(data, 0, data.Length);
				}
				catch
				{
					Console.WriteLine("Không gửi được sang VPM");
					sendTCPClient.Close();
					return false;
				}

				// Đóng kết nối
				sendTCPClient.Close();

				// Đợi 5s
				Thread.Sleep(2000);

				// Trả kết quả dựa theo trạng thái Online đã được set hay chưa
				if (GetCurrentJob(ipAddress, port).Contains(programFileName) == true) return true;
				return false;
			}
		}
		/// <summary>
		/// Unload tất cả các job đang mở
		/// </summary>
		/// <param name="ipAddress">192.168.125.129</param>
		/// <param name="port">20000</param>
		/// <returns></returns>
		public bool ForceUnloadAllProgram(string ipAddress, int port)
		{
			string cmd = "cmd";
			byte[] data;

			// Kết nối đến địa chỉ để lấy giá trị Online
			TcpClient sendTCPClient;
			sendTCPClient = new TcpClient();
			try
			{
				var result = sendTCPClient.BeginConnect(ipAddress, port, null, null);
				var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
				if (!success)
				{
					throw new Exception("kết nối không thành công");
					//kết nối k thành công
				}
				sendTCPClient.EndConnect(result);
			}
			catch { }
			if (sendTCPClient.Connected == false)
			{
				sendTCPClient.Close();
				return false;
			}
			Console.WriteLine("Đã kết nối thành công");

			// Điều khiển VPM Online
			using (Stream stream = sendTCPClient.GetStream())
			{
				// Gửi lệnh Online
				cmd = "cmd unloadallprograms\r\n";
				data = Encoding.UTF8.GetBytes(cmd);
				stream.WriteTimeout = 1000;
				try
				{
					stream.Write(data, 0, data.Length);
				}
				catch
				{
					Console.WriteLine("Không gửi được sang VPM");
					sendTCPClient.Close();
					return false;
				}

				// Đóng kết nối
				sendTCPClient.Close();

				// Trả kết quả dựa theo trạng thái Online đã được set hay chưa
				if (GetCurrentJob(ipAddress, port)[0] == '0') return true;
				return false;
			}
		}
		public VPMControl()
		{
		}
	}
}
