using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace 测试
{
    public static class 句柄获取
    {

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll",CharSet = CharSet.Auto) ]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


        public static void 窗口句柄获取1()
        {
            IntPtr 精确句柄 = IntPtr.Zero;
            string 目标名称 = 公共数据.MVVM统一接口.m.好友名称.Trim();

            EnumWindows((hWnd, _) =>
            {
                StringBuilder sb = new StringBuilder(256);
                GetWindowText(hWnd, sb, sb.Capacity);
                string 当前窗口名称= sb.ToString().Trim();
               if(当前窗口名称.IndexOf(目标名称,StringComparison.OrdinalIgnoreCase)>=0)
               {
                    精确句柄 = hWnd;
                    return false;
               }
                return true;
            }, IntPtr.Zero);
            if(精确句柄==IntPtr.Zero){ MessageBox.Show("没有找到句柄","句柄搜索"); return; }
            公共数据.得到的句柄 = 精确句柄;

        }


        public static void 窗口句柄获取2(){
            //只能返回顶层窗口，不适用
            IntPtr 句柄 = FindWindow(null, 公共数据.MVVM统一接口.m.好友名称);

            if (句柄 ==IntPtr.Zero){
                MessageBox.Show("没有找到句柄", "句柄搜索");
                return;


            }
            公共数据.得到的句柄=句柄 ;
        }

        public static void 窗口句柄获取3(){

            EnumWindows((hWnd, lParam) =>
            {
                StringBuilder windowTitle = new StringBuilder(256);
                StringBuilder className = new StringBuilder(256);
                uint processId;
                GetWindowText(hWnd, windowTitle, windowTitle.Capacity);
                GetClassName(hWnd, className, className.Capacity);
                GetWindowThreadProcessId(hWnd, out processId);
                if (IsWindowVisible(hWnd) && windowTitle.ToString().Contains(公共数据.MVVM统一接口.m.好友名称))
                {
                    公共数据.得到的句柄=hWnd;
                }
                return true;
            }, IntPtr.Zero);

        }

    }

}
