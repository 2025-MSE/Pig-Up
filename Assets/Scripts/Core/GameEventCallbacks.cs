using System;
using System.Collections.Generic;

namespace MSE.Core
{
    public class GameEventCallbacks
    {
        public static Action<Block, int[]> OnBlockBuilt;
    }
}
