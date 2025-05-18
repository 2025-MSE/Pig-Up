namespace MSE.Core
{
    public abstract class BlockStrategyBase
    {
        protected Block m_Block;

        public BlockStrategyBase(Block block)
        {
            m_Block = block;
        }

        public abstract void Initialize();
    }
}
