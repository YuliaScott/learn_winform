using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinForm_AIO.PreviewFileTest.PlayVideo
{
    /// <summary>
    /// 调用VLC播放器播放视频 参考链接：https://www.bbsmax.com/A/ZOJPw0jyzv/
    /// </summary>
    public partial class VideoForm : Form
    {
        private string filePath;
        public VideoForm(string filePath)
        {
            InitializeComponent();
            this.filePath = filePath;
        }

        private void VideoForm_Load(object sender, EventArgs e)
        {
            //设置全屏
            this.WindowState = FormWindowState.Maximized;
            //窗口置顶
            this.TopMost = true;
            //设置全屏
            pictureBox1.Size = new Size(this.Width, this.Height);
            //播放视频
            video_Show(filePath);
        }

        /// <summary>
        /// VLC播放器播放视频
        /// </summary>
        /// <param name="filePath"></param>
        private void video_Show(string filePath)
        {
            VlcPlayer.VlcPlayerBase VlcPlayerBase = new VlcPlayer.VlcPlayerBase(Environment.CurrentDirectory + "\\vlc\\plugins\\");
            VlcPlayerBase.SetRenderWindow(pictureBox1.Handle.ToInt32());
            VlcPlayerBase.LoadFile(filePath);
            VlcPlayerBase.Play();
        }
    }
}
