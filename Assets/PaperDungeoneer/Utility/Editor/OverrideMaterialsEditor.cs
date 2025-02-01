using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

namespace PaperDungeoneer.Utility.Editor
{
    public class OverrideMaterialsEditor : UnityEditor.Editor
    {
        [MenuItem("Assets/Override Materials", false, 30)]
        private static void OverrideMaterials()
        {
            // Get all selected FBX models
            var selectedObjects = Selection.objects
                .Where(obj => obj is GameObject)
                .Cast<GameObject>()
                .Where(go => AssetDatabase.GetAssetPath(go).EndsWith(".fbx"))
                .ToList();

            if (selectedObjects.Count == 0)
            {
                Debug.LogWarning("No FBX models selected.");
                return;
            }

            // Open a material picker
            string materialPath = EditorUtility.OpenFilePanel("Select Material", "Assets", "mat");
            if (string.IsNullOrEmpty(materialPath))
            {
                Debug.LogWarning("No material selected.");
                return;
            }

            // Convert the full path to a relative path
            materialPath = "Assets" + materialPath.Substring(Application.dataPath.Length);
            Material newMaterial = AssetDatabase.LoadAssetAtPath<Material>(materialPath);

            if (newMaterial == null)
            {
                Debug.LogWarning("Selected file is not a valid material.");
                return;
            }

            // Override materials in all selected FBX models
            foreach (var selectedObject in selectedObjects)
            {
                string assetPath = AssetDatabase.GetAssetPath(selectedObject);
                var importer = AssetImporter.GetAtPath(assetPath) as ModelImporter;

                if (importer == null)
                {
                    Debug.LogWarning($"Skipping {selectedObject.name}: Not a valid FBX model.");
                    continue;
                }

                // Configure the importer
                importer.materialImportMode = ModelImporterMaterialImportMode.ImportViaMaterialDescription;
                importer.materialLocation = ModelImporterMaterialLocation.InPrefab;
                importer.materialName = ModelImporterMaterialName.BasedOnModelNameAndMaterialName;
                importer.materialSearch = ModelImporterMaterialSearch.Local;

                // Get the source materials using reflection
                var sourceMaterials = typeof(ModelImporter)
                    .GetProperty("sourceMaterials", BindingFlags.NonPublic | BindingFlags.Instance)?
                    .GetValue(importer) as AssetImporter.SourceAssetIdentifier[];

                if (sourceMaterials == null || sourceMaterials.Length == 0)
                {
                    Debug.LogWarning($"Skipping {selectedObject.name}: No source materials found.");
                    continue;
                }

                // Override all source materials with the selected material
                foreach (var identifier in sourceMaterials)
                {
                    importer.AddRemap(identifier, newMaterial);
                }

                // Save and reimport the model
                importer.SaveAndReimport();
            }

            Debug.Log($"Overridden materials for {selectedObjects.Count} FBX models with {newMaterial.name}.");
        }

        [MenuItem("Assets/Override Materials", true)]
        private static bool ValidateOverrideMaterials()
        {
            // Only enable the menu item if FBX models are selected
            return Selection.objects
                .Any(obj => obj is GameObject && AssetDatabase.GetAssetPath(obj).EndsWith(".fbx"));
        }
    }
}