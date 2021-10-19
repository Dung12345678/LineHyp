using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BMS
{
	public partial class WarningForm : _Forms
	{
		public WarningForm()
		{
			InitializeComponent();
		}

		private void WarningForm_Load(object sender, EventArgs e)
		{
			textBox1.Focus();
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
	}
}
