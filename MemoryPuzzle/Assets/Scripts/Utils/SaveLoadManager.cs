using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class SaveLoadManager
{
    private static string savePath = Path.Combine(Application.persistentDataPath, "all_saves.json");

    private static SaveWrapper LoadAll()
    {
        if (!File.Exists(savePath))
            return new SaveWrapper();

        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<SaveWrapper>(json);
    }

    private static void SaveAll(SaveWrapper wrapper)
    {
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    public static void Save(string key, SaveDataCollection dataCollection)
    {
        var wrapper = LoadAll();
        // overwrite if exists
        var existing = wrapper.entries.Find(e => e.key == key);
        if (existing != null)
        {
            existing.dataCollection = dataCollection;
        }
        else
        {
            wrapper.entries.Add(new SaveEntry { key = key, dataCollection = dataCollection });
        }

        SaveAll(wrapper);
        Debug.Log($"Game saved for key {key}");
    }

    public static SaveDataCollection Load(string key)
    {
        var wrapper = LoadAll();
        var entry = wrapper.entries.Find(e => e.key == key);
        return entry?.dataCollection;
    }

    public static void Delete(string key)
    {
        var wrapper = LoadAll();
        wrapper.entries.RemoveAll(e => e.key == key);
        SaveAll(wrapper);
        Debug.Log($"Save deleted for key {key}");
    }

    public static bool HasSave(string key)
    {
        var wrapper = LoadAll();
        return wrapper.entries.Exists(e => e.key == key);
    }
}