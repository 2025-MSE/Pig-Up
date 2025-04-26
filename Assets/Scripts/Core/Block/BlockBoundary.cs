using UnityEngine;

namespace MSE.Core
{
    public class BlockBoundary : MonoBehaviour
    {
        public void SetBoundaryActive(bool isActive)
        {
            gameObject.layer = LayerMask.NameToLayer(isActive ? "BlockBoundary" : "Default");
        }
    }
}
