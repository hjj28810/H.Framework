using H.Framework.WPF.Infrastructure.Lists;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace H.Framework.WPF.Infrastructure.Utilities
{
    public static class Extensions
    {
        public static ThreadSafeObservableCollection<TChild> CopyToChild<T, TChild>(this ThreadSafeObservableCollection<T> Enum) where TChild : class, new()
        {
            T _t = (T)Activator.CreateInstance(typeof(T));
            TChild _tChild = (TChild)Activator.CreateInstance(typeof(TChild));
            PropertyInfo[] selfPropertys = _t.GetType().GetProperties();
            PropertyInfo[] childPropertys = _tChild.GetType().GetProperties();
            ThreadSafeObservableCollection<TChild> ChildEnum = new ThreadSafeObservableCollection<TChild>();
            foreach (T selfItem in Enum)
            {
                TChild childItem = new TChild();
                foreach (var property in selfPropertys)
                {
                    foreach (var childProperty in childPropertys)
                    {
                        if (property.Name == childProperty.Name)
                            property.SetValue(childItem, property.GetValue(selfItem, null), null);
                    }
                }
                ChildEnum.Add(childItem);
            }
            return ChildEnum;
        }

        public static T FindVisualParent<T>(this DependencyObject obj) where T : class
        {
            while (obj != null)
            {
                if (obj is T)
                    return obj as T;

                obj = VisualTreeHelper.GetParent(obj);
            }
            return null;
        }

        /// 获得指定元素的父元素
        /// </summary>
        /// <typeparam name="T">指定页面元素</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T GetParentObject<T>(this DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);

            while (parent != null)
            {
                if (parent is T)
                {
                    return (T)parent;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        /// <summary>
        /// 获得指定元素的所有子元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<T> GetChildObjects<T>(this DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T)
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child));
            }
            return childList;
        }

        /// <summary>
        /// 查找子元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetChildObject<T>(this DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }
    }
}