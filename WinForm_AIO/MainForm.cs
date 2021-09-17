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
    public partial class MainForm : Form
    {
        private int pageIndex = 1;
        private int pageSize = 12;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //隐藏窗口边框
            this.FormBorderStyle = FormBorderStyle.None;
            //全屏显示
            this.WindowState = FormWindowState.Maximized;
            //置顶显示
            this.TopMost = true;
            
            //开启Loading效果
            LoadingHelper.ShowLoadingScreen();
            //显示系统信息
            GetSysInfo();
            //显示视频数据
            GetVideoData(pageIndex, pageSize);
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }


        #region 实现的业务逻辑
        /// <summary>
        /// 获取系统信息
        /// </summary>
        public void GetSysInfo()
        {
            //显示系统当前时间
            Thread thread = new Thread(delegate ()
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
        /// 获取视频数据
        /// </summary>
        public void GetVideoData(int pageIndex, int pageSize)
        {
            Task.Run(() =>
             {
                 //var datas = MockData.Get(pageIndex, pageSize);
                 var datas = HttpService.GetAll(pageIndex, pageSize);
                 if (datas == null) return;

                 Invoke(new EventHandler(delegate
                 {
                     panel3.Controls.Clear();

                     #region 渲染分页数据
                     label4.Text = string.Format("共 {0} 个视频，当前第 {1} 页，共 {2} 页", datas.TotalCount, datas.PageIndex, datas.TotalPage);
                     button1.Enabled = pageIndex == 1 ? false : true;
                     button2.Enabled = pageIndex == datas.TotalPage ? false : true;
                     panel2.Visible = true;
                     #endregion

                     #region 渲染视频数据            
                     int sizewith = 400;
                     int sizeheight = 241;
                     int padding = 40;
                     int x = padding, y = padding;

                     for (var i = 0; i < datas.Data.Count; i++)
                     {
                         //视频块Panel
                         FlowLayoutPanel videoflow = new FlowLayoutPanel();
                         videoflow.Name = string.Format("Panel_{0}", datas.Data[i].VideoName);
                         videoflow.Size = new Size(sizewith, sizeheight);
                         videoflow.Location = new Point(x, y);
                         videoflow.FlowDirection = FlowDirection.TopDown;
                         //videoflow.Paint += new PaintEventHandler(panelbox_Paint);
                         videoflow.MouseDown += panelbox_MouseDown;
                         videoflow.MouseUp += panelbox_MouseUp;
                         videoflow.Tag = datas.Data[i].ID;
                         x += sizewith + padding;
                         if (x + sizewith > panel3.Width)
                         {
                             x = padding;
                             y += sizeheight + padding;
                         }
                         //视频封面
                         PictureBox picbox = new PictureBox();
                         picbox.Name = string.Format("Panel_PicBox_{0}", datas.Data[i].VideoName);
                         picbox.Load(datas.Data[i].Image);
                         picbox.Size = new Size(sizewith - padding * 2, Convert.ToInt32((sizewith - padding * 2) / 1.77));//图片大小：1920*1080；1.77=1920/1080
                         picbox.Margin = new Padding(padding, 0, padding, 0);
                         picbox.BackgroundImageLayout = ImageLayout.Stretch;//图片自适应显示
                         picbox.SizeMode = PictureBoxSizeMode.StretchImage;//图片自适应显示
                         picbox.MouseDown += picbox_MouseDown;
                         picbox.MouseUp += picbox_MouseUp;
                         picbox.Tag = datas.Data[i].ID;
                         //视频名称
                         Label label = new Label();
                         label.Name = string.Format("Panel_Label_{0}", datas.Data[i].VideoName);
                         label.Text = datas.Data[i].VideoName;
                         label.Font = new Font("黑体", 16, FontStyle.Bold);
                         label.Size = new Size(sizewith - padding * 2, 30);
                         label.Margin = new Padding(padding, 0, padding, 0);
                         label.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
                         label.TextAlign = ContentAlignment.MiddleCenter;
                         label.MouseDown += label_MouseDown;
                         label.MouseUp += label_MouseUp;
                         label.Tag = datas.Data[i].ID;
                         //上传时间
                         Label tLabel = new Label();
                         tLabel.Text = datas.Data[i].CreateTime;
                         tLabel.Font = new Font("黑体", 14, FontStyle.Regular);
                         tLabel.ForeColor = Color.FromArgb(178, 178, 178);
                         tLabel.Size = new Size(sizewith - padding * 2, 30);
                         tLabel.Margin = new Padding(padding, 0, padding, 0);
                         tLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
                         tLabel.TextAlign = ContentAlignment.MiddleCenter;

                         videoflow.Controls.Add(picbox);
                         videoflow.Controls.Add(label);
                         videoflow.Controls.Add(tLabel);
                         panel3.Controls.Add(videoflow);
                     }
                     #endregion
                 }));

                 //关闭Loading效果
                 LoadingHelper.CloseForm();
             });
        }
        #endregion

        #region 控件实现的事件
        /** Button  */
        private void button1_Click(object sender, EventArgs e)
        {
            pageIndex -= 1;
            GetVideoData(pageIndex, pageSize);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            pageIndex += 1;
            GetVideoData(pageIndex, pageSize);
        }
        /** FlowLayoutPanel  */
        public void panelbox_MouseDown(object sender, EventArgs e)
        {
            FlowLayoutPanel flp = sender as FlowLayoutPanel;
            if (flp != null)
            {
                //Panel
                flp.BackColor = Color.FromArgb(214, 234, 248);
                flp.Cursor = Cursors.Hand;
                //PictureBox
                PictureBox pb = flp.Controls[0] as PictureBox;
                pb.SizeMode = PictureBoxSizeMode.Zoom;
                //Label
                Label lb = flp.Controls[1] as Label;
                lb.ForeColor = Color.FromArgb(255, 255, 255);
            }
        }
        public void panelbox_MouseUp(object sender, EventArgs e)
        {
            FlowLayoutPanel flp = sender as FlowLayoutPanel;
            if (flp != null)
            {
                flp.BackColor = Control.DefaultBackColor;
                flp.Cursor = Cursors.Default;

                PictureBox pb = flp.Controls[0] as PictureBox;
                pb.SizeMode = PictureBoxSizeMode.StretchImage;

                Label lb = flp.Controls[1] as Label;
                lb.ForeColor = Color.FromArgb(0, 0, 0);

                ShowNewForm(flp.Tag.ToString());
            }
        }
        /** PictureBox  */
        public void picbox_MouseDown(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            if (pb != null)
            {
                pb.SizeMode = PictureBoxSizeMode.Zoom;

                pb.Parent.BackColor = Color.FromArgb(214, 234, 248);
                pb.Parent.Cursor = Cursors.Hand;

                pb.Parent.Controls[1].ForeColor = Color.FromArgb(255, 255, 255);
            }
        }
        public void picbox_MouseUp(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            if (pb != null)
            {
                pb.SizeMode = PictureBoxSizeMode.StretchImage;

                pb.Parent.BackColor = Control.DefaultBackColor;
                pb.Parent.Cursor = Cursors.Default;

                pb.Parent.Controls[1].ForeColor = Color.FromArgb(0, 0, 0);

                ShowNewForm(pb.Tag.ToString());
            }
        }
        /** Label  */
        public void label_MouseDown(object sender, EventArgs e)
        {
            Label lb = sender as Label;
            if (lb != null)
            {
                lb.ForeColor = Color.FromArgb(255, 255, 255);

                lb.Parent.BackColor = Color.FromArgb(214, 234, 248);
                lb.Parent.Cursor = Cursors.Hand;

                var pb = (PictureBox)lb.Parent.Controls[0];
                pb.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
        public void label_MouseUp(object sender, EventArgs e)
        {
            Label lb = sender as Label;
            if (lb != null)
            {
                lb.ForeColor = Color.FromArgb(0, 0, 0);

                lb.Parent.BackColor = Control.DefaultBackColor;
                lb.Parent.Cursor = Cursors.Default;

                var pb = (PictureBox)lb.Parent.Controls[0];
                pb.SizeMode = PictureBoxSizeMode.StretchImage;

                ShowNewForm(lb.Tag.ToString());
            }
        }
        public void panelbox_Paint(object sender, PaintEventArgs e)
        {
            //(sender as Panel).BackColor = Color.FromArgb(65, 204, 212, 230);
            ControlPaint.DrawBorder(e.Graphics, (sender as Panel).ClientRectangle,
                Color.Red, 1, ButtonBorderStyle.Solid,//左边
                Color.Red, 1, ButtonBorderStyle.Solid,//上边
                Color.Red, 1, ButtonBorderStyle.Solid,//右边
                Color.Red, 1, ButtonBorderStyle.Solid);//底边
        }

        /// <summary>
        /// 最小化当前窗口，最大化详情窗口
        /// </summary>
        /// <param name="selectedId"></param>
        private void ShowNewForm(string selectedId)
        {
            this.WindowState = FormWindowState.Minimized;
            this.TopMost = false;

            Form form2 = new InfoForm(this,selectedId);
            form2.FormBorderStyle = FormBorderStyle.None;
            form2.WindowState = FormWindowState.Maximized;
            form2.TopMost = true;
            form2.Show();
        }
        #endregion
    }
}
