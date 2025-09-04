using UnityEngine;

namespace CyberSpeed.MemoryPuzzleGame
{
    [CreateAssetMenu(fileName = "DifficultyDatabase", menuName = "MemoryPuzzle/DifficultyDatabase", order = 2)]
    public class DifficultyDatabase : ScriptableObject
    {
        public DifficultyConfig easyConfig;
        public DifficultyConfig mediumConfig;
        public DifficultyConfig hardConfig;

        public DifficultyConfig GetConfig(DifficultyLevel level)
        {
            return level switch
            {
                DifficultyLevel.Easy => easyConfig,
                DifficultyLevel.Medium => mediumConfig,
                DifficultyLevel.Hard => hardConfig,
                _ => mediumConfig
            };
        }
    }
}