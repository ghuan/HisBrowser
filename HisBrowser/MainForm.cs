using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Reflection;
using System;
using System.Runtime.InteropServices;
using HisBorwser;


namespace HisBrowser
{
    public partial class MainForm : Form
    {
        public static MainForm Instance;//外部访问此form

        public MainForm()
        {
            Instance = this;
            InitBrowser();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            InitializeComponent();

        }
       
        //js 调用的c#方法放在这里
        public class CallDll
        {
            public string getCookie()
            {
               
                return "";
            }
            public void setCookieAsync(string domain, string name, string value)
            {


            }
           
        }
        public ChromiumWebBrowser browser;

        public void InitBrowser()
        {
            
            CefSettings settings = new CefSettings();

            settings.IgnoreCertificateErrors = true;
            settings.CachePath = GetAppDir("Cache");//缓存地址
            
            Cef.Initialize(settings);
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;//允许JavaScript

           // cookieManager = CefSharp.Cef.GetGlobalCookieManager();//获取全局cookie管理器
            //cookie 管理：https://blog.csdn.net/zhuhongshu/article/details/81485730
            
            string initUrl = OperateIniFile.ReadIniData("default", "url", "", System.Environment.CurrentDirectory + "\\hisBrowser.ini");
            browser = new ChromiumWebBrowser(initUrl);
            //前台通过cbrowser 调用 c# 方法
            browser.RegisterJsObject("cbrowser", new CallDll(), BindingOptions.DefaultBinder);
            browser.Dock = DockStyle.Fill;

            browser.KeyboardHandler = new KeyboardHandler(this);
            browser.LoadingStateChanged += Browser_LoadingStateChanged;
            browser.DownloadHandler = new DownloadHandler();//添加下载功能

            this.Controls.Add(browser);

        }

        
        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            try { this.Focus(); } catch (Exception ex) {
                
            }
            
        }
        //数据存放地址
        private static string GetAppDir(string name)
        {
            
            return System.Environment.CurrentDirectory + "\\Application Data";
        }

        /**
         * 热键
         * */
        private void InitHotkeys()
        {

            // browser hotkeys
           
            KeyboardHandler.AddHotKey(this, OpenDeveloperTools, Keys.F12);
            KeyboardHandler.AddHotKey(this, RefreshActiveTab, Keys.F5);


        }
        //打开开发工具
        public void OpenDeveloperTools() {
            // browser.down
           
            browser.ShowDevTools();
        }
       
        //刷新
        public void RefreshActiveTab()
        {
            browser.Load(browser.Address);
        }
        private void MainForm_Load(object sender, EventArgs e)
        {

            InitHotkeys();

        }
    }
}


internal class SharpHotKey
{

    public Keys Key;
    public int KeyCode;
    public bool Ctrl;
    public bool Shift;
    public bool Alt;

    public Action Callback;

    public SharpHotKey(Action callback, Keys key, bool ctrl = false, bool shift = false, bool alt = false)
    {
        Callback = callback;
        Key = key;
        KeyCode = (int)key;
        Ctrl = ctrl;
        Shift = shift;
        Alt = alt;
    }

}