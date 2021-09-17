using PPTDraw.PPTOperate;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinForm_AIO.PreviewFileTest.PreviewPPT
{
    public partial class PPTForm : Form
    {
        private string filePath;
        public PPTForm(string filePath)
        {
            InitializeComponent();
            this.filePath = filePath;
        }

        [DllImport("user32.dll", EntryPoint = "SetParent")]
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        private void PPTForm_Load(object sender, EventArgs e)
        {
            //设置全屏
            this.WindowState = FormWindowState.Maximized;
            //窗口置顶
            this.TopMost = true;
            //预览PPT
            var operatePPT = new OperatePPT();
            operatePPT.PPTOpen(filePath);
            //将PPT窗口嵌入到Form窗体
            var objPresSet = operatePPT.objPresSet;
            if (objPresSet == null)
            {
                operatePPT.PPTClose();
                this.Close();
            }
            else
            {
                WindowWrapper handleWrapper = new WindowWrapper((IntPtr)objPresSet.SlideShowWindow.HWND);
                SetParent(handleWrapper.Handle, this.Handle);
                objPresSet.SlideShowWindow.Top = 0;
                objPresSet.SlideShowWindow.Left = 0;
            }
        }
    }
}
