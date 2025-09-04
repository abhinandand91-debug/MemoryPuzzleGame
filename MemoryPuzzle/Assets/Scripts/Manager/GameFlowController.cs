using System;
using System.Collections.Generic;
using UnityEngine;

namespace CyberSpeed.MemoryPuzzleGame
{
    public class GameFlowController : MonoBehaviour
    {
        [field: Header("Game Settings")]
        public int rows { get; private set; }
        public int cols { get; private set; }
        public static GameFlowController Instance { get; private set; }
       
        public Action<int, int> OnGameStarted;
        public Action<DifficultyConfig> OnGameComfigSet;
        public Action<IScoreData> OnGameOver;
        public Action OnGameSave;
        
        [Header("Screen References")]
        [SerializeField] private MainMenuUI mainMenuUIHandler;
        [SerializeField] private GamePlayUI gamePlayUIHandler;
        [SerializeField] private GameOverUI gameEndUIHandler;
        public CardGridBuilder cardGridBuilder;
        
        [Header("Level Banlancing")]
        public DifficultyLevel difficultyLevel;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            InitialiseGame();
            SoundManager.Instance.PlayMusic(false);
        }
        
        public void InitialiseGame()
        {
            Debug.Log("GameManager: InitialiseGame() called");
            mainMenuUIHandler.gameObject.SetActive(true);
            gameEndUIHandler.gameObject.SetActive(false);
            gamePlayUIHandler.gameObject.SetActive(false);
        }
        
        public void StartGame(int rowCount, int colCount)
        {
            rows = rowCount;
            cols = colCount;
            gamePlayUIHandler.gameObject.SetActive(true);
            OnGameStarted?.Invoke(rows, cols);
            var config = cardGridBuilder.difficultyDatabase.GetConfig(difficultyLevel);
            OnGameComfigSet?.Invoke(config);
            cardGridBuilder.CreateCardGrid(rows, cols);
        }
        
        public void ReStartGame()
        {
            Debug.Log($"GameManager: ReStartGame rows {rows} cols {cols}");
            StartGame(rows, cols);
        }

        public void GameOver()
        {
            Debug.Log("Game Over!");
            gamePlayUIHandler.gameObject.SetActive(false);
            gameEndUIHandler.gameObject.SetActive(true);
            string key = $"{rows}x{cols}";
            SaveLoadManager.Delete(key);
            OnGameOver?.Invoke(ScoreSystem.Instance);
        }
        
        public void GameSave()
        {
            Debug.Log("Game Saved!");
            string key = $"{rows}x{cols}";

            SaveDataCollection dataCollection = new SaveDataCollection
            {
                rows = rows,
                cols = cols,
                turns = ScoreSystem.Instance.Turns,
                matches = ScoreSystem.Instance.Matches,
                combo = ScoreSystem.Instance.Combo,
                score = ScoreSystem.Instance.Score,
                cardValues = new List<int>(),
                cardMatched = new List<bool>(),
                cardFaceDown = new List<bool>()
            };

            var allCards = FindObjectsOfType<CardView>();
            foreach (var card in allCards)
            {
                dataCollection.cardValues.Add(card.Value);
                dataCollection.cardMatched.Add(card.IsMatched);
                dataCollection.cardFaceDown.Add(card.IsFaceDown);
            }

            SaveLoadManager.Save(key, dataCollection);
            OnGameSave?.Invoke();
        }
        
        
        public void LoadGame(string key)
        {
            SaveDataCollection dataCollection = SaveLoadManager.Load(key);
            if (dataCollection == null)
            {
                Debug.LogWarning($"No save found for {key}");
                return;
            }

            rows = dataCollection.rows;
            cols = dataCollection.cols;
            gamePlayUIHandler.gameObject.SetActive(true);

            cardGridBuilder.ReleaseAllCards();
            cardGridBuilder.CreateCardGrid(rows, cols);

            var allCards = FindObjectsOfType<CardView>();
            for (int i = 0; i < allCards.Length; i++)
            {
                allCards[i].Initialize(dataCollection.cardValues[i],
                    cardGridBuilder.fruitsSpritesSO.GetSprite(dataCollection.cardValues[i]));

                if (dataCollection.cardMatched[i])
                    allCards[i].MarkAsMatched();
                else if (dataCollection.cardFaceDown[i])
                    allCards[i].ShowFront();
                else
                    allCards[i].ShowBack();
            }

            ScoreSystem.Instance.RestoreState(
                dataCollection.turns, dataCollection.matches, dataCollection.combo, dataCollection.score
            );
            
            //NEW: Sync MatchController with restored cards
            var matchController = FindObjectOfType<MatchSystem>();
            int matchedCount = 0;
            foreach (var card in allCards)
            {
                if (card.IsMatched) matchedCount++;
            }
            matchController.Initialize((rows * cols) / 2);
            matchController.SetMatchedPairs(matchedCount / 2); // each pair has 2 cards
            
        }
        
    }
}

