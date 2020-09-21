using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace H.Framework.WPF.Control.Controls
{
    [TemplatePart(Name = "PART_PasswordBox", Type = typeof(WatermarkPasswordBox))]
    public class PasswordBox : System.Windows.Controls.Control
    {
        private WatermarkPasswordBox PART_PasswordBox;

        static PasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PasswordBox), new FrameworkPropertyMetadata(typeof(PasswordBox)));
        }

        public PasswordBox()
        {
            GotFocus += PasswordBox_GotFocus;
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PART_PasswordBox != null)
                PART_PasswordBox.Focus();
        }

        #region PasswordEye

        public static readonly DependencyProperty PasswordEyeProperty = DependencyProperty.Register("PasswordEye", typeof(bool), typeof(PasswordBox)
          , new UIPropertyMetadata(true, OnPasswordEyeChanged));

        public bool PasswordEye
        {
            get
            {
                return (bool)GetValue(PasswordEyeProperty);
            }
            set
            {
                SetValue(PasswordEyeProperty, value);
            }
        }

        private static void OnPasswordEyeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion PasswordEye

        #region PasswordChar

        public static readonly DependencyProperty PasswordCharProperty = DependencyProperty.Register("PasswordChar", typeof(char), typeof(PasswordBox)
          , new UIPropertyMetadata('\u25CF', OnPasswordCharChanged)); //default is black bullet

        public char PasswordChar
        {
            get
            {
                return (char)GetValue(PasswordCharProperty);
            }

            set
            {
                SetValue(PasswordCharProperty, value);
            }
        }

        private static void OnPasswordCharChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion PasswordChar

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(PasswordBox), new UIPropertyMetadata(null));

        /// <summary>
        /// 水印
        /// </summary>
        [Description("获取或设置水印")]
        [Category("Defined Properties")]
        public object Watermark
        {
            get => GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        public static readonly DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register("WatermarkTemplate", typeof(DataTemplate), typeof(PasswordBox), new UIPropertyMetadata(null));

        /// <summary>
        /// 水印模板
        /// </summary>
        [Description("获取或设置水印模板")]
        [Category("Defined Properties")]
        public DataTemplate WatermarkTemplate
        {
            get => (DataTemplate)GetValue(WatermarkTemplateProperty);
            set => SetValue(WatermarkTemplateProperty, value);
        }

        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(PasswordBox), new UIPropertyMetadata(new CornerRadius(0, 0, 0, 0)));

        /// <summary>
        /// 圆角
        /// </summary>
        [Description("获取或设置圆角")]
        [Category("Defined Properties")]
        public CornerRadius BorderCornerRadius
        {
            get => (CornerRadius)GetValue(BorderCornerRadiusProperty);
            set => SetValue(BorderCornerRadiusProperty, value);
        }

        public static readonly DependencyProperty EyeColorProperty = DependencyProperty.Register("EyeColor", typeof(Brush), typeof(PasswordBox), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(155, 160, 170))));

        /// <summary>
        /// EysColor颜色
        /// </summary>
        [Description("获取或设置EysColor颜色")]
        [Category("Defined Properties")]
        public Brush EyeColor
        {
            get { return (Brush)GetValue(EyeColorProperty); }
            set { SetValue(EyeColorProperty, value); }
        }

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(PasswordBox), new UIPropertyMetadata(TextAlignment.Left));

        /// <summary>
        /// TextAlignment
        /// </summary>
        [Description("获取或设置TextAlignment")]
        [Category("Defined Properties")]
        public TextAlignment TextAlignment
        {
            get => (TextAlignment)GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(PasswordBox), new FrameworkPropertyMetadata
        {
            BindsTwoWayByDefault = true,
            DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            DefaultValue = "",
            PropertyChangedCallback = OnPasswordChanged
        });

        public string Password
        {
            get
            {
                return (string)GetValue(PasswordProperty);
            }
            set
            {
                SetValue(PasswordProperty, value);
            }
        }

        private static void OnPasswordChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is PasswordBox box)
                if (box.PART_PasswordBox != null)
                    if (box?.PART_PasswordBox?.Password != box?.Password)
                        box.PART_PasswordBox.Password = box.Password;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_PasswordBox = (WatermarkPasswordBox)GetTemplateChild("PART_PasswordBox");
            if (PART_PasswordBox != null)
                PART_PasswordBox.PasswordChanged += PART_PasswordBox_PasswordChanged;
        }

        private void PART_PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is WatermarkPasswordBox box)
                if (Password != box?.Password)
                    Password = box.Password;
        }
    }
}