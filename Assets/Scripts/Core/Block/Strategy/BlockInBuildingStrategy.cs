/// <summary>
/// Author: Dongjin Kuk
/// Description: The strategy when the block is in building(spawned by the building).
/// </summary>

namespace MSE.Core
{
    public class BlockInBuildingStrategy : BlockStrategyBase
    {
        public BlockInBuildingStrategy(Block block) : base(block)
        {
        }

        public override void Initialize()
        {
            m_Block.BoundaryObj.SetActive(false);
            m_Block.Detection.gameObject.SetActive(false);
            m_Block.DetecteeObj.SetActive(true);
            m_Block.SelectionObj.SetActive(false);
            m_Block.Renderer.SetTransparency(0.2f);
        }
    }
}
