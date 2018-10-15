using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace H.Framework.WPF.Control.Controls
{
    [TemplatePart(Name = "PART_PreviousBtn", Type = typeof(RadioButtonEx))]
    [TemplatePart(Name = "PART_FirstPageBtn", Type = typeof(RadioButtonEx))]
    [TemplatePart(Name = "PART_PreviousFiveBtn", Type = typeof(RadioButtonEx))]
    [TemplatePart(Name = "PART_PageBtnPanel", Type = typeof(StackPanel))]
    [TemplatePart(Name = "PART_NextFiveBtn", Type = typeof(RadioButtonEx))]
    [TemplatePart(Name = "PART_LastPageBtn", Type = typeof(RadioButtonEx))]
    [TemplatePart(Name = "PART_NextBtn", Type = typeof(RadioButtonEx))]
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

        public static readonly DependencyProperty BtnColorProperty = DependencyProperty.Register("BtnColor", typeof(Brush), typeof(Pager), new PropertyMetadata(new SolidColorBrush(Colors.CadetBlue), null));

        /// <summary>
        /// 选中色
        /// </summary>
        [Description("获取或设置选中色")]
        [Category("Defined Properties")]
        public Brush BtnColor
        {
            get => (Brush)GetValue(BtnColorProperty);
            set => SetValue(BtnColorProperty, value);
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
        private int _currentPage = 1;
        private ButtonEx _previousFiveBtn;
        private ButtonEx _nextFiveBtn;
        private ButtonEx _nextBtn;
        private ButtonEx _previousBtn;
        private RadioButtonEx _firstPageBtn;
        private RadioButtonEx _lastPageBtn;
        private int _midIndex;

        private void CreateControl()
        {
            _previousBtn = (ButtonEx)GetTemplateChild("PART_PreviousBtn");
            _firstPageBtn = (RadioButtonEx)GetTemplateChild("PART_FirstPageBtn");
            _previousFiveBtn = (ButtonEx)GetTemplateChild("PART_PreviousFiveBtn");
            _nextFiveBtn = (ButtonEx)GetTemplateChild("PART_NextFiveBtn");
            _lastPageBtn = (RadioButtonEx)GetTemplateChild("PART_LastPageBtn");
            _nextBtn = (ButtonEx)GetTemplateChild("PART_NextBtn");
            _panel = (StackPanel)GetTemplateChild("PART_PageBtnPanel");
            _firstPageBtn.Checked += FirstPageBtn_Click;
            _lastPageBtn.Checked += LastPageBtn_Click;
            _previousBtn.Click += PreviousBtn_Click;
            _nextBtn.Click += NextBtn_Click;
            _previousFiveBtn.Click += PreviousFiveBtn_Click;
            _nextFiveBtn.Click += NextFiveBtn_Click;

            if (PageSize <= 0)
                throw new Exception("每页数据量不能为0或负数");

            _pageCapacity = (int)Math.Ceiling((decimal)DataCount / PageSize);

            if (_pageCapacity > 1)
            {
                _lastPageBtn.Content = _pageCapacity;
                _lastPageBtn.Width = CalWidth(_pageCapacity);
                if (_pageCapacity > BtnCapacity + 1)
                {
                    ShowBtn(_nextFiveBtn);
                    _lastPageBtn.Visibility = Visibility.Visible;
                }
                AbleBtn(_nextBtn);
                var count = _pageCapacity > BtnCapacity ? BtnCapacity : _pageCapacity - 1;
                for (int i = 0; i < count; i++)
                {
                    var btn = new RadioButtonEx
                    {
                        Content = i + 2,
                        GroupName = "GroupName",
                        CheckedColor = BtnColor
                    };
                    //btn.Click += PageBtn_Click;
                    btn.Checked += PageBtn_Checked;
                    _panel.Children.Add(btn);
                }
                _midIndex = (int)Math.Ceiling((decimal)_panel.Children.Count / 2) - 1;
            }
        }

        private void PageBtn_Checked(object sender, RoutedEventArgs e)
        {
            var selectedIndex = (int)((RadioButtonEx)sender).Content;
            if (selectedIndex == _currentPage) return;
            MoveSelectedPage(selectedIndex);
            if (_currentPage == _pageCapacity)
            {
                ShowBtn(_previousFiveBtn);
                AbleBtn(_previousBtn);
                DisableBtn(_nextBtn);
            }
            FirstLastUnchecked();
        }

        private void NextFiveBtn_Click(object sender, RoutedEventArgs e)
        {
            FirstLastUnchecked();
            if (_currentPage < _pageCapacity - 7)
                MoveSelectedPage(_currentPage + 5);
            else
            {
                LastPageBtn_Click(sender, e);
                _lastPageBtn.IsChangedBackground = true;
            }
        }

        private void PreviousFiveBtn_Click(object sender, RoutedEventArgs e)
        {
            FirstLastUnchecked();
            if (_currentPage > 8)
                MoveSelectedPage(_currentPage - 5);
            else
            {
                FirstPageBtn_Click(sender, e);
                _firstPageBtn.IsChangedBackground = true;
            }
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (((ButtonEx)sender).Cursor == Cursors.No)
                return;
            FirstLastUnchecked();
            MoveSelectedPage(_currentPage + 1);
            if (_currentPage == _pageCapacity)
            {
                ShowBtn(_previousFiveBtn);
                AbleBtn(_previousBtn);
                DisableBtn(_nextBtn);
            }
        }

        private void PreviousBtn_Click(object sender, RoutedEventArgs e)
        {
            if (((ButtonEx)sender).Cursor == Cursors.No)
                return;
            FirstLastUnchecked();
            if (_previousFiveBtn.Visibility == Visibility.Collapsed)
            {
                for (int i = 0; i < _panel.Children.Count; i++)
                {
                    var item = _panel.Children[i] as RadioButtonEx;
                    if (item.IsChangedBackground)
                    {
                        item.IsChangedBackground = false;
                        item.IsChecked = false;
                        if (i != 0)
                            ((RadioButtonEx)_panel.Children[i - 1]).IsChangedBackground = true;
                        else
                            _firstPageBtn.IsChangedBackground = true;
                        break;
                    }
                }
            }
            if (_nextFiveBtn.Visibility == Visibility.Collapsed && _previousFiveBtn.Visibility == Visibility.Visible)
            {
                ((RadioButtonEx)_panel.Children[_panel.Children.Count - 1]).IsChangedBackground = true;
            }
            MoveSelectedPage(_currentPage - 1);
            if (_currentPage == 1)
            {
                ShowBtn(_nextFiveBtn);
                AbleBtn(_nextBtn);
                DisableBtn(_previousBtn);
            }
        }

        private void LastPageBtn_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = _pageCapacity;
            OnSelectedChanged(null, _currentPage);
            RebuildPage(_pageCapacity - 3);
            ShowBtn(_previousFiveBtn);
            AbleBtn(_previousBtn);
            DisableBtn(_nextBtn);
            UncheckRB();
        }

        private void FirstPageBtn_Click(object sender, RoutedEventArgs e)
        {
            _currentPage = 1;
            OnSelectedChanged(null, _currentPage);
            if (_pageCapacity > 6)
                RebuildPage(4);
            ShowBtn(_nextFiveBtn);
            AbleBtn(_nextBtn);
            DisableBtn(_previousBtn);
            UncheckRB();
        }

        private void MoveSelectedPage(int selectedPage)
        {
            _currentPage = selectedPage;
            OnSelectedChanged(null, selectedPage);
            AbleBtn(_nextBtn);
            AbleBtn(_previousBtn);
            if (_pageCapacity > BtnCapacity && selectedPage > 4 && selectedPage + 1 < _pageCapacity)
            {
                ShowBtn(_nextFiveBtn);
                if (selectedPage == _pageCapacity - 2)
                    RebuildPage(_pageCapacity - 3);
                else
                    RebuildPage(selectedPage);
                SelectedIndex(_midIndex);
                ShowBtn(_previousFiveBtn);
            }
            else if ((selectedPage == 3 || selectedPage == 4) && _pageCapacity > BtnCapacity && _previousFiveBtn.Visibility == Visibility.Visible)
            {
                RebuildPage(4);
                SelectedIndex(selectedPage - 2);
                ShowBtn(_nextFiveBtn);
            }
            else if ((selectedPage == 3 || selectedPage == 4) && _pageCapacity > BtnCapacity && _previousFiveBtn.Visibility == Visibility.Collapsed)
            {
                RebuildPage(selectedPage);
                SelectedIndex(selectedPage - 2);
            }
        }

        private int CalWidth(int num)
        {
            var len = num.ToString().Length == 1 ? 2 : num.ToString().Length;
            var width = 30;
            width += 6 * (len - 2);
            return width;
        }

        private void RebuildPage(int selectedPage)
        {
            var firstPage = selectedPage - _midIndex;
            if (selectedPage == 1)
                DisableBtn(_previousBtn);
            if (selectedPage == _pageCapacity)
                DisableBtn(_nextBtn);
            for (int i = 0; i < _panel.Children.Count; i++)
            {
                var btn = _panel.Children[i] as RadioButtonEx;
                btn.Content = firstPage + i;
                btn.Visibility = Visibility.Visible;
                btn.Width = CalWidth(firstPage + BtnCapacity);
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

        private void SelectedIndex(int index)
        {
            UncheckRB();
            (_panel.Children[index] as RadioButtonEx).IsChangedBackground = true;
        }

        private void UncheckRB()
        {
            foreach (var item in _panel.Children)
            {
                var b = item as RadioButtonEx;
                b.IsChecked = false;
                b.IsChangedBackground = false;
            }
        }

        private void DisableBtn(ButtonEx btn)
        {
            btn.Cursor = Cursors.No;
        }

        private void AbleBtn(ButtonEx btn)
        {
            btn.Cursor = Cursors.Hand;
        }

        private void ShowBtn(ButtonEx btn)
        {
            if (_pageCapacity > 6)
                btn.Visibility = Visibility.Visible;
        }

        private void FirstLastUnchecked()
        {
            _firstPageBtn.IsChecked = false;
            _firstPageBtn.IsChangedBackground = false;
            _lastPageBtn.IsChecked = false;
            _lastPageBtn.IsChangedBackground = false;
        }
    }
}