using System;
using System.Configuration;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForm_AIO.Common;
using WinForm_AIO.Services;

namespace WinForm_AIO
{
    public partial class InfoForm : Form
    {
        private Form form1;
        private string videoId;
        private Thread thread;
        private int playwidth = 174, playheight = 174; //视频播放图片宽度和高度

        public InfoForm(Form form1,string id)
        {
            InitializeComponent();
            this.form1 = form1;
            this.videoId = id;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //显示系统信息
            GetSysInfo();
            //显示视频详情
            GetThisVideo(videoId);
        }

        #region 实现的业务逻辑
        /// <summary>
        /// 获取系统信息
        /// </summary>
        public void GetSysInfo()
        {
            //显示系统当前时间
            thread = new Thread(delegate ()
            {
                while (true)
                {
                    //Invoke()的作用是在应用程序的主线程执行指定的委托。也可以在辅助线程 中修改UI线程 （或主线程）对象的属性
                    Invoke(new EventHandler(delegate
                    {
                        label2.Text = Common.WeekHelper.GetWeekOfDate() + " " + DateTime.Now.ToLongTimeString();
                    }));
                    Thread.Sleep(500);
                }
            });
            thread.IsBackground = true;//设置为后台线程
            thread.Start();//开启线程

            //当前系统版本
            label3.Text = ConfigurationManager.AppSettings["version"].ToString();
        }
        /// <summary>
        /// 获取视频详情
        /// </summary>
        public void GetThisVideo(string id)
        {
            Task.Run(() =>
            {
                //var data = MockData.GetThis(id);
                var data = HttpService.Get(id);
                if (data == null) return;

                Invoke(new EventHandler(delegate
                {
                    //视频封面
                    pictureBox2.Load(data.Image);
                    pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;//图片自适应显示
                    pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;//图片自适应显示
                    pictureBox2.Tag = data.Path;
                    pictureBox2.BackColor = Color.Transparent;
                    pictureBox2.MouseUp += picbox_MouseUp;
                    //视频播放按钮
                    PictureBox btnPlay = new PictureBox();
                    btnPlay.Image = Image.FromFile(@"Images/play.png");
                    btnPlay.Size = new Size(playwidth, playheight);
                    var locationX = (pictureBox2.Width / 2) - (playwidth / 2);
                    var locationY = (pictureBox2.Height / 2) - (playheight / 2);
                    btnPlay.Location = new Point(locationX, locationY);
                    btnPlay.Tag = data.Path;
                    btnPlay.MouseUp += btnPlay_MouseUp;
                    pictureBox2.Controls.Add(btnPlay);
                    //视频名称
                    label5.Text = data.VideoName;
                    //上传时间
                    label6.Text = data.CreateTime;
                    //二维码
                    var qrCodeImg = QrCodeHelper.CreateQrCode(data.CodePath, pictureBox3.Width, pictureBox3.Height);
                    pictureBox3.Image = qrCodeImg;
                }));
            });
        }
        #endregion

        #region 控件实现的事件
        private void picbox_MouseUp(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            if (pb != null)
            {
                PictureBox btnPlay = pb.Controls[0] as PictureBox;
                btnPlay.Visible = false;
                PlayVideo(pb.Tag.ToString(), pb);
            }
        }
        private void btnPlay_MouseUp(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            if (pb != null)
            {
                pb.Visible = false;
                PictureBox pbParent = pb.Parent as PictureBox;
                PlayVideo(pb.Tag.ToString(), pbParent);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            thread.Abort();
            this.Close();

            form1.WindowState = FormWindowState.Maximized;
            form1.TopMost = true;
        }
        private void PlayVideo(string url, PictureBox pb)
        {
            AxWMPLib.AxWindowsMediaPlayer axWMP = new AxWMPLib.AxWindowsMediaPlayer();
            #region 初始化对象
            /**
             * 引用第三方控件，除了实例化，还要初始化该对象，否则报错
             * 引发类型为“System.Windows.Forms.AxHost+InvalidActiveXStateException”的异常。
             */
            ((System.ComponentModel.ISupportInitialize)(axWMP)).BeginInit();
            pb.Controls.Add(axWMP);
            ((System.ComponentModel.ISupportInitialize)(axWMP)).EndInit();
            #endregion
            axWMP.Width = pb.Width;
            axWMP.Height = pb.Height;
            axWMP.URL = url;
            //axWMP.StatusChange += windowsMediaPlay_StatusChange;
        }
        private void windowsMediaPlay_StatusChange(object sender, EventArgs e)
        {
            /* 
             * [Winform]Media Player组件全屏播放的设置: https://www.cnblogs.com/wolf-sun/p/7054473.html
             * 
             * 0 Undefined Windows Media Player is in an undefined state.(未定义) 
               1 Stopped Playback of the current media item is stopped.(停止) 
               2 Paused Playback of the current media item is paused. When a media item is paused, resuming playback begins from the same location.(停留) 
               3 Playing The current media item is playing.(播放) 
               4 ScanForward The current media item is fast forwarding. 
               5 ScanReverse The current media item is fast rewinding. 
               6 Buffering The current media item is getting additional data from the server.(转换) 
               7 Waiting Connection is established, but the server is not sending data. Waiting for session to begin.(暂停) 
               8 MediaEnded Media item has completed playback. (播放结束) 
               9 Transitioning Preparing new media item. 
               10 Ready Ready to begin playing.(准备就绪) 
               11 Reconnecting Reconnecting to stream.(重新连接) 
           */
            AxWMPLib.AxWindowsMediaPlayer axWMP = sender as AxWMPLib.AxWindowsMediaPlayer;
            if (axWMP.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWMP.fullScreen = true;
            }
            else if (axWMP.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                Thread.Sleep(1000);
                axWMP.Ctlcontrols.play();
            }
        }
        #endregion
    }
}
