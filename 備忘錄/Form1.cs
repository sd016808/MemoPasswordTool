using Newtonsoft.Json;
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

namespace 備忘錄
{
    public partial class Form1 : Form
    {
        ViewModelController _controller = new ViewModelController();
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _controller = new ViewModelController();
            dataGridView1.DataSource = _controller.ViewModelList;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.AllowUserToResizeColumns = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = false;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Open File";
                // Set filter for file extension and default file extension
                openFileDialog.Filter = "Harris Dai|*.harrisDai";
                var result = openFileDialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(openFileDialog.FileName))
                {
                    string contentBeforeDecrypt = File.ReadAllText(openFileDialog.FileName);
                    TypePasswordBox box = new TypePasswordBox("請輸入軟體的解密密碼");
                    result = box.ShowDialog();
                    if (result != DialogResult.OK || string.IsNullOrWhiteSpace(box.textBox1.Text))
                        return;

                    string contentAfterDecrypt = AES.aesDecryptBase64(contentBeforeDecrypt, box.textBox1.Text);
                    var controller = JsonConvert.DeserializeObject<ViewModelController>(contentAfterDecrypt);
                    if (controller == null)
                        throw new Exception("密碼錯誤");

                    box = new TypePasswordBox("請輸入檔案的解密密碼");
                    result = box.ShowDialog();
                    if (result != DialogResult.OK || string.IsNullOrWhiteSpace(box.textBox1.Text))
                        return;

                    if (!controller.Password.Equals(box.textBox1.Text))
                        throw new Exception("密碼錯誤");

                    _controller = controller;
                    dataGridView1.DataSource = _controller.ViewModelList;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (_controller == null || !_controller.ViewModelList.Any())
                    return;

                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Harris Dai|*.harrisDai"; ;
                dlg.Title = "Save File";
                // If the file name is not an empty string open it for saving.
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK && dlg.FileName != "")
                {
                    TypePasswordBox box = new TypePasswordBox("請輸入檔案的加密密碼");
                    var result = box.ShowDialog();
                    if (result != DialogResult.OK || string.IsNullOrWhiteSpace(box.textBox1.Text))
                        return;

                    _controller.Password = box.textBox1.Text;
                    var contentBeforeEncrypt = JsonConvert.SerializeObject(_controller);
                    if (contentBeforeEncrypt == null)
                        return;

                    box = new TypePasswordBox("請輸入軟體的加密密碼");
                    result = box.ShowDialog();
                    if (result != DialogResult.OK || string.IsNullOrWhiteSpace(box.textBox1.Text))
                        return;

                    string contentAfterEncrypt = AES.aesEncryptBase64(contentBeforeEncrypt, box.textBox1.Text);
                    File.WriteAllText(dlg.FileName, contentAfterEncrypt);
                }
            }
            catch
            {

            }
        }
    }
}
