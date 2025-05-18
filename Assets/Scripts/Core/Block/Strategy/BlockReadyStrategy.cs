using UnityEngine;

namespace MSE.Core
{
    public class BlockReadyStrategy : BlockStrategyBase
    {
        public BlockReadyStrategy(Block block) : base(block)
        {
        }

        public override void Initialize()
        {
            m_Block.BoundaryObj.SetActive(false);
            m_Block.Detection.gameObject.SetActive(true);
            m_Block.DetecteeObj.SetActive(false);
            m_Block.SelectionObj.SetActive(false);
            m_Block.Renderer.SetTransparency(0.5f);
        }
    }
}
