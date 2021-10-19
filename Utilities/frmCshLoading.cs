using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CEFA
{
    public partial class frmCshLoading : Form
    {
        public bool _IsManual=true;
        public frmCshLoading()
        {
            InitializeComponent();
        }
        public frmCshLoading(bool IsProcess)
        {
            InitializeComponent();
            label1.Text = "Đang xử lí dữ liệu.....";
        }

        private void frmCshLoading_Load(object sender, EventArgs e)
        {
            picLoad.Image = Utilities.Properties.Resources.Processing;
        }

        private void frmCshLoading_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_IsManual == true)
                e.Cancel = true;
        }
        public void CloseForm()
        {
            _IsManual = false;
            this.Close();
        }


    }
}