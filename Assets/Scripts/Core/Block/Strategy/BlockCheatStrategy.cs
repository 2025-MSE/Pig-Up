/// <summary>
/// Author: Dongjin Kuk
/// Description: The strategy when the block is checked by the cheat.
/// </summary>

namespace MSE.Core
{
    public class BlockCheatStrategy : BlockStrategyBase
    {
        public BlockCheatStrategy(Block block) : base(block) { }

        public override void Initialize()
        {
            m_Block.BoundaryObj.SetActive(true);
            m_Block.Detection.gameObject.SetActive(false);
            m_Block.DetecteeObj.SetActive(false);
            m_Block.SelectionObj.SetActive(false);
            m_Block.Renderer.SetTransparency(1.0f);
        }
    }
}
