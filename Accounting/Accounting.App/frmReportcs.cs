using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting.Utility;
using Accounting.ViewModels;
namespace Accounting.App
{
    public partial class frmReportcs : Form
    {

        public int Type = 0;
        public frmReportcs()
        {
            InitializeComponent();
        }

        private void frmReportcs_Load(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                List<ListCustomerViewModel> list = new List<ListCustomerViewModel>();
                list.Add(new ListCustomerViewModel()
                {
                    CustomerID = 0,
                    FullName = "Select...!"
                });
                list.AddRange(db.CustomerRepository.GetCustomerNames());
                cbCustomer.DataSource = list;
                cbCustomer.DisplayMember = "FullName";
                cbCustomer.ValueMember = "CustomerID";
            }
            if (Type == 1)
                this.Text = "Income Report";
            else
                this.Text = "Outcome Report";
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            Filter();
        }

        void Filter()
        {
            dgReport.Rows.Clear();
            using (UnitOfWork db = new UnitOfWork())
            {
                List<DataLayer.Accounting> result = new List<DataLayer.Accounting>();


                if ((int)cbCustomer.SelectedValue != 0)
                {
                    int customerId = int.Parse(cbCustomer.SelectedValue.ToString());
                    result.AddRange(db.AccountingRepository.Get(c => c.TypeID == Type && c.CustomerID == customerId));

                }
                else
                    result.AddRange(db.AccountingRepository.Get(c => c.TypeID == Type));

                DateTime? startDate;
                DateTime? endDate;

                if (txtFromDate.Text != " /  / ")
                {
                    startDate = Convert.ToDateTime(txtFromDate.Text);
                    result = result.Where(c => c.DateTime >= startDate.Value).ToList();
                }
                if (txtToDate.Text != "  /  /")
                {
                    endDate = Convert.ToDateTime(txtToDate.Text);
                    result = result.Where(c => c.DateTime <= endDate.Value).ToList();
                }

                foreach (var accounting in result)
                {
                    string customerName = db.CustomerRepository.GetCustomerNameById(accounting.CustomerID);
                    dgReport.Rows.Add(accounting.ID, customerName, accounting.Amount, accounting.DateTime.ToShamsi(), accounting.Discription);
                }

            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Filter();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgReport.CurrentRow != null)
            {
                int id = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                if (MessageBox.Show("Are sure to Delete?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        db.AccountingRepository.Delete(id);
                        db.Save();
                        Filter();
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgReport.CurrentRow != null)
            {
                int id = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                frmNewTransaction frmNewTransaction = new frmNewTransaction();
                frmNewTransaction.AccountID = id;
                if (frmNewTransaction.ShowDialog() == DialogResult.OK)
                    Filter();
            }
        }
    }
}
