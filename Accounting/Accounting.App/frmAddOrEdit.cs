using Accounting.DataLayer;
using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValidationComponents;

namespace Accounting.App
{
    public partial class frmAddOrEdit : Form
    {
        //UnitOfWork db = new UnitOfWork();
        public int customerId = 0;
        public frmAddOrEdit()
        {
            InitializeComponent();
        }
        private void frmAddOrEdit_Load(object sender, EventArgs e)
        {
            if (customerId != 0)
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    this.Text = "Edit Customer";
                    btnSave.Text = "Edit";
                    var customer = db.CustomerRepository.GetCustomerById(customerId);
                    txtName.Text = customer.FullName;
                    txtMobile.Text = customer.Mobile;
                    txtEmail.Text = customer.Email;
                    txtAddress.Text = customer.Address;
                    picCustomer.ImageLocation = Application.StartupPath + "/Images/" + customer.CustomerImage;
                }
            }
        }

        private void btnSelectPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
                picCustomer.ImageLocation = openFile.FileName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                string imageName = Guid.NewGuid().ToString() +
                    Path.GetExtension(picCustomer.ImageLocation);
                string path = Application.StartupPath + "/Images/";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                picCustomer.Image.Save(path + imageName);

                Customer customer = new Customer()
                {
                    FullName = txtName.Text,
                    Mobile = txtMobile.Text,
                    Address = txtAddress.Text,
                    Email = txtEmail.Text,
                    CustomerImage = imageName
                };
                using (UnitOfWork db = new UnitOfWork())
                {
                    if (customerId == 0)
                    {
                        db.CustomerRepository.InsertCustomer(customer);
                    }
                    else
                    {
                        customer.CustomerID = customerId;
                        db.CustomerRepository.UpdateCustomer(customer);
                    }
                    db.Save();
                }
                DialogResult = DialogResult.OK;

            }
        }
    }
}
