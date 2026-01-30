using System;
using NaughtyAttributes;
using UnityEngine;

namespace GGJ_2026
{
	public class MaterialSetter : MyMonoBehaviour
	{
		[SerializeField] private MeshRenderer meshRenderer;
		[SerializeField] [OnValueChanged(nameof(OnMaterialValueChanged))] private Material material;

        private void OnMaterialValueChanged()
        {
            SetMaterial(material);
        }

        public void SetMaterial(Material material)
        {
            this.material = material;
            UpdateMaterial();
        }

		[Button]
		private void UpdateMaterial()
		{
			meshRenderer.sharedMaterial = material;
		}
    }
}