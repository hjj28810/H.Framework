using H.Framework.Core.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace H.Framework.Core.Utilities
{
    public static class TreeExtensions
    {
        public static IEnumerable<TItem> BuildTree<T, TId, TItem>(this IEnumerable<T> source,
            Func<T, TId> idSelector, Func<T, TId> pidSelector,
            Func<T, IEnumerable<TItem>, TItem> selector, TId rootId = default)
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
                Func<T, NodeItem<TId, TModel>> selector, TId rootId = default)
        {
            Func<T, IEnumerable<NodeItem<TId, TModel>>, NodeItem<TId, TModel>> newSelector = (m, c) =>
              {
                  var n = selector(m);
                  n.Children = c;
                  return n;
              };

            return BuildTree(source, idSelector, pidSelector, newSelector, rootId);
        }

        public static IEnumerable<T> BuildLineList<T, TId>(this IEnumerable<T> source,
                Func<T, TId> idSelector, Func<T, TId> pidSelector,
                Func<T, T> selector, TId rootId, List<T> all = null)
        {
            if (all == null)
                all = new List<T>();
            var hasParent = false;
            var parentID = default(TId);
            foreach (var item in source)
            {
                if (idSelector(item).Equals(rootId))
                {
                    parentID = pidSelector(item);
                    all.Add(item.MapTo(selector));
                    if (parentID != null)
                        hasParent = true;
                    break;
                }
            }
            if (hasParent)
                return BuildLineList(source, idSelector, pidSelector, selector, parentID, all);
            return all;
        }

        public static T BuildLine<T, TId>(this IEnumerable<T> source,
                Func<T, TId> idSelector, Func<T, TId> pidSelector, Func<T, T> pSelector, Func<T, T, T> selector, TId leafID, List<T> all = null)

        {
            var list = source.BuildLineList(idSelector, pidSelector, (m) => selector(m, default), leafID);
            return list.ProcessLine(new Dictionary<TId, T>(), idSelector, pidSelector, pSelector, (m, n) => selector(m, n), list.FirstOrDefault());
        }

        private static T ProcessLine<T, TId>(this IEnumerable<T> source, Dictionary<TId, T> dict, Func<T, TId> idSelector, Func<T, TId> pidSelector, Func<T, T> pSelector, Func<T, T, T> selector, T argModel = default(T))
        {
            var pModel = argModel.ProcessParent(pSelector);
            var model = pModel == null ? argModel : pModel;
            var hasParent = model == null;
            var parentID = default(TId);
            if (model != null)
                parentID = pidSelector(model);
            foreach (var item in source)
            {
                var id = idSelector(item);
                if (!hasParent && model != null)
                    hasParent = parentID.Equals(id);

                if (model == null || id.Equals(parentID))
                {
                    model = selector(model, item);
                    dict.Add(parentID, model);
                    break;
                }
            }

            return hasParent ? ProcessLine(source, dict, idSelector, pidSelector, pSelector, selector, model) : ProcessParent(model, dict, idSelector, selector);
        }

        private static T ProcessParent<T>(this T model, Func<T, T> pSelector)
        {
            var pModel = pSelector(model);
            if (pModel == null)
                return model;
            return ProcessParent(pModel, pSelector);
        }

        private static T ProcessParent<TId, T>(this T model, Dictionary<TId, T> dict, Func<T, TId> idSelector, Func<T, T, T> selector)
        {
            var id = idSelector(model);//得到model_id
            if (dict.TryGetValue(id, out T value))//获取model_id关联的子对象（已经包含model_id为父级）
            {
                var childID = idSelector(value);//获取model子对象的id
                if (dict.TryGetValue(childID, out T childValue))//获取子对象id关联的子对象（已经包含子对象id为父级）
                    return ProcessParent(selector(childValue, value), dict, idSelector, selector);
                else
                    return selector(value, model);
            }
            return model;
        }

        public static IEnumerable<T> ProcessList<T>(this IEnumerable<NodeItem<string, T>> list, Func<T, IEnumerable<T>> idSelector)
        {
            var listModel = new List<T>();
            foreach (var item in list)
            {
                listModel.Add(item.OriginalModel);
                if (idSelector.Invoke(item.OriginalModel).NotNullAny())
                    listModel.AddRange(ProcessList(item.Children, idSelector));
            }
            return listModel;
        }

        public static IEnumerable<T> GetChildren<T, TID>(this IEnumerable<T> list, Func<T, TID> idSelector, Func<T, TID> pidSelector, TID rootID)
        {
            var result = new List<T>();
            //获取Children
            var subordinate = list.Where(e => pidSelector(e).Equals(rootID)).ToList();
            //如果存在Children
            if (subordinate.NotNullAny())
            {
                result.AddRange(subordinate);
                foreach (var subo in subordinate)
                {
                    result.AddRange(GetChildren(list, idSelector, pidSelector, idSelector(subo)));
                }
            }
            return result;
        }
    }

    public class NodeItem<T, TModel>
    {
        //public class NodeState
        //{
        //    public bool Opened { get; set; }
        //    public bool Selected { get; set; }
        //    public bool Disabled { get; set; }
        //}

        //public NodeItem()
        //{
        //    State = new NodeState();
        //}

        public T ID { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        public string Type { get; set; }

        ///// <summary>
        ///// 节点图标
        ///// </summary>
        //public string Icon { get; set; }

        ///// <summary>
        ///// 选中状态
        ///// </summary>
        //public NodeState State { get; set; }

        public string Value { get; set; }
        public string ExtJson { get; set; }

        public TModel OriginalModel { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public IEnumerable<NodeItem<T, TModel>> Children { get; set; }
    }
}