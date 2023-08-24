using System.Collections.Generic;
using UnityEngine;

public class Skin : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> _meshRenderers;

    public void UpdateMaterial(Material material)
    {
        foreach (MeshRenderer meshRenderer in _meshRenderers)
        {
            meshRenderer.material = material;
        }
    }

    public static Material CloneMaterial(Material baseMaterial, Color color)
    {
        Material newMaterial = new Material(baseMaterial);
        newMaterial.color = color;
        return newMaterial;
    }
}
