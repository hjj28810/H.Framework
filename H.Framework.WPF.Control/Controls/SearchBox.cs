using H.Framework.WPF.Control.Adorners;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace H.Framework.WPF.Control.Controls
{
    [TemplatePart(Name = "PART_Cancel", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Textbox", Type = typeof(TextBox))]
    public class SearchBox : System.Windows.Controls.Control
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private TextBox txtBox;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            txtBox = GetTemplateChild("PART_Textbox") as TextBox;
            var cancelButton = GetTemplateChild("PART_Cancel") as Button;
            cancelButton.Click += (s1, e1) => { Text = string.Empty; txtBox.Focus(); };
        }

        static SearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchBox), new FrameworkPropertyMetadata(typeof(SearchBox)));
            ClipToBoundsProperty.OverrideMetadata(typeof(SearchBox), new FrameworkPropertyMetadata(true));
        }

        public SearchBox()
        {
            GotFocus += (s1, e1) => RemoveWatermarkAdorner();// AdornerLayer.GetAdornerLayer(this).Add(focusAdorner);
            LostFocus += (s1, e1) => { if (IsVisible) ShowWatermarkAdorner(); };// AdornerLayer.GetAdornerLayer(this).Remove(focusAdorner);
            IsVisibleChanged += SearchBox_IsVisibleChanged;
        }

        private void SearchBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var txt = (SearchBox)sender;
            var value = (bool)e.NewValue;
            //Trace.WriteLine("SearchBox_IsVisible:" + value);
            if (value)
                ShowWatermarkAdorner();
            else
                RemoveWatermarkAdorner();
        }

        //private void ShowFocusAdorner()
        //{
        //    var layer = AdornerLayer.GetAdornerLayer(this);
        //    if (layer != null)
        //    {
        //        if (layer.GetAdorners(this) == null)
        //        {
        //            layer.Add(new SearchBoxFocusAdorner(this));
        //        }
        //        else if (layer.GetAdorners(this).ToList().OfType<SearchBoxFocusAdorner>().Count() < 1)
        //        {
        //            layer.Add(new SearchBoxFocusAdorner(this));
        //        }
        //    }
        //}

        //private void RemoveFocusAdorner()
        //{
        //    var layer = AdornerLayer.GetAdornerLayer(this);
        //    if (layer != null)
        //    {
        //        if (layer.GetAdorners(this) != null)
        //        {
        //            layer.GetAdorners(this).OfType<SearchBoxFocusAdorner>().ToList().ForEach(p => layer.Remove(p));
        //        }
        //    }
        //    //ShowWatermarkAdorner();
        //}

        private void ShowWatermarkAdorner()
        {
            var layer = AdornerLayer.GetAdornerLayer(this);
            if (layer != null && txtBox != null && !txtBox.IsFocused && string.IsNullOrWhiteSpace(txtBox.Text))
            {
                if (layer.GetAdorners(txtBox) == null)
                {
                    layer.Add(new WatermarkAdorner(txtBox));
                }
                else if (layer.GetAdorners(txtBox).ToList().OfType<WatermarkAdorner>().Count() < 1)
                {
                    layer.Add(new WatermarkAdorner(txtBox));
                }
            }
        }

        private void RemoveWatermarkAdorner()
        {
            var layer = AdornerLayer.GetAdornerLayer(this);
            if (layer != null && txtBox != null)
            {
                if (layer.GetAdorners(txtBox) != null)
                {
                    layer.GetAdorners(txtBox).OfType<WatermarkAdorner>().ToList().ForEach(p => layer.Remove(p));
                }
            }
        }

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(string), typeof(SearchBox), new UIPropertyMetadata("Search"));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SearchBox), new UIPropertyMetadata(null, OnTextPropertyChangedCallback));

        public static void OnTextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = d as SearchBox;
            if (string.IsNullOrWhiteSpace(e.NewValue.ToString()))
                source.ShowWatermarkAdorner();
            else
                source.RemoveWatermarkAdorner();
        }

        public FontFamily CaneclFontFamily
        {
            get { return (FontFamily)GetValue(CaneclFontFamilyProperty); }
            set { SetValue(CaneclFontFamilyProperty, value); }
        }

        public static readonly DependencyProperty CaneclFontFamilyProperty =
            DependencyProperty.Register("CaneclFontFamily", typeof(FontFamily), typeof(SearchBox), new UIPropertyMetadata(new FontFamily("Webdings")));
    }
}