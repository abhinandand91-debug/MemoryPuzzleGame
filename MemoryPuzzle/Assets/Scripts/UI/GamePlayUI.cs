using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CyberSpeed.MemoryPuzzleGame
{
    public class GamePlayUI : MonoBehaviour
    {

        [FormerlySerializedAs("cardManager")] [SerializeField] private CardGridBuilder cardGridBuilder;
        [SerializeField] private TextMeshProUGUI turnText,matchesText,scoreText,comboText;
      
        [SerializeField] Button homeButton;
        [SerializeField] Button saveGameButton;

        [SerializeField] LoadGameConfirmationPopup loadGamepopup;
        private void OnEnable()
        {
            GameFlowController.Instance.OnGameStarted += SpawnGrid;
            if (ScoreSystem.Instance != null)
            {
                ScoreSystem.Instance.TurnChanged += OnTurnChanged;
                ScoreSystem.Instance.MatchStatsChanged += OnMatchStatsChanged; 
                ScoreSystem.Instance.ComboChanged += OnComboChanged;
            }
            
            homeButton.onClick.AddListener(OnHomeClick);
            saveGameButton.onClick.AddListener(OnClickSaveGame);
            loadGamepopup.Show(false);
        }
        
        private void OnDisable()
        {
            GameFlowController.Instance.OnGameStarted -= SpawnGrid;
            if (ScoreSystem.Instance != null)
            {
                ScoreSystem.Instance.TurnChanged -= OnTurnChanged;
                ScoreSystem.Instance.MatchStatsChanged -= OnMatchStatsChanged;
                ScoreSystem.Instance.ComboChanged -= OnComboChanged;
            }
            
            homeButton.onClick.RemoveListener(OnHomeClick);
            saveGameButton.onClick.RemoveListener(OnClickSaveGame);
        }
        
        private void SpawnGrid(int rows, int cols)
        {
            Debug.Log($"Spawning {rows}x{cols} board...");
            cardGridBuilder.CreateCardGrid(rows, cols);

            turnText.text = "Turn: 0";
            matchesText.text = "Matches: 0";
            scoreText.text = "Score: 0";
            comboText.text = "Combo: 0";
        }

        private void OnTurnChanged(int turns)
        {
            turnText.text = $"Turn: {turns}";
        }

        private void OnMatchStatsChanged(int matches, int score, int combo)
        {
            matchesText.text = $"Matches: {matches}";
            scoreText.text = $"Score: {score}";
            comboText.text = $"Combo: {combo}";
        }
        
        private void OnComboChanged(int combo)
        {
            comboText.text = $"Combo: {combo}";
        }
        
        private void OnHomeClick()
        {
            SoundManager.Instance.PlayButtonClick();

            if (GameFlowController.Instance.cardGridBuilder  != null)
                GameFlowController.Instance.cardGridBuilder.ReleaseAllCards();
            
            GameFlowController.Instance.InitialiseGame();
        }

        public void OnClickSaveGame()
        {
            GameFlowController.Instance.GameSave();
            loadGamepopup.Show(true);
            SoundManager.Instance.PlayButtonClick();
        }
    }
}
