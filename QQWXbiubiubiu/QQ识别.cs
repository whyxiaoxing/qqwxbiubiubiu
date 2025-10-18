using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Win32;
using test;
namespace 测试;

public class 窗口尺寸获取()
{
    [DllImport("kernel32.dll", SetLastError = false)]
    public static extern int MulDiv(int nNumber, int nNumerator, int nDenominator);
    [StructLayout(LayoutKind.Sequential)]
    
    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);

    private const int SM_XVIRTUALSCREEN = 76;
    private const int SM_YVIRTUALSCREEN = 77;
    private const int SM_CXVIRTUALSCREEN = 78;
    private const int SM_CYVIRTUALSCREEN = 79;

    public static void 转化绝对坐标(double x百分比, double y百分比)
    {
        
        if (!GetWindowRect(公共数据.得到的句柄, out RECT 窗口数据))
        {
            System.Windows.MessageBox.Show("获取窗口数据失败请重试", "函数错误", MessageBoxButton.OKCancel, MessageBoxImage.Stop);
           日志系统.E_Error("GetWindowRect函数出现错误，请检查逻辑");
            throw new Exception(" ");
        }
        string IN;
        int 窗口宽度 = 窗口数据.Right - 窗口数据.Left;
        int 窗口高度 = 窗口数据.Bottom - 窗口数据.Top;
        var 点击位置实际x = 窗口数据.Left + (int)(窗口宽度 * x百分比);
        var 点击位置实际y = 窗口数据.Top + (int)(窗口高度 * y百分比);
         IN = "得到的点击位置 x轴" + 点击位置实际x + " y轴" + 点击位置实际y;
        日志系统.I_Info(IN);
        int 虚拟左 = GetSystemMetrics(SM_XVIRTUALSCREEN);   
        int 虚拟顶 = GetSystemMetrics(SM_YVIRTUALSCREEN);   
        int 虚拟宽 = GetSystemMetrics(SM_CXVIRTUALSCREEN);  
        int 虚拟高 = GetSystemMetrics(SM_CYVIRTUALSCREEN);
        IN = "用户的屏幕分辨率为 witd：" + 虚拟宽 + " hight：" + 虚拟高;
        日志系统.I_Info(IN);
        公共数据.点击位置x = MulDiv(点击位置实际x - 虚拟左, 65535, 虚拟宽 - 1);
        公共数据.点击位置y = MulDiv(点击位置实际y - 虚拟顶, 65535, 虚拟高 - 1);

    }
}

public class 鼠标事件
{
    [StructLayout(LayoutKind.Sequential)]
    struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct InputUnion
    {
        [FieldOffset(0)] public MOUSEINPUT mi;
    }

    
    [StructLayout(LayoutKind.Sequential)]
    struct INPUT
    {
        public int type;
        public InputUnion U;
    }

    // ---- 常量 ----
    private const int INPUT_MOUSE = 0;
    private const uint MOUSEEVENTF_ABSOLUTE = 0x8000;


    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    public static void 鼠标事件use(uint 操作类型, uint 滚轮数据 = 0)
    {
        if (公共数据.点击位置x == 0 || 公共数据.点击位置y == 0)
        {
            System.Windows.MessageBox.Show("窗口取值失败，向开发者反馈", "算法错误", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            throw new Exception(" ");
        }

        var mi = new MOUSEINPUT
        {
            dx = 公共数据.点击位置x,
            dy = 公共数据.点击位置y,
            dwFlags =操作类型 | MOUSEEVENTF_ABSOLUTE,
            mouseData = 滚轮数据, 
            time = 0,
            dwExtraInfo = IntPtr.Zero
        };

        INPUT input = new INPUT
        {
            type = INPUT_MOUSE,
            U = new InputUnion { mi = mi }
        };

        uint sent = SendInput(1, new INPUT[] { input }, Marshal.SizeOf(typeof(INPUT)));
        if (sent != 1)
        {
            int err = Marshal.GetLastWin32Error();
            throw new InvalidOperationException($"SendInput (鼠标) 发送失败，Win32 错误：{err}");
        }
    }

}


public class 事件处理{

    [DllImport("shell32.dll", SetLastError = true)]
    private static extern int ShellExecute(
        IntPtr hwnd,
        string lpOperation,
        string lpFile,
        string lpParameters,
        string lpDirectory,
        int nShowCmd
    );
    public void  打开文件( string 路径 ){
        
        try
        {
           
            if (公共数据.当前文件位置.Length < 0)
            {
                System.Windows.MessageBox.Show("无法获取当前路径", "路径获取", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string html文件位置 = $"{公共数据.当前文件位置}\\固定信息\\{路径}";
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

            System.Diagnostics.Process.Start(browserpath, html文件位置);

        }
       catch (Exception err){

           System.Windows.MessageBox.Show($"{err}","error");
            return;
       }

   }


}

