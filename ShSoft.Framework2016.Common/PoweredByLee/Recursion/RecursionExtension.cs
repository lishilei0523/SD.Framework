using System.Collections.Generic;
using System.Linq;

namespace ShSoft.Framework2016.Common.PoweredByLee.Recursion
{
    /// <summary>
    /// 递归扩展方法工具类
    /// </summary>
    public static class RecursionExtension
    {
        #region # 递归扩展方法 —— static void Recursion<T>(this T node, ICollection<T> collection)
        /// <summary>
        /// 递归扩展方法，
        /// 自上向下递归
        /// </summary>
        /// <typeparam name="T">可递归类型</typeparam>
        /// <param name="node">节点</param>
        /// <param name="collection">可递归类型集</param>
        public static void Recursion<T>(this T node, ICollection<T> collection) where T : IRecursive<T>
        {
            if (!node.SubNodes.IsNullOrEmpty())
            {
                foreach (T subNode in node.SubNodes)
                {
                    collection.Add(subNode);
                    Recursion(subNode, collection);
                }
                collection.Add(node);
            }
        }
        #endregion

        #region # 尾递归扩展方法 —— static void TailRecursion<T>(this IEnumerable<T> nodes...
        /// <summary>
        /// 尾递归扩展方法，
        /// 自下向上递归
        /// </summary>
        /// <typeparam name="T">可递归类型</typeparam>
        /// <param name="nodes">节点集</param>
        /// <param name="collection">可递归类型集</param>
        public static void TailRecursion<T>(this IEnumerable<T> nodes, HashSet<T> collection) where T : IRecursive<T>
        {
            foreach (T node in nodes)
            {
                if (node.ParentNode != null)
                {
                    TailRecursion(nodes.Select(x => x.ParentNode), collection);
                }
            }

            collection.AddRange(nodes);
        }
        #endregion
    }
}
