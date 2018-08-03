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
        private Canvas PART_Panel;

        static BusyCircle()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyCircle), new FrameworkPropertyMetadata(typeof(BusyCircle)));
            ClipToBoundsProperty.OverrideMetadata(typeof(BusyCircle), new FrameworkPropertyMetadata(false));
            VisibilityProperty.OverrideMetadata(typeof(BusyCircle), new FrameworkPropertyMetadata(Visibility.Collapsed));
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
            PART_Panel = (Canvas)GetTemplateChild("PART_Panel");
            PART_Panel.Height = Diameter;
            PART_Panel.Width = Diameter;
        }

        private void BusyCircle_Unloaded(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void BusyCircle_Loaded(object sender, RoutedEventArgs e)
        {
            const double offset = Math.PI;
            const double step = Math.PI * 2 / 10.0;

            for (double i = 0; i < 9; i++)
            {
                var ellipse = new Ellipse();
                ellipse.Width = Diameter / 5;
                ellipse.Height = Diameter / 5;
                ellipse.Fill = new SolidColorBrush(Colors.GreenYellow);
                ellipse.Opacity = 1 - (i / 10);
                PART_Panel?.Children.Add(ellipse);
                SetPosition(ellipse, offset, i, step);
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

        private void SetPosition(Ellipse ellipse, double offset,
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