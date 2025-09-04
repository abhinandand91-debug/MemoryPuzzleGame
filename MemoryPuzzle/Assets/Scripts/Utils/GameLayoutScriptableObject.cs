using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameLayoutSize", menuName = "ScriptableObjects/GameLayoutSize", order = 1)]
public class GameLayoutScriptableObject : ScriptableObject
{
    
    [System.Serializable]
    public struct GameLayoutSize
    {
        public int rows;
        public int cols;

        public override string ToString()
        {
            return $"{rows} x {cols}";
        }
    }
    
    /// <summary>
    /// grid levels (rows, cols) must be even count
    /// </summary>
    public List<GameLayoutSize> gridLevels;
}

