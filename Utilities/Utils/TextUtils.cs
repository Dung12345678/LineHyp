using BMS.Business;
using BMS.Model;
using BMS.Utils;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using ICSharpCode.SharpZipLib.Zip;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Outlook = Microsoft.Office.Interop.Outlook;
using Word = Microsoft.Office.Interop.Word;

namespace BMS
{
	public static class TextUtils
	{
		#region Variables
		static private SqlConnection mySqlConnection;

		#region FORMAT
		public const string CurrencyFormat = "###,###,###.00";
		public const string CurrencyFormatVND = "###,###,###,###";
		public const string FomatShortDate = "dd/MM/yyyy";
		public const string FormatLongDate = "dd/MM/yyyy HH:mm";
		#endregion

		#region MESSAGE
		public static string Caption = "[TÂN PHÁT] - Thông báo";
		#endregion

		#region DATETIME
		public static DateTime MIN_DATE = new DateTime(1950, 1, 1);
		#endregion

		private static string[] Number_Patterns = new string[] { "{0:#,##0}", "{0:#,##0.0}", "{0:#,##0.00}", "{0:#,##0}.000", "{0:#,##0.0000}",
			"{0:#,##0.00000;#,##0.00000; }" };

		private static string[] Currency_Patterns = new string[] { "{0:$#,##0;($#,##0); }", "{0:$#,##0.0;($#,##0.0); }", "{0:$#,##0.00;($#,##0.00); }",
			"{0:$#,##0.000;($#,##0.000); }", "{0:$#,##0.0000;($#,##0.0000); }", "{0:$#,##0.00000;($#,##0.00000); }" };

		#endregion Variables

		#region Contructors

		#endregion Contructors

		#region Methods
		public static void SetContainsFilter(GridView view)
		{
			foreach (GridColumn col in view.Columns)
			{
				col.OptionsFilter.AutoFilterCondition = AutoFilterCondition.Contains;
			}
		}
		#region Tìm Min và Max trong 1 dãy
		public static decimal Min(List<decimal> lst)
		{
			try
			{
				decimal min = lst.Min(y => y);
				return min;
			}
			catch
			{
				return 0;
			}
			
		}

		#endregion
		#region Chuyển kiểu, ép kiểu
		public static string ToString(object x)
		{
			if (x != null)
			{
				return x.ToString().Trim();
			}
			return "";
		}

		/// <summary>
		/// Chuyển giá trị sang kiểu integer
		/// </summary>
		/// <param name="x">giá trị cần chuyển</param>
		/// <returns></returns>
		public static int ToInt(object x)
		{
			try
			{
				return Convert.ToInt32(x);
			}
			catch
			{
				return 0;
			}
		}

		/// <summary>
		/// Chuyển giá trị sang kiểu long
		/// </summary>
		/// <param name="x">giá trị cần chuyển</param>
		/// <returns></returns>
		public static Int64 ToInt64(object x)
		{
			try
			{
				return Convert.ToInt64(x);
			}
			catch
			{
				return 0;
			}
		}

		/// <summary>
		/// Chuyển giá trị sang kiểu decimal
		/// </summary>
		/// <param name="x">giá trị cần chuyển</param>
		/// <returns></returns>
		public static Decimal ToDecimal(object x)
		{
			try
			{
				return Convert.ToDecimal(x);
			}
			catch
			{
				return 0;
			}
		}

		/// <summary>
		/// Chuyển giá trị sang kiểu double
		/// </summary>
		/// <param name="x">giá trị cần chuyển</param>
		/// <returns></returns>
		public static Double ToDouble(object x)
		{
			try
			{
				return Convert.ToDouble(x);
			}
			catch
			{
				return 0;
			}
		}

		/// <summary>
		/// Chuyển giá trị sang kiểu bool
		/// </summary>
		/// <param name="x">giá trị cần chuyển</param>
		/// <returns></returns>
		public static bool ToBoolean(object x)
		{
			try
			{
				return Convert.ToBoolean(x);
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Chuyển giá trị chuỗi sang kiểu datetime
		/// </summary>
		/// <param name="date">chuỗi cần chuyển</param>
		/// <returns></returns>
		public static DateTime ToDate(string date)
		{
			try
			{
				try
				{
					return DateTime.Parse(date, new CultureInfo("en-US", true));
				}
				catch
				{
					return DateTime.Parse(date, new CultureInfo("fr-FR", true));
				}
			}
			catch
			{
				return TextUtils.MIN_DATE;
			}
		}

		public static DateTime ToDate1(string date)
		{
			try
			{
				try
				{
					return DateTime.Parse(date, new CultureInfo("vi-VN", true));
				}
				catch
				{
					return DateTime.Parse(date, new CultureInfo("fr-FR", true));
				}
			}
			catch
			{
				return TextUtils.MIN_DATE;
			}
		}

		public static DateTime? ToDate2(object x)
		{
			string date = "";
			if (x != null)
			{
				date = x.ToString();
			}
			try
			{
				try
				{
					return DateTime.Parse(date, new CultureInfo("en-US", true));
				}
				catch
				{
					return DateTime.Parse(date, new CultureInfo("fr-FR", true));
				}
			}
			catch
			{
				return null;
			}
		}

		public static DateTime ToDate3(object x)
		{
			string date = "";
			if (x != null)
			{
				date = x.ToString();
			}
			try
			{
				try
				{
					return DateTime.Parse(date, new CultureInfo("en-US", true));
				}
				catch
				{
					return DateTime.Parse(date, new CultureInfo("fr-FR", true));
				}
			}
			catch
			{
				return TextUtils.MIN_DATE;
			}
		}

		/// <summary>
		/// Chuyển giá trị datetime sang kiểu chuỗi ngày tháng định dạng Việt Nam
		/// </summary>
		/// <param name="date">Ngày cần chuyển</param>
		/// <returns></returns>
		public static string DateToStringVN(DateTime date)
		{
			return date.ToString("dd/MM/yyyy");
		}

		/// <summary>
		/// Chuyển dạng số thành dạng %
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		public static string FormatPercentNumber(Decimal x)
		{
			return String.Format("{0:#0.00}%", x);
		}

		#endregion Các hàm chuyển kiểu
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region Lấy dữ liệu từ cơ sở dữ liệu
		/// <summary>
		/// Hàm kết nối với CSDL SqlServer
		/// </summary>
		/// <returns>Trả về giá trị dạng bool</returns>
		static private bool connect()
		{
			string str = BMS.Utils.DBUtils.GetDBConnectionString();
			try
			{
				mySqlConnection = new SqlConnection(str);
				mySqlConnection.Open();
				return true;
			}
			catch (SqlException ex)
			{
				MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		/// <summary>
		/// Hàm đóng kết nối với CSDL SqlServer
		/// </summary>
		/// <returns></returns>
		static private bool disconnect()
		{
			try
			{
				mySqlConnection.Close();
				return true;
			}
			catch (SqlException ex)
			{
				MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		/// <summary>
		/// Lấy giá trị trả về của procedure với dạng bảng
		/// </summary>
		/// <param name="procedureName">Tên procedure</param>
		/// <param name="mySqlParameter">parametter</param>
		/// <param name="nameSetToTable">Tên bảng trả về</param>
		/// <returns></returns>
		static public DataTable GetTable(string procedureName, SqlParameter mySqlParameter, string nameSetToTable)
		{
			DataTable table = new DataTable();
			try
			{
				if (connect())
				{
					SqlCommand mySqlCommand = new SqlCommand(procedureName, mySqlConnection);
					mySqlCommand.CommandType = CommandType.StoredProcedure;
					SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(mySqlCommand);
					DataSet myDataSet = new DataSet();
					if (mySqlParameter != null)
						mySqlCommand.Parameters.Add(mySqlParameter);
					//mySqlCommand.ExecuteNonQuery();
					mySqlDataAdapter.Fill(myDataSet, nameSetToTable);
					table = myDataSet.Tables[nameSetToTable];
				}
			}
			catch (SqlException ex)
			{
				return new DataTable();
			}
			finally
			{
				disconnect();
			}
			return table;
		}
		public static string ExportExcelReturnFilePath(DevExpress.XtraGrid.Views.Grid.GridView grvData, string fileName)
		{
			grvData.OptionsPrint.AutoWidth = false;
			grvData.OptionsPrint.ExpandAllDetails = true;
			grvData.OptionsPrint.PrintDetails = true;

			grvData.OptionsPrint.UsePrintStyles = true;

			string filepath = "";

			FolderBrowserDialog d = new FolderBrowserDialog();
			if (d.ShowDialog() == DialogResult.OK)
			{
				filepath = d.SelectedPath + "//" + fileName + DateTime.Now.ToString("_dd-MM-yyyy-HH-mm-ss") + ".xls";
			}

			if (filepath != "")
			{
				try
				{
					grvData.ExportToXls(filepath);
				}
				catch
				{
				}
			}

			return filepath;
		}
		public static string ExportExcelReturnFilePathLocal(DevExpress.XtraGrid.Views.Grid.GridView grvData, string fileName)
		{
			grvData.OptionsPrint.AutoWidth = false;
			grvData.OptionsPrint.ExpandAllDetails = true;
			grvData.OptionsPrint.PrintDetails = true;

			grvData.OptionsPrint.UsePrintStyles = true;

			string filepath = "";
			filepath = Application.StartupPath + "//Print" + ".xls";


			if (filepath != "")
			{
				try
				{
					grvData.ExportToXls(filepath);
				}
				catch
				{
				}
			}

			return filepath;
		}

		/// <summary>
		/// Lấy giá trị trả về của procedure với dạng bảng
		/// </summary>
		/// <param name="procedureName">tên procedure</param>
		/// <param name="nameSetToTable">Tên bảng trả về</param>
		/// <param name="mySqlParameter">danh sách parameter</param>
		/// <returns></returns>
		static public DataTable GetTable(string procedureName, string nameSetToTable, params SqlParameter[] mySqlParameter)
		{
			DataTable table = new DataTable();
			try
			{
				if (connect())
				{
					SqlCommand mySqlCommand = new SqlCommand(procedureName, mySqlConnection);
					mySqlCommand.CommandType = CommandType.StoredProcedure;
					SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(mySqlCommand);
					DataSet myDataSet = new DataSet();
					for (int i = 0; i < mySqlParameter.Length; i++)
						mySqlCommand.Parameters.Add(mySqlParameter[i]);
					//mySqlCommand.ExecuteNonQuery();
					mySqlDataAdapter.Fill(myDataSet, nameSetToTable);
					table = myDataSet.Tables[nameSetToTable];
				}
			}
			catch (SqlException ex)
			{
				throw new Exception(ex.Message);
			}
			finally
			{
				disconnect();
			}
			return table;
		}

		public static DataTable LoadDataFromSP(string procedureName, string nameSetToTable, string[] paramName, object[] paramValue)
		{
			DataTable table = new DataTable();
			SqlConnection mySqlConnection = new SqlConnection(DBUtils.GetDBConnectionString(60));
			SqlParameter sqlParam;
			mySqlConnection.Open();

			try
			{
				SqlCommand mySqlCommand = new SqlCommand(procedureName, mySqlConnection);
				mySqlCommand.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(mySqlCommand);

				DataSet myDataSet = new DataSet();
				if (paramName != null)
				{
					for (int i = 0; i < paramName.Length; i++)
					{
						sqlParam = new SqlParameter(paramName[i], paramValue[i]);
						mySqlCommand.Parameters.Add(sqlParam);
					}
				}

				mySqlDataAdapter.Fill(myDataSet, nameSetToTable);

				table = myDataSet.Tables[nameSetToTable];
			}
			catch (SqlException e)
			{
				MessageBox.Show(e.ToString());
			}
			finally
			{
				mySqlConnection.Close();
			}

			return table;
		}

		public static DataSet GetListDataFromSP(string procedureName, string nameSetToTable, string[] paramName, object[] paramValue)
		{
			DataSet myDataSet = new DataSet();
			SqlConnection mySqlConnection = new SqlConnection(DBUtils.GetDBConnectionString(60));
			SqlParameter sqlParam;
			mySqlConnection.Open();

			try
			{
				SqlCommand mySqlCommand = new SqlCommand(procedureName, mySqlConnection);
				mySqlCommand.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter(mySqlCommand);

				if (paramName != null)
				{
					for (int i = 0; i < paramName.Length; i++)
					{
						sqlParam = new SqlParameter(paramName[i], paramValue[i]);
						mySqlCommand.Parameters.Add(sqlParam);
					}
				}

				mySqlDataAdapter.Fill(myDataSet);
			}
			catch (SqlException e)
			{
			}
			finally
			{
				mySqlConnection.Close();
			}

			return myDataSet;
		}


		/// <summary>
		/// Lấy giá trị trả về của 1 command với dạng bảng
		/// </summary>
		/// <param name="strComm">Chuỗi command</param>
		/// <returns></returns>
		static public DataTable Select(string strComm)
		{
			SqlConnection cnn = new SqlConnection(BMS.Utils.DBUtils.GetDBConnectionString());
			try
			{
				SqlCommand cmd = new SqlCommand();
				SqlDataAdapter da = new SqlDataAdapter();
				cnn.Open();
				cmd = new SqlCommand("spSearchAllForTrans", cnn);
				//cmd.CommandTimeout = 6000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add(new SqlParameter("@sqlCommand", strComm));
				//cmd.ExecuteNonQuery();

				da = new SqlDataAdapter(cmd);
				DataSet ds = new DataSet();
				da.Fill(ds);
				return ds.Tables[0];
			}
			catch (SqlException se)
			{
				return new DataTable();
				//throw new Exception("Sellect error :" + se.Message);
			}
			finally
			{
				cnn.Close();
			}
		}

		/// <summary>
		/// Lấy dữ liệu của 1 bảng trong sql
		/// </summary>
		/// <param name="tableName">Tên bảng</param>
		/// <param name="exp">Điều kiện</param>
		/// <returns>Trả về một Datatable</returns>
		public static DataTable Select(string tableName, BMS.Utils.Expression exp)
		{
			return TextUtils.Select(BMS.Utils.DBUtils.SQLSelect(tableName, exp));
		}

		/// <summary>
		/// Thực thi một câu lệnh Command
		/// </summary>
		/// <param name="strSQL">Chuỗi command</param>
		public static void ExcuteSQL(string strSQL)
		{
			SqlConnection cn = new SqlConnection(BMS.Utils.DBUtils.GetDBConnectionString());
			SqlCommand cmd = new SqlCommand(strSQL, cn);
			cmd.CommandType = CommandType.Text;
			//cmd.CommandTimeout = 0;
			cn.Open();
			try
			{
				cmd.CommandText = strSQL;
				cmd.ExecuteNonQuery();
			}
			catch (Exception)
			{
			}
			finally
			{
				cn.Close();
			}
		}

		public static object ExcuteScalar(string strSQL)
		{
			object value = null;

			SqlConnection cn = new SqlConnection(DBUtils.GetDBConnectionString());
			SqlCommand cmd = new SqlCommand(strSQL, cn);
			cmd.CommandType = CommandType.Text;
			//cmd.CommandTimeout = 0;
			cn.Open();

			try
			{
				cmd.CommandText = strSQL;
				value = cmd.ExecuteScalar();
			}
			catch
			{
			}
			finally
			{
				cn.Close();
			}

			return value;
		}

		/// <summary>
		/// Thực thi một câu lệnh update
		/// </summary>
		/// <param name="command">Chuỗi câu lệnh update</param>
		/// <returns></returns>
		public static Boolean UpdateByCommand(string command)
		{
			SqlConnection cnn = new SqlConnection(BMS.Utils.DBUtils.GetDBConnectionString());
			Boolean update = false;
			try
			{
				SqlCommand cmd = new SqlCommand();
				cnn.Open();
				cmd = new SqlCommand("spSearchAllForTrans", cnn);
				//cmd.CommandTimeout = 6000;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add(new SqlParameter("@sqlCommand", command));
				cmd.ExecuteNonQuery();
				update = true;
			}
			catch (SqlException se)
			{
				throw new Exception("UPDATE_ERROR :" + se.Message);
			}
			finally
			{
				cnn.Close();
			}
			return update;
		}

		/// <summary>
		/// Kiểm tra khi đăng nhập
		/// </summary>
		/// <param name="U">Tên đăng nhập</param>
		/// <param name="P">Password đăng nhập</param>
		/// <param name="mU">đối tượng Người sử dụng</param>
		/// <returns></returns>
		static public bool Log(string U, string P, ref UsersModel mU)
		{
			try
			{
				BMS.Utils.Expression exp = new BMS.Utils.Expression("LoginName", U, "=");
				exp = exp.And(new BMS.Utils.Expression("PassWordHash", BMS.Utils.MD5.EncryptPassword(P), "="));
				//exp = exp.And(new Expression("PassWordHash", MD5.EncodeChecksum(P), "="));
				ArrayList arrU = UsersBO.Instance.FindByExpression(exp);
				if ((arrU != null) && (arrU.Count > 0))
				{
					mU = (UsersModel)arrU[0];

					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public static bool HasPermission(string permissionCode)
		{
			try
			{
				DataTable dt = TextUtils.Select("select top 1 from vCheckPermission where Code = '" + permissionCode + "' and UserID = " + Global.UserID);
				if (dt.Rows.Count > 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static void ExcuteProcedure(string storeProcedureName, string[] paramName, object[] paramValue)
		{
			SqlConnection cn = new SqlConnection(BMS.Utils.DBUtils.GetDBConnectionString());
			try
			{
				cn = new SqlConnection(BMS.Utils.DBUtils.GetDBConnectionString());
				SqlCommand cmd = new SqlCommand(storeProcedureName, cn);
				cmd.CommandType = CommandType.StoredProcedure;
				//cmd.CommandTimeout = 0;
				SqlParameter sqlParam;
				cn.Open();
				if (paramName != null)
				{
					for (int i = 0; i < paramName.Length; i++)
					{
						sqlParam = new SqlParameter(paramName[i], paramValue[i]);
						cmd.Parameters.Add(sqlParam);
					}
				}
				cmd.ExecuteNonQuery();
			}
			catch (Exception)
			{

			}
			finally
			{
				cn.Close();
			}

		}

		public static string GetConfigValue(string key)
		{
			try
			{
				return ((ConfigSystemModel)ConfigSystemBO.Instance.FindByAttribute("KeyName", key)[0]).KeyValue;
			}
			catch
			{
			}
			return "";
		}

		#endregion Các hàm lấy dữ liệu từ cơ sở dữ liệu
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region Xử lý Chuỗi

		/// <summary>
		/// Viết hoa chữ cái đầu của một chuỗi
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string ToUpperFC(string str)
		{
			return str.Substring(0, 1).ToUpper() + str.Substring(1);
		}

		/// <summary>
		/// Thao
		/// Đổi từ số sang chữ tiếng việt
		/// </summary>
		/// <param name="Num">Giá trị số cần chuyển</param>
		/// <returns></returns>
		public static string NumericToString(decimal Num)
		{
			string[] Dvi = { "", "ngàn", "triệu", "tỷ", "ngàn" };
			string Result = "";
			long IntNum = (long)Num;
			for (int i = 0; i < 5; i++)
			{
				long Doc = (long)IntNum % 1000;
				if (Doc > 0) Result = NumericToString(Doc, IntNum < 1000) + " " + Dvi[i] + "" + Result;
				IntNum = (long)IntNum / 1000;
			}
			Result += " đồng" + ((((long)Num) % 1000 == 0) ? " chẵn" : "");
			if (Result.ToString().Trim().Substring(0, 1) == "m" && Result.ToString().Trim().Substring(3, 1) == "i")
				Result = "Mười " + Result.Remove(0, 5);
			return Result.Length == 4 ? "..." : Result;
		}

		/// <summary>
		/// Đổi từ số sang chữ Tiếng Anh
		/// </summary>
		/// <param name="number">Giá trị số cần chuyển</param>
		/// <returns></returns>
		public static string NumberToWordsENG(int number)
		{
			if (number == 0)
				return "zero";

			if (number < 0)
				return "minus " + NumberToWordsENG(Math.Abs(number));

			string words = "";

			if ((number / 1000000) > 0)
			{
				words += NumberToWordsENG(number / 1000000) + " million ";
				number %= 1000000;
			}

			if ((number / 1000) > 0)
			{
				words += NumberToWordsENG(number / 1000) + " thousand ";
				number %= 1000;
			}

			if ((number / 100) > 0)
			{
				words += NumberToWordsENG(number / 100) + " hundred ";
				number %= 100;
			}

			if (number > 0)
			{
				if (words != "")
					words += "and ";

				var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
				var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

				if (number < 20)
					words += unitsMap[number];
				else
				{
					words += tensMap[number / 10];
					if ((number % 10) > 0)
						words += "-" + unitsMap[number % 10];
				}
			}

			return words;
		}

		/// <summary>
		/// Thao
		/// Dùng để đổi dấu phân biệt hàng nghìn, hàng đơn vị trong các câu lệnh UPDATE
		/// </summary>
		/// <param name="strNumber">Giá trị số cần chuyển theo dạng chuỗi</param>
		/// <returns></returns>
		public static string DoiDau(string strNumber)
		{
			int length = 0; int pos = 0; string st = ""; string DoiDau1 = "";
			st = strNumber;
			pos = st.IndexOf(".", 0);
			while (pos > 0)
			{
				st = st.Substring(0, pos) + st.Remove(0, pos);
				pos = st.IndexOf(".", 0);
			}
			length = st.Length;
			pos = st.IndexOf(",", 0);
			if (pos > 0)
				DoiDau1 = st.Substring(0, pos) + "." + st.Remove(0, pos + 1);
			else
				DoiDau1 = st;

			return DoiDau1;
		}

		public static string ArrayToString(string separator, string[] arr)
		{
			if (arr.Length > 0)
			{
				return string.Join(separator, arr);
			}
			else
			{
				return "";
			}

		}
		#endregion
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region Xử lý thời gian

		/// <summary>
		/// Lay ve ngay thang cua he thong.
		/// </summary>
		/// <returns></returns>
		public static DateTime GetSystemDate()
		{
			try
			{
				return Convert.ToDateTime(TextUtils.GetTable("spGetDateSystem", null, "Table").Rows[0][0]);
			}
			catch (Exception ex)
			{
				return DateTime.Now;
			}
		}

		/// <summary>
		/// Lấy giá trị chênh lệch giữa 2 mốc thời gian
		/// </summary>
		/// <param name="Interval">dạng giá trị cần lấy h: Số giờ, day: số ngày, month: số tháng, year: số năm</param>
		/// <param name="Date1">Ngày bắt đầu</param>
		/// <param name="Date2">Ngày kết thúc</param>
		/// <returns></returns>
		public static int DateDiff(string Interval, DateTime Date1, DateTime Date2)
		{
			int intDateDiff = 0;
			TimeSpan time = Date1 - Date2;
			int timeHours = Math.Abs(time.Hours);
			int timeDays = Math.Abs(time.Days);

			switch (Interval.ToLower())
			{
				case "h": // hours
					intDateDiff = timeHours;
					break;
				case "d": // days
					time = Date1.Date - Date2.Date;
					intDateDiff = Math.Abs(time.Days);
					//intDateDiff = timeDays;
					break;
				case "w": // weeks
					intDateDiff = timeDays / 7;
					break;
				case "bw": // bi-weekly
					intDateDiff = (timeDays / 7) / 2;
					break;
				case "m": // monthly
					timeDays = timeDays - ((timeDays / 365) * 5);
					intDateDiff = timeDays / 30;
					break;
				case "bm": // bi-monthly
					timeDays = timeDays - ((timeDays / 365) * 5);
					intDateDiff = (timeDays / 30) / 2;
					break;
				case "q": // quarterly
					timeDays = timeDays - ((timeDays / 365) * 5);
					intDateDiff = (timeDays / 90);
					break;
				case "y": // yearly
					intDateDiff = (timeDays / 365);
					break;
			}

			return intDateDiff;
		}

		/// <summary>
		/// Thao
		/// Lấy số tuần của 1 năm
		/// </summary>
		/// <param name="Year">Năm cần lấy</param>
		/// <returns></returns>
		public static List<string> LoadAllWeekOfYear(int Year)
		{
			List<DateTime[]> weeks = new List<DateTime[]>();
			List<string> str = new List<string>();

			DateTime beginDate = new DateTime(Year, 01, 01);
			DateTime endDate = new DateTime(Year, 12, 31);

			DateTime monday = DateTime.Today;
			DateTime satday = DateTime.Today;

			while (beginDate < endDate)
			{
				beginDate = beginDate.AddDays(1);

				if (beginDate.DayOfWeek == DayOfWeek.Monday)
				{
					monday = beginDate;
				}
				else if (beginDate.DayOfWeek == DayOfWeek.Saturday)
				{
					satday = beginDate;
				}
				else if (beginDate.DayOfWeek == DayOfWeek.Sunday)
				{
					weeks.Add(new DateTime[] { monday, satday });
				}
			}
			int count = 0;
			for (int x = 1; x < weeks.Count; x++)
			{
				if (x == 1)
				{
					int startDay = weeks[x][0].Date.Day;
					if (startDay >= 4)
					{
						str.Add("Tuần 1: " + "02/01/" + Year + " - 0" + (startDay - 2) + "/01/" + Year);
						count = 1;
					}
				}

				str.Add("Tuần " + (x + count) + ": " + (weeks[x][0]).ToString("dd/MM/yyyy") + " - " + (weeks[x][1]).ToString("dd/MM/yyyy"));

				if (x == weeks.Count - 1)
				{
					int endDay = weeks[x][1].Date.Day;
					if (endDay <= 29)
					{
						str.Add("Tuần " + (weeks.Count + count) + ": " + (endDay + 2) + "/01/" + Year + " - 31/01/" + Year);
					}
				}
			}
			return str;
		}

		#endregion Cac ham xu li thoi gian
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////        
		#region Xứ lý với File, Folder

		/// <summary>
		/// Lưu trũ một file dựa trên giao diện lưu trữ của window
		/// </summary>
		/// <param name="title"></param>
		/// <param name="filter">Kiểu file (.doc,xls,...)</param>
		/// <param name="FileName">Tên file</param>
		/// <returns></returns>
		public static string ShowSaveFileDialog(string title, string filter, string FileName)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Title = title;
			dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			dlg.FileName = FileName;//name;
			dlg.Filter = filter;
			if (dlg.ShowDialog() == DialogResult.OK) return dlg.FileName;
			return "";
		}

		/// <summary>
		/// Mở một file
		/// </summary>
		/// <param name="fileName">đường dẫn file</param>
		public static void OpenFile(string fileName)
		{
			if (MessageBox.Show("Do you want to open file?", Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				try
				{
					System.Diagnostics.Process process = new System.Diagnostics.Process();
					process.StartInfo.FileName = fileName;
					process.StartInfo.Verb = "Open";
					process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
					process.Start();
				}
				catch
				{
					MessageBox.Show("File not found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		/// <summary>
		/// Kiểm tra xem đường dẫn có phải là 1 file không
		/// </summary>
		/// <param name="filePath">Đường dẫn</param>
		/// <returns></returns>
		public static bool IsFile(string filePath)
		{
			if (File.Exists(filePath))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Kiểm tra xem đường dẫn có phải là 1 thư mục không
		/// </summary>
		/// <param name="filePath">Đường dẫn</param>
		/// <returns></returns>
		public static bool IsFolder(string filePath)
		{
			if (Directory.Exists(filePath))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Copy file nhưng không ghi đề file trùng tên mà tạo ra một phiên bản mới. VD: a(1).txt
		/// </summary>
		/// <param name="sourceFile">Đường dẫn file nguồn</param>
		/// <param name="fileName">Tên file</param>
		/// <param name="destinationPath">Thư mục chứa file</param>
		/// <returns></returns>
		public static bool FileCopyWithoutOverwriting(string sourceFile, string fileName, string destinationPath)
		{
			// if destinationPath doesn't add with a slash, add one
			if ((destinationPath.EndsWith("\\") || destinationPath.EndsWith("/")) == false)
				destinationPath += "\\";

			try
			{
				// if file already exists in destination
				if (File.Exists(destinationPath + fileName))
				{
					// counter
					int count = 1;

					// extract extension
					FileInfo info = new FileInfo(sourceFile);
					string ext = info.Extension;
					string prefix;

					// if it has an extension, append it to a .
					if (ext.Length > 0)
					{
						// get filename without extension
						prefix = fileName.Substring(0, fileName.Length - ext.Length);
						//ext = "." + ext;
					}
					else
						prefix = fileName;

					// while not found an valid destination file name, increase counter
					while (File.Exists(destinationPath + fileName))
					{
						fileName = prefix + "(" + count.ToString() + ")" + ext;
						count++;
					}
					// copy file
					File.Copy(sourceFile, destinationPath + fileName);
				}
				else
				{
					File.Copy(sourceFile, destinationPath + fileName);
				}
				return true;
			}
			catch
			{
				return false;
			}

		}

		/// <summary>
		/// Lấy đường dẫn thư mục trên Server
		/// </summary>
		/// <param name="pathServerNotDrive"></param>
		/// <returns></returns>
		public static string GetPathServer()
		{
			string pathServer = "";
			try
			{
				ConfigSystemModel model = (ConfigSystemModel)ConfigSystemBO.Instance.FindByAttribute("KeyName", "UpdatePath")[0];
				pathServer = @"" + model.KeyValue;
			}
			catch
			{
			}

			return pathServer;
		}

		/// <summary>
		/// CreateFolder(Server.MapPath("Album/Folder1″)); sẽ tạo thư mục Folder1 trong thư mục Album của webroot
		/// </summary>
		/// <param name="strPath">Đường dẫn</param>
		public static void CreateFolder(string strPath)
		{

			try
			{

				if (Directory.Exists(strPath) == false)
				{

					Directory.CreateDirectory(strPath);

				}

			}

			catch { }

		}
		/// <summary>
		/// RenameFolder(Server.MapPath("Album/Folder1″), Server.MapPath("Album/Folder2″)); 
		/// Sẻ đổi tên thư mục có tên Folder1 thành Folder2 trong thư mục Album của webroot
		/// </summary>
		/// <param name="strOldFolderName"></param>
		/// <param name="strNewFolderName"></param>
		public static void RenameFolder(string strOldFolderName, string strNewFolderName)
		{
			try
			{
				Directory.Move(strOldFolderName, strNewFolderName);
			}
			catch { }
		}
		/// <summary>
		/// Hàm xóa hết các thư mục và file bên trong một thư mục: 
		/// </summary>
		/// <param name="directoryInfo"></param>
		public static void EmptyFolder(DirectoryInfo directoryInfo)
		{

			try
			{
				foreach (FileInfo file in directoryInfo.GetFiles())
				{
					file.Delete();
				}

				foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
				{
					//EmptyFolder(subfolder);
					subfolder.Delete(true);
				}
			}
			catch { }

		}
		/// <summary>
		/// Hàm Copy thư mục này đến thư mục khác
		/// </summary>
		/// <param name="ThuMucNguon"></param>
		/// <param name="ThucMucDich"></param>
		public static void CopyDirectory(DirectoryInfo ThuMucNguon, DirectoryInfo ThucMucDich)
		{
			try
			{
				if (!ThucMucDich.Exists)
				{
					ThucMucDich.Create();
				}

				FileInfo[] files = ThuMucNguon.GetFiles(); foreach (FileInfo file in files)
				{
					if ((File.Exists(System.IO.Path.Combine(ThucMucDich.FullName, file.Name))) == false)
					{
						file.CopyTo(Path.Combine(ThucMucDich.FullName, file.Name));
					}
				}

				//Xử lý thư mục con
				DirectoryInfo[] dirs = ThuMucNguon.GetDirectories();
				foreach (DirectoryInfo dir in dirs)
				{
					string ThucMucDichDir = Path.Combine(ThucMucDich.FullName, dir.Name); CopyDirectory(dir, new DirectoryInfo(ThucMucDichDir));
				}
			}

			catch { }

		}
		/// <summary>
		/// Hàm này sẽ xóa thư mục mục và nội dung bên trong của thư mục được chọn
		/// </summary>
		/// <param name="strFolderName"></param>
		public static void DeleteFolder(string strFolderName)
		{
			DirectoryInfo ThuMucNguonDir = new DirectoryInfo(strFolderName);

			if (Directory.Exists(strFolderName))
			{
				try
				{
					//EmptyFolder(ThuMucNguonDir);
					EmptyFolder(ThuMucNguonDir);
					Directory.Delete(strFolderName);
				}

				catch { }
			}
		}

		public static void RenameFileVB(string filePath, string newFileName)
		{
			if (Path.GetFileName(filePath).ToUpper() == newFileName.ToUpper()) return;
			(new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.RenameFile(filePath, newFileName);
		}

		public static void RenameFolderVB(string path, string newFolderName)
		{
			try
			{
				(new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.RenameDirectory(path, newFolderName);
			}
			catch
			{
			}
		}

		public static void DeleteFileToRecycle(string filePath)
		{
			(new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.DeleteFile(filePath, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
				Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
		}

		public static void DeleteFolderVB(string folderPath)
		{
			(new Microsoft.VisualBasic.Devices.ServerComputer()).FileSystem.DeleteDirectory(folderPath, Microsoft.VisualBasic.FileIO.DeleteDirectoryOption.DeleteAllContents);
		}

		public static void CopyFolderVB(string sourceFolder, string outputFolder)
		{
			try
			{
				new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyDirectory(sourceFolder, outputFolder, true);
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}
		}
		#endregion Xứ lý với File
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region Control
		#region GridControl Devexpress

		/// <summary>
		/// Bật chế hiển thị của GridControl để export
		/// </summary>
		/// <param name="grid"></param>
		public static void GridPreview(DevExpress.XtraGrid.GridControl grid)
		{
			if (grid.DataSource == null)
				return;
			// Check whether or not the XtraGrid control can be previewed. 
			if (!grid.IsPrintingAvailable)
			{
				MessageBox.Show("The 'DevExpress.XtraPrinting' Library is not found", "Error");
				return;
			}
			// Opens the Preview window. 
			grid.ShowRibbonPrintPreview();
		}

		/// <summary>
		/// In dữ liệu trong gridcontrol
		/// </summary>
		/// <param name="grid"></param>
		public static void GridPrint(DevExpress.XtraGrid.GridControl grid)
		{
			if (grid.DataSource == null)
				return;
			// Check whether or not the XtraGrid control can be previewed. 
			if (!grid.IsPrintingAvailable)
			{
				MessageBox.Show("The 'DevExpress.XtraPrinting' Library is not found", "Error");
				return;
			}
			// Opens the Preview window. 
			grid.Print();
		}

		/// <summary>
		/// Export dữ liệu trong BandedGridView thành file excel
		/// </summary>
		/// <param name="grvData"></param>
		public static void ExportExcel(DevExpress.XtraGrid.Views.BandedGrid.BandedGridView grvData)
		{
			grvData.OptionsPrint.AutoWidth = false;
			grvData.OptionsPrint.ExpandAllDetails = false;
			grvData.OptionsPrint.PrintDetails = true;

			grvData.OptionsPrint.UsePrintStyles = true;

			//string filepath = System.IO.Path.GetTempFileName();
			string filepath = System.IO.Path.GetTempFileName();
			filepath = filepath.Remove(filepath.LastIndexOf('.') + 1);
			filepath = String.Concat(filepath, "xls");

			if (filepath != "")
			{
				try
				{ grvData.ExportToXls(filepath, new XlsExportOptions(TextExportMode.Value)); }
				catch
				{ grvData.ExportToExcelOld(filepath); }

				Excel.Application xlApp = new Excel.Application();
				Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(filepath, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", true, false, 0, true, 1, 0);
				Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
				xlApp.Visible = true;
				System.IO.File.Delete(filepath);
				//System.Diagnostics.Process.Start(filepath);
			}
		}


		/// <summary>
		/// Export dữ liệu trong BandedGridView thành file pdf
		/// </summary>
		/// <param name="grvData"></param>
		public static void ExportPdf(DevExpress.XtraGrid.Views.BandedGrid.BandedGridView grvData)
		{
			string filepath = System.IO.Path.GetTempFileName();
			filepath = filepath.Remove(filepath.LastIndexOf('.') + 1);
			filepath = String.Concat(filepath, "pdf");

			grvData.OptionsPrint.AutoWidth = false;
			grvData.OptionsPrint.ExpandAllDetails = false;
			grvData.OptionsPrint.PrintDetails = true;

			grvData.OptionsPrint.UsePrintStyles = true;

			try
			{ grvData.ExportToPdf(filepath); }
			catch
			{ grvData.ExportToPdf(filepath); }

			System.Diagnostics.Process.Start(filepath);
		}

		/// <summary>
		/// Export dữ liệu trong GridView thành file excel
		/// </summary>
		/// <param name="grvData"></param>
		public static void ExportExcel(DevExpress.XtraGrid.Views.Grid.GridView grvData)
		{
			//string filepath = System.IO.Path.GetTempFileName();
			//filepath = filepath.Remove(filepath.LastIndexOf('.') + 1);
			//filepath = String.Concat(filepath, "xls");

			grvData.OptionsPrint.AutoWidth = false;
			grvData.OptionsPrint.ExpandAllDetails = false;
			grvData.OptionsPrint.PrintDetails = true;

			grvData.OptionsPrint.UsePrintStyles = true;

			string filepath = System.IO.Path.GetTempFileName();

			if (filepath != "")
			{
				try
				{ grvData.ExportToXls(filepath); }
				catch
				{ grvData.ExportToExcelOld(filepath); }

				try
				{
					Excel.Application xlApp = new Excel.Application();

					Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(filepath, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", true, false, 0, true, 1, 0);
					Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
					xlApp.Visible = true;
					System.IO.File.Delete(filepath);
				}
				catch
				{
					filepath = filepath.Replace(".tmp", ".xls");
					grvData.ExportToXls(filepath);
					System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("Excel.exe", String.Format("/r \"{0}\"", filepath));
					System.Diagnostics.Process.Start(startInfo);
				}
			}
			//try
			//{ grvData.ExportToXls(filepath); }
			//catch
			//{ grvData.ExportToExcelOld(filepath); }

			//System.Diagnostics.ProcessStartInfo startInfo =
			//     new System.Diagnostics.ProcessStartInfo("Excel.exe", String.Format("/r \"{0}\"", filepath));

			//System.Diagnostics.Process.Start(startInfo);
		}

		public static void ExportExcel(DevExpress.XtraGrid.Views.Grid.GridView grvData, string fileName, bool isShowChoosePath)
		{
			grvData.OptionsPrint.AutoWidth = false;
			grvData.OptionsPrint.ExpandAllDetails = true;
			grvData.OptionsPrint.PrintDetails = true;

			grvData.OptionsPrint.UsePrintStyles = true;

			string filepath = "";
			if (isShowChoosePath)
			{
				FolderBrowserDialog d = new FolderBrowserDialog();
				if (d.ShowDialog() == DialogResult.OK)
				{
					filepath = d.SelectedPath + "//" + fileName + DateTime.Now.ToString("_dd-MM-yyyy-HH-mm-ss") + ".xls";
				}
			}
			else
			{
				filepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "//" + fileName + DateTime.Now.ToString("_dd-MM-yyyy-HH-mm-ss") + ".xls";
			}

			if (filepath != "")
			{
				try
				{ grvData.ExportToXls(filepath); }
				catch
				{
				}
				if (File.Exists(filepath))
				{
					Process.Start(filepath);
				}
			}
		}

		public static void ExportExcel(DevExpress.XtraGrid.Views.Grid.GridView grvData, string path, string moduleCode)
		{
			grvData.OptionsPrint.AutoWidth = false;
			grvData.OptionsPrint.ExpandAllDetails = true;
			grvData.OptionsPrint.PrintDetails = true;

			grvData.OptionsPrint.UsePrintStyles = true;

			string filepath = path + "//" + moduleCode + ".xls";

			if (filepath != "")
			{
				try
				{ grvData.ExportToXls(filepath); }
				catch
				{
					//grvData.ExportToExcelOld(filepath); 
				}
				Process.Start(filepath);
			}
		}

		public static void ExportExcel(DevExpress.XtraGrid.Views.Grid.GridView grvData, string path, string fileName, bool openFile)
		{
			grvData.OptionsPrint.AutoWidth = false;
			grvData.OptionsPrint.ExpandAllDetails = false;
			grvData.OptionsPrint.PrintDetails = true;

			grvData.OptionsPrint.UsePrintStyles = true;

			string filepath = path + "//" + fileName + ".xls";

			if (filepath != "")
			{
				try
				{ grvData.ExportToXls(filepath); }
				catch
				{
					//grvData.ExportToExcelOld(filepath); 
				}
				if (openFile)
				{
					Process.Start(filepath);
				}
			}
		}

		#endregion GridControl Devexpress

		#region Gridview
		public static void FormatCurrencyColumnGrid(ref DataGridView grvData, string ColoumnName)
		{
			grvData.Columns[ColoumnName].DefaultCellStyle.Format = CurrencyFormat;
			grvData.Columns[ColoumnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
		}
		public static void FormatCurrencyColumnGrid(ref BMS.DatagridView grvData, string ColoumnName)
		{
			grvData.Columns[ColoumnName].DefaultCellStyle.Format = CurrencyFormat;
			grvData.Columns[ColoumnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
		}
		public static void FormatDateColumnGrid(ref DataGridView grvData, string ColoumnName)
		{
			grvData.Columns[ColoumnName].DefaultCellStyle.Format = FomatShortDate;
		}
		public static void FormatDateColumnGrid(ref BMS.DatagridView grvData, string ColoumnName)
		{
			grvData.Columns[ColoumnName].DefaultCellStyle.Format = FomatShortDate;
		}
		#endregion Gridview

		#region TextBox
		/// <summary>
		/// Thao
		/// Kiểm tra dữ liệu kiểu số khi nhập vào các ô textbox
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public static bool CheckNumber(KeyPressEventArgs e)
		{
			if (!Char.IsControl(e.KeyChar) && !Char.IsNumber(e.KeyChar) && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',' && (e.KeyChar != '-'))
				return true;
			else
				return false;
		}

		/// <summary>
		/// Thao
		/// Kiểm tra dữ liệu kiểu số khi nhập vào các ô textbox
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public static bool CheckInterger(KeyPressEventArgs e)
		{
			if (!Char.IsControl(e.KeyChar) && !Char.IsNumber(e.KeyChar) && !Char.IsDigit(e.KeyChar))
				return true;
			else
				return false;
		}

		#endregion TextBox

		#region Combobox

		/// <summary>
		/// Nhập dữ liệu dạng ArrayList vào Combobox
		/// </summary>
		/// <param name="item">tên combobox</param>
		/// <param name="list">dữ liệu</param>
		/// <param name="display">Trường hiển thị</param>
		/// <param name="value">Trường giá trị</param>
		public static void PopulateCombo(ComboBox item, ArrayList list, string display, string value)
		{
			if (list.Count > 0)
			{
				item.DataSource = list;
				item.DisplayMember = display;
				item.ValueMember = value;
			}
			else
			{
				item.DataSource = null;
				item.Items.Clear();
			}
			item.Focus();
		}

		/// <summary>
		/// Nhập dữ liệu dạng DataTable vào Combobox
		/// </summary>
		/// <param name="comboBox">tên combobox</param>
		/// <param name="data">dữ liệu</param>
		/// <param name="DisplayField">trường hiển thị</param>
		/// <param name="ValueField">Trường giá trị</param>
		/// <param name="NotSetItem">Giá trị khởi tạo</param>
		public static void PopulateCombo(ComboBox comboBox, DataTable data, string DisplayField, string ValueField, string NotSetItem)
		{
			if (NotSetItem != "")
			{
				DataRow dr = data.NewRow();
				dr[DisplayField] = NotSetItem;
				dr[ValueField] = 0;
				data.Rows.InsertAt(dr, 0);
			}
			comboBox.DataSource = data;
			comboBox.DisplayMember = DisplayField;
			comboBox.ValueMember = ValueField;
			comboBox.SelectedIndex = -1;
		}

		/// <summary>
		/// Đưa dữ liệu vào LookUpEdit trong Devexpress
		/// </summary>
		/// <param name="comboBox">LookUpEdit</param>
		/// <param name="data">dữ liệu muốn hiển thị trên lookupedit</param>
		/// <param name="DisplayField">Trường cần hiển thị</param>
		/// <param name="ValueField">Trường giá trị</param>
		/// <param name="NotSetItem">Giá trị khởi tạo</param>
		public static void PopulateCombo(DevExpress.XtraEditors.LookUpEdit comboBox, DataTable data, string DisplayField, string ValueField, string NotSetItem)
		{
			if (NotSetItem != null && NotSetItem != "")
			{
				DataRow dr = data.NewRow();
				dr[DisplayField] = NotSetItem;
				dr[ValueField] = 0;
				data.Rows.InsertAt(dr, 0);
			}
			comboBox.Properties.Columns.Clear();
			comboBox.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[]
			{
				new DevExpress.XtraEditors.Controls.LookUpColumnInfo(ValueField),
				new DevExpress.XtraEditors.Controls.LookUpColumnInfo(DisplayField),
			});
			//comboBox.Properties.Columns[ValueField].Visible = false;
			comboBox.Properties.DataSource = data;
			comboBox.Properties.DisplayMember = DisplayField;
			comboBox.Properties.ValueMember = ValueField;
			if (data.Rows.Count > 0)
				comboBox.EditValue = ((DataTable)comboBox.Properties.DataSource).Rows[0][ValueField];
		}

		public static void PopulateCombo(DevExpress.XtraEditors.SearchLookUpEdit comboBox, DataTable data, string DisplayField, string ValueField, string NotSetItem)
		{
			if (NotSetItem != null && NotSetItem != "")
			{
				DataRow dr = data.NewRow();
				dr[DisplayField] = NotSetItem;
				dr[ValueField] = 0;
				data.Rows.InsertAt(dr, 0);
			}

			comboBox.Properties.DataSource = data;
			comboBox.Properties.ValueMember = ValueField;
			comboBox.Properties.DisplayMember = DisplayField;
		}
		#endregion Combobox
		/// <summary>
		/// Mở form có check sự tồn tại của nó
		/// </summary>
		/// <param name="frm">tên form</param>
		public static void OpenForm(_Forms frm)
		{
			_Forms _frmShow = (_Forms)Application.OpenForms[frm.Name];
			if (_frmShow == null)
			{
				_frmShow = frm;
			}
			else
				frm.Dispose();

			_frmShow.Show();
			if (_frmShow.WindowState == FormWindowState.Minimized)
				_frmShow.WindowState = _frmShow.VisibleFormState;
			_frmShow.Activate();
		}

		/// <summary>
		/// Reset tất cả các textbox trong form
		/// </summary>
		/// <param name="frm"></param>
		public static void ClearTexbox(Form frm)
		{
			foreach (Control ctrl in frm.Controls)
			{
				if (ctrl.GetType() == typeof(TextBox))
				{
					((TextBox)ctrl).Clear();
				}
			}
		}

		/// <summary>
		/// Reset tất cả các textbox trong các container
		/// </summary>
		/// <param name="ctrCollection"></param>
		static public void ClearTextBox(System.Windows.Forms.Control.ControlCollection ctrCollection)
		{
			foreach (object myObject in ctrCollection)
			{
				if (myObject is TextBox)
				{
					((TextBox)myObject).Clear();
				}
			}
		}

		/// <summary>
		/// Reset tất cả các textbox trong các container, chọn textbox sẽ được focus 
		/// </summary>
		/// <param name="ctrCollection">list container control</param>
		/// <param name="txtFocus">textbox sẽ được focus </param>
		static public void ClearTextBox(System.Windows.Forms.Control.ControlCollection ctrCollection, TextBox txtFocus)
		{
			ClearTextBox(ctrCollection);
			txtFocus.Focus();
		}
		#endregion Control
		////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		#region Process Excel

		/// <summary>
		/// Lấy danh sách các tên sheet trong file excel
		/// </summary>
		/// <param name="filePath">Đường dẫn file Excel</param>
		/// <returns></returns>
		public static List<string> ListSheetInExcel(string filePath)
		{
			OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
			String strExtendedProperties = String.Empty;
			sbConnection.DataSource = filePath;
			if (Path.GetExtension(filePath).Equals(".xls"))//for 97-03 Excel file
			{
				sbConnection.Provider = "Microsoft.Jet.OLEDB.4.0";
				strExtendedProperties = "Excel 8.0;HDR=Yes;IMEX=1";//HDR=ColumnHeader,IMEX=InterMixed
			}
			else if (Path.GetExtension(filePath).Equals(".xlsx") || Path.GetExtension(filePath).Equals(".xlsm"))  //for 2007 Excel file
			{
				sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
				strExtendedProperties = "Excel 12.0;HDR=Yes;IMEX=1";
			}
			sbConnection.Add("Extended Properties", strExtendedProperties);

			List<string> listSheet = new List<string>();
			OleDbConnection conn = new OleDbConnection(sbConnection.ToString());
			try
			{
				conn.Open();
				DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
				foreach (DataRow drSheet in dtSheet.Rows)
				{
					if (drSheet["TABLE_NAME"].ToString().EndsWith("$") || drSheet["TABLE_NAME"].ToString().EndsWith("$'"))
					{
						listSheet.Add(drSheet["TABLE_NAME"].ToString().Replace("$", "").Replace("#", ".").Replace("'", ""));
					}
				}
			}
			catch (Exception ex)
			{
				ShowError(ex);
			}
			finally
			{
				conn.Close();
			}

			return listSheet;
		}

		/// <summary>
		/// Lấy dữ liệu file excel dưới dạng Datatable
		/// </summary>
		/// <param name="filename">Đường dẫn file Excel</param>
		/// <param name="sheetName">Tên sheet cần lấy dữ liệu</param>
		/// <returns></returns>
		public static DataTable ExcelToDatatable(string filename)
		{
			OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
			String strExtendedProperties = String.Empty;
			sbConnection.DataSource = filename;
			if (Path.GetExtension(filename).Equals(".xls"))//for 97-03 Excel file
			{
				sbConnection.Provider = "Microsoft.Jet.OLEDB.4.0";
				strExtendedProperties = "Excel 8.0;HDR=Yes;IMEX=1";//HDR=ColumnHeader,IMEX=InterMixed
			}
			else if (Path.GetExtension(filename).Equals(".xlsx") || Path.GetExtension(filename).Equals(".xlsm"))  //for 2007 Excel file
			{
				sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
				strExtendedProperties = "Excel 12.0;HDR=Yes;IMEX=1";
			}
			sbConnection.Add("Extended Properties", strExtendedProperties);
			OleDbConnection conn = new OleDbConnection(sbConnection.ToString());
			DataTable dt = new DataTable();
			try
			{
				conn.Open();
				OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM [" + ListSheetInExcel(filename)[0] + "$]", conn);
				DataSet ds = new DataSet();
				da.Fill(ds);
				dt = ds.Tables[0];
			}
			catch (Exception ex)
			{

			}
			finally
			{
				conn.Close();
			}

			return dt;
		}

		public static DataTable ExcelToDatatable(string filename, string sheetName)
		{
			OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
			String strExtendedProperties = String.Empty;
			sbConnection.DataSource = filename;
			//if (Path.GetExtension(filename).Equals(".xls"))//for 97-03 Excel file
			//{
			//    sbConnection.Provider = "Microsoft.Jet.OLEDB.4.0";
			//    strExtendedProperties = "Excel 8.0;HDR=Yes;IMEX=1";//HDR=ColumnHeader,IMEX=InterMixed
			//}
			//else //if (Path.GetExtension(filename).Equals(".xlsx"))  //for 2007 Excel file
			//{
			sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
			strExtendedProperties = "Excel 12.0;HDR=Yes;IMEX=1";
			//}
			sbConnection.Add("Extended Properties", strExtendedProperties);
			OleDbConnection conn = new OleDbConnection(sbConnection.ToString());
			DataTable dt = new DataTable();
			try
			{
				conn.Open();

				//OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM [" + ListSheetInExcel(filename)[0] + "$]", conn);
				OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM [" + sheetName.Replace("'", "") + "$]", conn);
				DataSet ds = new DataSet();
				da.Fill(ds);
				dt = ds.Tables[0];
			}
			catch (Exception ex)
			{

			}
			finally
			{
				conn.Close();
			}

			return dt;
		}

		public static DataTable ExcelToDatatableNoHeader(string filename, string sheetName)
		{
			OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
			String strExtendedProperties = String.Empty;
			sbConnection.DataSource = filename;
			//if (Path.GetExtension(filename).Equals(".xls"))//for 97-03 Excel file
			//{
			//    sbConnection.Provider = "Microsoft.Jet.OLEDB.4.0";
			//    strExtendedProperties = "Excel 8.0;HDR=No;IMEX=1";//HDR=ColumnHeader,IMEX=InterMixed
			//}
			//else //if (Path.GetExtension(filename).Equals(".xlsx"))  //for 2007 Excel file
			//{
			sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
			strExtendedProperties = "Excel 12.0;HDR=No;IMEX=1";
			//}
			sbConnection.Add("Extended Properties", strExtendedProperties);
			OleDbConnection conn = new OleDbConnection(sbConnection.ToString());
			DataTable dt = new DataTable();
			try
			{
				conn.Open();

				//OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM [" + ListSheetInExcel(filename)[0] + "$]", conn);
				OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM [" + sheetName.Replace("'", "") + "$]", conn);
				DataSet ds = new DataSet();
				da.Fill(ds);
				dt = ds.Tables[0];
			}
			catch (Exception ex)
			{
			}
			finally
			{
				conn.Close();
			}

			return dt;
		}

		public static void ExcelFindAndReplace(string filePath, string findText, string replaceText)
		{
			Excel.Application app = default(Excel.Application);
			Excel.Workbook workBook = default(Excel.Workbook);
			Excel.Worksheet workSheet = default(Excel.Worksheet);
			try
			{
				app = new Excel.Application();
				app.Workbooks.Open(filePath);
				workBook = app.Workbooks[1];
				workSheet = (Excel.Worksheet)workBook.Worksheets[1];

				workSheet.Cells.Replace(What: findText, Replacement: replaceText, LookAt: Excel.XlLookAt.xlPart,
				SearchOrder: Excel.XlSearchOrder.xlByRows, MatchCase: true, SearchFormat: true, ReplaceFormat: true);

				workSheet.PageSetup.LeftHeader = workSheet.PageSetup.LeftHeader.Replace(findText, replaceText);
				workSheet.PageSetup.CenterHeader = workSheet.PageSetup.CenterHeader.Replace(findText, replaceText);
				workSheet.PageSetup.RightHeader = workSheet.PageSetup.RightHeader.Replace(findText, replaceText);
				workSheet.PageSetup.LeftFooter = workSheet.PageSetup.LeftFooter.Replace(findText, replaceText);
				workSheet.PageSetup.CenterFooter = workSheet.PageSetup.CenterFooter.Replace(findText, replaceText);
				workSheet.PageSetup.RightFooter = workSheet.PageSetup.RightFooter.Replace(findText, replaceText);
			}
			catch (Exception)
			{
			}
			finally
			{
				app.ActiveWorkbook.Save();
				app.Workbooks.Close();
				app.Quit();
			}
		}

		public static void GridToCsv(DataGridView dGV, string filePathCSV)
		{
			string stOutput = "";
			string sHeaders = "";
			for (int j = 0; j < dGV.Columns.Count; j++)
				sHeaders = sHeaders.ToString() + Convert.ToString(dGV.Columns[j].HeaderText) + "\t";
			stOutput += sHeaders + "\r\n";
			// Export data.
			for (int i = 0; i < dGV.RowCount - 1; i++)
			{
				string stLine = "";
				for (int j = 0; j < dGV.Rows[i].Cells.Count; j++)
					stLine = stLine.ToString() + dGV.Rows[i].Cells[j].Value.ToString() + "\t";
				stOutput += stLine + "\r\n";
			}
			FileStream fs = new FileStream(filePathCSV, FileMode.Create);
			StreamWriter bw = new StreamWriter(fs, Encoding.Unicode);
			bw.Write(stOutput); //write the encoded file
			bw.Flush();
			bw.Close();
			fs.Close();
		}

		public static void DatatableToCSV(DataTable dataTable, string filePathCSV)
		{
			string stOutput = "";
			string sHeaders = "";
			for (int j = 0; j < dataTable.Columns.Count; j++)
				sHeaders = sHeaders.ToString() + Convert.ToString(dataTable.Columns[j].Caption.Replace("\n", "")) + "\t";
			stOutput += sHeaders + "\r\n";
			// Export data.
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				string stLine = "";
				for (int j = 0; j < dataTable.Rows[i].ItemArray.Count(); j++)
					stLine = stLine.ToString() + dataTable.Rows[i].ItemArray[j].ToString().Replace("\n", "") + "\t";
				stOutput += stLine + "\r\n";
			}
			FileStream fs = new FileStream(filePathCSV, FileMode.Create);
			StreamWriter bw = new StreamWriter(fs, Encoding.Unicode);
			bw.Write(stOutput); //write the encoded file
			bw.Flush();
			bw.Close();
			fs.Close();
		}
		#endregion Process Excel

		#region Mails

		public static bool SetEmailSend(string sSubject, string sBody, string sTo, string sCC, string sFilename, string sDisplayname)
		{
			bool isSuccess;
			try
			{
				// Create the Outlook application by using inline initialization.
				Outlook.Application oApp = new Outlook.Application();

				//Create the new message by using the simplest approach.
				Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);

				//Add a recipient.
				// TODO: Change the following recipient where appropriate.
				Outlook.Recipient oRecip = (Outlook.Recipient)oMsg.Recipients.Add("e-mail address");
				oRecip.Resolve();

				//Set the basic properties.
				oMsg.Subject = sSubject;
				oMsg.Body = sBody;

				//Add an attachment.
				// TODO: change file path where appropriate
				String sSource = sFilename;
				int iPosition = (int)oMsg.Body.Length + 1;
				int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
				Outlook.Attachment oAttach = oMsg.Attachments.Add(sFilename, iAttachType, iPosition, sDisplayname);

				//Send the message.
				oMsg.Save();
				oMsg.Send();

				isSuccess = true;
				//Explicitly release objects.
				oRecip = null;
				oAttach = null;
				oMsg = null;
				oApp = null;
			}
			catch (Exception)
			{
				isSuccess = false;
			}
			return isSuccess;
		}
		//
		public static bool SetEmailSend(string sSubject, string sBody, string sTo, string cc)
		{
			bool isSuccess;
			try
			{
				Outlook.Application oApp = new Outlook.Application();
				Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
				Outlook.Inspector oInspector = oMsg.GetInspector;
				oMsg.Subject = sSubject;
				oMsg.HTMLBody = sBody;
				oMsg.To = sTo;
				oMsg.CC = cc;
				oMsg.Send();
				isSuccess = true;

				oMsg = null;
				oApp = null;
			}
			catch (Exception)
			{
				isSuccess = false;
			}
			return isSuccess;
		}

		public static bool SetEmailSendHasAttach(string sSubject, string sBody, string sTo, string cc, List<string> attachments)
		{
			bool isSuccess;
			try
			{
				Outlook.Application oApp = new Outlook.Application();
				Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
				Outlook.Inspector oInspector = oMsg.GetInspector;
				oMsg.Subject = sSubject;
				oMsg.To = sTo;
				oMsg.CC = cc;

				oMsg.Display();
				oMsg.HTMLBody = sBody + oMsg.HTMLBody;

				if (attachments.Count > 0)
				{
					foreach (string attachment in attachments)
					{
						if (File.Exists(attachment))
							oMsg.Attachments.Add(attachment);
					}
				}

				oMsg.Send();
				isSuccess = true;

				oMsg = null;
				oApp = null;
			}
			catch (Exception)
			{
				isSuccess = false;
			}
			return isSuccess;
		}

		public static void SetEmailSendHasAttach1(string sSubject, string sBody, string sTo, string cc, List<string> attachments)
		{
			//bool isSuccess;
			try
			{
				Outlook.Application oApp = new Outlook.Application();
				Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
				Outlook.Inspector oInspector = oMsg.GetInspector;
				oMsg.Subject = sSubject;
				oMsg.To = sTo;
				oMsg.CC = cc;

				oMsg.HTMLBody = sBody + oMsg.HTMLBody;

				if (attachments.Count > 0)
				{
					foreach (string attachment in attachments)
					{
						if (File.Exists(attachment))
							oMsg.Attachments.Add(attachment);
					}
				}

				//oMsg.Send();
				//isSuccess = true;
				oMsg.Display(true);

				oMsg = null;
				oApp = null;
			}
			catch (Exception)
			{
				//isSuccess = false;
			}
			//return isSuccess;
		}

		#endregion Mails

		#region Process Word
		private static void SearchAndReplaceInStory(Word.Range rngStory, string strSearch, string strReplace)
		{
			rngStory.Find.ClearFormatting();
			rngStory.Find.Replacement.ClearFormatting();
			rngStory.Find.Text = strSearch;
			rngStory.Find.Replacement.Text = strReplace;
			rngStory.Find.Wrap = Word.WdFindWrap.wdFindContinue;

			object arg1 = Type.Missing; // Find Pattern
			object arg2 = Type.Missing; //MatchCase
			object arg3 = Type.Missing; //MatchWholeWord
			object arg4 = Type.Missing; //MatchWildcards
			object arg5 = Type.Missing; //MatchSoundsLike
			object arg6 = Type.Missing; //MatchAllWordForms
			object arg7 = Type.Missing; //Forward
			object arg8 = Type.Missing; //Wrap
			object arg9 = Type.Missing; //Format
			object arg10 = Type.Missing; //ReplaceWith
			object arg11 = Word.WdReplace.wdReplaceAll; //Replace
			object arg12 = Type.Missing; //MatchKashida
			object arg13 = Type.Missing; //MatchDiacritics
			object arg14 = Type.Missing; //MatchAlefHamza
			object arg15 = Type.Missing; //MatchControl

			rngStory.Find.Execute(ref arg1, ref arg2, ref arg3, ref arg4, ref arg5, ref arg6, ref arg7, ref arg8, ref arg9, ref arg10, ref arg11, ref arg12, ref arg13, ref arg14, ref arg15);
		}

		public static void FindReplaceAnywhere(Word.Application app, string findText, string replaceText)
		{
			var doc = app.ActiveDocument;
			//Word.WdStoryType lngJunk = doc.Sections[1].Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.StoryType;
			foreach (Word.Range rngStory in doc.StoryRanges)
			{
				var internalRangeStory = rngStory;
				do
				{
					SearchAndReplaceInStory(internalRangeStory, findText, replaceText);
					try
					{
						switch (internalRangeStory.StoryType)
						{
							case Word.WdStoryType.wdEvenPagesHeaderStory: // 6
							case Word.WdStoryType.wdPrimaryHeaderStory:   // 7
							case Word.WdStoryType.wdEvenPagesFooterStory: // 8
							case Word.WdStoryType.wdPrimaryFooterStory:   // 9
							case Word.WdStoryType.wdFirstPageHeaderStory: // 10
							case Word.WdStoryType.wdFirstPageFooterStory: // 11

								if (internalRangeStory.ShapeRange.Count != 0)
								{
									foreach (Word.Shape oShp in internalRangeStory.ShapeRange)
									{
										if (oShp.TextFrame.HasText != 0)
										{
											SearchAndReplaceInStory(oShp.TextFrame.TextRange, findText, replaceText);
										}
									}
								}
								break;

							default:
								break;
						}
					}
					catch
					{
					}


					internalRangeStory = internalRangeStory.NextStoryRange;
				} while (internalRangeStory != null);
			}
		}

		public static void RepalaceText(Word.Document doc, string oldtext, string newText)
		{
			doc.Content.Find.Execute(oldtext, false, true, false, false, false, true, 1, false, newText, 2,
			false, false, false, false);
		}

		public static void WordFindAndReplace(string filePath, string findText, string replaceText)
		{
			Word.Application word = new Word.Application();
			Word.Document doc = new Word.Document();
			try
			{
				doc = word.Documents.Open(filePath);
				//doc.Activate();
				TextUtils.FindReplaceAnywhere(word, findText, replaceText);
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.ToString(), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				doc.Save();
				doc.Close();
				word.Quit();
			}
		}

		public static void FindandReplace(Word.Document doc, string Findtext, string ReplaceText)
		{
			Word.Range myStoryRange = doc.Range();

			//First search the main document using the Selection
			Word.Find myFind = myStoryRange.Find;
			myFind.Text = Findtext;
			myFind.Replacement.Text = ReplaceText;
			myFind.Forward = true;
			myFind.Wrap = Word.WdFindWrap.wdFindContinue;
			myFind.Format = false;
			myFind.MatchCase = false;
			myFind.MatchWholeWord = false;
			myFind.MatchWildcards = false;
			myFind.MatchSoundsLike = false;
			myFind.MatchAllWordForms = false;
			myFind.Execute(Replace: Word.WdReplace.wdReplaceAll);

			//'Now search all other stories using Ranges
			foreach (Word.Range otherStoryRange in doc.StoryRanges)
			{
				if (otherStoryRange.StoryType != Word.WdStoryType.wdMainTextStory)
				{
					Word.Find myOtherFind = otherStoryRange.Find;
					myOtherFind.Text = Findtext;
					myOtherFind.Replacement.Text = ReplaceText;
					myOtherFind.Wrap = Word.WdFindWrap.wdFindContinue;
					myOtherFind.Execute(Replace: Word.WdReplace.wdReplaceAll);
				}

				// 'Now search all next stories of other stories (doc.storyRanges dont seem to cascades in sub story)
				Word.Range nextStoryRange = otherStoryRange.NextStoryRange;
				while (nextStoryRange != null)
				{
					Word.Find myNextStoryFind = nextStoryRange.Find;
					myNextStoryFind.Text = Findtext;
					myNextStoryFind.Replacement.Text = ReplaceText;
					myNextStoryFind.Wrap = Word.WdFindWrap.wdFindContinue;
					myNextStoryFind.Execute(Replace: Word.WdReplace.wdReplaceAll);

					nextStoryRange = nextStoryRange.NextStoryRange;
				}

			}
		}

		public static void RunMacroInWord(string filePath, string macroName, string parameter)
		{
			Word.Application word = new Word.Application();
			Word.Document doc = new Word.Document();
			try
			{
				doc = word.Documents.Open(filePath);
				word.Run(macroName, parameter);
			}
			catch (Exception)
			{
			}
			finally
			{
				doc.Save();
				doc.Close();
				word.Quit();
			}
		}
		#endregion

		#region Cac ham xu li khac

		/// <summary>
		/// Lấy giá trị trong bảng ConfigSystem
		/// </summary>
		/// <param name="keyName">trường đại diện của giá trị</param>
		/// <returns></returns>
		public static string[] GetConfigSystem(string keyName)
		{
			ConfigSystemModel config = new ConfigSystemModel();
			ArrayList arr = ConfigSystemBO.Instance.FindByAttribute("KeyName", keyName);
			if (arr.Count == 0)
			{
				throw new Exception(String.Format("[{0}] does not exist in [ConfigSystem].", keyName));
			}
			else
				config = (ConfigSystemModel)arr[0];

			List<string> vals = new List<string>();
			PropertyInfo[] props = config.GetType().GetProperties();
			foreach (PropertyInfo p in props)
				if (p.Name.StartsWith("KeyValue"))
					vals.Add(p.GetValue(config, null).ToString());

			return vals.ToArray();
		}

		/// <summary>
		/// Khởi tạo thông số để kết nối với đầu đọc thẻ
		/// </summary>
		/// <param name="com">Đối tượng xử lý các sự kiện liên quan đến cổng COM</param>
		/// <param name="displayWindow">Control dùng để hiển thị dữ liệu đọc vào từ đầu đọc</param>
		public static void InitializeCardReader(ref BMS.Utils.CommunicationManager com, Control displayWindow)
		{
			com = new BMS.Utils.CommunicationManager();
			com.BaudRate = "9600";
			com.DataBits = "8";
			com.Parity = "None";
			com.StopBits = "1";
			com.DisplayWindow = displayWindow;
			com.CurrentTransmissionType = BMS.Utils.CommunicationManager.TransmissionType.Text;
			try
			{
				string[] settings = File.ReadAllLines("settings.ini");
				for (int i = 0; i < settings.Length; i++)
				{
					if (settings[i].StartsWith("COMPort"))
						com.PortName = settings[i].Split('=')[1].Trim().ToUpper();
				}
			}
			catch
			{
				com.PortName = "COM7";
				MessageBox.Show("Cổng COM của đầu đọc thẻ chưa được thiết lập!\nCổng mặc định (COM7) sẽ được sử dụng.", TextUtils.Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// Hàm trả địa chỉ IP hoặc tên máy hiện tại
		/// </summary>
		/// <param name="IP"></param>
		/// <returns>IP=true: Trả lại địa chỉ IP. IP=false: Trả lại tên máy</returns>
		public static string GetHostName()
		{
			return Dns.GetHostName();
		}
		/// <summary>
		/// Kiểm tra một chuối có phải là định dạng của email không
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static bool IsEmail(string str)
		{
			bool State = true;
			if (!Regex.IsMatch(str, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
			{
				State = false;
			}

			return State;
		}

		private static string NumberToString(long Num)
		{
			string[] Number = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
			return Number[Num];
		}

		public static string NumberToStringH(long Num)
		{
			string[] Number = { "không", "Một", "Hai", "Ba", "Bốn", "Năm", "Sáu", "Bảy", "Tám", "Chín" };
			return Number[Num];
		}

		/// <summary>
		/// Chuyển kiểu số sang kiểu chữ tiếna việt
		/// </summary>
		/// <param name="Num"></param>
		/// <param name="Dau"></param>
		/// <returns></returns>
		public static string NumericToString(long Num, bool Dau)
		{
			long Tram = (long)Num / 100;
			long Chuc = (long)(Num % 100) / 10;
			long Dvi = Num % 10;

			string Doc = (((Tram == 0) && (Dau)) ? "" : (" " + NumberToString(Tram) + " trăm")) + ((Chuc == 0) ? (((Tram == 0) && Dau) ? "" : ((Dvi == 0) ? "" : " lẻ")) : ((Chuc == 1) ? " mười" : (" " + NumberToString(Chuc) + " mươi"))) + (((Dvi == 5) && (Chuc > 0)) ? " năm" : ((Dvi == 0) ? "" : " " + NumberToString(Dvi)));
			return Doc;
		}

		/// <summary>
		/// Định dạng số dưới dạng tiền Việt Nam
		/// </summary>
		/// <param name="amount"></param>
		/// <returns></returns>
		public static string FormatVND(decimal amount)
		{
			if (amount == 0) { return "0"; }
			else
			{
				return amount.ToString(TextUtils.CurrencyFormatVND);
			}
		}

		/// <summary>
		/// Hiển thị Thông báo lỗi
		/// </summary>
		/// <param name="ex"></param>
		public static void ShowError(Exception ex)
		{
			MessageBox.Show(ex.Message, TextUtils.Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static void ShowError(string content)
		{
			MessageBox.Show(content, TextUtils.Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static void ShowError(string content, Exception ex)
		{
			MessageBox.Show(content + Environment.NewLine + ex.Message, TextUtils.Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		public static void ReleaseComObject(object obj)
		{
			try
			{
				System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
				obj = null;
			}
			catch (Exception)
			{
				obj = null;
			}
		}

		public static void EndProcess(string processName)
		{
			Process[] ps = Process.GetProcesses().Where(o => o.ProcessName.ToUpper().Contains(processName.ToUpper())).ToArray();
			foreach (Process p in ps)
			{
				p.Kill();
			}
		}
		#endregion

		#region LinQ
		public static DataTable GetDistinctDatatable(DataTable dt, string column)
		{
			try
			{
				DataTable dt1 = dt.AsEnumerable()
							.GroupBy(r => r.Field<string>(column))
							.Select(g => g.First())
							.Distinct()
							.CopyToDataTable();
				return dt1;
			}
			catch
			{
				return dt;
			}
		}
		#endregion LinQ

		#region Download Thiet Ke
		public static void DownloadFtpFolder(string ftpPath, string localPath)
		{
			Directory.CreateDirectory(localPath + "/" + Path.GetFileName(ftpPath));
			string[] fileTable = DocUtils.GetContentList(ftpPath);
			if (fileTable != null)
			{
				foreach (string item in fileTable)
				{
					long size = 0;
					try
					{
						size = DocUtils.GetFileSize(ftpPath + "/" + item);
					}
					catch
					{
					}
					if (size == 0)
					{
						DownloadFtpFolder(ftpPath + "/" + item, localPath + "/" + Path.GetFileName(ftpPath));
					}
					else
					{
						DocUtils.DownloadFile(localPath + "/" + Path.GetFileName(ftpPath), item, ftpPath + "/" + item);
					}
				}
			}
		}

		public static void DownloadAllTK(string ftpPath, string localPath)
		{
			Directory.CreateDirectory(localPath + "/" + ftpPath);
			string[] fileTable = DocUtils.GetContentList(ftpPath);
			if (fileTable != null)
			{
				foreach (string item in fileTable)
				{
					long size = 0;
					try
					{
						size = DocUtils.GetFileSize(ftpPath + "/" + item);
					}
					catch
					{
					}
					if (size == 0)
					{
						//Directory.CreateDirectory(ftpPath + "/" + item);
						DownloadAllTK(ftpPath + "/" + item, localPath);
					}
					else
					{
						DocUtils.DownloadFile(localPath + "/" + ftpPath, item, ftpPath + "/" + item);
					}
				}
			}
		}

		public static string DownloadAll(string moduleCode)
		{
			DocUtils.InitFTPQLSX();
			int version = 0;
			string initPath = @"" + TextUtils.Select("ConfigSystem", new Utils.Expression("KeyName", "ModuleVersion")).Rows[0]["KeyValue"].ToString();
			initPath += "/" + moduleCode;
			if (!Directory.Exists(initPath))
			{
				version = 0;
			}
			else
			{
				version = (Directory.GetDirectories(initPath, "*", SearchOption.TopDirectoryOnly).Count());
			}

			initPath += "/" + version;

			string ftpPathCk = string.Format("/Thietke.Ck/{0}/{1}.Ck", moduleCode.Substring(0, 6), moduleCode);
			string ftpPathDn = string.Format("/Thietke.Dn/{0}/{1}.Dn", moduleCode.Substring(0, 6), moduleCode);

			//download folder co khi
			//using (WaitDialogForm fWait = new WaitDialogForm("Vui lòng chờ trong giây lát", "Đang download dữ liệu cơ khí"))
			//{
			if (DocUtils.CheckExits(ftpPathCk))
			{
				DownloadAllTK(ftpPathCk, initPath);

				try
				{
					string filePathIAM = string.Format(@"W:\ModuleVersion\{0}\{1}\Thietke.Ck\{2}\{0}.Ck\3D.{0}\{0}.iam", moduleCode, version, moduleCode.Substring(0, 6));

					//IPTDetail.LoadData(filePathIAM);
					Image img = null; //IPTDetail.Image;
					img.Save(@"\\SERVER\data2\Dulieu_TPA\PHANMEM\ModuleImage\" + moduleCode + ".jpg", ImageFormat.Jpeg);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Update ảnh module không thành công!" + ex.Message, TextUtils.Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			//}

			//download folder dien
			//using (WaitDialogForm fWait = new WaitDialogForm("Vui lòng chờ trong giây lát", "Đang download dữ liệu điện"))
			//{
			if (DocUtils.CheckExits(ftpPathDn))
			{
				DownloadAllTK(ftpPathDn, initPath);
			}
			//}

			//download folder dien tu
			//using (WaitDialogForm fWait = new WaitDialogForm("Vui lòng chờ trong giây lát", "Đang download dữ liệu điện tử"))
			//{
			DataTable dtDMVT = TextUtils.ExcelToDatatable(initPath + "/" + ftpPathCk + "/VT." + moduleCode + ".xlsm", "DMVT");
			if (dtDMVT.Rows.Count > 0)
			{
				DataRow[] listRow = dtDMVT.Select("F4 like '%PCB%'");
				if (listRow.Count() > 0)
				{
					DataTable dtDT = listRow.CopyToDataTable();
					foreach (DataRow row in dtDT.Rows)
					{
						string code = row["F4"].ToString();

						string ftpPathDt = string.Format("/Thietke.Dt/PCB/{0}", code);
						if (DocUtils.CheckExits(ftpPathDt))
						{
							DownloadAllTK(ftpPathDt, initPath);
						}
					}
				}
			}
			//}
			//Kết thúc download điện tử

			return initPath;
		}

		public static string GetPathOfCum(string cumCode)
		{
			string moduleCode = cumCode.Substring(0, 10);
			string fptPath = string.Format(@"Thietke.Ck\{0}\{1}.Ck\3D.{1}\", moduleCode.Substring(0, 6), moduleCode);
			string[] splitCum = cumCode.Split('.');
			int count = splitCum.Length;
			if (count >= 4)
			{
				string ne = moduleCode;
				for (int i = 2; i < count; i++)
				{
					ne += "." + splitCum[i];
					if (i == splitCum.Length - 1)
					{
						fptPath += ne;
					}
					else
					{
						fptPath += ne + "\\";
					}
				}
			}
			else
			{
				return fptPath + cumCode;
			}
			return fptPath;
		}

		public static List<string> GetListFolderInFtpPath(string ftpPath)
		{
			List<string> listPath = new List<string>();
			string[] fileTable = DocUtils.GetContentList(ftpPath);
			if (fileTable != null)
			{
				foreach (string item in fileTable)
				{
					long size = 0;
					try
					{
						size = DocUtils.GetFileSize(ftpPath + "/" + item);
					}
					catch
					{
					}
					if (size == 0)
					{
						listPath.Add(ftpPath + "/" + item);
					}
				}
			}
			return listPath;
		}

		public static List<string> GetListFileInFtpPath(string ftpPath)
		{
			List<string> listPath = new List<string>();
			string[] fileTable = DocUtils.GetContentList(ftpPath);
			if (fileTable != null)
			{
				foreach (string item in fileTable)
				{
					long size = 0;
					try
					{
						size = DocUtils.GetFileSize(ftpPath + "/" + item);
					}
					catch
					{
					}
					if (size > 0)
					{
						listPath.Add(ftpPath + "/" + item);
					}
				}
			}
			return listPath;
		}
		#endregion


		/// <summary>
		/// Chuyển đổi giữa unicode dựng sẵn và unicode tổ hợp
		/// </summary>
		/// <param name="text">chuổi cần chuyển đổi</param>
		/// <param name="type">
		/// type = 0: dựng sẵn sang tổ hợp
		/// type = 1: tổ hợp sang dựng sẵn 
		/// </param>
		/// <returns></returns>
		public static string ConvertUnicode(string text, int type)
		{
			string filePath = Application.StartupPath + "/UnicodeConvert.xlsx";
			DataTable dt = TextUtils.ExcelToDatatableNoHeader(filePath, "Sheet1");
			foreach (DataRow row in dt.Rows)
			{
				string kytuDungSan = row[0].ToString().Trim();
				string kytuToHop = row[1].ToString().Trim();
				if (type == 0)
				{
					if (text.Contains(kytuDungSan))
						text = text.Replace(kytuDungSan, kytuToHop);
				}
				else
				{
					if (text.Contains(kytuToHop))
						text = text.Replace(kytuToHop, kytuDungSan);
				}
			}
			return text;
		}

		public static void ZipFolder(string fileOutput)
		{
			try
			{
				// Lấy về dường dẫn đến thư mục chứa file với ví dụ fileOutput như trên đường dẫn sẽ là C://FileToZip
				string filePathFull = fileOutput;

				// Lấy về danh sách các file trong thư mục C://FileToZip
				string[] files = Directory.GetFiles(filePathFull);

				// Lấy về tên file ở đây tên file lấy về sẽ là: Zipfile
				string fileName = Path.GetFileName(fileOutput);

				// Tên file zip được tạo ra ở đây tên sẽ là: C://FileToZip//Zipfile//Zipfile.zip
				string zipFile = Path.GetDirectoryName(filePathFull) + "\\" + fileName + ".zip";

				ZipOutputStream zipOut = new ZipOutputStream(File.Create(zipFile));

				// Lấy từng file trong thư mục C://FileToZip để tiến hành nén thành một file zip
				foreach (string file in files)
				{
					// Lấy về thông tin file có trong folder FileToZip
					FileInfo fileInfo = new FileInfo(file);
					ZipEntry entry = new ZipEntry(fileInfo.Name);

					FileStream fileStream = File.OpenRead(file);
					byte[] buffer = new byte[Convert.ToInt32(fileStream.Length)];
					fileStream.Read(buffer, 0, (int)fileStream.Length);
					entry.DateTime = fileInfo.LastWriteTime;
					entry.Size = fileStream.Length;
					fileStream.Close();

					zipOut.PutNextEntry(entry);
					zipOut.Write(buffer, 0, buffer.Length);

					// Xoá  file sau khi được nén
					//File.Delete(file);
				}

				zipOut.Finish();
				zipOut.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private static string ExtractTextFromPdf(string newFileNameWithImageAndText)
		{
			using (Stream newpdfStream = new FileStream(newFileNameWithImageAndText, FileMode.Open, FileAccess.ReadWrite))
			{
				PdfReader pdfReader = new PdfReader(newpdfStream);
				string text = PdfTextExtractor.GetTextFromPage(pdfReader, 1, new SimpleTextExtractionStrategy());
				return text;
			}
		}

		#region Lấy thông tin máy tính
		public static string GetSerialNumberDiskDrive()
		{
			string value = "";
			try
			{
				ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_DiskDrive");
				ManagementObjectCollection collection = searcher.Get();

				foreach (ManagementObject share in collection)
				{
					string name = share["Name"].ToString();
					foreach (PropertyData PC in share.Properties)
					{
						string proName = PC.Name;
						string proValue = ToString(PC.Value).Trim();
						if (proName.ToLower() == "serialnumber")
						{
							value = proValue;
							break;
						}
					}
					if (value != "")
					{
						break;
					}
				}
			}
			catch
			{
			}

			return value;
		}

		#endregion

		#endregion Methods

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
		public static void errorLog(string name, string nd, string line)
		{
			try
			{
				if (!File.Exists(Application.StartupPath + "/Error.txt"))
				{
					File.WriteAllText(Application.StartupPath + "/Error.txt", name + " " + nd + " " + DateTime.Now.ToString("dd-MM-YYYY HH:mm:ss") + " " + line + "\r\n");
				}
				else
				{
					File.AppendAllText(Application.StartupPath + "/Error.txt", name + " " + nd + " " + DateTime.Now.ToString("dd-MM-YYYY HH:mm:ss") + " " + line + "\r\n");
				}
			}
			catch
			{

			}
		}
	}
}