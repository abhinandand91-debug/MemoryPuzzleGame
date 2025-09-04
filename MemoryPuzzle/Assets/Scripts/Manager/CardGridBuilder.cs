using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CyberSpeed.MemoryPuzzleGame
{
    public class CardGridBuilder : MonoBehaviour
    {
        [SerializeField] public FruitsSpritesScriptable fruitsSpritesSO;
        [SerializeField] private GridLayoutGroup gridObject;
        [SerializeField] private MatchSystem matchSystem;
        [SerializeField] private GameObject cardPrefab;
        public DifficultyDatabase difficultyDatabase;
        private int totalPairs;

        private ObjectPool objectPool;

        // Represents a card pair
        private struct CardData
        {
            public int Value; // unique identifier for the pair
            public int SpriteIndex; // which sprite this pair uses
        }

        private void Awake()
        {
            objectPool = gridObject.gameObject.AddComponent<ObjectPool>();
            objectPool.InitializePool(cardPrefab);
        }

        public void ReleaseAllCards()
        {
            if (objectPool != null)
            {
                objectPool.ReleaseAll();
            }
        }

       
        public void CreateCardGrid(int rows, int cols )
        {
            // Clear old cards
            ReleaseAllCards();

            // Apply difficulty config
            DifficultyConfig config = difficultyDatabase.GetConfig(GameFlowController.Instance.difficultyLevel);

            // 🔹 Calculate pairs (respecting difficulty)
            int totalCards = rows * cols;
            int totalPairs = totalCards / 2;
            if (!config.useFullGrid)
                totalPairs = Mathf.Min(totalPairs, config.maxPairs);

            // 🔹 Ensure GridLayout is set correctly
            gridObject.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridObject.constraintCount = cols;

            Debug.Log($"CreateCardGrid: rows={rows}, cols={cols}, pairs={totalPairs}, difficulty={GameFlowController.Instance.difficultyLevel}");

            // 🔹 Use factory to build cards
            var factory = GridFactoryProvider.GetFactory();
            var cards = factory.CreateGrid(rows, cols, objectPool, fruitsSpritesSO, matchSystem, config);

            // 🔹 Init MatchSystem with difficulty timings + pair count
            matchSystem.MismatchHideDelay = config.mismatchHideDelay;
            matchSystem.InitialRevealDuration = config.initialRevealDuration;
            matchSystem.Initialize(totalPairs);
            matchSystem.RunInitialReveal();
        }

        
    }
}