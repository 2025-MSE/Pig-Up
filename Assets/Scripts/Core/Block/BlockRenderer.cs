/// <summary>
/// Author: Dongjin Kuk
/// Description: We control the block's transparency by this component.
/// </summary>

using System.Collections.Generic;
using UnityEngine;

namespace MSE.Core
{
    public class BlockRenderer : MonoBehaviour
    {
        private List<Renderer> m_Renderers = new List<Renderer>();

        private void Awake()
        {
            if (transform.TryGetComponent(out Renderer renderer))
            {
                m_Renderers.Add(renderer);
            }
            FindRenderer(transform);
        }

        public void SetTransparency(float alpha)
        {
            foreach (Renderer renderer in m_Renderers)
            {
                foreach (Material mat in renderer.materials)
                {
                    Color color = mat.color;
                    color.a = alpha;
                    mat.color = color;
                }
            }
        }

        private void FindRenderer(Transform root)
        {
            foreach (Transform tr in root)
            {
                if (tr.TryGetComponent(out Renderer renderer))
                {
                    m_Renderers.Add(renderer);
                }

                FindRenderer(tr);
            }
        }
    }
}