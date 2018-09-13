using System;
using System.Collections.Generic;
using System.Linq;

namespace H.Framework.Core.Utilities
{
    public static class TreeExtensions
    {
        public static IEnumerable<TItem> BuildTree<T, TId, TItem>(this IEnumerable<T> source,
            Func<T, TId> idSelector, Func<T, TId> pidSelector,
            Func<T, IEnumerable<TItem>, TItem> selector, TId rootId = default(TId))
        {
            var dict = new Dictionary<TId, List<T>>();
            foreach (var item in source)
            {
                var pid = pidSelector(item);
                if (!dict.TryGetValue(pid, out List<T> list))
                {
                    list = new List<T>();
                    dict.Add(pid, list);
                }

                list.Add(item);
            }

            return ProcessDict(dict, idSelector, selector, rootId);
        }

        private static IEnumerable<TItem> ProcessDict<T, TId, TItem>(IDictionary<TId, List<T>> dict,
          Func<T, TId> idSelector, Func<T, IEnumerable<TItem>, TItem> selector, TId pid)
        {
            var all = new List<TItem>();

            if (dict.TryGetValue(pid, out List<T> items))
            {
                all.AddRange(items.Select(m =>
                {
                    var id = idSelector(m);
                    var children = ProcessDict(dict, idSelector, selector, id);
                    return selector(m, children);
                }));
            }

            return all;
        }

        public static IEnumerable<NodeItem<TId, TModel>> BuildTree<T, TId, TModel>(this IEnumerable<T> source,
                Func<T, TId> idSelector, Func<T, TId> pidSelector,
                Func<T, NodeItem<TId, TModel>> selector, TId rootId = default(TId))
        {
            Func<T, IEnumerable<NodeItem<TId, TModel>>, NodeItem<TId, TModel>> newSelector = (m, c) =>
              {
                  var n = selector(m);
                  n.Children = c;
                  return n;
              };

            return BuildTree(source, idSelector, pidSelector, newSelector, rootId);
        }
    }

    public class NodeItem<T, TModel>
    {
        public class NodeState
        {
            public bool Opened { get; set; }
            public bool Selected { get; set; }
            public bool Disabled { get; set; }
        }

        public NodeItem()
        {
            State = new NodeState();
        }

        public T ID { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 节点图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 选中状态
        /// </summary>
        public NodeState State { get; set; }

        public string Code { get; set; }
        public int OrderNumber { get; set; }

        public TModel OriginalModel { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public IEnumerable<NodeItem<T, TModel>> Children { get; set; }
    }
}