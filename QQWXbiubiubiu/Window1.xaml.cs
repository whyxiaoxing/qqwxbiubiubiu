using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using 测试;

namespace test
{

    public partial class APPmessage : Window
    {
        public APPmessage()
        {
            InitializeComponent();
        }


        private void 事件处理1(object sender, RoutedEventArgs e)
        {

            
            string url = "xiaoxing.cosplaywhy.us.kg/main";
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
            String s = key.GetValue("").ToString();
            String browserpath = null;
            if (s.StartsWith("\""))
            {
                browserpath = s.Substring(1, s.IndexOf('\"', 1) - 1);
            }
            else
            {
                browserpath = s.Substring(0, s.IndexOf(" "));
            }

            System.Diagnostics.Process.Start(browserpath, url);
            日志系统.I_Info("用户打开了网站");

        }
        private void 事件处理2(object sender, RoutedEventArgs e)
        {

            
            string url = "https://github.com/whyxiaoxing/qqwxbiubiubiu";
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
            String s = key.GetValue("").ToString();
            String browserpath = null;
            if (s.StartsWith("\""))
            {
                browserpath = s.Substring(1, s.IndexOf('\"', 1) - 1);
            }
            else
            {
                browserpath = s.Substring(0, s.IndexOf(" "));
            }

            System.Diagnostics.Process.Start(browserpath, url);
            日志系统.I_Info("用户打开了网站");

        }

      
    }
}
