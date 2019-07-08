using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace H.Framework.WPF.Infrastructure.Utilities
{
  public class GridHelper
    {
        private static void RefreshGrid(Grid grid, int lineWidth, Brush color)
        {
            for (var i = grid.Children.Count - 1; i > 0; i--)
            {
                var child = grid.Children[i];

                var bd = child as Border;
                if (bd != null && bd.Tag != null && bd.Tag.ToString() == "gridline")
                {
                    grid.Children.Remove(bd);
                }
            }
            var rows = grid.RowDefinitions.Count;
            var cols = grid.ColumnDefinitions.Count;
            //边界考虑
            if (rows == 0)
            {
                rows = 1;
            }
            if (cols == 0)
            {
                cols = 1;
            }
            //生成行列
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    var thick = new Thickness(lineWidth, lineWidth, 0, 0);
                    var margin = new Thickness(-lineWidth / 2d, -lineWidth / 2d, 0, 0);
                    //边界考虑
                    if (i == 0)
                    {
                        margin.Top = 0;
                    }
                    if (i == rows - 1)
                    {
                        thick.Bottom = lineWidth;
                    }
                    if (j == 0)
                    {
                        margin.Left = 0;
                    }
                    if (j == cols - 1)
                    {
                        thick.Right = lineWidth;
                    }
                    var bd = new Border
                    {
                        BorderThickness = thick,
                        Margin = margin,
                        BorderBrush = color,
                        Tag = "gridline"
                    };
                    Grid.SetRow(bd, i);
                    Grid.SetColumn(bd, j);
                    grid.Children.Add(bd);
                }
            }
            grid.InvalidateArrange();
            grid.InvalidateVisual();
        }

        #region 线颜色

        // Using a DependencyProperty as the backing store for LineColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineColorProperty =
            DependencyProperty.RegisterAttached("LineColor", typeof(Brush), typeof(GridHelper),
                new PropertyMetadata(Brushes.Black, LineColorPropertyChanged));

        public static Brush GetLineColor(DependencyObject obj)
        {
            return (Brush)obj.GetValue(LineColorProperty);
        }

        public static void SetLineColor(DependencyObject obj, Brush value)
        {
            obj.SetValue(LineColorProperty, value);
        }

        private static void LineColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            if (grid == null)
            {
                return;
            }
            var showLines = GetShowGridLines(grid);
            var color = GetLineColor(grid);
            var lineWidth = GetLineWidth(grid);
            if (showLines)
            {
                //  grid.SnapsToDevicePixels = true;
                grid.Loaded += delegate { RefreshGrid(grid, lineWidth, color); };
            }
        }

        #endregion 线颜色

        #region 线宽度

        // Using a DependencyProperty as the backing store for LineWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineWidthProperty =
            DependencyProperty.RegisterAttached("LineWidth", typeof(int), typeof(GridHelper),
                new PropertyMetadata(1, LineWidthPropertyChanged));

        public static int GetLineWidth(DependencyObject obj)
        {
            return (int)obj.GetValue(LineWidthProperty)
                ;
        }

        public static void SetLineWidth(DependencyObject obj, int value)
        {
            obj.SetValue(LineWidthProperty, value);
        }

        private static void LineWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            if (grid == null)
            {
                return;
            }
            var showLines = GetShowGridLines(grid);
            var color = GetLineColor(grid);
            var lineWidth = GetLineWidth(grid);
            if (showLines)
            {
                // grid.SnapsToDevicePixels = true;
                grid.Loaded += delegate { RefreshGrid(grid, lineWidth, color); };
            }
        }

        #endregion 线宽度

        #region 是否显示线

        // Using a DependencyProperty as the backing store for ShowGridLines.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowGridLinesProperty =
            DependencyProperty.RegisterAttached("ShowGridLines", typeof(bool), typeof(GridHelper),
                new PropertyMetadata(false, ShowGridLinesPropertyChanged));

        public static bool GetShowGridLines(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowGridLinesProperty);
        }

        public static void SetShowGridLines(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowGridLinesProperty, value);
        }

        private static void ShowGridLinesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as Grid;
            if (grid == null)
            {
                return;
            }
            var showLines = GetShowGridLines(grid);
            var color = GetLineColor(grid);
            var lineWidth = GetLineWidth(grid);
            if (showLines)
            {
                //  grid.SnapsToDevicePixels = true;
                grid.Loaded += delegate { RefreshGrid(grid, lineWidth, color); };
            }
        }

        #endregion 是否显示线
    }
}