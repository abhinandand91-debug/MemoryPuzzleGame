using UnityEngine;
using UnityEditor;
using System.IO;

public static class OpenSaveLocationEditor
{
    [MenuItem("Tools/Open Save File Location")]
    public static void OpenSaveLocation()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "all_saves.json");
        string folderPath = Path.GetDirectoryName(filePath);

        if (File.Exists(filePath))
        {
            EditorUtility.RevealInFinder(filePath);
            Debug.Log($"Opened save file location: {filePath}");
        }
        else
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            EditorUtility.RevealInFinder(folderPath);
            Debug.Log($"Save file not found. Opened save folder instead: {folderPath}");
        }
    }
}