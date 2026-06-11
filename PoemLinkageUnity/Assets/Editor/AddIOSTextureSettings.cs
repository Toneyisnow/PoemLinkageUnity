using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class AddIOSTextureSettings
{
    [MenuItem("Tools/Add iOS Texture Settings to Resources/Images")]
    public static void AddIOSSettings()
    {
        string resourcesImagesPath = "Assets/Resources/images";
        
        if (!Directory.Exists(resourcesImagesPath))
        {
            EditorUtility.DisplayDialog("Error", "Resources/images directory not found!", "OK");
            return;
        }

        // Get all PNG files in the directory
        string[] pngFiles = Directory.GetFiles(resourcesImagesPath, "*.png");
        
        if (pngFiles.Length == 0)
        {
            EditorUtility.DisplayDialog("Info", "No PNG files found in Resources/images", "OK");
            return;
        }

        int modifiedCount = 0;
        List<string> modifiedFiles = new List<string>();

        foreach (string pngFile in pngFiles)
        {
            string metaFile = pngFile + ".meta";
            
            if (File.Exists(metaFile))
            {
                string metaContent = File.ReadAllText(metaFile);
                
                // Check if iPhone platform already exists
                if (!metaContent.Contains("buildTarget: iPhone"))
                {
                    // Find the Standalone platform settings block and add iPhone after it
                    string iPhoneSettings = @"  - serializedVersion: 4
    buildTarget: iPhone
    maxTextureSize: 2048
    resizeAlgorithm: 0
    textureFormat: -1
    textureCompression: 1
    compressionQuality: 50
    crunchedCompression: 0
    allowsAlphaSplitting: 0
    overridden: 0
    ignorePlatformSupport: 0
    androidETC2FallbackOverride: 0
    forceMaximumCompressionQuality_BC6H_BC7: 0
";

                    // Find where to insert iPhone settings
                    string standalonePlatformMarker = "buildTarget: Standalone";
                    int markerIndex = metaContent.IndexOf(standalonePlatformMarker);
                    
                    if (markerIndex != -1)
                    {
                        // Find the end of Standalone block (next buildTarget or spriteSheet)
                        int standaloneBlockStart = metaContent.LastIndexOf("  - serializedVersion:", markerIndex);
                        int nextBlockStart = metaContent.IndexOf("  - serializedVersion:", markerIndex + 1);
                        int spriteSheetStart = metaContent.IndexOf("  spriteSheet:", markerIndex);

                        int insertIndex;
                        if (nextBlockStart != -1 && (spriteSheetStart == -1 || nextBlockStart < spriteSheetStart))
                        {
                            insertIndex = nextBlockStart;
                        }
                        else
                        {
                            insertIndex = spriteSheetStart;
                        }

                        if (insertIndex != -1)
                        {
                            metaContent = metaContent.Insert(insertIndex, iPhoneSettings);
                            File.WriteAllText(metaFile, metaContent);
                            modifiedCount++;
                            modifiedFiles.Add(Path.GetFileName(pngFile));
                        }
                    }
                }
            }
        }

        // Refresh the asset database
        AssetDatabase.Refresh();

        string message = $"Modified {modifiedCount} texture files.\n\n";
        if (modifiedFiles.Count > 0 && modifiedFiles.Count <= 10)
        {
            message += "Files: " + string.Join(", ", modifiedFiles);
        }
        else if (modifiedFiles.Count > 10)
        {
            message += $"First 10 files:\n" + string.Join("\n", modifiedFiles.GetRange(0, 10)) + "\n...and more";
        }

        EditorUtility.DisplayDialog("Success", message, "OK");
    }
}
