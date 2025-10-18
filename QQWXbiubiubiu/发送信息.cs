using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using test;
using WindowsInput;
using WindowsInput.Native;
namespace 测试
{

    public class 发送信息
    {
        
        const int with = 1024 << 13;

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);


        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern bool AllowSetForegroundWindow(int dwProcessId);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr 句柄);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, UIntPtr dwExtraInfo);




        private string[] 得到的Json数据 = [];
        private int Json数据长度;
        public void 基本窗口数据获取()
        {
            
           if (公共数据.MVVM统一接口.m.用户选择2== "FindWindow"){

                句柄获取.窗口句柄获取2();
           }else if(公共数据.MVVM统一接口.m.用户选择2== "EnumWindows"){
                句柄获取.窗口句柄获取1();

            }else if (公共数据.MVVM统一接口.m.用户选择2== "GetClassName")
            {

              句柄获取.窗口句柄获取3();

            }else {


                System.Windows.MessageBox.Show("没有该选项", "句柄获取方式", MessageBoxButton.OKCancel);
                throw new Exception("error");

            }

            string IN = "得到的句柄" + 公共数据.得到的句柄;
            日志系统.I_Info(IN);

            GetWindowThreadProcessId(公共数据.得到的句柄, out 公共数据.目标窗口PID);
             IN = "GetWindowThreadProcessId得到的PID：" + 公共数据.目标窗口PID;
            test.日志系统.I_Info(IN);
            bool __if = AllowSetForegroundWindow(公共数据.目标窗口PID);
            
            if (__if == true)
            {
                test.日志系统.I_Info("已经成功设定窗口可以置顶");
            }
            else
            {
                test.日志系统.E_Error("windows系统不给设定窗口置顶权限");
            }

            if (公共数据.MVVM统一接口.m.消息次数 <= 0 || 公共数据.得到的句柄 == IntPtr.Zero || 公共数据.json文件路径 == " ")
            {
                System.Windows.MessageBox.Show("缺少必须填写的数据", "用户基本数据数据获取", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception("错误");
            }

            try
            {
                using (FileStream read = new FileStream(公共数据.json文件路径, FileMode.Open, FileAccess.Read))
                {

                    JsonDocument document = JsonDocument.Parse(read);
                    JsonElement root = document.RootElement;
                    var list = new List<string>();
                    foreach (JsonElement element in root.EnumerateArray())
                    {
                        list.Add(element.ToString());
                        if (list.Count >= with)
                        {
                            System.Windows.MessageBox.Show("Json文件数据过大，请减少数据", "json读取", MessageBoxButton.OK, MessageBoxImage.Error);
                            throw new Exception("错误");
                        }
                    }
                    得到的Json数据 = list.ToArray();
                }
                Json数据长度 = 得到的Json数据.Length;
            }

            catch (FileNotFoundException)
            {
                System.Windows.MessageBox.Show("没有找到指定的Json文件", "json读取", MessageBoxButton.OK, MessageBoxImage.Error);
                test.日志系统.E_Error("没有找到置顶的Json文件");
                throw new Exception("错误");
            }
            catch (DirectoryNotFoundException)
            {
                System.Windows.MessageBox.Show("指定路径存在错误", "json读取", MessageBoxButton.OK, MessageBoxImage.Error);
                test.日志系统.E_Error("用户选择的路径存在错误");
                throw new Exception("错误");
            }
            catch (JsonException error)
            {
                System.Windows.MessageBox.Show($"Json文件出现错误：{error.Message}", "json读取", MessageBoxButton.OK, MessageBoxImage.Error);
                test.日志系统.E_Error("Json文件未知错误，用户需要检查JSON文件");
                throw new Exception("错误");
            }
            catch (Exception error)
            {
                System.Windows.MessageBox.Show($"未知错误：{error.Message}", "json读取", MessageBoxButton.OK, MessageBoxImage.Error);
                test.日志系统.E_Error("未知错误");
                throw new Exception("错误");
            }
           
        }


        //鼠标按键定义
        private uint 鼠标左键按下 = 0x0002;
        private uint 鼠标左键抬起 = 0x0004;
        
        //鼠标方式
        private static uint 移动鼠标 = 0x0001;

        private const uint 绝对标志 = 0x8000;



        private int SHOW特用_还原 = 9;
        private int SHOW特用_最小化 = 6;

       

        public async Task 发送信息模块(CancellationToken 状态)
        {   
            
           
            if (IsIconic(公共数据.得到的句柄) == true)
            {
                test.日志系统.I_Info("窗口已最小化，尝试还原窗口,并跳过SetForegroundWindow");
                ShowWindow(公共数据.得到的句柄, SHOW特用_还原);
                goto A;
            }

            //前置AllowSetForegroundWindow
            var 置顶结果 = SetForegroundWindow(公共数据.得到的句柄);
            if (true == 置顶结果)
            {
                goto A;
            }
            else
            {   //利用ShowWindow会置顶窗口特点,(保底)
                test.日志系统.E_Error("SetForegroundWindow失败，尝试ShowWindow强制置顶");
                var 最小化结果 = ShowWindow(公共数据.得到的句柄, SHOW特用_最小化);
                if (true == 最小化结果)
                {
                    var _if = System.Windows.MessageBox.Show("最小化窗口失败,请手动最小化窗口", "窗口处理", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (_if ==  MessageBoxResult.OK)
                    {
                        ShowWindow(公共数据.得到的句柄, SHOW特用_还原);
                        goto A;
                    }
                    else
                    {
                        test.日志系统.E_Error("用户取消窗口最小化");
                        throw new Exception("错误");
                    }

                }


            }
        A:

            test.日志系统.I_Info("进行激活窗口");
            窗口尺寸获取.转化绝对坐标(0.5, 0.9);
            if(公共数据.MVVM统一接口.m.用户选择 == "SendInput"){

                鼠标事件.鼠标事件use(移动鼠标);
                鼠标事件.鼠标事件use(鼠标左键按下);
                Thread.Sleep(50);
                鼠标事件.鼠标事件use(鼠标左键抬起);

            }else if(公共数据.MVVM统一接口.m.用户选择 == "mouse_event")
            {
                mouse_event(移动鼠标 | 绝对标志, (uint)公共数据.点击位置x, (uint)公共数据.点击位置y, 0, UIntPtr.Zero);
                mouse_event(鼠标左键按下, 0, 0, 0, UIntPtr.Zero);
                Thread.Sleep(50);
                mouse_event(鼠标左键抬起, 0, 0, 0, UIntPtr.Zero);

            }else{
              System.Windows.MessageBox.Show("没有该选项","鼠标点击模式",MessageBoxButton.OKCancel);
                throw new Exception("error");
            }

                for (int i = 0; i < 公共数据.MVVM统一接口.m.消息次数; i++)
                {
                    状态.ThrowIfCancellationRequested();
                    var 种子 = 公共数据.种子();
                    var 随机数 = new Random(种子).Next(0, Json数据长度 - 1);
                    var 发送内容 = 得到的Json数据[随机数];

                    var sim = new InputSimulator();
                    sim.Keyboard.TextEntry(发送内容);
                    状态.WaitHandle.WaitOne(1000);
                    sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);

                }
            
        }
    }
}
