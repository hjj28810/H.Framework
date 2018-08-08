using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace H.Framework.WPF.Control.Controls
{
    [TemplatePart(Name = "MainGrid", Type = typeof(Grid))]
    public class RectangleLoading : System.Windows.Controls.Control
    {
        static RectangleLoading()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RectangleLoading), new FrameworkPropertyMetadata(typeof(RectangleLoading)));
            ClipToBoundsProperty.OverrideMetadata(typeof(RectangleLoading), new FrameworkPropertyMetadata(true));
        }

        public RectangleLoading()
        {
        }

        public static readonly DependencyProperty ShowUpProperty = DependencyProperty.Register("ShowUp", typeof(Visibility), typeof(RectangleLoading), new PropertyMetadata(Visibility.Hidden, new PropertyChangedCallback(OnShowUpPropertyChanged)));

        /// <summary>
        /// 是否ShowUp
        /// </summary>
        [Description("获取或设置是否ShowUp")]
        [Category("Defined Properties")]
        public Visibility ShowUp
        {
            get { return (Visibility)GetValue(ShowUpProperty); }
            set { SetValue(ShowUpProperty, value); }
        }

        private static void OnShowUpPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RectangleLoading obj = (RectangleLoading)d;
            if ((Visibility)e.NewValue == Visibility.Visible)
            {
                //Storyboard sb = (Storyboard)obj.FindResource("leftAnimation");
                //sb.Begin();
            }
            else
            {
            }
        }

        public static readonly DependencyProperty LoadingTextProperty = DependencyProperty.Register("LoadingText", typeof(string), typeof(RectangleLoading), new PropertyMetadata("Loading...", null));

        /// <summary>
        /// 显示的LoadingText
        /// </summary>
        [Description("获取或设置是否LoadingText")]
        [Category("Defined Properties")]
        public string LoadingText
        {
            get { return (string)GetValue(LoadingTextProperty); }
            set { SetValue(LoadingTextProperty, value); }
        }
    }
}