using System.Collections.Generic;
using UnityEngine.Serialization;


[System.Serializable]
public class SaveDataCollection
{
    public int rows;
    public int cols;
    public int turns;
    public int matches;
    public int combo;
    public int score;

    public List<int> cardValues;
    public List<bool> cardMatched;
    public List<bool> cardFaceDown;
}

[System.Serializable]
public class SaveCollection
{
    public Dictionary<string, SaveDataCollection> saves = new Dictionary<string, SaveDataCollection>();
}

[System.Serializable]
public class SaveEntry
{
    public string key;
    public SaveDataCollection dataCollection;
}

[System.Serializable]
public class SaveWrapper
{
    public List<SaveEntry> entries = new List<SaveEntry>();
}