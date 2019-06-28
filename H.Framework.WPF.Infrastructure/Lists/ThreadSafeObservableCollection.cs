using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;

namespace H.Framework.WPF.Infrastructure.Lists
{
    public class ThreadSafeObservableCollection<T> : ObservableCollection<T>
    {
        private readonly ReaderWriterLockSlim _lockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            //base.OnCollectionChanged(e);

            NotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler = CollectionChanged;

            if (notifyCollectionChangedEventHandler != null)
            {
                var list = notifyCollectionChangedEventHandler.GetInvocationList();
                foreach (var @delegate in list)
                {
                    var handler = (NotifyCollectionChangedEventHandler)@delegate;
                    if (handler.Target is DispatcherObject dispatcherObject)
                    {
                        var dispatcher = dispatcherObject.Dispatcher;
                        if (dispatcher != null && !dispatcher.CheckAccess())
                        {
                            dispatcher.BeginInvoke((Action)(() => handler.Invoke(this,
                                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))),
                                DispatcherPriority.DataBind);

                            continue;
                        }
                    }

                    handler.Invoke(this, e);
                }
            }
        }

        protected override void SetItem(int index, T item)
        {
            _lockSlim.EnterWriteLock();

            try
            {
                base.SetItem(index, item);
            }
            finally
            {
                _lockSlim.ExitWriteLock();
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            _lockSlim.EnterUpgradeableReadLock();

            try
            {
                _lockSlim.EnterWriteLock();

                try
                {
                    base.OnPropertyChanged(e);
                }
                finally
                {
                    _lockSlim.ExitWriteLock();
                }
            }
            finally
            {
                _lockSlim.ExitUpgradeableReadLock();
            }
        }

        protected override void InsertItem(int index, T item)
        {
            _lockSlim.EnterWriteLock();

            try
            {
                base.InsertItem(index, item);
            }
            finally
            {
                _lockSlim.ExitWriteLock();
            }
        }

        protected override void RemoveItem(int index)
        {
            _lockSlim.EnterWriteLock();

            try
            {
                base.RemoveItem(index);
            }
            finally
            {
                _lockSlim.ExitWriteLock();
            }
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            _lockSlim.EnterWriteLock();

            try
            {
                base.MoveItem(oldIndex, newIndex);
            }
            finally
            {
                _lockSlim.ExitWriteLock();
            }
        }

        protected override void ClearItems()
        {
            _lockSlim.EnterWriteLock();

            try
            {
                base.ClearItems();
            }
            finally
            {
                _lockSlim.ExitWriteLock();
            }
        }

        public override bool Equals(object obj)
        {
            if (!_lockSlim.IsWriteLockHeld)
            {
                _lockSlim.EnterReadLock();
            }

            try
            {
                return base.Equals(obj);
            }
            finally
            {
                if (!_lockSlim.IsWriteLockHeld)
                {
                    _lockSlim.ExitReadLock();
                }
            }
        }

        public override int GetHashCode()
        {
            if (!_lockSlim.IsWriteLockHeld)
            {
                _lockSlim.EnterReadLock();
            }

            try
            {
                return base.GetHashCode();
            }
            finally
            {
                if (!_lockSlim.IsWriteLockHeld)
                {
                    _lockSlim.ExitReadLock();
                }
            }
        }
    }
}