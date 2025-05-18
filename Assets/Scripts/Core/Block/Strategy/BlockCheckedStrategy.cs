namespace MSE.Core
{
    public class BlockCheckedStrategy : BlockStrategyBase
    {
        public BlockCheckedStrategy(Block block) : base(block)
        {
        }

        public override void Initialize()
        {
            m_Block.SetChecked(true);
        }
    }
}
