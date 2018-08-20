using System.Windows;
using System.Windows.Controls;

namespace H.Framework.WPF.Control.Controls.Capture
{
    /// <summary>
    /// ToolBarControl.xaml 的交互逻辑
    /// </summary>
    public partial class ToolBarControl : UserControl
    {
        //确定事件
        public delegate void OKEventHander();

        public event OKEventHander OnOK;

        //取消事件
        public delegate void CancelEventHander();

        public event CancelEventHander OnCancel;

        //保存事件
        public delegate void SaveCaptureEventHander();

        public event SaveCaptureEventHander OnSaveCapture;

        public ToolBarControl()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            OnSaveCapture?.Invoke();
            buttonComplete_Click(sender, e);
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            OnCancel?.Invoke();
        }

        private void buttonComplete_Click(object sender, RoutedEventArgs e)
        {
            OnOK?.Invoke();
        }
    }
}