using System;
using System.Windows;
using System.Windows.Controls;

namespace H.Framework.WPF.Infrastructure.Utilities
{
    public class ListBoxExtender : DependencyObject
    {
        #region Properties

        public static readonly DependencyProperty AutoScrollToCurrentItemProperty = DependencyProperty.RegisterAttached("AutoScrollToCurrentItem",
            typeof(bool), typeof(ListBoxExtender), new UIPropertyMetadata(default(bool), OnAutoScrollToCurrentItemChanged));

        /// <summary>
        /// Returns the value of the AutoScrollToCurrentItemProperty
        /// </summary>
        /// <param name="obj">The dependency-object whichs value should be returned</param>
        /// <returns>The value of the given property</returns>
        public static bool GetAutoScrollToCurrentItem(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoScrollToCurrentItemProperty);
        }

        /// <summary>
        /// Sets the value of the AutoScrollToCurrentItemProperty
        /// </summary>
        /// <param name="obj">The dependency-object whichs value should be set</param>
        /// <param name="value">The value which should be assigned to the AutoScrollToCurrentItemProperty</param>
        public static void SetAutoScrollToCurrentItem(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollToCurrentItemProperty, value);
        }

        public static readonly DependencyProperty IsSelectedInitProperty =
          DependencyProperty.RegisterAttached("IsSelectedInit",
              typeof(bool),
              typeof(ListBoxExtender),
              new FrameworkPropertyMetadata(false, OnIsSelectedInitPropertyChanged));

        public static bool GetIsSelectedInit(DependencyObject d)
        {
            return (bool)d.GetValue(IsSelectedInitProperty);
        }

        public static void SetIsSelectedInit(DependencyObject d, bool value)
        {
            d.SetValue(IsSelectedInitProperty, value);
        }

        #endregion Properties

        #region Events

        /// <summary>
        /// This method will be called when the AutoScrollToCurrentItem
        /// property was changed
        /// </summary>
        /// <param name="sender">The sender (the ListBox)</param>
        /// <param name="e">Some additional information</param>
        public static void OnAutoScrollToCurrentItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if ((listBox == null) || (listBox.Items == null))
                return;

            var enable = (bool)e.NewValue;
            var autoScrollToCurrentItemWorker = new EventHandler((_1, _2) => OnAutoScrollToCurrentItem(listBox, listBox.Items.CurrentPosition));

            if (enable)
                listBox.Items.CurrentChanged += autoScrollToCurrentItemWorker;
            else
                listBox.Items.CurrentChanged -= autoScrollToCurrentItemWorker;
        }

        /// <summary>
        /// This method will be called when the ListBox should
        /// be scrolled to the given index
        /// </summary>
        /// <param name="listBox">The ListBox which should be scrolled</param>
        /// <param name="index">The index of the item to which it should be scrolled</param>
        public static void OnAutoScrollToCurrentItem(ListBox listBox, int index)
        {
            if (listBox != null && listBox.Items != null && listBox.Items.Count > index && index >= 0)
                listBox.ScrollIntoView(listBox.Items[index]);
        }

        private static void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = sender as ListBox;
            if (box.SelectedItem != null)
            {
                box.SelectedIndex = -1;
            }
        }

        private static void OnIsSelectedInitPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ListBox box))
                return;

            if (GetIsSelectedInit(d))
            {
                box.SelectionChanged -= SelectionChanged;
                box.SelectionChanged += SelectionChanged;
            }
        }

        #endregion Events
    }
}