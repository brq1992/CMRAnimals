using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;


[System.Serializable]
public class MeshAssetRule : ScriptableObject
{
//#if UNITY_5_6_OR_NEWER
    public ModelImporterMeshCompression m_MeshCompression;
    public bool m_IsReadable;
    public bool m_ImportBlendShapes;
//#else if UNITY_2017_2_OR_NEWER

//#endif
    //model
    public bool m_ImportMaterials;
    public bool optimizeMeshForGPU;
    public bool m_AddColliders;
    public bool keepQuads;
    public bool swapUVChannels;
    public bool generateSecondaryUV;
    public float secondaryUVHardAngle;
    public float secondaryUVPackMargin;
    public float secondaryUVAngleDistortion;
    public float secondaryUVAreaDistortion;
    public float normalSmoothAngle;
    public ModelImporterNormals normalImportMode;
    public ModelImporterTangents tangentImportMode;
    public ModelImporterMaterialName m_MaterialName;
    public ModelImporterMaterialSearch m_MaterialSearch;
    public bool m_weldVertices;
    //

    //Rig
    public ModelImporterAnimationType AnimationType;
    public bool isOptimizeObject;
    //


    //Animation
    public bool ImportAnimation;
    public bool GenerateAnimatorController;
    public bool IsLoop;
    //
    public static MeshAssetRule CreateAssetRule()
    {
        MeshAssetRule assetRule = CreateInstance<MeshAssetRule>();
        assetRule.ApplyDefaults();
        return assetRule;
    }

    public void ApplyDefaults()
    {
        m_MeshCompression = ModelImporterMeshCompression.Off;
        m_IsReadable = false;
        m_ImportBlendShapes = false;
        m_AddColliders = false;
        keepQuads = false;
        m_weldVertices = true;
        swapUVChannels = false;
        generateSecondaryUV = false;
        normalImportMode = ModelImporterNormals.None;
        tangentImportMode = ModelImporterTangents.None;
        m_ImportMaterials = true;
        m_MaterialName = ModelImporterMaterialName.BasedOnMaterialName;
        m_MaterialSearch = ModelImporterMaterialSearch.Everywhere;

        AnimationType = ModelImporterAnimationType.None;

        ImportAnimation = false;
    }

    public void ApplyMeshSettings(AssetImporter assetImporter)
    {
        ModelImporter importer = assetImporter as ModelImporter;

        if (importer != null)
            ApplyMeshSettings((ModelImporter) assetImporter);
    }

    void ApplyMeshSettings(ModelImporter importer)
    {
        bool dirty = false;

#if UNITY_5_6_2

        if (importer.weldVertices != m_weldVertices)
        {
            importer.weldVertices = m_weldVertices;
            dirty = true;
        }

#endif
        if (importer.meshCompression != m_MeshCompression)
        {
            importer.meshCompression = m_MeshCompression;
            dirty = true;
        }

        if (importer.isReadable != m_IsReadable)
        {
            importer.isReadable = m_IsReadable;
            dirty = true;
        }

        if (importer.optimizeMesh != optimizeMeshForGPU)
        {
            importer.optimizeMesh = optimizeMeshForGPU;
            dirty = true;
        }

        if (importer.importBlendShapes != m_ImportBlendShapes)
        {
            importer.importBlendShapes = m_ImportBlendShapes;
            dirty = true;
        }

        if (importer.addCollider != m_AddColliders)
        {
            importer.addCollider = m_AddColliders;
            dirty = true;
        }

        if (importer.keepQuads != keepQuads)
        {
            importer.keepQuads = keepQuads;
            dirty = true;
        }

        if (importer.swapUVChannels != swapUVChannels)
        {
            importer.swapUVChannels = swapUVChannels;
            dirty = true;
        }

        if (importer.generateSecondaryUV != generateSecondaryUV)
        {
            importer.generateSecondaryUV = generateSecondaryUV;
            dirty = true;
        }


        //Normals & Tangents
        if (importer.importNormals != normalImportMode)
        {
            importer.importNormals = normalImportMode;
            dirty = true;
        }

        if (importer.importNormals == ModelImporterNormals.Calculate)
        {
            importer.normalSmoothingAngle = normalSmoothAngle;
        }

        if (importer.importTangents != tangentImportMode)
        {
            importer.importTangents = tangentImportMode;
            dirty = true;
        }

        //materials
        if (importer.importMaterials != m_ImportMaterials)
        {
            importer.importMaterials = m_ImportMaterials;
        }

        if (importer.importMaterials)
        {
            importer.materialName = m_MaterialName;
            importer.materialSearch = m_MaterialSearch;
        }
        //

        if (importer.animationType != AnimationType)
        {
            importer.animationType = AnimationType;
            importer.optimizeGameObjects = isOptimizeObject;
        }

        if (importer.importAnimation != ImportAnimation)
        {
            importer.importAnimation = ImportAnimation;
        }
        Debug.Log("tes");
        if (importer.importAnimation&&importer.clipAnimations.Length != 0)
        {
            Debug.Log("test");
            if (GenerateAnimatorController)
            {
                Debug.Log("test!");
            }
            if (IsLoop)
            {
                for (int i = 0; i < importer.clipAnimations.Length; i++)
                {
                    importer.clipAnimations[i].loop = true;
                }
            }
            
        }

        if (dirty)
        {
            Debug.Log("Modifying Model Import Settings, An Import will now occur and the settings will be checked to be OK again during that import");
            importer.SaveAndReimport();
        }
        else
        {
            Debug.Log("Models' Import Settings OK");
        }
    }

    public bool IsMatch(AssetImporter importer)
    {
        if (importer is ModelImporter)
        {
            return true;
        }
        return false;
    }
}
