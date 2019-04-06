using CefSharp;
using HisBrowser;
using System.Windows.Forms;

namespace HisBorwser
{
    internal class DownloadHandler : IDownloadHandler
    {


        public void OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    browser.CloseBrowser(false);//关闭原来的窗口
                    callback.Continue(@"D:\Users\" +
                            System.Security.Principal.WindowsIdentity.GetCurrent().Name +
                            @"\Downloads\" +
                            downloadItem.SuggestedFileName,
                        showDialog: true);
                }
                
            }


        }

        public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
           
        }

        public bool OnDownloadUpdated(DownloadItem downloadItem)
        {

            return false;
        }
    }
}