using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows;
using 测试;

namespace test
{
    public class UserConfig
    {
        
        public string usersend_num { get; set; }
        public string userkill_obj { get; set; }
        public string userlog_site { get; set; }
        public string userJosn_site { get; set; }
        public string user_mouse_click {  get; set; }
        public string user_INTPAT { get; set; }
    }

    public partial class App : System.Windows.Application
    {
        private Mutex 单例检测;
        protected override void OnStartup(StartupEventArgs e)
        {
            bool _IF开启;
            单例检测 = new Mutex(true, "QQWX自动轰炸", out _IF开启);
            if (!_IF开启){

                System.Windows.MessageBox.Show("请勿多开本程序", "已有程序正在运行", MessageBoxButton.OKCancel);
                System.Environment.Exit(0);
               
            }
            ShowSplashScreenAsync();
            base.OnStartup(e);
        }

        private async Task ShowSplashScreenAsync()
        {
            
            var splash = new SplashScreen(".\\pic\\3.png");
            splash.Show(false, true);

            var mainWindow = new MainWindow
            {
                DataContext = 公共数据.MVVM统一接口.m
            };

            this.MainWindow = mainWindow;

           

            // 配置文件路径
            string configPath = ".\\userconfig.json";

            string userLogSite = "";
            string userJsonSite = "";
            int 轰炸次数 = 0;
            string userKillObj = "";
            string usermouse = "";
            string user_INTPAT = "";

            try
            {
                if (!File.Exists(configPath))
                    throw new Exception($"初始化失败：文件 \"{configPath}\" 不存在");

                using var fs = File.OpenRead(configPath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var config = JsonSerializer.Deserialize<UserConfig>(fs, options);

                // 如果配置为空则使用默认值
                userLogSite = System.IO.Path.GetFullPath(config?.userlog_site ?? userLogSite);
                userJsonSite = System.IO.Path.GetFullPath(config?.userJosn_site ?? userJsonSite);
                usermouse =config?.user_mouse_click ?? usermouse;
                轰炸次数 = int.TryParse(config?.usersend_num, out var v) ? v : 0;
                userKillObj = config?.userkill_obj ?? string.Empty;
                user_INTPAT = config?.user_INTPAT ?? string.Empty;
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    System.Windows.MessageBox.Show($"启动程序失败，请查看错误日志：{ex}", "初始化错误", MessageBoxButton.OKCancel);
                    MainWindow?.Close();
                });
                return;
            }

            // 初始化日志系统
            Task.Run(() => 日志系统.初始化(userLogSite));
            日志系统.I_Info("程序开始运行");
            日志系统.I_Info("已经完成日志系统加载");
            日志系统.I_Info("尝试初始化用户数据");

            // 绑定配置到 ViewModel 和公共数据
            公共数据.MVVM统一接口.m.好友名称 = userKillObj;
            公共数据.MVVM统一接口.m.消息次数 = 轰炸次数;
            公共数据.MVVM统一接口.m.用户选择 = usermouse.ToString();
            公共数据.MVVM统一接口.m.用户选择2 = user_INTPAT.ToString();
            公共数据.日志文件位置 = userLogSite;
            公共数据.json文件路径 = userJsonSite;


            // 输出调试日志
            日志系统.I_Info($"读取到的数据：Log={userLogSite}, JSON={userJsonSite}, 次数={轰炸次数}, 对象={userKillObj}");

            // 显示主窗口
            await Dispatcher.BeginInvoke(new Action(() =>
            {
                
                splash.Close(TimeSpan.FromMilliseconds(250));
                mainWindow.Show();
            }));
        }

}

}

