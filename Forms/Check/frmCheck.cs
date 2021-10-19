using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static BMS.frmProductCheckHistory1;

namespace BMS
{
	public partial class frmCheck : _Forms
	{
		string pathConfigFitRow = Application.StartupPath + "/ConfigFitRow.txt";
		string pathConfigFat = Application.StartupPath + "/ConfigFat.txt";
		public CheckAll _CheckAll;
		public frmCheck()
		{
			InitializeComponent();
		}

		private void WarningForm_Load(object sender, EventArgs e)
		{
		
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Dispose();
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				btnClose_Click(null, null);
			}
		}

		private void chkFitRow_CheckedChanged(object sender, EventArgs e)
		{
			if (chkFitRow.Checked)
			{
				_CheckAll(true, "Fit");
			}
			File.WriteAllText(pathConfigFitRow, chkFitRow.Checked ? "1" : "0");
		}

		private void chkFat_CheckedChanged(object sender, EventArgs e)
		{
			if (!chkFat.Checked)
			{
				_CheckAll(false, "Fat");
			}
			File.WriteAllText(pathConfigFat, chkFat.Checked ? "1" : "0");
		}
	}
}
