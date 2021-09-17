1. 这是用于学习WindowForm的示例，分为两个部分：第一部分是展示数据列表和查看详情，第二部分是预览不同格式的文件(目前支持png、jpg、ppt、pptx、mp4、mov格式)。

   

2. 若演示第一、二部分，需更改根目录下Program.cs文件，Main函数中改为new MainForm()运行第一部分；改为new LearnFileTest()运行第二部分。

   

3. 第一部分的详细功能：
   * 1）右侧顶部显示系统当前时间；
   * 2）视频列表UI样式；
   * 3）开发HttpClient，创建新线程获取数据；
   * 4）支持分页显示数据(上一页、下一页)；
   * 5）动态添加控件；
   * 6）PictureBox控件加载网络图片(URL)；
   * 7）点击某个视频的动态效果；
   * 8）点击某个视频进入详情页面，并传递参数；
   * 9）详情页视频封面显示播放图标，始终垂直居中于不同大小的视频封面；
   * 10）点击播放图标，使用Window Media Player控件播放视频；
   * 11）生成二维码；



4. 第二部分的详细功能：
   * 1）点击选择文件按钮，打开选择本地文件窗口；
   * 2）点击预览按钮，全屏预览不同格式的文件：
      * ppt/pptx：加载Microsoft Office组件预览；
      * mp4/mov：加载VLC播放器播放；
   * 3）关闭当前预览窗口，快捷键Alt+F4



5. 加载Microsoft Office组件预览注意点：

   * 1）nuget两个office dll：
      * Microsoft.Office.Interop.PowerPoint.15.0.4420.1017
      * MicrosoftOfficeCore.15.0.0

   * 2）安装官方office软件并激活(推荐安装官方软件，否则程序无法调用本机注册表中的office相关配置)：
     * cn_office_professional_plus_2013_with_sp1_x64_dvd_3921920.iso


