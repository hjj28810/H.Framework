using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace H.Framework.WPF.Control.Controls
{
    public class ExpanderEx : Expander
    {
        static ExpanderEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpanderEx), new FrameworkPropertyMetadata(typeof(ExpanderEx)));
            ClipToBoundsProperty.OverrideMetadata(typeof(ExpanderEx), new FrameworkPropertyMetadata(false));
        }
    }
}