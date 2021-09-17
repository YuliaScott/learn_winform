using System;
using System.Windows.Forms;
using WinForm_AIO.PreviewFileTest;

namespace WinForm_AIO
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LearnTestForm());
        }
    }
}
