using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace H.Framework.WPF.Infrastructure.Lists
{
    [DebuggerDisplay("Count = {Count}")]
    [ComVisible(false)]
    public class ThreadSafeObservableCollection<T> : ObservableCollection<T>
    {
        private readonly Dispatcher dispatcher;

        public ThreadSafeObservableCollection()
            : this(Enumerable.Empty<T>())
        {
        }

        public ThreadSafeObservableCollection(Dispatcher dispatcher)
            : this(Enumerable.Empty<T>(), dispatcher)
        {
        }

        public ThreadSafeObservableCollection(IEnumerable collection)
            : this(collection, Dispatcher.CurrentDispatcher)
        {
        }

        public ThreadSafeObservableCollection(IEnumerable collection, Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;

            foreach (T item in collection)
            {
                this.Add(item);
            }
        }

        protected override void SetItem(int index, T item)
        {
            this.ExecuteOrInvoke(() => this.SetItemBase(index, item));
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            this.ExecuteOrInvoke(() => this.MoveItemBase(oldIndex, newIndex));
        }

        protected override void ClearItems()
        {
            this.ExecuteOrInvoke(this.ClearItemsBase);
        }

        protected override void InsertItem(int index, T item)
        {
            this.ExecuteOrInvoke(() => this.InsertItemBase(index, item));
        }

        protected override void RemoveItem(int index)
        {
            this.ExecuteOrInvoke(() => this.RemoveItemBase(index));
        }

        private void RemoveItemBase(int index)
        {
            base.RemoveItem(index);
        }

        private void InsertItemBase(int index, T item)
        {
            base.InsertItem(index, item);
        }

        private void ClearItemsBase()
        {
            base.ClearItems();
        }

        private void MoveItemBase(int oldIndex, int newIndex)
        {
            base.MoveItem(oldIndex, newIndex);
        }

        private void SetItemBase(int index, T item)
        {
            base.SetItem(index, item);
        }

        private void ExecuteOrInvoke(Action action)
        {
            if (this.dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                this.dispatcher.Invoke(action);
            }
        }
    }
}