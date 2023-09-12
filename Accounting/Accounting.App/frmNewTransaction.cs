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
using ValidationComponents;

namespace Accounting.App
{
    public partial class frmNewTransaction : Form
    {
        UnitOfWork db;
        public int AccountID = 0;
        public frmNewTransaction()
        {
            InitializeComponent();
        }

        private void frmNewTransaction_Load(object sender, EventArgs e)
        {
            db = new UnitOfWork();
            dgCustomers.AutoGenerateColumns = false;
            dgCustomers.DataSource = db.CustomerRepository.GetCustomerNames();
            if (AccountID != 0)
            {
                var account = db.AccountingRepository.GetById(AccountID);
                txtAmount.Text = account.Amount.ToString();
                txtDescription.Text = account.Discription;
                txtName.Text = db.CustomerRepository.GetCustomerNameById(account.CustomerID);
                if (account.TypeID == 1)
                    rdIncome.Checked = true;
                else
                    rdOutcome.Checked = true;
                this.Text = "Edit";
                btnSave.Text = "Edit";
            }
            db.Dispose();
        }

        private void dgCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = dgCustomers.CurrentRow.Cells[0].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            db = new UnitOfWork();
            if (BaseValidator.IsFormValid(this.components))
            {
                if (rdIncome.Checked || rdOutcome.Checked)
                {
                    DataLayer.Accounting accounting = new DataLayer.Accounting()
                    {
                        Amount = int.Parse(txtAmount.Value.ToString()),
                        CustomerID = db.CustomerRepository.GetCustomerIdByName(txtName.Text),
                        TypeID = (rdIncome.Checked) ? 1 : 2,
                        Discription = txtDescription.Text,
                        DateTime = DateTime.Now,
                    };
                    if (AccountID == 0)
                        db.AccountingRepository.Insert(accounting);
                    else
                    {
                            accounting.ID = AccountID;
                            db.AccountingRepository.Upadate(accounting);
 
                    }

                    db.Save();
                    db.Dispose();
                    DialogResult = DialogResult.OK;
                }
                else
                    MessageBox.Show("Select Income Or Outcome!");

            }
        }
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            dgCustomers.AutoGenerateColumns = false;
            dgCustomers.DataSource = db.CustomerRepository.GetCustomerNames(txtFilter.Text);
        }
    }
}
