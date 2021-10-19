using BMS.Business;
using BMS.Model;
using BMS.Utils;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using MSScriptControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BMS.MessageHelper;

namespace BMS
{
	public partial class frmProductCheckHistory1 : _Forms
	{
		public delegate void CheckAll(bool Check, string name);
		#region biến
		public VPMControl VPMControl = new VPMControl();
		string _ProductWorkingName = "";
		string Sub = "";
		string _ProductCode = "";
		int andonActive = 0;
		ScriptControl sc = new ScriptControl();
		decimal valueFormula;
		int _NoIndefinitely = -9999;
		bool _isStartColor = false;
		string CDOld = "";
		string CD = "";
		string CDSkip = "";
		int _currentIndex = 0;
		int _totalTimeDelay = 0;
		int _taktTime = 10;
		string _step;
		int _PlanID = 0;
		int _status = 0;
		bool _isStart = false;
		bool _isLogin = false;
		DateTime _startMakeTime;
		DateTime _endMakeTime;
		bool _CheckTimeOutFat;
		int timeout = 0;
		public int _indexColumSauPCA = 0;
		public long _WorkerID = 0;
		string _AritelID = "";
		int focus = 0;
		int _workingStepID = 9999;
		string _order = "";
		string _DataSendCom = "";
		string _productCode = "";
		string _tienTo = "";
		string _stt = "";
		int _stepIndex = 0;
		Color _colorEmpty;
		string _GroupCode;
		int _index = 999;
		int oldHeight = 0;
		int oldHeightGrid = 0;
		string productGroupCode = "";
		string productTypeCode = "";
		string _formulaX1 = "";
		string _formulaX2 = "";
		string _NameCol = "";
		string _NameColSauPCA = "";
		string _formulaX3 = "";
		int _sortOrderCalculate = -1000;
		string _NameColumnVPM = "";
		int _ColumnVPM = 0;
		int _ColumnSauPCA = 0;
		string _ColumnNameSauPCA = "";
		string _SttQR = "";
		int _SortOrderValue = -9999;
		int _SortOrder1 = -99999;
		int _sortOrderBehind = -9999;
		decimal _GMD = 0;
		decimal _AutoFill = 0;// Giá trị Tự động điền cột khi check được giá trị
		int _rowGMD = 0;
		int _sortOrderGMD = -99999;
		int _sortOrderAutoFillColumns = -99999; // Số thứ tự tự động điền giá trị cột 
		double _EngineWeightMin = 0;
		double _EngineWeightMax = 0;
		double _EngineWeight = 0;
		double _PlateWeight = 0;
		double _JigWeight = 0;
		int _IDProduct = 0;

		int _RowVPM = 999;
		int _RowMotor = 999;
		int _RowPin = 999;
		int _RowPinSL = 999;
		int _RowSauPCA = 999;
		string _socketIPAddressPicking = "172.0.0.1";
		string _socketIPVPM = "192.168.125.129";
		int _socketPortVPM = 20000;
		int _socketPortPicking = 2000;
		int _PeriodTime = 0;
		string RealFat = "0";
		int RealFatContentMin = 9999;
		int RealFatContentMax = 9999;
		string FatContent = "";
		bool _Print = false;
		double Average = 9999;
		//DateTime _sTimeDelay;

		string data;
		int _productID = 0;
		#endregion
		#region thread
		//Thread _threadDelay;

		Thread _threadRisk;
		Thread _threadConnect;
		Thread _threadFreeze;
		Thread _threadCheckColorCD;
		Thread _threadResetSocket;
		Thread _threadGetAndonDetailsByCD;
		private Thread _threadLoadAndon;
		Thread _threadTakeTime;
		Thread _threadInPutVPM;
		#endregion
		int FocusbtnSave = 0;
		string _socketIPAddress = "192.168.1.46";
		int _socketPort = 3000;
		Socket _socket;
		Socket _socketPicking;
		Socket _socketVPM;
		ASCIIEncoding _encoding = new ASCIIEncoding();
		string folderUpdate = "";
		string _stepCode = "";
		bool _enable = true;
		int _focusColumns = 0;
		int _currentStatus = 999;
		int column = 9999;// hiển thị cột trong grid đag focus
		int row = 9999;// Hiển thị dòng trong grid dag focus
		#region Model
		ProductionPlanModel _Planmodel = new ProductionPlanModel();
		ProductModel _Product = new ProductModel();
		DateTime _sTimeRisk;
		DateTime _eTimeRisk;
		DataTable _dtData = new DataTable();
		AndonModel _oAndonModel = new AndonModel();
		DataTable _DataShipTo = new DataTable();
		#endregion
		#region Đường dẫn
		private string _pathFileBanDo = Path.Combine(Application.StartupPath, "BanDo.txt");
		string _pathFileConfigUpdate = Path.Combine(Application.StartupPath, "ConfigUpdate.txt");
		string _pathError = Path.Combine(Application.StartupPath, "Errors");
		string pathConfigFitRow = Application.StartupPath + "/ConfigFitRow.txt";
		string pathConfigFat = Application.StartupPath + "/ConfigFat.txt";
		string pathSub = Application.StartupPath + "/Sub.txt";
		string _pathFolderUpdate = "";
		string _pathUpdateServer = "";
		string _pathFileVersion = "";
		#endregion
		DataTable dtCD;
		//Danh sách dòng #1#
		List<int> _lst = new List<int>();
		//Danh sách công đoạn bỏ qua
		List<string> _lstCD = new List<string>();
		//Danh sách dòng #4#
		List<int> _lst4 = new List<int>();
		//danh sách công thức
		List<string> _lstFormula = new List<string>();
		//Danh sách SortOrder theo công thức
		List<int> _lstSortOrder = new List<int>();
		//bool _isStart = false;
		SerialPort Comport;
		//form in
		frmPrint _frm;

		public frmProductCheckHistory1()
		{
			InitializeComponent();

			//event Khi focus vào textbox
			//textBox5.GotFocus += GotFocusTexbox;

			if (File.Exists(pathConfigFitRow))
			{
				string valueFit = File.ReadAllText(pathConfigFitRow);
				chkFitRow.Checked = TextUtils.ToInt(valueFit) == 0 ? false : true;
			}
			if (File.Exists(pathConfigFat))
			{
				string valueFat = File.ReadAllText(pathConfigFat);
				chkFat.Checked = TextUtils.ToInt(valueFat) == 0 ? false : true;
			}
			if (!File.Exists(pathSub))
			{
				File.WriteAllText(pathSub, "0");
			}
		}
		private void frmProductCheckHistory1_Load(object sender, EventArgs e)
		{
			_colorEmpty = Color.FromArgb(255, 192, 255);
			string version = File.ReadAllText(Application.StartupPath + "/Version.txt");

			this.Text += "  -  Version: " + version.Trim();
			string a = this.Text;
			Sub = File.ReadAllText(Application.StartupPath + "/Sub.txt");
			cboSub.SelectedIndex = TextUtils.ToInt(Sub);
			if (File.Exists(Application.StartupPath + "/CD.txt"))
			{
				CD = File.ReadAllText(Application.StartupPath + "/CD.txt");
				if (CD.Trim() != "")
				{
					btnNotUseCD8.Visible = true;
				}
				lblCD.Text = CD.Trim();
			}

			//decimal a = 456.999m;
			//decimal b = Math.Truncate(a);
			////int c = TextUtils.ToInt(b);
			//return;
			sc.Language = "VBScript";

			DocUtils.InitFTPQLSX();

			//Check update version
			updateVersion();

			//Show frmPrint
			if (CD.Trim() == "CDTest")
			{
				_frm = new frmPrint();
				//_frm.ShowInTaskbar = false;
				_frm.Show();
				btnSave.Visible = false;
			}

			ToolTip toolTip1 = new ToolTip();
			toolTip1.ShowAlways = true;
			toolTip1.SetToolTip(this.btnSave, "F12");

			initBackColor();
			oldHeight = this.Height;
			oldHeightGrid = grdData.Height;

			txtWorker.Focus();
			LoadConfigShipTo();
			andonActive = TextUtils.ToInt(TextUtils.ExcuteScalar($"select top 1 KeyValue from ConfigSystem where KeyName = 'IsAndonActiveHyp'"));
			//andonActive = 1;
			if (andonActive == 0) return;



			_oAndonModel = new AndonModel();
			ConnectAnDon();
			//ConnectAnDonPicking();
			//ConnectVPM();
			sendDataTCP("11", "10");
			//Thread hiển thị giá trị delay
			_threadGetAndonDetailsByCD = new Thread(new ThreadStart(threadShowAndonDetails));
			_threadGetAndonDetailsByCD.IsBackground = true;
			_threadGetAndonDetailsByCD.Start();

			// Chạy thread load Andon theo thời gian hiện tại
			_threadLoadAndon = new Thread(new ThreadStart(threadLoadAndon));
			_threadLoadAndon.IsBackground = true;
			_threadLoadAndon.Start();


			// Chạy thread connect all
			_threadConnect = new Thread(new ThreadStart(ConnectAll));
			_threadConnect.IsBackground = true;
			_threadConnect.Start();


			//startThreadDelay();
			//Đóng băng công đoạn khi đã hoàn thành
			startThreadFreeze();
			startThreadCheckColorCD();
			startThreadResetSocket();

			//Nhận giá trị TaktTime
			_threadTakeTime = new Thread(new ThreadStart(TakeTime));
			_threadTakeTime.IsBackground = true;
			_threadTakeTime.Start();

			// Lấy giá link dường dẫn gửi lên ftp
			//23/07/2021
			//_threadInPutVPM = new Thread(new ThreadStart(InPutVPM));
			//_threadInPutVPM.IsBackground = true;
			//_threadInPutVPM.Start();
		}
		void ConnectAll()
		{
			while (true)
			{
				Thread.Sleep(200);
				try
				{
					ConnectAnDon();

					ConnectAnDonPicking();
					//this.Invoke((MethodInvoker)delegate
					//{
					//	if (cboWorkingStep.Text.Trim() == "CD10")
					//		ConnectVPM();
					//});

				}
				catch
				{

				}
			}
		}
		void ConnectCom()
		{
			try
			{
				Comport = new SerialPort("COM131", 9600, Parity.Even, 8, StopBits.One);
				Comport.Open();
				Comport.DataReceived += new SerialDataReceivedEventHandler(DataReceivedEventHandler);
			}
			catch
			{

			}
		}
		public void SendCom(string data)
		{
			try
			{
				if (timeout == 20)
				{
					byte[] header = { };// chèn ký tự đặc biệt ở đầu
					byte[] arraydata = Encoding.ASCII.GetBytes(data);//chuyển dữ liệu từ dạng string sang dạng mảng byte
					byte[] terminal = { 0x0d, 0x0a };// chèn ký tự đặc biệt ở cuối
					byte[] TotalDataSend = new byte[arraydata.Length + terminal.Length]; // tổng số phần tử trong mảng TotalData
					/// gán 2 mảng byte vào 1 mảng lớn
					//header.CopyTo(TotalDataSend, 0);
					//arraydata.CopyTo(TotalDataSend, header.Length);
					// Gửi dữ liệu
					arraydata.CopyTo(TotalDataSend, 0);
					terminal.CopyTo(TotalDataSend, arraydata.Length);
					Comport.Write(TotalDataSend, 0, TotalDataSend.Length);
				}
				else
				{
					_CheckTimeOutFat = true;
					Checktimeout.Start();
					_DataSendCom = data;
					byte[] header = { };// chèn ký tự đặc biệt ở đầu
					byte[] arraydata = Encoding.ASCII.GetBytes(data);//chuyển dữ liệu từ dạng string sang dạng mảng byte
					byte[] terminal = { 0x0d, 0x0a };// chèn ký tự đặc biệt ở cuối
					byte[] TotalDataSend = new byte[arraydata.Length + terminal.Length]; // tổng số phần tử trong mảng TotalData
					/// gán 2 mảng byte vào 1 mảng lớn
					//header.CopyTo(TotalDataSend, 0);
					//arraydata.CopyTo(TotalDataSend, header.Length);
					// Gửi dữ liệu
					arraydata.CopyTo(TotalDataSend, 0);
					terminal.CopyTo(TotalDataSend, arraydata.Length);
					Comport.Write(TotalDataSend, 0, TotalDataSend.Length);
				}
			}
			catch
			{
				MessageBox.Show("Lỗi time out", "Thông báo", MessageBoxButtons.OK);
			}


		}

		/// <summary>
		/// lấy dữ liệu từ com
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
					if (data == "OK" || data == "OKSM" || data == "OKNOSM")
					{
						_CheckTimeOutFat = false;
						Checktimeout.Stop();
						timeout = 0;
						RealFat = "0";
						//RealFatContentMin = 9999;
						//RealFatContentMax = 9999;
						FatContent = "";
						//Average = 9999;
					}
					if (data.Contains("@"))
					{
						_CheckTimeOutFat = false;
						Checktimeout.Stop();
						timeout = 0;
						string value = data.Split('@')[0];
						UpdateWeightProduct(value);
					}
					// Điều kiện để nhảy vào lấy giá trị phần chụp ảnh
					//23/07/2021
					if (data.Trim().ToUpper().Contains("RESULT"))
					{
						string[] arr = data.Split(',');
						try
						{
							if (arr.Length <= 0) return;
							//điền giá trị lên con trỏ chuột
							//SendDataToHID($"{arr[1]}\r\n");
							folderUpdate = arr[2].Trim();
							this.Invoke((MethodInvoker)delegate
							{
								grvData.SetRowCellValue(_RowVPM, _NameColumnVPM, arr[1]);
								//Next ô tiếp theo nếu có (không thì sẽ nhảy xuống ô đánh giá textbox)
								setFocusCell(getRowIndex(_ColumnVPM), _ColumnVPM);
								DocUtils.InitFTPQLSX();
								try
								{
									Thread.Sleep(500);
									DocUtils.UploadFile(folderUpdate, @"ImageMo\IMG");
									//MessageBox.Show("Update thành công");
								}
								catch
								{
									MessageBox.Show("Update ảnh thất bại");
								}
							});
						}
						catch
						{

						}
					}

				}
			}
			catch
			{

			}
		}
		async void UpdateWeightProduct(string value)
		{
			Task task = Task.Factory.StartNew(() =>
			{
				try
				{
					double Weight = TextUtils.ToDouble(value.Trim()) - _PlateWeight - _JigWeight;
					double WeightMin = Weight - 200;
					double WeightMax = Weight + 200;
					string Standard = WeightMin + "~" + WeightMax;
					this.Invoke((MethodInvoker)delegate
					{
						int ValueMax = TextUtils.ToInt(TextUtils.ToDouble(grvData.GetRowCellValue(_RowSauPCA, colMaxValue)));
						if (ValueMax == 0)
						{

							SetValue(_RowSauPCA, WeightMin, WeightMax, Standard);

							//grvData.SetRowCellValue(_RowSauPCA, colMinValue, WeightMin);
							//grvData.SetRowCellValue(_RowSauPCA, colMaxValue, WeightMax);
							//grvData.SetRowCellValue(_RowSauPCA, colStandardValue, Standard);
							//Check đã có giá trị chưa nếu chưa thì update có rồi thì thôi
							ProductModel product = (ProductModel)ProductBO.Instance.FindByPK(_IDProduct);
							product.EngineWeight = Weight;
							product.EngineWeightMax = WeightMax;
							product.EngineWeightMin = WeightMin;

							ProductBO.Instance.Update(product);
							//SendCom("OKDONE");

							//ProductModel product1 = (ProductModel)ProductBO.Instance.FindByPK(_IDProduct);
							//if (product1.EngineWeight != 0)
							//{
							//	SendCom("NOSM");
							//}
						}
						//điền giá trị lên con trỏ chuột
						string HID = Weight + "\n";
						//SendDataToHID(HID);
						grvData.SetRowCellValue(_RowSauPCA, _ColumnNameSauPCA, HID);
						//grvData.SetFocusedRowCellValue(_ColumnNameSauPCA, HID);
					});
				}
				catch (Exception ex)
				{
					ErrorLog.errorLog($"{ex}", "Lỗi khi gửi OKDone ,NOSM và điền giá trị HID", Environment.NewLine);
				}
			}
			);

			await task;

		}
		void SetValue(int Row, double Min, double Max, string Standar)
		{
			grvData.SetRowCellValue(Row, colStandardValue, Standar);
			grvData.SetRowCellValue(Row, colMinValue, Min);
			grvData.SetRowCellValue(Row, colMaxValue, Max);

		}
		void ConnectAnDon()
		{
			if (Sub != "0")
			{
				return;
			}
			try
			{
				if (_socket != null && _socket.Connected)
				{

				}
				else
				{
					//Load ra config trong database lấy takt time, địa chỉ tcp, port
					DataTable dtConfig = TextUtils.Select("SELECT TOP 1 * FROM dbo.AndonConfig with (nolock)");
					_taktTime = TextUtils.ToInt(dtConfig.Rows[0]["Takt"]);
					_socketIPAddress = TextUtils.ToString(dtConfig.Rows[0]["TcpIp"]);
					_socketPort = TextUtils.ToInt(dtConfig.Rows[0]["SocketPort"]);
					IPAddress ipAddOut = IPAddress.Parse(_socketIPAddress);
					IPEndPoint endPoint = new IPEndPoint(ipAddOut, _socketPort);
					_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					_socket.Connect(endPoint);
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show("Can't connect to Andon!");
				_socket = null;
			}


		}

		void ConnectAnDonPicking()
		{
			if (Sub != "0")
			{
				return;
			}
			try
			{
				if (!IsSocketConnected(_socketPicking))
				{
					_socketPicking = null;
				}
				else
				{
					//Load ra config trong database lấy địa chỉ tcp, port
					DataTable dtConfig = TextUtils.Select("SELECT TOP 1 * FROM [ShiStock].[dbo].[AndonPickingConfig] with (nolock)");
					_socketIPAddressPicking = TextUtils.ToString(dtConfig.Rows[0]["TcpIp"]);
					_socketPortPicking = TextUtils.ToInt(dtConfig.Rows[0]["SocketPort"]);
					IPAddress ipAddOut = IPAddress.Parse(_socketIPAddressPicking);
					IPEndPoint endPoint = new IPEndPoint(ipAddOut, _socketPortPicking);
					_socketPicking = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					_socketPicking.Connect(endPoint);
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show("Can't connect to Andon!");
				_socketPicking = null;
			}


		}
		void ConnectVPM()
		{
			if (Sub != "0")
			{
				return;
			}
			try
			{
				if (_socketVPM != null && _socketVPM.Connected)
				{

				}
				else
				{
					//Load ra config trong database lấy địa chỉ tcp, port
					_socketIPVPM = "192.168.125.129";
					//_socketIPVPM = "127.0.0.1";
					_socketPortVPM = 20000;
					IPAddress ipAddOut = IPAddress.Parse(_socketIPVPM);
					IPEndPoint endPoint = new IPEndPoint(ipAddOut, _socketPortVPM);
					_socketVPM = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					_socketVPM.Connect(endPoint);
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show("Can't connect to Andon!");
				_socketVPM = null;
			}


		}

		void TakeTime()
		{
			ASCIIEncoding encoding = new ASCIIEncoding();

			while (true)
			{
				Thread.Sleep(100);
				try
				{
					string value = "";
					//Nhận giá trị TaktTime
					if (_socket != null && _socket.Connected)
					{
						try
						{
							byte[] buffer = new byte[500];
							_socket.Receive(buffer);
							value = encoding.GetString(buffer).Replace("\0", "").Trim();
						}
						catch (Exception ex)
						{
							_socket = null;
						}
					}
					if (value == "")
					{
						continue;
					}
					this.Invoke((MethodInvoker)delegate
					{
						lblTakt.Text = value.Trim();
						if (lblTakt.Text == TextUtils.ToString(_taktTime - 1))
						{
							_startMakeTime = DateTime.Now;
							_PeriodTime = 0;
						}
						if (TextUtils.ToInt(lblTakt.Text) < 0)
						{
							_PeriodTime = TextUtils.ToInt(value.Trim()) * -1;
						}
					});
				}
				catch
				{

				}
			}
		}
		/// <summary>
		/// lấy đường dẫn ảnh và đẩy file ảnh lên server 
		/// </summary>
		void InPutVPM()
		{
			ASCIIEncoding encoding = new ASCIIEncoding();

			while (true)
			{
				Thread.Sleep(100);
				try
				{
					string value = "";

					string[] arr;
					//Nhận giá trị Ảnh
					if (_socketVPM != null && _socketVPM.Connected)
					{
						try
						{
							byte[] buffer = new byte[500];
							_socketVPM.Receive(buffer);
							value = encoding.GetString(buffer).Replace("\0", "").Trim();
							arr = value.Split(',');
							try
							{
								if (arr.Length <= 0) return;
								//điền giá trị lên con trỏ chuột
								//SendDataToHID($"{arr[1]}\r\n");
								folderUpdate = arr[2].Trim();
								this.Invoke((MethodInvoker)delegate
								{
									grvData.SetRowCellValue(_RowVPM, _NameColumnVPM, arr[1]);
									//Next ô tiếp theo nếu có (không thì sẽ nhảy xuống ô đánh giá textbox)
									setFocusCell(getRowIndex(_ColumnVPM), _ColumnVPM);
									DocUtils.InitFTPQLSX();
									try
									{
										Thread.Sleep(500);
										DocUtils.UploadFile(folderUpdate, @"ImageMo\IMG");
										//MessageBox.Show("Update thành công");
									}
									catch
									{
										MessageBox.Show("Update ảnh thất bại");
									}
								});
							}
							catch
							{

							}
						}
						catch (Exception ex)
						{
							_socketVPM = null;
							//ConnectAnDonPicking();
						}

					}

				}
				catch
				{

				}
			}
		}
		/// <summary>
		/// Gửi code ra hid contro
		/// </summary>
		/// <param name="inputStr">giá trị hiển thị</param>
		private void SendDataToHID(string inputStr)
		{
			// Gửi code ra HID
			SendKeysSupported.SendWait("RTCLine.", "0", inputStr);
		}
		void LoadConfigShipTo()
		{
			_DataShipTo = TextUtils.Select("select * from ConfigShipTo");
		}
		void updateVersion()
		{
			if (!File.Exists(_pathFileConfigUpdate)) return;
			try
			{
				string[] lines = File.ReadAllLines(_pathFileConfigUpdate);
				if (lines == null) return;
				if (lines.Length < 2) return;

				string[] stringSeparators = new string[] { "||" };
				string[] arr = lines[1].Split(stringSeparators, 4, StringSplitOptions.RemoveEmptyEntries);

				if (arr == null) return;
				if (arr.Length < 4) return;

				_pathFolderUpdate = Path.Combine(Application.StartupPath, arr[1].Trim());
				_pathUpdateServer = arr[2].Trim();
				_pathFileVersion = Path.Combine(Application.StartupPath, arr[3].Trim());

				if (!Directory.Exists(_pathError))
				{
					Directory.CreateDirectory(_pathError);
				}
				if (!Directory.Exists(_pathFolderUpdate))
				{
					Directory.CreateDirectory(_pathFolderUpdate);
				}
				if (!File.Exists(_pathFileVersion))
				{
					File.Create(_pathFileVersion);
					File.WriteAllText(_pathFileVersion, "1");
				}
				int currentVerion = TextUtils.ToInt(File.ReadAllText(_pathFileVersion).Trim());
				string[] listFileSv = DocUtils.GetFilesList(_pathUpdateServer);
				if (listFileSv == null) return;
				if (listFileSv.Length == 0) return;

				List<string> lst = listFileSv.ToList();
				lst = lst.Where(o => o.Contains(".zip")).ToList();
				int newVersion = lst.Max(o => TextUtils.ToInt(Path.GetFileNameWithoutExtension(o)));

				if (newVersion != currentVerion)
				{
					Process.Start(Path.Combine(Application.StartupPath, "UpdateVersion.exe"));
				}
			}
			catch
			{
				MessageBox.Show("Can't connect to server!");
				return;
			}
		}
		void checkColorCD()
		{
			while (true)
			{
				Thread.Sleep(50);
				try
				{
					DataTable dt = TextUtils.Select("select top 1 * from StatusColorCD with (nolock)");
					if (dt.Rows.Count == 0) continue;

					string step = "";
					this.Invoke((MethodInvoker)delegate
					{
						if (cboWorkingStep.Text.Trim() == "")
						{
							step = CD;
						}
						else
							step = cboWorkingStep.Text.Trim();
					});
					_status = TextUtils.ToInt(dt.Rows[0][step]);
					this.Invoke((MethodInvoker)delegate
					{

						Control control = pannelBot.Controls.Find("lblCD", true)[0];
						switch (_status)
						{
							case 1:
								control.BackColor = Color.White;
								break;
							case 2:
								control.BackColor = Color.Yellow;
								break;
							case 3:
								control.BackColor = Color.Red;
								break;
							case 4:
								control.BackColor = Color.Lime;
								break;
							case 5:
								control.BackColor = Color.FromArgb(192, 192, 255);
								break;
							case 6:
								control.BackColor = Color.FromArgb(255, 192, 128);
								break;
							default:
								break;
						}
					});


				}
				catch (Exception)
				{

				}
			}
		}
		void startThreadFreeze()
		{
			_threadFreeze = new Thread(checkFreeze);
			_threadFreeze.IsBackground = true;
			_threadFreeze.Start();
		}
		void threadShowAndonDetails()
		{
			while (true)
			{
				Thread.Sleep(1000);
				if (string.IsNullOrWhiteSpace(_step)) continue;
				try
				{
					DataSet dts = TextUtils.GetListDataFromSP("spGetAndonDetailsByCD", "AnDonDetails"
						   , new string[1] { "@CD" }
						   , new object[1] { _step });
					DataTable data = dts.Tables[0];

					this.Invoke((MethodInvoker)delegate
					{
						txtNumDelay.Text = TextUtils.ToString(data.Rows[0]["TotalDelayNum"]);
						txtTimeDelay.Text = TextUtils.ToString(data.Rows[0]["TotalDelayTime"]);
					});
				}
				catch (Exception ex)
				{
					File.AppendAllText(_pathError + "/Error_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt",
							 DateTime.Now.ToString("HH:mm:ss") + ":threadShowAndonDetails(): " + ex.ToString() + Environment.NewLine);
				}
			}
		}
		void threadLoadAndon()
		{
			while (true)
			{
				Thread.Sleep(500);
				try
				{
					// Store load Andon theo ngày giờ hiện tại
					ArrayList arr = AndonBO.Instance.GetListObject("spGetAndonByDateTimeNow", new string[] { }, new object[] { });
					if (arr.Count > 0)
					{
						_oAndonModel = (AndonModel)arr[0];
					}
					else
					{
						_isStart = false;
						_oAndonModel = new AndonModel();
					}
				}
				catch (Exception ex)
				{
					_oAndonModel = new AndonModel();

					File.AppendAllText(_pathError + "/Error_" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt",
						DateTime.Now.ToString("HH:mm:ss") + ":LoadAndon(): " + ex.ToString() + Environment.NewLine);
				}
			}
		}
		void startThreadResetSocket()
		{
			_threadResetSocket = new Thread(resetSocket);
			_threadResetSocket.IsBackground = true;
			_threadResetSocket.Start();
		}
		void startThreadCheckColorCD()
		{
			_threadCheckColorCD = new Thread(checkColorCD);
			_threadCheckColorCD.IsBackground = true;
			_threadCheckColorCD.Start();
		}
		void resetSocket()
		{
			while (true)
			{
				Thread.Sleep(1000);

				if (_socket == null)
				{
					try
					{
						IPAddress ipAddOut = IPAddress.Parse(_socketIPAddress);
						IPEndPoint endPoint = new IPEndPoint(ipAddOut, _socketPort);
						_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
						_socket.Connect(endPoint);
					}
					catch
					{
						_socket = null;
					}
				}
			}
		}

		/// <summary>
		/// Kiểm tra xem có công đoạn khác nào bị delay, và công đoạn này ko bị delay
		/// Thì sẽ đóng băng form lại ko cho thao tác
		/// </summary>
		/// 
		void checkFreeze()
		{
			while (true)
			{
				Thread.Sleep(500);
				try
				{
					this.Invoke((MethodInvoker)delegate
					{
						if (cboSub.SelectedIndex != 0) return;

						if (cboWorkingStep.Text.Trim() != "")
							_stepCode = cboWorkingStep.Text.ToString().Trim();
						else
						{
							_stepCode = CD;
						}
						if (_stepCode == "") return;
						DataTable dt = TextUtils.Select("select top 1 * from StatusColorCD");
						if (dt.Rows.Count == 0) return;
						//Hiển thị số màu trong bảng StatusColorCD
						if (_stepCode.Contains("CD"))
							_currentStatus = TextUtils.ToInt(dt.Rows[0][_stepCode]);
						// Mở khóa CD
						if (_currentStatus != 4)
						{
							_enable = true;
							this.Invoke((MethodInvoker)delegate
							{
								if (btnNotUseCD8.Text == "Không sử dụng")
									panelTop.Enabled = pannelBot.Enabled = grdData.Enabled = _enable;
								if (_focusColumns == 1)
								{
									if (FocusbtnSave == 0)
									{
										//focus vào ô của cột tiếp theo
										setFocusCell(row, column);
									}
									else
									{
										btnSave.Focus();
									}
									if (txtQRCode.Text.Trim() == "")
									{
										txtQRCode.Focus();
									}

									_focusColumns = 0;
								}
							});
						}
						// Đóng khóa CD
						if (_currentStatus == 4)
						{
							_enable = false;
							_focusColumns = 1;
							this.Invoke((MethodInvoker)delegate
							{
								if (btnNotUseCD8.Text == "Không sử dụng")
									panelTop.Enabled = pannelBot.Enabled = grdData.Enabled = _enable;
							});
						}
					});

				}
				catch (Exception)
				{
				}
			}
		}

		/// <summary>
		/// Gửi thông điệp lên andon
		/// </summary>
		/// <param name="value">Giá trị, trạng thái</param>
		/// <param name="type">1:sự cố, 2: đã hoàn thành, 3: cập nhật SL thực tế, 4: khởi động ca</param>
		void sendDataTCP(string value, string type)
		{
			if (andonActive == 0) return;
			try
			{

				//Gửi tín hiệu delay xuống server Andon qua TCP/IP
				if (_socket != null && _socket.Connected)
				{
					this.Invoke((MethodInvoker)delegate
					{
						if (cboSub.SelectedIndex != 0) return;
						string sendData;
						if (cboWorkingStep.Text.Trim() != "")
						{
							sendData = string.Format("{0};{1};{2}", cboWorkingStep.Text.Trim(), value, type);
						}
						else
							sendData = string.Format("{0};{1};{2}", CD.Trim(), value, type);
						byte[] data = _encoding.GetBytes(sendData);
						_socket.Send(data);

					});
				}
			}
			catch (Exception ex)
			{
				//Ghi log vào 
				_socket = null;
				MessageBox.Show(ex.ToString() + Environment.NewLine);
			}
		}
		/// <summary>
		/// Gửi dữ liệu cho Ảnh a Tú
		/// </summary>
		/// <param name="value"></param>
		/// <param name="type"></param>
		void sendDataTCPVPM(string value)
		{
			if (andonActive == 0) return;
			try
			{

				if (_socketVPM != null && _socketVPM.Connected)
				{
					this.Invoke((MethodInvoker)delegate
					{
						string sendData = "";
						if (cboWorkingStep.Text.Trim() != "")
						{
							sendData = string.Format("{0}", value);
						}
						byte[] data = _encoding.GetBytes(sendData);
						_socketVPM.Send(data);

					});
				}
			}
			catch (Exception ex)
			{
				//Ghi log vào 
				_socket = null;
				MessageBox.Show(ex.ToString() + Environment.NewLine);
			}
		}
		static bool IsSocketConnected(Socket socketPicking)
		{
			try
			{
				return !((socketPicking.Poll(1000, SelectMode.SelectRead) && (socketPicking.Available == 0)) || !socketPicking.Connected);
			}
			catch
			{
				return false;
			}
		}
		void sendDataTCPAnDonPicking(string value)
		{
			try
			{
				if (!IsSocketConnected(_socketPicking))
				{
					_socketPicking = null;
				}
				else
				{
					//Gửi tín hiệu delay xuống server Andon qua TCP/IP
					this.Invoke((MethodInvoker)delegate
					{
						try
						{
							if (cboSub.SelectedIndex != 0) return;
							string sendData;
							if (cboWorkingStep.SelectedIndex > 0)
							{
								sendData = string.Format("{0};{1};{2}", cboWorkingStep.Text.Trim(), value, "");

								byte[] data = _encoding.GetBytes(sendData);
								_socketPicking.Send(data);
							}
						}
						catch
						{

						}
					});
				}
			}
			catch (Exception ex)
			{
				//Ghi log vào 
				_socketPicking = null;
				MessageBox.Show(ex.ToString() + Environment.NewLine);
			}
		}

		/// <summary>
		/// Set focus vào cell trên grid
		/// </summary>
		/// <param name="indexRow"></param>
		/// <param name="indexColum"></param>
		/// 
		void resetControl()
		{
			/*
			 * reset lại tiêu đề cột, các kết quả check
			 */
			for (int i = 3; i < 8; i++)
			{
				grvData.Columns["RealValue" + (i - 2)].Caption = "#";

				Control control = pannelBot.Controls.Find("textbox" + (i - 2), false)[0];
				control.Text = "";
			}
		}

		void initBackColor()
		{
			if (cboWorkingStep.SelectedIndex > 0)
			{
				cboWorkingStep.BackColor = Color.White;
			}
			else
			{
				cboWorkingStep.BackColor = _colorEmpty;
			}

			if (string.IsNullOrWhiteSpace(txtQRCode.Text))
			{
				txtQRCode.BackColor = _colorEmpty;
			}
			else
			{
				txtQRCode.BackColor = Color.White;
			}

			if (string.IsNullOrWhiteSpace(txtWorker.Text))
			{
				txtWorker.BackColor = _colorEmpty;
			}
			else
			{
				txtWorker.BackColor = Color.White;
			}

			if (string.IsNullOrWhiteSpace(txtOrder.Text))
			{
				txtOrder.BackColor = _colorEmpty;
			}
			else
			{
				txtOrder.BackColor = Color.White;
			}
		}

		void loadComboStep(string productCode)
		{
			dtCD = TextUtils.LoadDataFromSP("spGetProductStep_ByProductCode", "A"
			   , new string[1] { "@ProductCode" }
			   , new object[1] { productCode });
			if (dtCD == null) return;
			if (dtCD.Rows.Count == 0) return;

			_ProductCode = productCode;
			DataRow dr = dtCD.NewRow();
			dr["ID"] = 0;
			dr["ProductStepCode"] = "";
			productGroupCode = TextUtils.ToString(dtCD.Rows[0]["ProductGroupCode"]);
			productTypeCode = TextUtils.ToString(dtCD.Rows[0]["ProductTypeCode"]);
			dtCD.Rows.InsertAt(dr, 0);
			cboWorkingStep.DataSource = dtCD;
			cboWorkingStep.DisplayMember = "ProductStepCode";
			cboWorkingStep.ValueMember = "ID";


			if (_stepIndex > 0 && _stepIndex < dtCD.Rows.Count)
			{
				cboWorkingStep.SelectedIndex = _stepIndex;
			}
		}

		/// <summary>
		/// Load danh sách công đoạn kiểm tra
		/// </summary>
		void loadDataWorkingStep()
		{
			if (!string.IsNullOrWhiteSpace(txtQRCode.Text.Trim()))
			{
				string orderCode = txtQRCode.Text.Trim();
				string[] arr = orderCode.Split(' ');
				if (arr.Length > 1)
				{
					loadComboStep(arr[1]);

				}
				else
				{
					cboWorkingStep.DataSource = null;
				}
			}
			else
			{
				cboWorkingStep.DataSource = null;
			}
		}
		int getPreviousIndex()
		{
			int valueIndex = grvData.RowCount;
			for (int i = grvData.RowCount; i >= 0; i--)
			{
				int valueType = TextUtils.ToInt(grvData.GetRowCellValue(i, colValueType));
				_SortOrderValue = TextUtils.ToInt(grvData.GetRowCellValue(i, colSortOrder));

				if (valueType == 1)//Kiểu giá trị
				{
					if (!_lstSortOrder.Contains(_SortOrderValue))
					{
						valueIndex = i;
						break;
					}
				}
			}
			return valueIndex;
		}
		int getSumHeightRows()
		{
			int total = 0;
			GridViewInfo vi = grvData.GetViewInfo() as GridViewInfo;
			for (int i = 0; i < grvData.RowCount; i++)
			{
				GridRowInfo ri = vi.RowsInfo.FindRow(i);
				if (ri != null)
					total += ri.Bounds.Height;
			}

			return total;
		}
		int getRowIndex(int columnIndex)
		{
			int rowIndex = -1;
			for (int i = 0; i < _dtData.Rows.Count; i++)
			{
				DataRow r = _dtData.Rows[i];
				string value = TextUtils.ToString(r["RealValue" + (columnIndex - 2)]);
				int type = TextUtils.ToInt(r["ValueType"]);
				int checkValueType = TextUtils.ToInt(r["CheckValueType"]);
				string standardValue = TextUtils.ToString(r["StandardValue"]);
				int sortOrder = TextUtils.ToInt(r["SortOrder"]);

				string WorkingName = TextUtils.ToString(r["WorkingName"]);

				if (_lst4.Contains(i))
				{
					for (int j = 1; j < 6; j++)
					{
						string nameCol = "RealValue" + $"{j}";
						value = TextUtils.ToString(grvData.GetRowCellValue(i, nameCol));
						if (value != "")
							break;
					}
					if (!string.IsNullOrWhiteSpace(value))
						continue;
				}
				// Nếu có giá trị thì sẽ bỏ qua 
				if ((string.IsNullOrWhiteSpace(value) && type > 0) ||
					(checkValueType == 2 && string.IsNullOrWhiteSpace(value) &&
					!string.IsNullOrWhiteSpace(standardValue)))
				{
					//Nếu trùng với SortOrder của công thức được add thì bỏ qua
					if (!_lstSortOrder.Contains(sortOrder))
					{
						rowIndex = i;
						break;
					}

				}
			}
			if (rowIndex == -1)
			{
				rowIndex = grvData.RowCount + 1;
			}
			return rowIndex;
		}

		//void GotFocusTexbox(object sender, EventArgs e)
		//{
		//	//TextBox textBox = (TextBox)sender;
		//	//int textIndex = TextUtils.ToInt(textBox.Tag);
		//	//int columnIndex = textIndex + 2;
		//	//if (cboWorkingStep.Text.Trim() == "CD5")
		//	//	calculateShim(columnIndex);
		//}

		void setFocusCell(int indexRow, int indexColum)
		{
			row = indexRow;
			column = indexColum;
			if (indexRow <= grvData.RowCount - 1)
			{
				grvData.FocusedRowHandle = indexRow;
				_indexColumSauPCA = indexColum;
				grvData.FocusedColumn = grvData.VisibleColumns[indexColum];
				if (indexRow == _RowVPM)
				{
					_ColumnVPM = indexColum;
					_NameColumnVPM = "RealValue" + (indexColum - 2);

					switch (indexColum - 2)
					{
						case 1:
							_NameCol = colRealValue1.Caption;
							break;
						case 2:
							_NameCol = colRealValue2.Caption;
							break;
						case 3:
							_NameCol = colRealValue3.Caption;
							break;
						case 4:
							_NameCol = colRealValue4.Caption;
							break;
						case 5:
							_NameCol = colRealValue5.Caption;
							break;
					}
					_NameCol = txtOrder.Text.Trim() + "_" + _tienTo + _NameCol + " " + _productCode;
					string Data = "2:" + "ID" + "," + _NameCol;
					SendCom(Data);
					//sendDataTCPVPM(Data);
				}
				grvData.ShowEditor();
			}
			else
			{
				try
				{
					(pannelBot.Controls.Find("textBox" + (indexColum - 2), true)[0]).Focus();

				}
				catch (Exception)
				{

				}
			}
		}
		/// <summary>
		/// Chuyển tới dòng tiếp theo khi nhận là kiểu giá trị
		/// </summary>
		/// <param name="index">Số dòng đang focus</param>
		/// <returns></returns>
		int getNextIndex(int index)
		{
			int valueIndex = grvData.RowCount;
			for (int i = index + 1; i < grvData.RowCount; i++)
			{
				int valueType = TextUtils.ToInt(grvData.GetRowCellValue(i, colValueType));
				string value = "";
				for (int j = 1; j < 6; j++)
				{
					string nameCol = "RealValue" + $"{j}";
					value = TextUtils.ToString(grvData.GetRowCellValue(i, nameCol));
					if (value != "")
						break;
				}
				//#1# && #4#
				if ((_lst.Contains(i) || _lst4.Contains(i)) && (!string.IsNullOrWhiteSpace(value)))
				{
					continue;
				}
				if (valueType == 1)//Kiểu giá trị
				{
					valueIndex = i;
					break;
				}
			}
			return valueIndex;
		}

		/// <summary>
		/// Set caption cho các cột nhập giá trị kiểm tra
		/// Cái này làm cho dự án thêm hạng mục kiểm tra order sản phẩm
		/// </summary>
		void setCaptionGridColumn()
		{
			for (int i = 0; i < 6; i++)
			{
				if (_stt.Length > (int.Parse(_stt) + i).ToString().Length)
				{
					int lech = _stt.Length - (int.Parse(_stt) + i).ToString().Length;
					StringBuilder sokhong = new StringBuilder();
					for (int j = 0; j < lech; j++)
					{
						sokhong.Append("0");
					}
					grvData.Columns["RealValue" + (i + 1)].Caption = sokhong + (int.Parse(_stt) + i).ToString();
				}
				else
				{
					grvData.Columns["RealValue" + (i + 1)].Caption = (int.Parse(_stt) + i).ToString();
				}
			}
		}



		void loadGridData()
		{
			//Stopwatch s = new Stopwatch();
			//s.Start();

			if (string.IsNullOrEmpty(txtWorker.Text.Trim()) && cboWorkingStep.SelectedIndex != 0)
			{
				MessageBox.Show("Bạn chưa nhập tên người làm", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				txtWorker.Focus();
				return;
			}
			_step = cboWorkingStep.Text.Trim();

			if (cboWorkingStep.Text.Trim() == "CD1")
			{
				sendDataTCP("0", "4");
			}
			_isStartColor = false;
			_workingStepID = TextUtils.ToInt(cboWorkingStep.SelectedValue);
			string qrCode = txtQRCode.Text.Trim();
			if (string.IsNullOrWhiteSpace(qrCode)) return;

			if (cboWorkingStep.SelectedIndex > 0)
			{
				cboWorkingStep.BackColor = Color.White;
			}
			else
			{
				cboWorkingStep.BackColor = _colorEmpty;
			}

			resetControl();

			if (_workingStepID == 0)
			{
				_dtData = null;
				grdData.DataSource = null;
				txtStepName.Text = "";
				return;
			}

			//Sinh ra file lưu tên công đoạn
			File.WriteAllText(Application.StartupPath + "\\CD.txt", cboWorkingStep.Text.Trim());

			/*
			 * Tách chuỗi QrCode
			 */
			string orderCode = txtQRCode.Text.Trim();
			string[] arr1 = orderCode.Split(' ');
			if (arr1.Length > 1)
			{
				_order = arr1[0];
				_productCode = arr1[1].Trim();
				string[] arr;
				if (_order.Contains("-"))
				{
					arr = _order.Split('-');
					_tienTo = (arr[0] + "-" + arr[1] + "-");
					_stt = arr[2];
				}
				else
				{
					arr = Regex.Split(_order, @"\D+");
					_stt = arr[arr.Length - 1];
					_tienTo = _order.Substring(0, _order.IndexOf(_stt));
				}
			}

			//Ghi dữ liệu trạng thái vào bảng StatusCD
			string stepCode = cboWorkingStep.Text.Trim();
			//if (stepCode == "CD8-1") stepCode = "CD81";
			//string sqlUpdate = string.Format("Update StatusCD WITH (ROWLOCK) set {0} = 1", stepCode);
			//TextUtils.ExcuteSQL(sqlUpdate);

			//Hiển thị nút không sử dụng khi chọn xong công đoạn
			btnNotUseCD8.Visible = true;
			//Lấy công thức từ dữ liệu bảng ConfigFormula theo ProductGroupCode và ProductTypeCode

			//Gán dữ liệu vào grid
			DataSet ds = ProductCheckHistoryDetailBO.Instance.GetDataSet("spGetWorkingByProduct",
				new string[] { "@WorkingStepID", "@WorkingStepCode", "@ProductCode" },
				new object[] { _workingStepID, stepCode, arr1[1].ToString() });

			_dtData = ds.Tables[0];
			txtStepName.Text = TextUtils.ToString(ds.Tables[1].Rows.Count > 0 ? ds.Tables[1].Rows[0][0] : "");

			_GroupCode = TextUtils.ToString(ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0]["ProductGroupCode"] : "");
			txtName.Text = TextUtils.ToString(ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0]["ProductName"] : "");
			txtGroupCode.Text = _GroupCode;// TextUtils.ToString(ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0]["LoaiMo"] : "");
										   //txtGoal.Text = TextUtils.ToString(ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0]["Goal"] : "");
			txtProductTypeCode.Text = TextUtils.ToString(ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0]["ProductTypeCode"] : "");
			string gunNumber = TextUtils.ToString(ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0]["GunNumber"] : "");
			string jobNumber = TextUtils.ToString(ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0]["JobNumber"] : "");
			string qtyOcBanGa = TextUtils.ToString(ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0]["QtyOcBanGa"] : "");
			string qtyOcBanThat = TextUtils.ToString(ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0]["QtyOcBanThat"] : "");
			_EngineWeightMin = TextUtils.ToDouble(ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0]["EngineWeightMin"] : 0);
			_EngineWeightMax = TextUtils.ToDouble(ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0]["EngineWeightMax"] : 0);
			_EngineWeight = TextUtils.ToDouble(ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0]["EngineWeight"] : 0);
			_IDProduct = TextUtils.ToInt(ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0]["ID"] : "");
			_PlateWeight = TextUtils.ToDouble(ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0]["PlateWeight"] : 0);
			_JigWeight = TextUtils.ToDouble(ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0]["JigWeight"] : 0);
			_productID = TextUtils.ToInt(ds.Tables[2].Rows.Count > 0 ? ds.Tables[2].Rows[0]["ID"] : 0);
			//Hiển thị Công đoạn bỏ qua của PID 
			string CDSkip = TextUtils.ToString(TextUtils.ExcuteScalar($"SELECT top 1 CDSkip FROM SkipCD WHERE Product='{TextUtils.ToString(arr1[1])}'"));
			_lstCD = CDSkip.Split(',').ToList();
			if (_DataShipTo != null && _DataShipTo.Rows.Count != 0)
			{
				if (_DataShipTo.Select($"ShipTo = '{txtGoal.Text.Trim()}' and IsHidden = 1").Count() > 0)
				{
					if (_dtData != null && _dtData.Rows.Count != 0)
						_dtData = _dtData.Select("SortOrder < 1000").CopyToDataTable();
				}
			}
			grdData.DataSource = _dtData;
			//check cân master
			//Điền giá trị tiêu chuẩn cho Bơm mỡ
			GiaTriTT();
			//if (_EngineWeight == 0)
			//{
			//	SendCom("SM");
			//}
			//else
			//{
			//	SendCom("NOSM");
			//}

			//23/07/2021
			//Gửi mã khung loại sản phẩm khi là CD10
			//if (cboWorkingStep.Text.Trim() == "CD10" && cboSub.SelectedIndex == 0)
			//{
			//	JobChange($"KHUNG_{txtProductTypeCode.Text.Trim()}.vp");
			//}


			for (int i = 0; i < grvData.RowCount; i++)
			{
				string rowVPM = TextUtils.ToString(grvData.GetRowCellValue(i, colProductWorkingName));
				//Lấy dòng điền chụp ảnh 
				if (rowVPM.ToUpper().Contains("#3#"))
				{
					_RowVPM = i;
				}
				//lấy dòng để cân
				if (rowVPM.ToUpper().Contains("#2#"))
				{
					_RowSauPCA = i;
					if (cboWorkingStep.Text.Trim().Contains("TP-"))
					{
						//Check giá trị tiêu chuẩn có chưa + Giá trị khối lượng (có rồi ) thì thêm vào 
						int Stand = TextUtils.ToInt(TextUtils.ToDouble(grvData.GetRowCellValue(_RowSauPCA, colMaxValue)));
						if (Stand == 0 && _EngineWeight != 0 && _EngineWeightMax != 0)
						{
							string StandValue = _EngineWeightMin + "~" + _EngineWeightMax;
							//Gán giá trị min max và giá trị tiêu chuẩn lên grv
							SetValue(_RowSauPCA, _EngineWeightMin, _EngineWeightMax, StandValue);
						}
					}
				}
				//Motor
				if (rowVPM.ToUpper().Contains("#5#"))
				{
					try
					{
						_RowMotor = i;
						//Check đã có mã Motor chưa
						string Motor = TextUtils.ToString(grvData.GetRowCellValue(_RowMotor, colStandardValue));
						if (Motor.Trim() == "0")
						{
							//Tìm kiếm Mã Motor theo Order trong base ShiStock
							DataTable dt = TextUtils.Select($"SELECT TOP 1 ArticleID FROM [ShiStock].[dbo].[OrderPart] WHERE OrderCodeAndCnt ='{txtOrder.Text.Trim()}' AND Shelf=''");
							if (dt != null && dt.Rows.Count > 0)
							{
								//Hiển thị lên gridview 
								grvData.SetRowCellValue(_RowMotor, colStandardValue, dt.Rows[0]["ArticleID"]);
								//Update vào base 
								TextUtils.ExcuteSQL($"UPDATE dbo.[ProductWorking] SET PeriodValue='{dt.Rows[0]["ArticleID"]}' WHERE ProductID = {_productID} AND ProductStepID={_workingStepID} AND IsDeleted = 0  AND WorkingName LIKE '%#5#%'");
							}
						}
					}
					catch (Exception ex)
					{
						TextUtils.errorLog($"Lỗi mã Motor {txtOrder.Text.Trim()}", $"{ex}", "Line Hyp");
					}
				}
				//if (workingName.ToUpper().Contains("#6#"))
				//{
				//	//dòng cân

				//}
				//Check mã Pin
				if (rowVPM.ToUpper().Contains("#7#"))
				{
					try
					{
						_RowPin = i;
						string PinCode = TextUtils.ToString(grvData.GetRowCellValue(_RowPin, colStandardValue));
						if (PinCode == "0")
						{
							DataTable dt = TextUtils.Select($"SELECT TOP 1 ArticleID, Qty FROM [ShiStock].[dbo].[OrderPart] WHERE OrderCodeAndCnt ='{txtOrder.Text.Trim()}' AND Description LIKE '%RING GEAR PIN%'");
							if (dt != null && dt.Rows.Count > 0)
							{
								//Hiển thị lên gridview 
								grvData.SetRowCellValue(_RowPin, colStandardValue, dt.Rows[0]["ArticleID"]);
								//Update vào base 
								TextUtils.ExcuteSQL($"UPDATE dbo.[ProductWorking] SET PeriodValue='{dt.Rows[0]["ArticleID"]}' WHERE ProductID = {_productID} AND ProductStepID={_workingStepID} AND IsDeleted = 0  AND WorkingName LIKE '%#7#%'");
							}
						}
					}
					catch (Exception ex)
					{
						TextUtils.errorLog($"Lỗi mã pin {txtOrder.Text.Trim()}", $"{ex}", "Line Hyp");
					}

				}
				decimal SL = 0;
				if (rowVPM.ToUpper().Contains("#8#"))
				{
					try
					{
						_RowPinSL = i;
						int PinCode = TextUtils.ToInt(grvData.GetRowCellValue(_RowPin, colStandardValue));
						if (PinCode == 0)
						{
							DataTable dt = TextUtils.Select($"SELECT TOP 1 ArticleID, Qty FROM [ShiStock].[dbo].[OrderPart] WHERE OrderCodeAndCnt ='{txtOrder.Text.Trim()}' AND Description LIKE '%RING GEAR PIN%'");
							if (dt != null && dt.Rows.Count > 0)
							{
								SL = TextUtils.ToDecimal(dt.Rows[0]["Qty"]) / TextUtils.ToDecimal(txtQty.Text.Trim());
								//Hiển thị lên gridview 
								grvData.SetRowCellValue(_RowPinSL, colStandardValue, SL);
								//Update vào base 
								TextUtils.ExcuteSQL($"UPDATE dbo.[ProductWorking] SET PeriodValue='{SL}' WHERE ProductID = {_productID} AND ProductStepID={_workingStepID} AND IsDeleted = 0  AND WorkingName LIKE '%#8#%'");
							}
						}
					}
					catch (Exception ex)
					{
						TextUtils.errorLog($"Lỗi số lượng pin {SL},{txtQty.Text.Trim()}", $"{ex}", "Line Hyp");
					}
				}
			}

			string file = string.Format("{0};{1};{2};{3}", gunNumber, jobNumber, qtyOcBanGa, qtyOcBanThat);
			try
			{
				System.IO.File.WriteAllText(Application.StartupPath + "\\SettingsTouque.txt", file);
			}
			catch
			{
			}

			// Set lại chiều cao của dòng
			if (grvData.RowCount > 0 && chkFitRow.Checked)
			{
				grvData.RowHeight = -1;
				int totalHeightRow = this.getSumHeightRows();
				if ((oldHeightGrid - grvData.ColumnPanelRowHeight - 30) > totalHeightRow)
				{
					grvData.RowHeight = (oldHeightGrid - grvData.ColumnPanelRowHeight - 30) / grvData.RowCount;
				}
			}
			if ((cboWorkingStep.Text == "CD3" || cboWorkingStep.Text.Trim().Contains("TP-")) && txtGroupCode.Text.Trim().ToUpper() == "DAIFUKU")
			{
				_lstFormula.Clear();
				_lstSortOrder.Clear();
				string sql = $"select * from ConfigFormulaAll where ProductTypeCode = '{productTypeCode}' and ProductGroupCode = '{productGroupCode}' Order By STT";
				DataTable dt = TextUtils.Select(sql);
				if (dt != null && dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						string formula = TextUtils.ToString(dt.Rows[i]["Formula"]);
						if (formula != "")
						{
							int sortOrder = TextUtils.ToInt(formula.Split(';')[1]);
							_lstSortOrder.Add(sortOrder);
							//formula = formula.Split(';')[0];
							_lstFormula.Add(formula);
						}
					}
				}
			}
			if (cboWorkingStep.Text.Trim() == "CD5" && txtGroupCode.Text.Trim().ToUpper() != "DAIFUKU")
			{
				_lstFormula.Clear();
				_lstSortOrder.Clear();
				string sql = $"select * from ConfigFormulaAll where ProductTypeCode = '{productTypeCode}' and ProductGroupCode = '{productGroupCode}' Order By STT";
				DataTable dt = TextUtils.Select(sql);
				if (dt != null && dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						string formula = TextUtils.ToString(dt.Rows[i]["Formula"]);
						if (formula != "")
						{
							int sortOrder = TextUtils.ToInt(formula.Split(';')[1]);
							_lstSortOrder.Add(sortOrder);
							//formula = formula.Split(';')[0];
							_lstFormula.Add(formula);
						}
					}
					for (int i = 0; i < grvData.RowCount; i++)
					{
						_ProductWorkingName = TextUtils.ToString(grvData.GetRowCellValue(i, colProductWorkingName));
						if (_ProductWorkingName.ToUpper().Contains("GMD"))
						{
							_rowGMD = i;
							_sortOrderGMD = TextUtils.ToInt(grvData.GetRowCellValue(i, colSortOrder));
							_GMD = TextUtils.ToDecimal(grvData.GetRowCellValue(i, colStandardValue));
							grvData.SetRowCellValue(i, colRealValue1, _GMD);
						}
					}
				}
			}
			//Nhảy đến ô cần điền giá trị đầu tiên
			int cIndex = grvData.Columns["RealValue1"].VisibleIndex;
			setFocusCell(getRowIndex(cIndex), cIndex);
			_isStartColor = true;
			setCaptionGridColumn();
			_currentIndex = 1;//đánh dấu số thứ tự sản phẩm được check trong order 
		}
		/// <summary>
		/// Gửi jib loại sản phẩm qua VPM
		/// </summary>
		/// <param name="JobName">Loại sản phẩm</param>
		public async void JobChange(string JobName)
		{
			try
			{
				//bool result = VPMControl.ForceUnloadAllProgram("192.168.125.129", 20000);
				bool result = VPMControl.ForceUnloadAllProgram("192.168.125.129", 20000);
				if (result == false)
				{
					if (MessageBox.Show("kết nối cam không thành công", "Thông báo", MessageBoxButtons.OK) == DialogResult.OK)
					{
						return;
					}
				}
				Task LoadJobTask = Task.Factory.StartNew(() =>
				{
					return VPMControl.ForceLoadJob("192.168.125.129", 20000, JobName);
				}).ContinueWith((loadJobResult) =>
				{
					if (loadJobResult.Result == false)
					{
						if (VPMControl.ForceLoadJob("192.168.125.129", 20000, JobName) == false)
						{
							if (MessageBox.Show("Đổi job không thành công", "Thông báo", MessageBoxButtons.OK) == DialogResult.OK)
							{
								return;
							}
						}
					}
				});
				await LoadJobTask;
			}
			catch (Exception)
			{
				throw;
			}

		}
		/// <summary>
		/// lấy giá trị tiêu chuẩn của mỡ và gửi cho trung
		/// </summary>
		void GiaTriTT()
		{
			//ToDo: Check có nên gửi giá trị tiêu chuẩn hay chưa 
			// Lấy dữ liệu giá trị tiêu chuẩn của CD10 Bơm mỡ
			if (chkFat.Checked && cboWorkingStep.Text.Trim() == "CD10")
			{
				for (int i = 0; i < grvData.RowCount; i++)
				{
					FatContent = TextUtils.ToString(grvData.GetRowCellValue(i, colProductWorkingName)); // Mục kiểm tra
					if (FatContent.ToUpper().Contains("LOẠI MỠ"))
					{
						RealFat = TextUtils.ToString(grvData.GetRowCellValue(i, colStandardValue));//giá trị tiêu chuẩn
					}
					if (FatContent.ToUpper().Contains("LƯỢNG MỠ"))
					{
						RealFatContentMin = TextUtils.ToInt(grvData.GetRowCellValue(i, colMinValue));// giá trị tiêu chuẩn min 
						RealFatContentMax = TextUtils.ToInt(grvData.GetRowCellValue(i, colMaxValue));// giá trị tiêu chuẩn max
						Average = (RealFatContentMin + RealFatContentMax) * 1.0 / 2; // Tính giá trị trung bình 
					}
				}
				// gửi giá trị tiêu chuẩn qua Trung
				if (RealFat.Trim() != "0" || Average != 9999)
					SendCom("1:" + RealFat + ";" + Average + ";" + RealFatContentMin + ";" + RealFatContentMax + ";" + $"KHUNG_{txtProductTypeCode.Text.Trim()}");
			}
		}
		private void cboWorkingStep_SelectedIndexChanged(object sender, EventArgs e)
		{
			//_lstCD.Clear();
			_index = 999;
			//Danh sách công thức
			_lstFormula.Clear();
			//Danh sách SortOrder theo công thức
			_lstSortOrder.Clear();
			//Mã AritelID để so sánh Motor
			_AritelID = "";
			_RowMotor = 999;
			_RowPin = 999;
			_RowPinSL = 999;
			_lst.Clear();
			_lst4.Clear();
			_sortOrderGMD = -99999;
			_SortOrder1 = -99999;
			_RowSauPCA = 999;
			_RowVPM = 999;
			_NoIndefinitely = -9999;
			_lstMotor.Clear();
			if (cboWorkingStep.Text.Trim() == "CD10" || cboWorkingStep.Text.Trim().Contains("TP-"))
			{

				if (Comport != null && Comport.IsOpen)
				{
				}
				else
				{
					if (chkFat.Checked)
					{
						// Kết nối Com
						ConnectCom();
					}
				}


			}
			loadGridData();
			//Check bỏ qua công đoạn
			if (CheckSkipCD(0, 0) == false)
			{
				MessageBox.Show($"Sản phẩm chưa đi qua công đoạn {CDOld}");
			}
		}
		/// <summary>
		/// Check bỏ qua công đoạn
		/// </summary>
		/// <param name="TextOrCb">0:Check ComboWorking, 1:Check text đánh giá</param>
		/// <param name="columnIndex">khi 1:Check text đánh giá thì truyền columnIndex qua</param>
		/// <returns></returns>
		bool CheckSkipCD(int TextOrCb, int columnIndex)
		{
			//Check bỏ qua công đoạn
			if (!cboWorkingStep.Text.Contains("TP"))
			{
				for (int i = cboWorkingStep.SelectedIndex - 1; i > 0; i--)
				{
					if (dtCD == null) return false;
					if (dtCD.Rows.Count <= 0) return false;
					CDOld = TextUtils.ToString(dtCD.Rows[i]["ProductStepCode"]);
					if (_lstCD.Contains(CDOld))
					{
						continue;
					}
					else
					{
						//Check có trong database chưa 
						if (cboWorkingStep.Text.Trim() != TextUtils.ToString(dtCD.Rows[1]["ProductStepCode"]))
						{
							int check = 0;
							if (TextOrCb == 0)
							{
								check = TextUtils.ToInt(TextUtils.ExcuteScalar($"SELECT TOP 1 1 FROM[SumitomoHyp].[dbo].[AndonDetail]  WHERE ProductStepCode='{CDOld}' AND QrCode='{txtQRCode.Text.Trim()}' AND OrderCode='{txtOrder.Text.Trim()}'"));
							}
							else
							{
								check = TextUtils.ToInt(TextUtils.ExcuteScalar($"SELECT TOP 1 1 FROM[SumitomoHyp].[dbo].[AndonDetail]  WHERE ProductStepCode='{CDOld}' AND QrCode='{_tienTo + grvData.Columns[columnIndex].Caption + " " + _productCode}' AND OrderCode='{txtOrder.Text.Trim()}'"));
							}
							if (check == 0)
							{
								return false;
							}
							return true;

						}
					}
				}
			}
			return true;

		}
		private void btnSave_Click(object sender, EventArgs e)
		{
			if (grvData.RowCount == 0)
			{
				return;
			}

			int productID = _productID;
			int stepID = TextUtils.ToInt(cboWorkingStep.SelectedValue);
			if (productID == 0)
			{
				MessageBox.Show(string.Format("Không tồn tại sản phẩm có mã [{0}]!", _productCode.Trim()), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (stepID == 0)
			{
				MessageBox.Show(string.Format("Bạn chưa chọn công đoạn nào!", _productCode.Trim()), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (string.IsNullOrEmpty(txtWorker.Text.Trim()))
			{
				MessageBox.Show(string.Format("Bạn chưa điền người làm!", _productCode.Trim()), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				txtWorker.Focus();
				return;
			}
			bool isHasValue = false;
			for (int i = 1; i < 6; i++)
			{
				Control control = pannelBot.Controls.Find("textbox" + i, false)[0];
				if (control.Text == "0" || control.Text == "1" || control.Text == "13" || control.Text == "03")
				{
					isHasValue = true;
					break;
				}
			}
			if (!isHasValue)
			{
				//MessageBox.Show("Bạn chưa nhập các giá trị kiểm tra!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			DialogResult result = MessageBox.Show("Bạn có chắc muốn cất dữ liệu?", "Cất?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (result == DialogResult.No) return;

			int count = _dtData.Rows.Count;
			//TimeSpan b = new TimeSpan();
			//Stopwatch stopwatch = new Stopwatch();
			//stopwatch.Start();

			for (int i = 3; i < 8; i++)
			{
				Control control = pannelBot.Controls.Find("textbox" + (i - 2), false)[0];
				string columnCaption = grvData.Columns[i].Caption;
				string qrCode = "";
				if (string.IsNullOrEmpty(control.Text.Trim()))
					continue;
				else
				{
					qrCode = _tienTo + columnCaption + " " + _productCode;
				}
				//if (string.IsNullOrWhiteSpace(control.Text.Trim()) || TextUtils.ToInt(control.Text.Trim()) > 1
				//	|| TextUtils.ToInt(control.Text.Trim()) < 0)
				//{
				//	continue;
				//}
				if (string.IsNullOrWhiteSpace(control.Text.Trim()) || TextUtils.ToInt(control.Text.Trim()) > 1 && TextUtils.ToInt(control.Text.Trim()) < 3 || TextUtils.ToInt(control.Text.Trim()) > 3 && TextUtils.ToInt(control.Text.Trim()) < 13 || TextUtils.ToInt(control.Text.Trim()) > 13
					|| TextUtils.ToInt(control.Text.Trim()) < 0)
				{
					continue;
				}
				string orderCode = string.IsNullOrWhiteSpace(txtOrder.Text.Trim()) ?
						(_tienTo.Contains("-") ? _tienTo.Substring(0, _tienTo.Length - 1) : _order) :
						txtOrder.Text.Trim();

				/*
				 * Insert Master
				 */
				if (cboWorkingStep.Text.ToUpper() == "CD6" || cboWorkingStep.Text.ToUpper().StartsWith("TP-"))
				{
					try
					{
						ProductCheckHistoryModel master = new ProductCheckHistoryModel();
						master.CreatedBy = txtWorker.Text.Trim();
						master.CreatedDate = DateTime.Now;
						master.DateLR = DateTime.Now;
						master.OrderCode = orderCode;
						master.ProductCode = _productCode;
						master.ProductID = productID;
						master.QRCode = qrCode;
						master.UpdatedBy = txtWorker.Text.Trim();
						master.UpdatedDate = DateTime.Now;
						//master.Main = Sub;
						master.SalesOrder = txtSalesOrder.Text.Trim();
						Expression exp2 = new Expression("QRCode", qrCode);
						Expression exp1 = new Expression("OrderCode", orderCode);
						ArrayList arr = ProductCheckHistoryBO.Instance.FindByExpression(exp1.And(exp2));
						saveMaster(master, arr);
					}
					catch
					{

					}
					//ProductCheckHistoryBO.Instance.Insert(master);
				}
				/*
				 * Insert Detail dữ liệu kiểm tra vào bảng
				 */

				List<ProductCheckHistoryDetailModel> lstDetail = new List<ProductCheckHistoryDetailModel>();
				for (int j = 0; j < count; j++)
				{
					ProductCheckHistoryDetailModel cModel = new ProductCheckHistoryDetailModel();
					//cModel.ProductCheckHistoryID = masterID;
					cModel.ProductStepID = stepID;
					cModel.ProductStepCode = cboWorkingStep.Text.Trim();
					cModel.ProductStepName = txtStepName.Text.Trim();
					cModel.SSortOrder = TextUtils.ToInt(grvData.GetRowCellValue(j, colSSortOrder));

					cModel.ProductWorkingID = TextUtils.ToInt(grvData.GetRowCellValue(j, colWorkingID));
					cModel.ProductWorkingName = TextUtils.ToString(grvData.GetRowCellValue(j, colProductWorkingName));
					cModel.WSortOrder = TextUtils.ToInt(grvData.GetRowCellValue(j, colSortOrder));

					cModel.WorkerCode = txtWorker.Text.Trim();
					cModel.StandardValue = TextUtils.ToString(grvData.GetRowCellValue(j, colStandardValue));
					cModel.RealValue = TextUtils.ToString(grvData.GetRowCellValue(j, "RealValue" + (i - 2)));
					cModel.ValueType = TextUtils.ToInt(grvData.GetRowCellValue(j, colValueType));
					cModel.ValueTypeName = cModel.ValueType == 1 ? "Giá trị\n数値" : "Check mark";
					cModel.EditValue1 = "";
					cModel.EditValue2 = "";
					cModel.StatusResult = TextUtils.ToInt(control.Text.Trim());
					cModel.ProductID = productID;
					cModel.QRCode = qrCode;
					cModel.OrderCode = orderCode;
					//cModel.PackageNumber = _tienTo.Contains("-") ? _tienTo.Split('-')[1] : "";
					//cModel.QtyInPackage = columnCaption;
					cModel.Approved = "";
					cModel.Monitor = "";
					cModel.DateLR = DateTime.Now;
					cModel.EditContent = "";
					cModel.EditDate = DateTime.Now;
					cModel.ProductCode = _productCode;
					cModel.ProductOrder = _order;
					cModel.Line = cboSub.Text.Trim();
					lstDetail.Add(cModel);

					//ProductCheckHistoryDetailBO.Instance.Insert(cModel);

				}
				saveDetail(lstDetail);

				if (cboWorkingStep.Text.Trim().ToUpper().Contains("CD6"))
				{
					_Planmodel.Show = 3;
					savePlan(_Planmodel);
				}
				//Update Số lượng thực tế vào kế hoạch sản xuất 

				if (cboWorkingStep.Text.Trim().ToUpper().Contains("TP-"))
				{
					try
					{
						if (TextUtils.ToInt(control.Text.Trim()) == 1 || TextUtils.ToInt(control.Text.Trim()) == 13)
						{
							_Planmodel.RealQty += 1; //Cộng vào số lượng thực tế
							if (_Planmodel.Qty <= _Planmodel.RealQty)//So sánh số lượng thực tế với số lượng 
							{
								_Planmodel.Status = 1;// Trạng thái đã xong 1 , Trạng thái chưa xong 2, Trạng thái Quá Hạn 3
							}
						}
						if (TextUtils.ToInt(control.Text.Trim()) == 0 || TextUtils.ToInt(control.Text.Trim()) == 03)
						{
							_Planmodel.QtyNG += 1; //Cộng vào số lượng NG
						}
						_Planmodel.Show = 4;
						savePlan(_Planmodel);

					}
					catch
					{

					}
				}

			}
			if (chkFat.Checked && cboWorkingStep.Text.Trim() == "CD10")
			{
				RealFat = "0";
				Average = 0;
				SendCom(RealFat + ";" + Average + ";" + 0 + ";" + 0);
			}

			//stopwatch.Stop();
			//b = stopwatch.Elapsed;

			// MessageBox.Show(string.Format("Time: {0}", b.Seconds));

			_stepIndex = cboWorkingStep.SelectedIndex;
			_currentIndex = 0;
			_PlanID = 0;
			_totalTimeDelay = 0;
			FocusbtnSave = 0;
			grdData.DataSource = null;
			txtQRCode.Text = "";
			txtOrder.Text = "";
			cboWorkingStep.DataSource = null;
			//btnNotUseCD8.Visible = false;
			resetControl();
			txtQRCode.Focus();
		}
		async void savePlan(ProductionPlanModel item)
		{
			Task task = Task.Factory.StartNew(() =>
			{
				ProductionPlanBO.Instance.Update(item);
			}
			);
			await task;
		}
		async void saveMaster(ProductCheckHistoryModel master, ArrayList arr)
		{
			Task task = Task.Factory.StartNew(() =>
				{
					if (arr.Count > 0)
					{
						return;
					}

					ProductCheckHistoryBO.Instance.Insert(master);
				}
			);
			await task;
		}

		async void saveDetail(List<ProductCheckHistoryDetailModel> lstDetail)
		{
			Task task = Task.Factory.StartNew(() =>
				{
					foreach (ProductCheckHistoryDetailModel item in lstDetail)
					{
						ProductCheckHistoryDetailBO.Instance.Insert(item);
					}
				}
			);
			await task;
		}

		private void txtQRCode_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				txtOrder.Focus();
			}
		}
		private void txtOrder_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				try
				{
					string orderFull = txtOrder.Text.Trim();
					_Planmodel = ((ProductionPlanModel)ProductionPlanBO.Instance.FindByAttribute("OrderCodeFull", orderFull)[0]);
					if (_Planmodel.ID <= 0)
					{
						MessageBox.Show("Không tồn tại Order!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						txtOrder.Focus();
						txtOrder.SelectAll();
						return;
					}
					txtQty.Text = TextUtils.ToString(_Planmodel.Qty);
					txtQtyReal.Text = TextUtils.ToString(_Planmodel.RealQty);
					txtGoal.Text = _Planmodel.ShipTo;
					txtSalesOrder.Text = _Planmodel.SalesOrder;
					_Print = _Planmodel.Prints;

					cboWorkingStep.Focus();
					loadDataWorkingStep();
					//cboWorkingStep.Focus();
				}
				catch
				{
					MessageBox.Show("Nhập lại Order!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					txtOrder.SelectAll();
				}
			}
		}
		private void frmProductCheckHistory_FormClosed(object sender, FormClosedEventArgs e)
		{
			//if (_threadDelay != null) _threadDelay.Abort();
			if (_threadRisk != null) _threadRisk.Abort();
			if (_threadFreeze != null) _threadFreeze.Abort();
			File.WriteAllText(pathSub, $"{cboSub.SelectedIndex}");
			Application.Exit();
		}
		private void grvData_KeyDown(object sender, KeyEventArgs e)
		{
			if (grvData.FocusedRowHandle == grvData.RowCount - 1 && e.KeyCode == Keys.Down)//dòng cuối cùng
			{
				int indexColumn = grvData.FocusedColumn.VisibleIndex;
				(pannelBot.Controls.Find("textBox" + (indexColumn - 2), true)[0]).Focus();
			}
		}
		void calculateShim(int columnindex, int currentSortOrder)
		{
			int rowIndexFormula = -99999;
			//Lấy ra danh sách fomula chứa sortorder hiện tại
			List<string> lstFormula = _lstFormula.Where(o => o.Contains($"#{currentSortOrder}#")).ToList();
			for (int j = 0; j < lstFormula.Count; j++)
			{
				string Formula1 = lstFormula[j];

				if (Formula1.Trim() == "") continue;
				int SortOrderFormula = TextUtils.ToInt(Formula1.Split(';')[1]);
				string Formula = (Formula1.Split(';')[0]);
				//Láy giá trị tưng dòng một
				for (int i = 0; i < grvData.RowCount; i++)
				{
					int sortOrder = TextUtils.ToInt(grvData.GetRowCellValue(i, colSortOrder));
					decimal value = TextUtils.ToDecimal(grvData.GetRowCellValue(i, "RealValue" + (columnindex - 2)));
					//int SortOrderFormula = _lstSortOrder[j];

					if (sortOrder == SortOrderFormula)
					{
						rowIndexFormula = i;
					}
					Formula = Formula.Replace($"#{sortOrder}#", value.ToString());
				}
				if ((cboWorkingStep.Text.Trim() == "CD3" || cboWorkingStep.Text.Trim().Contains("TP-")) && txtGroupCode.Text.Trim().ToUpper() == "DAIFUKU")
				{
					//CD3 hoặc TP sau khi Replace trả về dạng Min(1,2,3,4)-Max(1,2,3,4)
					//Vị trí đầu tiên Max
					decimal max = 0;
					decimal min = 0;
					if (Formula.ToUpper().Contains("MAX"))
					{
						int ShelfMax = Formula.IndexOf("MAX(");
						//Vị trí cuối cùng sau Max
						string ChuoiMax = Formula.Substring(ShelfMax + 4);
						int ShelfMaxLast = ChuoiMax.IndexOf(")", ShelfMax);
						ChuoiMax = ChuoiMax.Substring(0, ShelfMaxLast);
						//tách chuỗi qua ',' 
						List<decimal> lst = new List<decimal>();
						foreach (string st in ChuoiMax.Split(','))
						{
							lst.Add(TextUtils.ToDecimal(st));
						}
						max = lst.Max();
						Formula = Formula.Replace($"MAX({ChuoiMax})", lst.Max().ToString());
					}
					if (Formula.ToUpper().Contains("MIN"))
					{
						int ShelfMin = Formula.IndexOf("MIN(");
						//Vị trí cuối cùng sau Max
						string ChuoiMin = Formula.Substring(ShelfMin + 4);
						int ShelfMinLast = ChuoiMin.IndexOf(")", ShelfMin);
						ChuoiMin = ChuoiMin.Substring(0, ShelfMinLast);
						//tách chuỗi qua ',' 
						List<decimal> lst = new List<decimal>();
						foreach (string st in ChuoiMin.Split(','))
						{
							lst.Add(TextUtils.ToDecimal(st));
						}
						min = lst.Min();
						Formula = Formula.Replace($"MIN({ChuoiMin})", lst.Min().ToString());
					}
					decimal value = max - min;
					grvData.SetRowCellValue(rowIndexFormula, "RealValue" + (columnindex - 2), value);
					continue;
				}
				if (!Formula.Contains("#"))
				{
					////int a = Math.Truncate((decimal)"Min()");
					//List<decimal> lst11=new List<decimal>();
					//lst11.Add(1);
					//lst11.Add(5);
					//lst11.Add(6);
					//lst11.Add(3);
					//valueFormula = TextUtils.Min(lst11);
					valueFormula = Math.Truncate((decimal)sc.Eval(Formula));
					grvData.SetRowCellValue(rowIndexFormula, "RealValue" + (columnindex - 2), valueFormula);
				}
			}
		}

		private void grvData_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
		{
			if (!_isStartColor) return;
			int sortOrder = TextUtils.ToInt(grvData.GetRowCellValue(e.RowHandle, colSortOrder));
			if (_lstSortOrder.Contains(sortOrder))
				return;
			string value = TextUtils.ToString(grvData.GetRowCellValue(e.RowHandle, colRealValue1));
			string workingName = TextUtils.ToString(grvData.GetRowCellValue(e.RowHandle, colProductWorkingName));

			//So sánh giá trị tiêu chuẩn với giá trị nhập vào nếu sai thì selectAll
			int CheckTypeValue = TextUtils.ToInt(grvData.GetFocusedRowCellValue(colCheckValueType));
			string StandardValue = TextUtils.ToString(grvData.GetFocusedRowCellValue(colStandardValue));
			if (workingName.Trim().Contains("#5#") || workingName.Trim().ToLower() == "checkorder")
			{

			}
			else
			{
				if (e.RowHandle >= 0 && e.Column.VisibleIndex > 2)
				{
					try
					{
						ColumnView view = grdData.FocusedView as ColumnView;
						string value1 = TextUtils.ToString(view.FocusedValue);
						//check nếu là dạng string 
						if (CheckTypeValue > 1)
						{
							//if (value1.Trim().ToUpper() != StandardValue.Trim().ToUpper())
							//{
							//	return;
							//}
						}
						//check nếu là dạng số 
						else
						{
							decimal min = TextUtils.ToDecimal(grvData.GetFocusedRowCellValue(colMinValue));
							decimal max = TextUtils.ToDecimal(grvData.GetFocusedRowCellValue(colMaxValue));
							if (TextUtils.ToDecimal(value1.Trim()) < min || TextUtils.ToDecimal(value1.Trim()) > max)
							{
								return;
							}
						}
					}
					catch
					{

					}
				}

			}


			if (e.RowHandle >= 0 && e.Column.VisibleIndex == 3)
			{

				//#1# điền giá trị 5 cột theo cột 1 của dòng đó 
				if (workingName.ToUpper().Contains("#1#"))
				{
					_SortOrder1 = TextUtils.ToInt(grvData.GetRowCellValue(e.RowHandle, colSortOrder));
					if (!_lst.Contains(e.RowHandle))
						_lst.Add(e.RowHandle);
					_NoIndefinitely = e.RowHandle;
					for (int j = 2; j < 7; j++)
					{
						grvData.SetRowCellValue(e.RowHandle, "RealValue" + (j), value);
					}
				}
			}

			//Gửi OrderCode-QrCode-Loại Sản Phẩm
			//if (workingName.ToUpper().Contains("#3#"))
			//{
			//	_RowVPM = e.RowHandle;
			//}
			if (e.RowHandle >= 0 && e.Column.VisibleIndex > 2)
			{
				//Check motor
				if (workingName.Trim().Contains("#5#"))
				{
					LoadColorMotor(StandardValue, e);
				}
				//#4# điền giá trị 1 cột rồi bỏ qua
				if (workingName.ToUpper().Contains("#4#"))
				{
					if (!_lst4.Contains(e.RowHandle))
						_lst4.Add(e.RowHandle);
				}
				_index = e.Column.VisibleIndex - 2;
				Control control = pannelBot.Controls.Find("textbox" + (_index), false)[0];
				if (control.Text.Trim() == "0" || control.Text.Trim() == "1")
				{
					control.BackColor = Color.White;
				}
				else
				{
					control.BackColor = _colorEmpty;
				}
				//Check đo min max 
				if ((cboWorkingStep.Text == "CD3" || cboWorkingStep.Text.Trim().Contains("TP-")) && txtGroupCode.Text.Trim().ToUpper() == "DAIFUKU")
				{
					//Tính toán lấy giá trị min và max để hiển thị 
					//if (_NoIndefinitely != e.RowHandle)
					//{
					//_NoIndefinitely = e.RowHandle;
					calculateShim(e.Column.VisibleIndex, sortOrder);
					//}
					//this.setFocusCell(getNextIndex(e.RowHandle), grvData.FocusedColumn.VisibleIndex);
					this.setFocusCell(getRowIndex(e.Column.VisibleIndex), grvData.FocusedColumn.VisibleIndex);
					focus = 1;
					return;
				}
				if (cboWorkingStep.Text != "CD5")
				{
					this.setFocusCell(getNextIndex(e.RowHandle), grvData.FocusedColumn.VisibleIndex);
					focus = 1;
					//this.setFocusCell(getRowIndex(e.Column.VisibleIndex), grvData.FocusedColumn.VisibleIndex);
					return;
				}
				if (_NoIndefinitely != e.RowHandle)
				{
					_NoIndefinitely = e.RowHandle;
					calculateShim(e.Column.VisibleIndex, sortOrder);
				}

				//this.setFocusCell(getNextIndex(e.RowHandle), grvData.FocusedColumn.VisibleIndex);
				this.setFocusCell(getRowIndex(e.Column.VisibleIndex), grvData.FocusedColumn.VisibleIndex);
				focus = 1;
			}

		}
		private void grvData_RowCellStyle3(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
		{
			if (!_isStartColor) return;
			if (e.RowHandle < 0) return;

			if (e.Column.VisibleIndex < 3) return;

			string value = TextUtils.ToString(grvData.GetRowCellValue(e.RowHandle, e.Column));
			int checkValueType = TextUtils.ToInt(grvData.GetRowCellValue(e.RowHandle, colCheckValueType));
			string standardValue = TextUtils.ToString(grvData.GetRowCellValue(e.RowHandle, colStandardValue));
			string productWorkingName = TextUtils.ToString(grvData.GetRowCellValue(e.RowHandle, colProductWorkingName));
			string columnCaption = e.Column.Caption;

			if (checkValueType == 2 && !string.IsNullOrWhiteSpace(value.Trim()) && !string.IsNullOrWhiteSpace(standardValue.Trim()))
			{
				if (productWorkingName.Trim().ToLower() == "checkorder")
				{
					/*
					 * Mục này kiểm tra giá trị của qrcode của từng con sản phẩm
					 */
					string qrCode = txtQRCode.Text.Trim();
					string[] arrQR = qrCode.Split(' ');
					string firstWord = arrQR[0];
					int lenghtFirstWord = firstWord.Length;
					if (firstWord.Contains("-"))
					{
						//Dạng qrcode có tiền tố chứa ký tự '-' VD: MVNL0123-0-2 911KV5067J25T
						firstWord = firstWord.Split('-')[0];
						lenghtFirstWord = firstWord.Length;
						if (value.Length < lenghtFirstWord)
						{
							e.Appearance.BackColor = Color.Red;
						}
						else
						{
							string currentValue = value.Substring(0, lenghtFirstWord);
							if (currentValue.ToLower() == firstWord.ToLower())
							{
								e.Appearance.BackColor = Color.FromArgb(102, 255, 255);
							}
							else
							{
								e.Appearance.BackColor = Color.Red;

							}
						}
					}
					else
					{
						//Dạng qrcode có tiền tố không chứa ký tự '-', VD: VN0123 PA120932
						string orderText = _tienTo + columnCaption;
						if (orderText.Length != firstWord.Length)
						{
							orderText = _tienTo + "0" + columnCaption;
						}
						if (value.Length < orderText.Length)
						{
							e.Appearance.BackColor = Color.Red;
						}
						else
						{
							string currentValue = value.Substring(0, orderText.Length);
							if (currentValue.ToLower() == orderText.ToLower())
							{
								e.Appearance.BackColor = Color.FromArgb(102, 255, 255);
							}
							else
							{
								e.Appearance.BackColor = Color.Red;
							}
						}
					}
				}
				else
				{
					/*
					 * Kiểm tra ghi chép dạng giá trị, nhưng giá trị tiêu chuẩn dạng text chứ không phải dạng số
					 */
					string[] arr = value.Split(',');
					if (arr.Length > 0)
					{
						if (arr[0].ToLower() != standardValue.ToLower())
						{
							e.Appearance.BackColor = Color.Red;
						}
						else
						{
							e.Appearance.BackColor = Color.FromArgb(102, 255, 255);
						}
					}
					else
					{
						e.Appearance.BackColor = _colorEmpty;
					}
				}

				return;
			}

			int valueType = TextUtils.ToInt(grvData.GetRowCellValue(e.RowHandle, colValueType));
			if (valueType <= 0)
			{
				if (checkValueType == 2 && string.IsNullOrWhiteSpace(value.Trim()) && !string.IsNullOrWhiteSpace(standardValue.Trim()))
				{
					e.Appearance.BackColor = _colorEmpty;
				}
				if (value.ToUpper() == "OK")
				{
					e.Appearance.BackColor = Color.FromArgb(102, 255, 255);
				}
				else if (value.ToUpper() == "NG")
				{
					e.Appearance.BackColor = Color.Red;
				}
				return;
			}

			if (string.IsNullOrWhiteSpace(value))
			{
				e.Appearance.BackColor = _colorEmpty;
			}
			else
			{
				decimal number = TextUtils.ToDecimal(value);
				decimal min = TextUtils.ToDecimal(grvData.GetRowCellValue(e.RowHandle, colMinValue));
				decimal max = TextUtils.ToDecimal(grvData.GetRowCellValue(e.RowHandle, colMaxValue));
				if (number < min || number > max)
				{
					e.Appearance.BackColor = Color.Red;
					e.Appearance.ForeColor = Color.FromArgb(255, 255, 0);
				}
				else
				{
					e.Appearance.BackColor = Color.FromArgb(102, 255, 255);
				}

			}

			//102, 255, 255
		}
		private void grvData_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
		{
			if (!_isStartColor) return;
			if (e.RowHandle < 0) return;
			if (e.Column.VisibleIndex < 3) return;
			string value = TextUtils.ToString(grvData.GetRowCellValue(e.RowHandle, e.Column));
			int checkValueType = TextUtils.ToInt(grvData.GetRowCellValue(e.RowHandle, colCheckValueType));
			string standardValue = TextUtils.ToString(grvData.GetRowCellValue(e.RowHandle, colStandardValue));
			string productWorkingName = TextUtils.ToString(grvData.GetRowCellValue(e.RowHandle, colProductWorkingName));
			string columnCaption = e.Column.Caption;
			int SortOrder = TextUtils.ToInt(grvData.GetRowCellValue(e.RowHandle, colSortOrder));
			if (checkValueType == 2 && !string.IsNullOrWhiteSpace(value.Trim()) && !string.IsNullOrWhiteSpace(standardValue.Trim()))
			{
				if (productWorkingName.Trim().ToLower() == "checkorder")
				{
					/*
					 * Mục này kiểm tra giá trị của qrcode của từng con sản phẩm
					 */
					string qrCode = txtQRCode.Text.Trim();
					string[] arrQR = qrCode.Split(' ');
					string firstWord = arrQR[0];
					int lenghtFirstWord = firstWord.Length;
					if (firstWord.Contains("-"))
					{
						//Dạng qrcode có tiền tố chứa ký tự '-' VD: MVNL0123-0-2 911KV5067J25T
						firstWord = firstWord.Split('-')[0];
						lenghtFirstWord = firstWord.Length;
						if (value.Length < lenghtFirstWord)
						{
							e.Appearance.BackColor = Color.Red;
							FocusGridview(e, SortOrder);
						}
						else
						{
							string currentValue = value.Substring(0, lenghtFirstWord);
							if (currentValue.ToLower() == firstWord.ToLower())
							{
								e.Appearance.BackColor = Color.FromArgb(102, 255, 255);
							}
							else
							{
								e.Appearance.BackColor = Color.Red;
								FocusGridview(e, SortOrder);
							}
						}
					}
					else
					{
						//Dạng qrcode có tiền tố không chứa ký tự '-', VD: VN0123 PA120932
						string orderText = _tienTo + columnCaption;
						if (orderText.Length != firstWord.Length)
						{
							orderText = _tienTo + "0" + columnCaption;
						}
						if (value.Length < orderText.Length)
						{
							e.Appearance.BackColor = Color.Red;
							FocusGridview(e, SortOrder);
						}
						else
						{
							string currentValue = value.Substring(0, orderText.Length);
							if (currentValue.ToLower() == orderText.ToLower())
							{
								e.Appearance.BackColor = Color.FromArgb(102, 255, 255);
							}
							else
							{
								e.Appearance.BackColor = Color.Red;
								FocusGridview(e, SortOrder);
							}
						}
					}
				}
				else
				{
					/*
					 * Kiểm tra ghi chép dạng giá trị, nhưng giá trị tiêu chuẩn dạng text chứ không phải dạng số
					 */
					string[] arr = value.Split(',');
					if (arr.Length > 0)
					{
						if (arr[0].ToLower() != standardValue.ToLower())
						{
							e.Appearance.BackColor = Color.Red;
							if (!productWorkingName.Contains("#5#"))
								FocusGridview(e, SortOrder);
						}
						else
						{
							e.Appearance.BackColor = Color.FromArgb(102, 255, 255);
						}

					}
					else
					{
						e.Appearance.BackColor = _colorEmpty;
					}

					//check ký tự khi bắn motor check aritelID theo giá trị tiêu chuẩn
					if (productWorkingName.Trim().Contains("#5#"))
					{
						if (!_lstMotor.Contains($"{value.Trim().ToLower()}"))
						{
							e.Appearance.BackColor = Color.Red;
							FocusGridview(e, TextUtils.ToInt(grvData.GetRowCellValue(e.RowHandle, colSortOrder)));
						}
						else
						{
							e.Appearance.BackColor = Color.FromArgb(102, 255, 255);
						}
					}
				}

				return;
			}

			int valueType = TextUtils.ToInt(grvData.GetRowCellValue(e.RowHandle, colValueType));
			if (valueType <= 0)
			{
				if (checkValueType == 2 && string.IsNullOrWhiteSpace(value.Trim()) && !string.IsNullOrWhiteSpace(standardValue.Trim()))
				{
					e.Appearance.BackColor = _colorEmpty;
				}
				if (value.ToUpper() == "OK")
				{
					e.Appearance.BackColor = Color.FromArgb(102, 255, 255);
				}
				else if (value.ToUpper() == "NG")
				{
					FocusGridview(e, SortOrder);
					e.Appearance.BackColor = Color.Red;
				}
				return;
			}

			if (string.IsNullOrWhiteSpace(value))
			{
				e.Appearance.BackColor = _colorEmpty;
			}
			else
			{
				decimal number = TextUtils.ToDecimal(value);
				decimal min = TextUtils.ToDecimal(grvData.GetRowCellValue(e.RowHandle, colMinValue));
				decimal max = TextUtils.ToDecimal(grvData.GetRowCellValue(e.RowHandle, colMaxValue));
				if (number < min || number > max)
				{
					e.Appearance.BackColor = Color.Red;
					FocusGridview(e, SortOrder);
					e.Appearance.ForeColor = Color.FromArgb(255, 255, 0);
				}
				else
				{
					e.Appearance.BackColor = Color.FromArgb(102, 255, 255);
				}
			}

			//102, 255, 255
		}

		void FocusGridview(DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e, int SortOrder)
		{
			//Nếu là ô check max thì dừng lại 
			if (TextUtils.ToInt(grvData.GetRowCellValue(e.RowHandle, colValueType)) != 1)
			{
				return;
			}
			if (_lstSortOrder.Contains(SortOrder))
				return;
			if (focus == 1)
			{
				focus = 0;
				grvData.Focus();
				grvData.FocusedRowHandle = e.RowHandle;
				grvData.FocusedColumn = e.Column;
			}
		}
		List<string> _lstMotor = new List<string>();
		void LoadColorMotor(string standardValue, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
		{
			_AritelID = TextUtils.ToString(TextUtils.ExcuteScalar($"SELECT TOP 1 ArticleID FROM [SystemData].[dbo].[CheckMotor] WHERE MotorInspSealNo=N'{TextUtils.ToString(grvData.GetRowCellValue(_RowMotor, e.Column.FieldName))}' or CardNo=N'{TextUtils.ToString(grvData.GetRowCellValue(_RowMotor, e.Column.FieldName))}' Or ArticleID=N'{TextUtils.ToString(grvData.GetRowCellValue(_RowMotor, e.Column.FieldName))}'"));
			if (_AritelID.Trim().ToUpper() != "")
			{
				if (standardValue.ToLower() == "0")
				{
					//Update StandValue trong bảng ProductWorking với mã #5# theo ProductID và ProductStepID
					TextUtils.ExcuteSQL($"UPDATE dbo.[ProductWorking] SET PeriodValue='{_AritelID}' WHERE ProductID = {_productID} AND ProductStepID={_workingStepID} AND IsDeleted = 0  AND WorkingName LIKE '%#5#%'");
					grvData.SetRowCellValue(_RowMotor, colStandardValue, _AritelID);
				}
				//So sánh với giá trị tiêu chuẩn nếu đúng thì Add List Card_No
				if (TextUtils.ToString(grvData.GetRowCellValue(_RowMotor, colStandardValue)).ToLower() == _AritelID.ToLower())
				{
					_lstMotor.Add(TextUtils.ToString(grvData.GetRowCellValue(_RowMotor, e.Column.FieldName)).ToLower());
				}
			}

		}
		private void txt_TextChanged(object sender, EventArgs e)
		{
			TextBox txt = (TextBox)sender;
			if (string.IsNullOrWhiteSpace(txt.Text.Trim()))
			{
				txt.BackColor = _colorEmpty;

			}
			else
			{
				txt.BackColor = Color.White;
			}
		}
		private void textBox_TextChanged(object sender, EventArgs e)
		{
			TextBox txt = (TextBox)sender;

			if (txt.Text.Trim() == "0" || txt.Text.Trim() == "1")
			{
				txt.BackColor = Color.White;
			}
			else
			{
				txt.BackColor = _colorEmpty;
			}
		}
		private void textBox_KeyDown(object sender, KeyEventArgs e)
		{
			_NoIndefinitely = -999999;
			if (grvData.RowCount == 0) return;
			TextBox textBox = (TextBox)sender;
			int textIndex = TextUtils.ToInt(textBox.Tag);
			int columnIndex = textIndex + 2;
			//Khi ấn mũi tên đi lên thì focus vào cột tương ứng
			if (e.KeyCode == Keys.Up)
			{
				this.setFocusCell(getPreviousIndex(), columnIndex);
				//setFocusCell(getRowIndex(columnIndex), columnIndex);
			}
			if (e.KeyCode == Keys.Enter)
			{
				if (CheckSkipCD(1, columnIndex) == false)
				{
					MessageBox.Show($"Sản phẩm chưa đi qua công đoạn {CDOld}");
					return;
				}
				string value = ((TextBox)sender).Text;
				string valueString = "";// 
				if (value == "1" || value == "13")
				{
					valueString = "OK";
				}
				else if (value == "0" || value == "03")
				{
					valueString = "NG";
				}
				if (value == "1" || value == "13")
				{
					//Check phải điền hết các giá trị mới cho đánh giá ok
					for (int i = 0; i < _dtData.Rows.Count; i++)
					{
						DataRow r = _dtData.Rows[i];
						int checkValueType = TextUtils.ToInt(r["CheckValueType"]);
						string standardValue = TextUtils.ToString(r["StandardValue"]);
						string WorkingName = TextUtils.ToString(r["WorkingName"]);
						int valueType = TextUtils.ToInt(r["ValueType"]);
						if (WorkingName.Contains("#4#")) continue;
						if (valueType == 0) continue;
						if (checkValueType == 2 && !string.IsNullOrWhiteSpace(standardValue))
						{
							continue;
						}
						if (valueType > 0)
						{
							string RealValue = TextUtils.ToString(grvData.GetRowCellValue(i, "RealValue" + (columnIndex - 2)));
							if (RealValue.Trim() == "")
							{
								MessageBox.Show("Bạn phải nhập đầy đủ giá trị theo cột đánh giá", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
								((TextBox)sender).Text = "";
								setFocusCell(getRowIndex(columnIndex), columnIndex);
								return;
							}
						}
					}
				}
				//Set giá trị các ô có giá trị check mark
				for (int i = 0; i < _dtData.Rows.Count; i++)
				{
					DataRow r = _dtData.Rows[i];
					int checkValueType = TextUtils.ToInt(r["CheckValueType"]);
					string standardValue = TextUtils.ToString(r["StandardValue"]);
					int valueType = TextUtils.ToInt(r["ValueType"]);
					if (valueType > 0) continue;

					if (checkValueType == 2 && !string.IsNullOrWhiteSpace(standardValue))
					{
						continue;
					}
					if (valueType == 0)
					{
						r["RealValue" + (columnIndex - 2)] = valueString;
					}
				}
				//Set tiêu đề cột trước khi chuyển sang cột mới
				if (value == "1" || value == "0" || value == "13" || value == "03")
				{
					//grvData.Columns["RealValue" + (columnIndex - 2)].Caption = (int.Parse(_stt) + columnIndex - 3).ToString();
					if (columnIndex < 7)
					{
						//focus vào ô của cột tiếp theo
						if (_step == "CD5" && _GMD != 0)
						{
							grvData.SetRowCellValue(_rowGMD, "RealValue" + (columnIndex - 1), _GMD);
						}
						setFocusCell(getRowIndex(columnIndex + 1), columnIndex + 1);
					}
					//if (textBox5.Text.Trim() == "1" || textBox5.Text.Trim() == "0")
					else
					{
						FocusbtnSave = 1;
						btnSave.Focus();
					}
					if (value == "13" || value == "03")
					{
						FocusbtnSave = 1;
						btnSave.Focus();
					}
					//if (cboWorkingStep.Text.Trim().ToUpper().Contains("926YM"))
					//{
					//	try
					//	{
					//		// gửi data qua frm Print
					//		_frm._SendData(txtQRCode.Text.Trim(), txtOrder.Text.Trim());
					//	}
					//	catch
					//	{ }
					//}
					//ToDo: Không tìm thấy ID AnDon nên không lưu lại
					if (_oAndonModel.ID > 0)
					{
						//Cập nhật trạng thái đã làm xong
						sendDataTCP("4", "2");
					}
					if (_startMakeTime == new DateTime())
					{
						_startMakeTime = DateTime.Now;
					}
					_endMakeTime = DateTime.Now;
					//Cất vào bảng AndonDetail
					AndonDetailModel andonDetail = new AndonDetailModel();
					andonDetail.ProductCode = _productCode;
					andonDetail.ProductID = _productID;
					andonDetail.ProductStepID = TextUtils.ToInt(cboWorkingStep.SelectedValue);
					andonDetail.QrCode = _tienTo + grvData.Columns[columnIndex].Caption + " " + _productCode;
					andonDetail.OrderCode = txtOrder.Text.Trim();
					andonDetail.ProductStepCode = cboWorkingStep.Text.Trim();
					andonDetail.StartTime = _startMakeTime;
					andonDetail.EndTime = _endMakeTime;
					//andonDetail.MakeTime = TextUtils.ToInt(Math.Round((_endMakeTime - _startMakeTime).TotalSeconds, 0));
					try
					{
						andonDetail.MakeTime = _taktTime - TextUtils.ToInt(lblTakt.Text);
					}
					catch (Exception)
					{
					}
					_isStart = false;//reset lại startMakeTime
					andonDetail.AnDonID = _oAndonModel.ID;
					andonDetail.ShiftID = _oAndonModel.ShiftID;
					andonDetail.Takt = _oAndonModel.Takt;
					if (_PeriodTime > 0 && _PeriodTime < 200)
					{
						andonDetail.Type = 1;
						andonDetail.PeriodTime = _PeriodTime;
					}
					else
					{
						andonDetail.Type = 3;
						andonDetail.PeriodTime = _PeriodTime;
					}
					andonDetail.WorkerCode = txtWorker.Text.Trim();
					andonDetail.OkStatus = TextUtils.ToInt(value);
					AndonDetailBO.Instance.Insert(andonDetail);

					//tăng qty actual
					if (cboWorkingStep.Text.Trim() == "CD6")
					{
						sendDataTCP("0", "3");
					}
					try
					{
						TextUtils.ExcuteProcedure("DeleteKTOrKNNew", new string[] { "@Order", "@PID", "@CD" }, new object[] { txtOrder.Text.Trim(), -TextUtils.ToInt(textBox.Tag), cboWorkingStep.Text.Trim() });
						//sendDataTCPAnDonPicking(_productCode + " " + txtOrder.Text.Trim());
						if (cboSub.SelectedIndex == 0)
							sendDataTCPAnDonPicking(_tienTo + grvData.Columns[columnIndex].Caption + " " + txtOrder.Text.Trim());
					}
					catch
					{

					}
					if (cboWorkingStep.Text.Trim() == "CDTest" && (value == "1" || value == "13") && _Print == true)
					{
						try
						{
							int productID = _productID;
							int stepID = TextUtils.ToInt(cboWorkingStep.SelectedValue);
							if (productID == 0)
							{
								MessageBox.Show(string.Format("Không tồn tại sản phẩm có mã [{0}]!", _productCode.Trim()), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}
							if (stepID == 0)
							{
								MessageBox.Show(string.Format("Bạn chưa chọn công đoạn nào!", _productCode.Trim()), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}
							if (string.IsNullOrEmpty(txtWorker.Text.Trim()))
							{
								MessageBox.Show(string.Format("Bạn chưa điền người làm!", _productCode.Trim()), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
								txtWorker.Focus();
								return;
							}
							int count = _dtData.Rows.Count;
							Control control = pannelBot.Controls.Find("textbox" + (columnIndex - 2), false)[0];
							string columnCaption = grvData.Columns[columnIndex].Caption;
							string qrCode = "";
							if (string.IsNullOrEmpty(control.Text.Trim()))
							{

							}
							else
							{
								qrCode = _tienTo + columnCaption + " " + _productCode;
							}
							string orderCode = string.IsNullOrWhiteSpace(txtOrder.Text.Trim()) ?
									(_tienTo.Contains("-") ? _tienTo.Substring(0, _tienTo.Length - 1) : _order) :
									txtOrder.Text.Trim();
							List<ProductCheckHistoryDetailModel> lstDetail = new List<ProductCheckHistoryDetailModel>();
							for (int j = 0; j < count; j++)
							{
								ProductCheckHistoryDetailModel cModel = new ProductCheckHistoryDetailModel();
								//cModel.ProductCheckHistoryID = masterID;
								cModel.ProductStepID = stepID;
								cModel.ProductStepCode = cboWorkingStep.Text.Trim();
								cModel.ProductStepName = txtStepName.Text.Trim();
								cModel.SSortOrder = TextUtils.ToInt(grvData.GetRowCellValue(j, colSSortOrder));

								cModel.ProductWorkingID = TextUtils.ToInt(grvData.GetRowCellValue(j, colWorkingID));
								cModel.ProductWorkingName = TextUtils.ToString(grvData.GetRowCellValue(j, colProductWorkingName));
								cModel.WSortOrder = TextUtils.ToInt(grvData.GetRowCellValue(j, colSortOrder));

								cModel.WorkerCode = txtWorker.Text.Trim();
								cModel.StandardValue = TextUtils.ToString(grvData.GetRowCellValue(j, colStandardValue));
								cModel.RealValue = TextUtils.ToString(grvData.GetRowCellValue(j, "RealValue" + (columnIndex - 2)));
								cModel.ValueType = TextUtils.ToInt(grvData.GetRowCellValue(j, colValueType));
								cModel.ValueTypeName = cModel.ValueType == 1 ? "Giá trị\n数値" : "Check mark";
								cModel.EditValue1 = "";
								cModel.EditValue2 = "";
								cModel.StatusResult = TextUtils.ToInt(control.Text.Trim());
								cModel.ProductID = productID;
								cModel.QRCode = qrCode;
								cModel.OrderCode = orderCode;
								//cModel.PackageNumber = _tienTo.Contains("-") ? _tienTo.Split('-')[1] : "";
								//cModel.QtyInPackage = columnCaption;
								cModel.Approved = "";
								cModel.Monitor = "";
								cModel.DateLR = DateTime.Now;
								cModel.EditContent = "";
								cModel.EditDate = DateTime.Now;
								cModel.ProductCode = _productCode;
								cModel.ProductOrder = _order;
								cModel.Line = cboSub.Text.Trim();
								lstDetail.Add(cModel);

								//ProductCheckHistoryDetailBO.Instance.Insert(cModel);

							}
							saveDetail(lstDetail);

							// gửi data qua frm Print
							_frm._SendData(txtQRCode.Text.Trim(), txtOrder.Text.Trim());

							if (columnIndex == 7)
							{
								_stepIndex = cboWorkingStep.SelectedIndex;
								_currentIndex = 0;
								_PlanID = 0;
								_totalTimeDelay = 0;

								grdData.DataSource = null;
								txtQRCode.Text = "";
								txtOrder.Text = "";
								cboWorkingStep.DataSource = null;
								//btnNotUseCD8.Visible = false;
								resetControl();
								txtQRCode.Focus();
							}
						}
						catch
						{ }
					}

				}
				else
				{
					this.setFocusCell(getPreviousIndex(), columnIndex);
					//	setFocusCell(getRowIndex(columnIndex), columnIndex);
				}

			}
		}
		private void textBox_GotFocus(object sender, EventArgs e)
		{
			((TextBox)sender).SelectAll();
		}
		private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (cboWorkingStep.Text.Trim() != "CDTest")
				btnSave_Click(null, null);
		}
		private void startRiskToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//startThreadRisk();
			_sTimeRisk = DateTime.Now;
			this.BackColor = Color.Red;

			//Gửi tín hiệu risk qua server Andon qua TCP/IP
			sendDataTCP("3", "1");
		}
		private void endRiskToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_eTimeRisk = DateTime.Now;
			if (_sTimeRisk == new DateTime())
			{
				_sTimeRisk = DateTime.Now;
			}
			try
			{
				frmChooseRisk frm = new frmChooseRisk();
				if (frm.ShowDialog() == DialogResult.OK)
				{
					//Ghi dữ liệu sự cố vào bảng Andon
					AndonDetailModel detail = new AndonDetailModel();
					detail.ProductID = _productID;
					detail.ProductCode = _productCode;
					detail.OrderCode = txtOrder.Text.Trim();
					detail.ProductStepID = TextUtils.ToInt(cboWorkingStep.SelectedValue);
					detail.ProductStepCode = cboWorkingStep.Text.Trim();
					detail.Type = 2;
					if (_oAndonModel != null)
					{
						detail.Takt = _oAndonModel.Takt;
						detail.AnDonID = _oAndonModel.ID;
						detail.ShiftID = _oAndonModel.ShiftID;
					}
					detail.PeriodTime = TextUtils.ToInt(Math.Round((_eTimeRisk - _sTimeRisk).TotalSeconds, 0));
					detail.MakeTime = 0;
					detail.StartTime = _sTimeRisk;
					detail.EndTime = _eTimeRisk;
					detail.RiskDescription = frm.RiskDescription;
					detail.WorkerCode = txtWorker.Text;

					AndonDetailBO.Instance.Insert(detail);
					// kiểm tra lại trạng thái nút Không sử dụng để gửi tín hiệu.
					if (btnNotUseCD8.Text == "Không sử dụng")
					{
						sendDataTCP("11", "10");
					}
					else
					{
						sendDataTCP("4", "2");
					}
					this.BackColor = Color.WhiteSmoke;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
		bool _isUse = false;
		private void btnNotUseCD8_Click(object sender, EventArgs e)
		{
			//Khi nhấn ko sử dụng công đoạn 8 thì nó luôn cập nhật vào bảng statuscolorCD = 4 
			if (!_isUse)
			{
				//startThreadNotUseCD8();
				//TextUtils.ExcuteSQL(@"update AndonConfig set IsStopCD8 = 1");
				sendDataTCP("10", "10");
				btnNotUseCD8.Text = "Sử dụng";
				_isUse = true;
			}
			else
			{
				//TextUtils.ExcuteSQL(@"update AndonConfig set IsStopCD8 = 0");
				//if (_threadNotUseCD8 != null) _threadNotUseCD8.Abort();
				sendDataTCP("11", "10");
				_isUse = false;
				btnNotUseCD8.Text = "Không sử dụng";
			}
			grdData.Enabled = txtOrder.Enabled = txtQRCode.Enabled = !_isUse;
			textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = textBox5.Enabled = !_isUse;
		}

		private void loginToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!_isLogin)
			{
				frmLogin frm = new frmLogin();
				frm.ShowDialog();
				if (frm.DialogResult == DialogResult.OK)
				{
					_isLogin = true;
				}
			}
			else
			{
				DialogResult rs = MessageBox.Show("Bạn muốn đăng xuất?", "Thông báo", MessageBoxButtons.YesNo);
				if (rs == DialogResult.No) return;
				_isLogin = false;
			}
		}

		private void txtWorker_Leave(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(txtWorker.Text.Trim()))
			{
				MessageBox.Show("Bạn chưa điền tên người làm!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				txtWorker.Focus();
			}
		}

		private void txtWorker_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				txtQRCode.Focus();
			}
		}

		private void chkFitRow_CheckedChanged(object sender, EventArgs e)
		{
			if (chkFitRow.Checked)
			{
				if (grvData.RowCount > 0)
				{
					grvData.RowHeight = -1;
					int totalHeightRow = this.getSumHeightRows();
					if ((oldHeightGrid - grvData.ColumnPanelRowHeight - 30) > totalHeightRow)
					{
						grvData.RowHeight = (oldHeightGrid - grvData.ColumnPanelRowHeight - 30) / grvData.RowCount;
					}
				}
			}
			else
			{
				grvData.RowHeight = -1;
				//loadGridData();
			}
			File.WriteAllText(pathConfigFitRow, chkFitRow.Checked ? "1" : "0");
		}

		private void cboWorkingStep_TextChanged(object sender, EventArgs e)
		{
			lblCD.Text = cboWorkingStep.Text.Trim();
		}

		private void Checktimeout_Tick(object sender, EventArgs e)
		{
			if (_CheckTimeOutFat == true)
			{
				timeout = timeout + 1;
				if (timeout == 20)
				{
					MessageBox.Show("Xin hãy bật phần mềm đo mỡ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					SendCom(_DataSendCom);
					timeout = 0;
				}
			}
		}

		private void btnShow_Click(object sender, EventArgs e)
		{
			if (!File.Exists(_pathFileBanDo)) return;
			try
			{
				string[] lines = File.ReadAllLines(_pathFileBanDo);
				if (lines == null) return;
				if (lines.Length < 2) return;

				string[] stringSeparators = new string[] { "||" };
				string[] arr = lines[1].Split(stringSeparators, 4, StringSplitOptions.RemoveEmptyEntries);

				if (arr == null) return;
				if (arr.Length < 4) return;
				string name = "";
				string path = Path.Combine(Application.StartupPath, arr[1].Trim());
				string pathUpdate = arr[2].Trim();
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				if (_ProductCode == "") return;
				string[] fileList = Directory.GetFiles(pathUpdate);//lay danh sách file cho vao mảng
				string strFileName = "";
				//duyet mang file trong thư mục
				foreach (string i in fileList)
				{
					strFileName = "";
					strFileName = Path.GetFileName(i).Trim();
					if (strFileName.ToUpper().Contains($"{_ProductCode.ToUpper()}"))
					{
						name = strFileName;
						break;
					}
				}
				Process.Start($"{pathUpdate}\\{name}");
			}
			catch
			{
				MessageBox.Show("Không tìm thấy File PDF", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void chkFat_CheckedChanged(object sender, EventArgs e)
		{
			if (!chkFat.Checked)
			{
				_CheckTimeOutFat = false;
				Checktimeout.Stop();
			}
			File.WriteAllText(pathConfigFat, chkFat.Checked ? "1" : "0");
		}

		private void grvData_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//GridView view = sender as GridView;
			//if (view.FocusedRowHandle == _RowVPM)
			//	e.Cancel = true;
			//if (view.FocusedRowHandle == _RowSauPCA)
			//	e.Cancel = true;
		}

		private void btnImage_Click(object sender, EventArgs e)
		{

			try
			{
				string name = "";
				string pathUpdate = folderUpdate;
				if (_ProductCode == "") return;
				Process.Start($"{pathUpdate}");
			}
			catch
			{
				MessageBox.Show("Không tìm thấy File Ảnh", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void setWeightToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (cboWorkingStep.Text.Trim().Contains("TP-"))
			{
				_ColumnSauPCA = _indexColumSauPCA;
				_ColumnNameSauPCA = "RealValue" + (_indexColumSauPCA - 2);
				try
				{
					if (chkFat.Checked && cboWorkingStep.Text.Trim().Contains("TP-"))
					{
						_Product = (ProductModel)ProductBO.Instance.FindByAttribute("ProductCode", _ProductCode)[0];
						if (_Product.EngineWeight == 0 && _Product.EngineWeightMax == 0 && _Product.EngineWeightMin == 0)
						{
							SendCom("SM");
						}
						else
						{
							SendCom("NOSM");
						}

					}
				}
				catch (Exception ex)
				{
					ErrorLog.errorLog("Lỗi gửi tín hiệu cho CD sau PCA", "Lỗi Gửi value", Environment.NewLine);
				}
			}
		}
	}
}

