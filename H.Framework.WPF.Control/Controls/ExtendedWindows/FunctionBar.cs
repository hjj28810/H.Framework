using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace H.Framework.WPF.Control.Controls.ExtendedWindows
{
    public class FunctionBar : HeaderedItemsControl
    {
        public FunctionBar()
        {
        }

        public ObservableCollection<object> Options { get; } = new ObservableCollection<object>();
    }

    public class WindowFunctionBar : FunctionBar
    {
        static WindowFunctionBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowFunctionBar), new FrameworkPropertyMetadata(typeof(WindowFunctionBar)));
        }
    }
}