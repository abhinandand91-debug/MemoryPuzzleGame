using UnityEngine;
namespace CyberSpeed.MemoryPuzzleGame
{
    [CreateAssetMenu(fileName = "DifficultyConfig", menuName = "MemoryPuzzle/DifficultyConfig", order = 1)]
    public class DifficultyConfig : ScriptableObject
    {
        [Header("Grid Settings")]
        public int maxPairs;
        public bool useFullGrid = true;

        [Header("Gameplay Tweaks")]
        public float mismatchHideDelay = 0.6f;
        public float initialRevealDuration = 2f;

        [Header("Scoring Tweaks")]
        public int basePointsPerMatch = 10;
    }
}