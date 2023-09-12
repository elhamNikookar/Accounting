using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting.Business;
using Accounting.Utility;
using Accounting.ViewModels.Accounting;

namespace Accounting.App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            frmCustomers frmCustomers = new frmCustomers();
            frmCustomers.ShowDialog();
        }

        private void btnNewTransaction_Click(object sender, EventArgs e)
        {
            frmNewTransaction frmNewTransact = new frmNewTransaction();
            frmNewTransact.ShowDialog();
        }

        private void btnReportIncome_Click(object sender, EventArgs e)
        {
            frmReportcs frmReportcs = new frmReportcs();
            frmReportcs.Type = 1;
            frmReportcs.ShowDialog();
        }

        private void btnReportOutcome_Click(object sender, EventArgs e)
        {
            frmReportcs frmReportcs = new frmReportcs();
            frmReportcs.Type = 2;
            frmReportcs.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.ToShamsi();
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
            this.Hide();
            frmLogin frmLogin = new frmLogin();
            if(frmLogin.ShowDialog() == DialogResult.OK)
            {
                lblDate.Text = DateTime.Now.ToShamsi();
                lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
                this.Show();
                Report();
            }
            else
                Application.Exit();
        }

        void Report()
        {
            ReportViewModel report = Account.ReportForMain();
            lblIncome.Text = report.Recive.ToString("#,0");
            lblOutcome.Text = report.Pay.ToString("#,0");
            lblBenefit.Text = report.AccountBalance.ToString("#,0");
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void btnLoginEdit_Click(object sender, EventArgs e)
        {
            /*frmLogin frmLogin = new frmLogin(); 
            frmLogin.IsEdit = true;
            frmLogin.ShowDialog();
            */
        }
    }
}
