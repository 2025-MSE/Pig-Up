using System.Collections.Generic;
using UnityEngine;

namespace MSE.Core
{
    public class BlockRenderer : MonoBehaviour
    {
        private List<Renderer> m_Renderers = new List<Renderer>();

        private void Awake()
        {
            Renderer rRenderer = GetComponent<Renderer>();
            m_Renderers.Add(rRenderer);
            foreach (Transform tr in transform)
            {
                Renderer cRenderer = tr.GetComponent<Renderer>();
                m_Renderers.Add(cRenderer);
            }
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
    }
}