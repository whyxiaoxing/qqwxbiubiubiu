using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Serilog;
using Serilog.Events;
using 测试;


namespace test
{
    public partial class MainWindow : Window
    {
        MVVM视图逻辑 USE数据 = new MVVM视图逻辑();

        public MainWindow()
        {
            InitializeComponent();

            this.Closing += async (sender, e) =>
            {
                关闭前操作();
                日志系统.关闭();
            };


        }

        private void 关闭前操作()
        {
            string IN = $"写入的userkill_obj {USE数据.好友名称}\nuserjson的usersend_num {USE数据.消息次数}\niuserjson的log位置{公共数据.日志文件位置}\nuserjson的json文件位置{公共数据.json文件路径}";
            日志系统.I_Info(IN);
            if (公共数据.json文件路径 == null)
            {
                System.Windows.MessageBox.Show("写入时候json为空","userconfig配置",MessageBoxButton.OKCancel);
                return;
            }
            string path = ".\\userconfig.json";
            var config = JsonSerializer.Deserialize<test.UserConfig>(File.ReadAllText(path, Encoding.UTF8));
            config.usersend_num = USE数据.消息次数.ToString();
            config.userkill_obj = USE数据.好友名称.ToString();
            config.userJosn_site = 公共数据.json文件路径;

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            File.WriteAllText(path, JsonSerializer.Serialize(config, options), Encoding.UTF8);


        }


        private void 事件触发1(object sender, MouseButtonEventArgs e)
        {
            日志系统.I_Info("用户代开了readme");
            事件处理 实例化内容 = new 事件处理();
            实例化内容.打开文件("readme.html");

        }
        private void 打开文件框(object sender, RoutedEventArgs e)
        {


            Microsoft.Win32.OpenFileDialog 打开文件对象 = new Microsoft.Win32.OpenFileDialog();
            日志系统.I_Info("用户选择json文件");
            打开文件对象.Filter = "JSON文件 (*.json)|*.json";
            打开文件对象.CheckFileExists = true;
            打开文件对象.CheckPathExists = true;
            if (打开文件对象.ShowDialog() == true)
            {
                公共数据.json文件路径 = 打开文件对象.FileName;

                string IN = "用户选择的文件路径：" + 公共数据.json文件路径;
                日志系统.I_Info(IN);
            }

        }

        private void 事件触发2(object sender, RoutedEventArgs e)
        {
            日志系统.I_Info("用户代开了日志");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = "explorer.exe",
                Arguments = 公共数据.日志文件位置

            });
        }
        private void 事件触发3(object sender, RoutedEventArgs e)
        {

            APPmessage 子窗口 = new APPmessage();
            子窗口.Owner = this;
            子窗口.ShowDialog();

        }

        private static CancellationTokenSource 任务_令牌源;
        private static bool 任务状态 = false;
        private static readonly object 锁 = new object();
        private static void 启动任务(CancellationToken 状态, ToggleButton 按钮)
        {
            try
            {
                发送信息 USE = new 发送信息();
                USE.基本窗口数据获取();
                日志系统.I_Info("已执行窗口数据获取");

                日志系统.I_Info("开始数据发送");

                USE.发送信息模块(状态);
            }
            catch (Exception ex)
            {


            }



            lock (锁) { 任务状态 = false; }
            按钮.Dispatcher.Invoke(() =>
            {
                按钮.IsChecked = false;
            }
             );


        }
        private void 启动(object sender, RoutedEventArgs e)
        {
            var 按钮 = sender as ToggleButton;
            lock (锁)
            {
                if (任务状态 is true)
                {

                    暂停(null, null);
                }
            }
            任务_令牌源 = new CancellationTokenSource();

            任务状态 = true;
            启动任务(任务_令牌源.Token, 按钮);


        }

        private void 暂停(object sender, RoutedEventArgs e)
        {
            lock (锁)
            {

                if (任务状态 && 任务_令牌源 != null && !任务_令牌源.IsCancellationRequested)
                {
                    任务_令牌源.Cancel();
                    任务状态 = false;
                }
            }


        }


    }
    public static class 日志系统
    {
        private static readonly BlockingCollection<日志信息> 日志入口 = new();
        private static Thread? logThread;
        private class 日志信息
        {
            public LogEventLevel Level { get; set; }
            public string Message { get; set; } = string.Empty;
        }
        private static void 处理日志循环()
        {
            foreach (var msg in 日志入口.GetConsumingEnumerable())
            {
                if (msg.Level == LogEventLevel.Information)
                    Log.Information(msg.Message);
                else if (msg.Level == LogEventLevel.Error)
                    Log.Error(msg.Message);
            }
        }
        public static void 关闭()
        {
            日志入口.CompleteAdding();
            logThread?.Join();
            Log.CloseAndFlush();
        }
        public static void 初始化(string logDirectory)
        {
            // 创建目录
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            var 时间戳_num = 公共数据.时间戳();
            string logFile = System.IO.Path.Combine(logDirectory, $"{时间戳_num}.log");

            // 配置 Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(logFile, outputTemplate: "{Message}{NewLine}")
                .CreateLogger();

            // 启动日志线程
            logThread = new Thread(处理日志循环)
            {
                IsBackground = true
            };
            logThread.Start();
        }
        public static void I_Info(string content)
        {
            日志入口.Add(new 日志信息
            {
                Level = LogEventLevel.Information,
                Message = $"[{公共数据.时间戳()}-[INFO]:] {content}"
            });
        }

        public static void E_Error(string content)
        {
            日志入口.Add(new 日志信息
            {
                Level = LogEventLevel.Error,
                Message = $"[{公共数据.时间戳()}-[ERROR]:] {content}"
            });
        }
    }


}