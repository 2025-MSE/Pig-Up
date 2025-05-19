namespace MSE.Core
{
    public class BlockInFieldStrategy : BlockStrategyBase
    {
        public BlockInFieldStrategy(Block block) : base(block)
        {
        }

        public override void Initialize()
        {
            m_Block.BoundaryObj.SetActive(false);
            m_Block.Detection.gameObject.SetActive(false);
            m_Block.DetecteeObj.SetActive(false);
            m_Block.SelectionObj.SetActive(true);
            m_Block.Renderer.SetTransparency(1f);
        }
    }
}
