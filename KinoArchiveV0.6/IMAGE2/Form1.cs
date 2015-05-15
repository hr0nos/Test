using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IMAGE2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Form_load
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "admin";
            textBox2.Text = "0000";
        }
        #endregion

        #region button_enter
        private void button3_Click_1(object sender, EventArgs e)
        {
            check_admin();
        }
        #endregion

        #region check_enter_on_textbox2
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                check_admin();
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                Close();
            }
        }
        #endregion

        #region check_admin_func
        private void check_admin()
        {
            if (textBox1.Text == "admin" && textBox2.Text == "0000")
            {
                MessageBox.Show("Здравствуйте админ! :)");
                this.Hide();
                Form3 f3 = new Form3();
                f3.ShowDialog();
                this.Close();
            }
            else if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Заполните пустые поля!");
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }
        #endregion

        #region check_enter_on_textbox1
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                check_admin();
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                Close();
            }
        }
        #endregion

        #region button_close
        private void button2_Click(object sender, EventArgs e)
        {
            Close();


        }
        #endregion

        #region link_event
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Вы вошли в систему как пользователь!");
            this.Hide();
            Form_User fu = new Form_User();
            fu.ShowDialog();
            this.Close();
        }
        #endregion

    }
}