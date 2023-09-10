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
using Accounting.DataLayer.Context;

namespace Accounting.App
{
    public partial class frmLogin : Form
    {
        public bool IsEdit = false;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    if (IsEdit)
                    {
                        var login = db.LoginRepository.Get(c => c.UserName == txtUserName.Text).First();
                        login.UserName = txtUserName.Text;
                        login.Password = txtPassword.Text;
                        db.LoginRepository.Upadate(login);
                        db.Save();
                        Application.Restart();

                    }
                    else
                    {
                        if (db.LoginRepository.Get(c => c.UserName == txtUserName.Text && c.Password == txtPassword.Text).Any())
                            DialogResult = DialogResult.OK;
                        else
                            MessageBox.Show("Your User or Password is Not Valid!");
                    }
                }
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (IsEdit)
            {
                this.Text = "Setting For Login";
                btnLogin.Text = "Save Change";
                using (UnitOfWork db = new UnitOfWork())
                {
                    var login = db.LoginRepository.Get(c => c.UserName == txtUserName.Text).First();
                    txtUserName.Text = login.UserName;
                    txtPassword.Text = login.Password;
                }
            }
        }
    }
}
