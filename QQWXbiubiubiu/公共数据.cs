using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using test;

namespace 测试;

public static class 公共数据
{   //杂乱定义 
   
    public static int 点击位置x = 0;

    public static int 点击位置y = 0;

    public static string 日志文件位置;

    //固定用户数据获取
    public static IntPtr 得到的句柄 = IntPtr.Zero;

    public static int 目标窗口PID;

    public static string json文件路径;
    
 

    //程序必要数据
    public static string  当前文件位置 = Directory.GetCurrentDirectory();

    static public int 种子()
    {
        byte[] 熵源 = RandomNumberGenerator.GetBytes(4);
        int 格式化熵 = BitConverter.ToInt32(熵源, 0);
        return 格式化熵;

    }
    public static string 时间戳()
    {
        try
        {
            string tzId = OperatingSystem.IsWindows()
                ? "China Standard Time"
                : "Asia/Shanghai";

            DateTime beijingTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                DateTime.UtcNow, tzId);

            return beijingTime.ToString("yyyy-MM-dd-HH-mm-ss");
        }
        catch (TimeZoneNotFoundException)
        {
            return DateTime.UtcNow.AddHours(8)
                       .ToString("yyyy-MM-dd-HH-mm-ss");
        }
    }
    public class MVVM统一接口{

       public static MVVM视图逻辑  m = new MVVM视图逻辑();
     



    }

}



public class MVVM视图逻辑 : INotifyPropertyChanged
{


    public int 消息次数
    {
        get => moudle_数据存储.数据对象1;
        set
        {
            if (value != moudle_数据存储.数据对象1)
            {
                moudle_数据存储.数据对象1 = value;
                OnPropertyChanged(nameof(消息次数));
            }else{

                moudle_数据存储.数据对象1 = 0;

            }
        }
    }

    public string 好友名称
    {
        get => moudle_数据存储.数据对象2;
        set
        {
            if (value != moudle_数据存储.数据对象2)
            {
                moudle_数据存储.数据对象2 = value;
                OnPropertyChanged();
            }
        }
    }



    public ObservableCollection<选项1_鼠标模式> 用户选择选项 { get; } = new()
    {

      new 选项1_鼠标模式{ 数据对象3 ="SendInput"},
      new 选项1_鼠标模式{ 数据对象3 = "mouse_event"}
    };

    public ObservableCollection<选项1_句柄获取模式> 用户选择项2 { get; } = new()
    {

       new 选项1_句柄获取模式{ 数据对象4 = "FindWindow"},
       new 选项1_句柄获取模式{数据对象4 = "EnumWindows"},
       new 选项1_句柄获取模式{数据对象4= "GetClassName"}

    };

    public string _数据对象3;
    
    public string 用户选择{

        get => _数据对象3 ;
        set{
          _数据对象3 = value;
          OnPropertyChanged(nameof(用户选择));

        }

    }


    public string _数据对象4;
    public string 用户选择2{

        get => _数据对象4;
        set{

            _数据对象4 = value;
            OnPropertyChanged(nameof(_数据对象4));

        }

    }

   public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    private static class moudle_数据存储
    {

        public static int 数据对象1 { get; set; }
        public static string 数据对象2 { get; set; } = string.Empty;

 
    }
    public class 选项1_鼠标模式
    {
        public  string 数据对象3 { get; set; } = string.Empty;

        public  override string ToString() => 数据对象3;
    }

    public class 选项1_句柄获取模式{
      public string 数据对象4 { get; set; } = string.Empty;
        public override string ToString()=> 数据对象4 ;
       

    }
}
