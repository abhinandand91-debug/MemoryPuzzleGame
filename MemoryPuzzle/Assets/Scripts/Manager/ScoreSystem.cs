using System;
using UnityEngine;

namespace CyberSpeed.MemoryPuzzleGame
{
    public interface IScoreData
    {
        int Turns { get; }
        int Matches { get; }
        int Combo { get; }   // number of consecutive matches beyond the first
        int Score { get; }
    }

    public class ScoreSystem : MonoBehaviour, IScoreData
    {
        public static ScoreSystem Instance { get; private set; }

        [Header("Scoring")]
        [SerializeField] private int basePointsPerMatch = 10;

        public int Turns { get; private set; }
        public int Matches { get; private set; }
        public int Combo { get; private set; }   // streak beyond the first
        public int Score { get; private set; }

        private int streakCount; // internal: counts all consecutive matches (1,2,3,...)

        // Events
        public event Action<int> TurnChanged;
        public event Action<int, int, int> MatchStatsChanged; 
        public event Action<int> ComboChanged;
        

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            ResetAll();
        }

        public void ResetAll()
        {
            Turns = 0;
            Matches = 0;
            streakCount = 0;
            Combo = 0;
            Score = 0;
            
            TurnChanged?.Invoke(Turns);
            MatchStatsChanged?.Invoke(Matches, Score, Combo);
            ComboChanged?.Invoke(Combo);
        }

        /// <summary>
        /// Configure scoring from a DifficultyConfig
        /// </summary>
        public void ApplyDifficulty(DifficultyConfig config)
        {
            if (config != null)
                basePointsPerMatch = config.basePointsPerMatch;
            else
                basePointsPerMatch = 10; // fallback default
        }

        public void IncrementTurn()
        {
            Turns++;
            TurnChanged?.Invoke(Turns);
        }

        public void OnMatch()
        {
            Matches++;

            // increase streak count
            streakCount++;

            // Combo is streak beyond first match
            Combo = Mathf.Max(0, streakCount - 1);

            // score: base * streakCount (so later matches reward more)
            Score += basePointsPerMatch * streakCount;

            MatchStatsChanged?.Invoke(Matches, Score, Combo);
            ComboChanged?.Invoke(Combo);

            SoundManager.Instance?.PlayMatchFound();
        }

        public void OnMismatch()
        {
            streakCount = 0;
            Combo = 0;
            ComboChanged?.Invoke(Combo);
            SoundManager.Instance?.PlayMismatch();
        }

        public void RestoreState(int turns, int matches, int combo, int score)
        {
            Turns = turns;
            Matches = matches;
            Combo = Mathf.Max(0, combo);
            Score = Mathf.Max(0, score);

            // restore streakCount based on combo
            streakCount = Combo + 1; // safe fallback, though ideally you'd persist streakCount too

            TurnChanged?.Invoke(Turns);
            MatchStatsChanged?.Invoke(Matches, Score, Combo);
            ComboChanged?.Invoke(Combo);
        }
    }
}
