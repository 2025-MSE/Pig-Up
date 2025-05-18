using System;

namespace MSE.Core
{
    public class GameEventCallbacks
    {
        public static Action<Building> OnBuildingSpawned;
        public static Action<Block, int[]> OnBlockBuilt;
    }
}
