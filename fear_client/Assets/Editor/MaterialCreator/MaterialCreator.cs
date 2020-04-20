using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// Editor script. Extends the Unity editor to automate the creation of materials based on filenames.
/// </summary>
public class CreateMaterial : Editor
{
    [MenuItem("Tools/CreateMaterialForTexture")]

    static void CreateMaterials()
    {
        try
        {
            AssetDatabase.StartAssetEditing();
            var textures = Selection.GetFiltered(typeof(Texture), SelectionMode.Assets).Cast<Texture>(); // Get an array of Texures
            string path = AssetDatabase.GetAssetPath(textures.First());
            path = path.Substring(0, path.LastIndexOf("_")) + ".mat"; // So long as there's an albedo texture this works

            if (AssetDatabase.LoadAssetAtPath(path, typeof(Material)) != null)
            {
                Debug.LogWarning("Deleting old material: " + path);
                AssetDatabase.DeleteAsset(path);
            }
            var mat = new Material(Shader.Find("Standard"));

            foreach (var tex in textures)
            {
                Debug.Log(tex.name);
                switch (tex.name) // Depending on the file name, it will assign the appropriate texture.
                {
                    case var ambientOcclusion when new Regex(".*AO").IsMatch(ambientOcclusion):
                        mat.SetTexture("_OcclusionMap", tex);
                        break;
                    case var albedoTransparency when new Regex(".*AlbedoTransparency").IsMatch(albedoTransparency):
                        mat.SetTexture("_MainTex", tex);
                        break;
                    case var emission when new Regex(".*Emission").IsMatch(emission):
                        mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive; // Enables emission texture in material.
                        mat.SetColor("_EmissionColor", Color.white); // Set emission to white, since this is default when creating a mat manually.
                        mat.EnableKeyword("_EMISSION"); // Enables emission in the shader.
                        mat.SetTexture("_EmissionMap", tex);
                        break;
                    case var metallicSmoothness when new Regex(".*MetallicSmoothness").IsMatch(metallicSmoothness):
                        mat.SetTexture("_MetallicGlossMap", tex);
                        break;
                    case var normal when new Regex(".*Normal").IsMatch(normal):
                        mat.SetTexture("_BumpMap", tex);
                        break;
                    default:
                        Debug.LogError("Unknown texture name");
                        break;

                }
            }
            AssetDatabase.CreateAsset(mat, path);
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.SaveAssets();
        }
    }
}