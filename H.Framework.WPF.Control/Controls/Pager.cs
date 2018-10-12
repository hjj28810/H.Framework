using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace H.Framework.WPF.Control.Controls
{
    [TemplatePart(Name = "PART_PreviousBtn", Type = typeof(ButtonEx))]
    [TemplatePart(Name = "PART_FirstPageBtn", Type = typeof(ButtonEx))]
    [TemplatePart(Name = "PART_PreviousFiveBtn", Type = typeof(ButtonEx))]
    [TemplatePart(Name = "PART_PageBtnPanel", Type = typeof(StackPanel))]
    [TemplatePart(Name = "PART_NextFiveBtn", Type = typeof(ButtonEx))]
    [TemplatePart(Name = "PART_LastPageBtn", Type = typeof(ButtonEx))]
    [TemplatePart(Name = "PART_NextBtn", Type = typeof(ButtonEx))]
    public class Pager : System.Windows.Controls.Control
    {
        static Pager()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Pager), new FrameworkPropertyMetadata(typeof(Pager)));
        }

        public static readonly DependencyProperty DataCountProperty = DependencyProperty.Register("DataCount", typeof(int), typeof(Pager), new PropertyMetadata(0, null));

        /// <summary>
        /// 数据总数
        /// </summary>
        [Description("获取或设置数据总数")]
        [Category("Defined Properties")]
        public int DataCount
        {
            get => (int)GetValue(DataCountProperty);
            set => SetValue(DataCountProperty, value);
        }

        public static readonly DependencyProperty PageSizeProperty = DependencyProperty.Register("PageSize", typeof(int), typeof(Pager), new PropertyMetadata(1, null));

        /// <summary>
        /// 每页数据数
        /// </summary>
        [Description("获取或设置每页数据数")]
        [Category("Defined Properties")]
        public int PageSize
        {
            get => (int)GetValue(PageSizeProperty);
            set => SetValue(PageSizeProperty, value);
        }

        public static readonly DependencyProperty BtnCapacityProperty = DependencyProperty.Register("BtnCapacity", typeof(int), typeof(Pager), new PropertyMetadata(5, null));

        /// <summary>
        /// 动态页数BTN
        /// </summary>
        [Description("获取或设置动态页数BTN")]
        [Category("Defined Properties")]
        public int BtnCapacity
        {
            get => (int)GetValue(BtnCapacityProperty);
            set => SetValue(BtnCapacityProperty, value);
        }

        public static readonly RoutedEvent SelectedChangedEvent = EventManager.RegisterRoutedEvent("SelectedChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(Pager));

        [Description("选择项后触发")]
        public event RoutedPropertyChangedEventHandler<object> SelectedChanged
        {
            add { AddHandler(SelectedChangedEvent, value); }
            remove { RemoveHandler(SelectedChangedEvent, value); }
        }

        protected virtual void OnSelectedChanged(object oldValue, object newValue)
        {
            Trace.WriteLine(newValue);
            RaiseEvent(new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, SelectedChangedEvent));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CreateControl();
        }

        private StackPanel _panel;
        private int _pageCapacity;
        private int _currentPage;
        private ButtonEx _previousFiveBtn;
        private ButtonEx _nextFiveBtn;
        private ButtonEx _nextBtn;
        private ButtonEx _previousBtn;

        private void CreateControl()
        {
            _previousBtn = (ButtonEx)GetTemplateChild("PART_PreviousBtn");
            var firstPageBtn = (ButtonEx)GetTemplateChild("PART_FirstPageBtn");
            _previousFiveBtn = (ButtonEx)GetTemplateChild("PART_PreviousFiveBtn");
            _nextFiveBtn = (ButtonEx)GetTemplateChild("PART_NextFiveBtn");
            var lastPageBtn = (ButtonEx)GetTemplateChild("PART_LastPageBtn");
            _nextBtn = (ButtonEx)GetTemplateChild("PART_NextBtn");
            _panel = (StackPanel)GetTemplateChild("PART_PageBtnPanel");
            firstPageBtn.Click += FirstPageBtn_Click;
            lastPageBtn.Click += LastPageBtn_Click;
            _previousBtn.Click += PreviousBtn_Click;
            _nextBtn.Click += NextBtn_Click;
            _previousFiveBtn.Click += PreviousFiveBtn_Click;
            _nextFiveBtn.Click += NextFiveBtn_Click;

            if (PageSize <= 0)
                throw new Exception("每页数据量不能为0或负数");

            _pageCapacity = (int)Math.Ceiling((decimal)DataCount / PageSize);

            if (_pageCapacity > 1)
            {
                lastPageBtn.Content = _pageCapacity;
                lastPageBtn.Width = CalWidth(_pageCapacity);
                if (_pageCapacity > BtnCapacity)
                {
                    _nextFiveBtn.Visibility = Visibility.Visible;
                    lastPageBtn.Visibility = Visibility.Visible;
                }
                _nextBtn.IsEnabled = true;
                var count = _pageCapacity > BtnCapacity ? BtnCapacity : _pageCapacity;
                for (int i = 0; i < count; i++)
                {
                    var btn = new ButtonEx
                    {
                        Content = i + 2
                    };
                    btn.Click += PageBtn_Click;
                    _panel.Children.Add(btn);
                }
            }
        }

        private void NextFiveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _pageCapacity - 7)
                MoveSelectedPage(_currentPage + 5);
            else
                LastPageBtn_Click(sender, e);
        }

        private void PreviousFiveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 8)
                MoveSelectedPage(_currentPage - 5);
            else
                FirstPageBtn_Click(sender, e);
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            MoveSelectedPage(_currentPage + 1);
            if (_currentPage == _pageCapacity)
            {
                _previousFiveBtn.Visibility = Visibility.Visible;
                _previousBtn.IsEnabled = true;
                _nextBtn.IsEnabled = false;
            }
        }

        private void PreviousBtn_Click(object sender, RoutedEventArgs e)
        {
            MoveSelectedPage(_currentPage - 1);
            if (_currentPage == 1)
            {
                _nextFiveBtn.Visibility = Visibility.Visible;
                _nextBtn.IsEnabled = true;
                _previousBtn.IsEnabled = false;
            }
        }

        private void LastPageBtn_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = _pageCapacity;
            OnSelectedChanged(null, _currentPage);
            RebuildPage(_pageCapacity - 3);
            _previousFiveBtn.Visibility = Visibility.Visible;
            _previousBtn.IsEnabled = true;
            _nextBtn.IsEnabled = false;
        }

        private void FirstPageBtn_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            OnSelectedChanged(null, _currentPage);
            RebuildPage(4);
            _nextFiveBtn.Visibility = Visibility.Visible;
            _nextBtn.IsEnabled = true;
            _previousBtn.IsEnabled = false;
        }

        private void PageBtn_Click(object sender, RoutedEventArgs e)
        {
            MoveSelectedPage((int)((ButtonEx)sender).Content);
        }

        private void MoveSelectedPage(int selectedPage)
        {
            _currentPage = selectedPage;
            OnSelectedChanged(null, selectedPage);
            _nextBtn.IsEnabled = true;
            _previousBtn.IsEnabled = true;
            if (_pageCapacity > BtnCapacity && selectedPage > 4 && selectedPage + 1 < _pageCapacity)
            {
                _nextFiveBtn.Visibility = Visibility.Visible;
                if (selectedPage == _pageCapacity - 2)
                    RebuildPage(_pageCapacity - 3);
                else
                    RebuildPage(selectedPage);
                _previousFiveBtn.Visibility = Visibility.Visible;
            }
            else if ((selectedPage == 3 || selectedPage == 4) && _pageCapacity > BtnCapacity && _previousFiveBtn.Visibility == Visibility.Visible)
            {
                RebuildPage(4);
                _nextFiveBtn.Visibility = Visibility.Visible;
            }
        }

        private int CalWidth(int num)
        {
            var len = num.ToString().Length;
            var width = 30;
            width += 6 * (len - 2);
            return width;
        }

        private void RebuildPage(int selectedPage)
        {
            var midIndex = (int)Math.Ceiling((decimal)_panel.Children.Count / 2) - 1;
            var firstPage = selectedPage - midIndex;
            if (selectedPage == 1)
                _previousBtn.IsEnabled = false;
            if (selectedPage == _pageCapacity)
                _nextBtn.IsEnabled = false;
            for (int i = 0; i < _panel.Children.Count; i++)
            {
                var btn = _panel.Children[i] as ButtonEx;
                btn.Content = firstPage + i;
                btn.Visibility = Visibility.Visible;
                if (firstPage + i == _pageCapacity)
                {
                    btn.Visibility = Visibility.Collapsed;
                    _nextFiveBtn.Visibility = Visibility.Collapsed;
                }
                if (firstPage + i == _pageCapacity - 1)
                {
                    _nextFiveBtn.Visibility = Visibility.Collapsed;
                }
                if (firstPage + i == 1)
                {
                    btn.Visibility = Visibility.Collapsed;
                    _previousFiveBtn.Visibility = Visibility.Collapsed;
                }
                if (firstPage + i == 2)
                {
                    _previousFiveBtn.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}