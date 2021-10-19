using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;

namespace BMS
{
	class ConnectCom
	{
		SerialPort Comport;

		string data = "";
		int timeout = 0;

		/// <summary>
		/// kết nối tới com 
		/// </summary>
		/// <param name="Name">Tên com VD (COM131)</param>
		public void ConnectComAuto(string Name)
		{
			try
			{
				Comport = new SerialPort($"{Name}", 9600, Parity.Even, 8, StopBits.One);
				Comport.Open();
				Comport.DataReceived += new SerialDataReceivedEventHandler(DataReceivedEventHandler);
			}
			catch
			{

			}
		}

		/// <summary>
		/// Nhận Data qua Com
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void DataReceivedEventHandler(object sender, SerialDataReceivedEventArgs e)
		{
			try
			{
				// Đọc Com 
				byte[] buffer = new byte[100];//Có 100 phần tử 
				int LengthData = Comport.Read(buffer, 0, buffer.Length);
				if (buffer[LengthData - 2] == (0x0d) && buffer[LengthData - 1] == (0x00a))
				{
					data = Encoding.ASCII.GetString(buffer, 0, LengthData - 2);
				}
			}
			catch
			{

			}
		}

		/// <summary>
		/// Gửi Data qua Com
		/// </summary>
		/// <param name="data"></param>
		//public void SendCom(string data)
		//{
		//	try
		//	{
		//		if (timeout == 20)
		//		{
		//			byte[] header = { };// chèn ký tự đặc biệt ở đầu
		//			byte[] arraydata = Encoding.ASCII.GetBytes(data);//chuyển dữ liệu từ dạng string sang dạng mảng byte
		//			byte[] terminal = { 0x0d, 0x0a };// chèn ký tự đặc biệt ở cuối
		//			byte[] TotalDataSend = new byte[arraydata.Length + terminal.Length]; // tổng số phần tử trong mảng TotalData
		//			/// gán 2 mảng byte vào 1 mảng lớn
		//			//header.CopyTo(TotalDataSend, 0);
		//			//arraydata.CopyTo(TotalDataSend, header.Length);
		//			// Gửi dữ liệu
		//			arraydata.CopyTo(TotalDataSend, 0);
		//			terminal.CopyTo(TotalDataSend, arraydata.Length);
		//			Comport.Write(TotalDataSend, 0, TotalDataSend.Length);
		//		}
		//		else
		//		{
		//			_CheckTimeOutFat = true;
		//			Checktimeout.Start();
		//			byte[] header = { };// chèn ký tự đặc biệt ở đầu
		//			byte[] arraydata = Encoding.ASCII.GetBytes(data);//chuyển dữ liệu từ dạng string sang dạng mảng byte
		//			byte[] terminal = { 0x0d, 0x0a };// chèn ký tự đặc biệt ở cuối
		//			byte[] TotalDataSend = new byte[arraydata.Length + terminal.Length]; // tổng số phần tử trong mảng TotalData
		//			/// gán 2 mảng byte vào 1 mảng lớn
		//			//header.CopyTo(TotalDataSend, 0);
		//			//arraydata.CopyTo(TotalDataSend, header.Length);
		//			// Gửi dữ liệu
		//			arraydata.CopyTo(TotalDataSend, 0);
		//			terminal.CopyTo(TotalDataSend, arraydata.Length);
		//			Comport.Write(TotalDataSend, 0, TotalDataSend.Length);
		//		}
		//	}
		//	catch
		//	{

		//	}


		//}
	}
}
