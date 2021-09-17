using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinForm_AIO.PreviewFileTest.PreviewPic
{
    public partial class PicForm : Form
    {
        private string filePath;
        public PicForm(string filePath)
        {
            InitializeComponent();
            this.filePath = filePath;
        }

        private void PicForm_Load(object sender, EventArgs e)
        {
            //设置全屏
            this.WindowState = FormWindowState.Maximized;
            //窗口置顶
            this.TopMost = true;
            //预览图片
            picturebox_Show(filePath);
        }

        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="filePath"></param>
        private void picturebox_Show(string filePath)
        {
            pictureBox1.Size = new Size(this.Width, this.Height);//new Size(this.Width, Convert.ToInt32(this.Width / Math.Round((double)(pic.Image.Width / pic.Image.Height), 2)));
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;//图片自适应显示
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;//图片自适应显示
            pictureBox1.Image = Image.FromFile(filePath);
        }
    }
}
