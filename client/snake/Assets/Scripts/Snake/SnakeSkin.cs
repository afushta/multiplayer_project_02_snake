using System.Collections.Generic;
using UnityEngine;

public class SnakeSkin : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> _meshRenderers;

    public Material Material { get; private set; }

    public void UpdateMaterial(Material material)
    {
        Material = material;
        foreach (MeshRenderer meshRenderer in _meshRenderers)
        {
            meshRenderer.sharedMaterial = material;
        }
    }

    private static Material CloneMaterial(Material baseMaterial, Color color)
    {
        Material newMaterial = new Material(baseMaterial);
        newMaterial.color = color;
        return newMaterial;
    }

    public static void PrepareMaterial(string colorString, Material baseMaterial, out Material newMaterial)
    {
        newMaterial = baseMaterial;

        if (ColorUtility.TryParseHtmlString(colorString, out Color color))
        {
            newMaterial = CloneMaterial(newMaterial, color);
        }
        else
        {
            Debug.LogWarning($"Failed to parse color string {colorString}");
        }
    }
}
