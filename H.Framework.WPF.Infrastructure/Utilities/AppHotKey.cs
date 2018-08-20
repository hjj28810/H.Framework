using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;

namespace H.Framework.WPF.Infrastructure.Utilities
{
    public class AppHotKey
    {
        public const int DefaultKeyID = 0x3572;

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        //如果函数执行成功，返回值不为0。
        //如果函数执行失败，返回值为0。要得到扩展错误信息，调用GetLastError。
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(
            IntPtr hWnd,                //要定义热键的窗口的句柄
            int id,                     //定义热键ID（不能与其它ID重复）
            KeyModifiers fsModifiers,   //标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效
            Keys vk                     //定义热键的内容
            );

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(
            IntPtr hWnd,                //要取消热键的窗口的句柄
            int id                      //要取消热键的ID
            );

        //定义了辅助键的名称（将数字转变为字符以便于记忆，也可去除此枚举而直接使用数值）
        [Flags()]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Ctrl = 2,
            Shift = 4,
            WindowsKey = 8
        }

        /// <summary>
        /// 注册热键
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="hotKey_id">热键ID</param>
        /// <param name="keyModifiers">组合键</param>
        /// <param name="key">热键</param>
        public static bool RegKey(IntPtr hwnd, int hotKey_id, string[] keysArr)
        {
            try
            {
                if (keysArr == null || !keysArr.Any())
                    return false;
                var keyModifiersList = new List<KeyModifiers>();
                var keys = Keys.None;
                foreach (var item in keysArr)
                {
                    if (DicKeyModifiers.ContainsKey(item))
                        keyModifiersList.Add(DicKeyModifiers[item]);
                    if (DicKeys.ContainsKey(item))
                        keys = DicKeys[item];
                }
                var keyModifiers = KeyModifiers.None;
                if (keyModifiersList.Count == 1)
                    keyModifiers = keyModifiersList[0];
                if (keyModifiersList.Count == 2)
                    keyModifiers = keyModifiersList[0] | keyModifiersList[1];
                if (keyModifiersList.Count == 3)
                    keyModifiers = keyModifiersList[0] | keyModifiersList[1] | keyModifiersList[2];
                if (keyModifiers == KeyModifiers.None || keys == Keys.None)
                    return false;
                if (!RegisterHotKey(hwnd, hotKey_id, keyModifiers, keys))
                {
                    if (Marshal.GetLastWin32Error() == 1409) { System.Windows.MessageBox.Show("热键被占用 ！", "提醒", MessageBoxButton.OK, MessageBoxImage.Stop); }
                    else
                    {
                        System.Windows.MessageBox.Show("注册热键失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return false;
                    }
                }
                return true;
            }
            catch (Exception) { return false; }
        }

        /// <summary>
        /// 注销热键
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="hotKey_id">热键ID</param>
        public static void UnRegKey(IntPtr hwnd, int hotKey_id)
        {
            //注销Id号为hotKey_id的热键设定
            UnregisterHotKey(hwnd, hotKey_id);
        }

        private static Dictionary<string, KeyModifiers> DicKeyModifiers
        {
            get
            {
                var keys = new Dictionary<string, KeyModifiers>();
                keys.Add("CTRL", KeyModifiers.Ctrl);
                keys.Add("ALT", KeyModifiers.Alt);
                keys.Add("SHIFT", KeyModifiers.Shift);
                return keys;
            }
        }

        private static Dictionary<string, Keys> DicKeys
        {
            get
            {
                var keys = new Dictionary<string, Keys>();
                keys.Add("A", Keys.A);
                keys.Add("B", Keys.B);
                keys.Add("C", Keys.C);
                keys.Add("D", Keys.D);
                keys.Add("E", Keys.E);
                keys.Add("F", Keys.F);
                keys.Add("G", Keys.G);
                keys.Add("H", Keys.H);
                keys.Add("I", Keys.I);
                keys.Add("J", Keys.J);
                keys.Add("K", Keys.K);
                keys.Add("L", Keys.L);
                keys.Add("M", Keys.M);
                keys.Add("N", Keys.N);
                keys.Add("O", Keys.O);
                keys.Add("P", Keys.P);
                keys.Add("Q", Keys.Q);
                keys.Add("R", Keys.R);
                keys.Add("S", Keys.S);
                keys.Add("T", Keys.T);
                keys.Add("U", Keys.U);
                keys.Add("V", Keys.V);
                keys.Add("W", Keys.W);
                keys.Add("X", Keys.X);
                keys.Add("Y", Keys.Y);
                keys.Add("Z", Keys.Z);
                return keys;
            }
        }
    }
}