using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EYERemoteDeskptopApp
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection(@"Data Source=localhost;Initial Catalog=PBL4;Integrated Security=True");
        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            String username, user_password;

            username = txtTaikhoan.Text;
            user_password = txtMK.Text;

            try {
                String querry = "Select * from Account where username = '" + txtTaikhoan.Text + "'and password = '" + txtMK.Text + "'";
                SqlDataAdapter sda = new SqlDataAdapter(querry, conn);
                DataTable dtable = new DataTable();
                sda.Fill(dtable);
                if (dtable.Rows.Count > 0)
                {
                    username = txtTaikhoan.Text;
                    user_password = txtMK.Text;

                    MenuForm menu = new MenuForm();
                    menu.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid login details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTaikhoan.Clear();
                    txtMK.Clear();

                    txtTaikhoan.Focus();
                }

            }
            catch(Exception ex) { }
            finally { conn.Close(); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtTaikhoan.Clear();
            txtMK.Clear();
            txtTaikhoan.Focus();
        }
    }
}
