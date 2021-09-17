using System;
using System.Windows.Forms;
using WinForm_AIO.PreviewFileTest.PreviewPic;
using WinForm_AIO.PreviewFileTest.PreviewPPT;
using WinForm_AIO.PreviewFileTest.PlayVideo;

namespace WinForm_AIO.PreviewFileTest
{
    public partial class LearnTestForm : Form
    {
        public LearnTestForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "请选择文件";
            //fileDialog.Filter = "图像文件(*.jpg;*.jpg;*.jpeg;*.gif;*.png)|*.jpg;*.jpeg;*.gif;*.png";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fileDialog.FileName.ToString();//返回文件的完整路径
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var localFilePath = textBox1.Text;
            if (localFilePath == "")
            {
                MessageBox.Show("请选择文件！");
            }

            var fileExt = System.IO.Path.GetExtension(localFilePath).Replace(".", "").ToLower();
            switch (fileExt)
            {
                case "png":
                    new PicForm(localFilePath).Show();
                    break;
                case "jpg":
                    new PicForm(localFilePath).Show();
                    break;
                case "ppt":
                    new PPTForm(localFilePath).Show();
                    break;
                case "pptx":
                    new PPTForm(localFilePath).Show();
                    break;
                case "mp4":
                    new VideoForm(localFilePath).Show();
                    break;
                case "mov":
                    new VideoForm(localFilePath).Show();
                    break;
            }
        }
    }
}
