using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace H.Framework.WPF.Control.Controls
{
    [TemplatePart(Name = "PART_Panel", Type = typeof(Canvas))]
    public class BusyCircle : System.Windows.Controls.Control
    {
        private Canvas _partPanel;

        static BusyCircle()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyCircle), new FrameworkPropertyMetadata(typeof(BusyCircle)));
            ClipToBoundsProperty.OverrideMetadata(typeof(BusyCircle), new FrameworkPropertyMetadata(false));
        }

        public static readonly DependencyProperty BrushColorProperty = DependencyProperty.Register("BrushColor", typeof(Brush), typeof(BusyCircle), new PropertyMetadata(new SolidColorBrush(Colors.GreenYellow), null));

        /// <summary>
        /// 整体颜色
        /// </summary>
        [Description("获取或设置整体颜色")]
        [Category("Defined Properties")]
        public Brush BrushColor
        {
            get => (Brush)GetValue(BrushColorProperty);
            set => SetValue(BrushColorProperty, value);
        }

        public static readonly DependencyProperty DiameterProperty = DependencyProperty.Register("Diameter", typeof(double), typeof(BusyCircle), new PropertyMetadata(20.0, null));

        /// <summary>
        /// 直径大小
        /// </summary>
        [Description("获取或设置直径大小")]
        [Category("Defined Properties")]
        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set { SetValue(DiameterProperty, value); }
        }

        public BusyCircle()
        {
            IsVisibleChanged += BusyCircle_IsVisibleChanged;
            Loaded += BusyCircle_Loaded;
            Unloaded += BusyCircle_Unloaded;
        }

        private void BusyCircle_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                Start();
            else
                Stop();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _partPanel = (Canvas)GetTemplateChild("PART_Panel");
        }

        private void BusyCircle_Unloaded(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private const double _offset = Math.PI;
        private const double _step = Math.PI * 2 / 10.0;

        private void BusyCircle_Loaded(object sender, RoutedEventArgs e)
        {
            for (double i = 0; i < 9; i++)
            {
                var ellipse = new Ellipse
                {
                    Width = Diameter / 5,
                    Height = Diameter / 5,
                    Fill = BrushColor,
                    Opacity = 1 - (i / 10)
                };
                SetPosition(ellipse, _offset, i, _step);
                _partPanel?.Children.Add(ellipse);
            }
        }

        private void Start()
        {
            //Mouse.OverrideCursor = Cursors.Wait;
        }

        private void Stop()
        {
            //Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void SetPosition(DependencyObject ellipse, double offset,
            double posOffSet, double step)
        {
            var value = Diameter * 5 / 12;
            ellipse.SetValue(Canvas.LeftProperty, value
                + Math.Sin(offset + posOffSet * step) * value);

            ellipse.SetValue(Canvas.TopProperty, value
                + Math.Cos(offset + posOffSet * step) * value);
        }
    }
}