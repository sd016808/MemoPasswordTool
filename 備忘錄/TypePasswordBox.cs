using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 備忘錄
{
    public partial class TypePasswordBox : Form
    {
        public TypePasswordBox(string text)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void TypePasswordBox_Shown(object sender, EventArgs e)
        {
            this.textBox1.Focus();
        }
    }
}
