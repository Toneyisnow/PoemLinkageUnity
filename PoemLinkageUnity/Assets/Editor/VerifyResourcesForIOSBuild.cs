using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class VerifyResourcesForIOSBuild
{
    [MenuItem("Tools/Verify Resources for iOS Build")]
    public static void VerifyResources()
    {
        string resourcesPath = "Assets/Resources";
        List<string> missingFiles = new List<string>();
        List<string> checkedDirectories = new List<string>();

        // Expected stage files for categories 1, 2, 3 with stages 01-09
        for (int category = 1; category <= 3; category++)
        {
            for (int stage = 1; stage <= 9; stage++)
            {
                int stageId = category * 100 + stage;
                
                // Check for JSON file
                string jsonPath = $"{resourcesPath}/stages/stage_{stageId}.json";
                if (!File.Exists(jsonPath))
                {
                    missingFiles.Add($"Missing: {jsonPath}");
                }

                // Check for preview image
                string prePath = $"{resourcesPath}/images/stage_{stageId}_pre.png";
                if (!File.Exists(prePath))
                {
                    missingFiles.Add($"Missing: {prePath}");
                }

                // Check for poem image
                string poemPath = $"{resourcesPath}/images/stage_{stageId}_poem.png";
                if (!File.Exists(poemPath))
                {
                    missingFiles.Add($"Missing: {poemPath}");
                }
            }
        }

        // Check required images
        string[] requiredImages = new[]
        {
            $"{resourcesPath}/images/select_background.png",
            $"{resourcesPath}/images/category-title-1.png",
            $"{resourcesPath}/images/category-title-2.png",
            $"{resourcesPath}/images/category-title-3.png",
        };

        foreach (string imagePath in requiredImages)
        {
            if (!File.Exists(imagePath))
            {
                missingFiles.Add($"Missing: {imagePath}");
            }
        }

        // Display results
        if (missingFiles.Count > 0)
        {
            string message = $"Found {missingFiles.Count} missing files:\n\n";
            for (int i = 0; i < System.Math.Min(10, missingFiles.Count); i++)
            {
                message += missingFiles[i] + "\n";
            }
            if (missingFiles.Count > 10)
            {
                message += $"... and {missingFiles.Count - 10} more";
            }
            EditorUtility.DisplayDialog("Resource Verification - MISSING FILES", message, "OK");
        }
        else
        {
            // Count total files
            string[] allPngs = Directory.GetFiles($"{resourcesPath}/images", "*.png");
            string[] allJsons = Directory.GetFiles($"{resourcesPath}/stages", "*.json");
            
            string message = $"✓ All required files found!\n\n";
            message += $"Images: {allPngs.Length} files\n";
            message += $"Stage JSON files: {allJsons.Length} files\n";
            message += $"\nTotal: {allPngs.Length + allJsons.Length} resource files";
            
            EditorUtility.DisplayDialog("Resource Verification - SUCCESS", message, "OK");
        }
    }
}
