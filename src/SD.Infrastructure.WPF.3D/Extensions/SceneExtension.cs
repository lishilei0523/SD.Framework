using HelixToolkit.SharpDX.Model.Scene;
using System.Collections.Generic;

namespace SD.Infrastructure.WPF.ThreeDims.Extensions
{
    /// <summary>
    /// 场景扩展
    /// </summary>
    public static class SceneExtension
    {
        #region # 获取Mesh节点 —— static MeshNode GetMeshNode(this SceneNode sceneNode)
        /// <summary>
        /// 获取Mesh节点
        /// </summary>
        /// <param name="sceneNode">场景节点</param>
        /// <returns>Mesh节点</returns>
        /// <remarks>如果没有，返回null</remarks>
        public static MeshNode GetMeshNode(this SceneNode sceneNode)
        {
            if (sceneNode is MeshNode meshNode)
            {
                return meshNode;
            }
            if (sceneNode is GroupNode groupNode)
            {
                return GetMeshNode(groupNode.Items[0]);
            }

            return null;
        }
        #endregion

        #region # 获取Mesh节点列表 —— static ICollection<MeshNode> GetMeshNodes(this SceneNode...
        /// <summary>
        /// 获取Mesh节点列表
        /// </summary>
        /// <param name="sceneNode">场景节点</param>
        /// <returns>Mesh节点列表</returns>
        public static ICollection<MeshNode> GetMeshNodes(this SceneNode sceneNode)
        {
            ICollection<MeshNode> meshNodes = new HashSet<MeshNode>();
            sceneNode.GetMeshNodes(meshNodes);

            return meshNodes;
        }
        #endregion


        //Private

        #region # 获取Mesh节点列表 —— static void GetMeshNodes(this SceneNode sceneNode...
        /// <summary>
        /// 获取Mesh节点列表
        /// </summary>
        /// <param name="sceneNode">场景节点</param>
        /// <param name="meshNodes">Mesh节点列表</param>
        private static void GetMeshNodes(this SceneNode sceneNode, ICollection<MeshNode> meshNodes)
        {
            if (sceneNode is MeshNode meshNode)
            {
                meshNodes.Add(meshNode);
            }
            if (sceneNode is GroupNode groupNode)
            {
                foreach (SceneNode groupNodeItem in groupNode.Items)
                {
                    groupNodeItem.GetMeshNodes(meshNodes);
                }
            }
        }
        #endregion
    }
}
