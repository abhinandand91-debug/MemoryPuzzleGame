using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CyberSpeed.MemoryPuzzleGame
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI turnText,matchesText,comboText;
        [SerializeField] Button rePlayButton, homeButton;
       
        public void OnEnable()
        {
            GameFlowController.Instance.OnGameOver += OnGameoverUIUpdate;
            rePlayButton.onClick.AddListener(OnReplayClick);
            homeButton.onClick.AddListener(OnHomeClick);
        }

        public void OnDisable()
        {
            GameFlowController.Instance.OnGameOver -= OnGameoverUIUpdate;
            rePlayButton.onClick.RemoveListener(OnReplayClick);
            homeButton.onClick.RemoveListener(OnHomeClick);
        }
        
        public void Reset()
        {
            turnText.text = "Turn: 0";
            matchesText.text = "Matches: 0";
            comboText.text = "Combo: 0";
        }

        private void OnGameoverUIUpdate(IScoreData scoreData)
        {
            turnText.text = $"Turns: {scoreData.Turns}";
            matchesText.text = $"Matches: {scoreData.Matches}";
            comboText.text = $"Combo: {scoreData.Combo}";
            
            SoundManager.Instance.PlayGameOver();
        }

        private void OnHomeClick()
        {
            if (GameFlowController.Instance.cardGridBuilder != null)
                GameFlowController.Instance.cardGridBuilder .ReleaseAllCards();
            
            GameFlowController.Instance.InitialiseGame();
            //HideWithDuration(0.5f);
            gameObject.SetActive(false);
        } 
        
        private void OnReplayClick()
        {
            if (GameFlowController.Instance.cardGridBuilder  != null)
                GameFlowController.Instance.cardGridBuilder .ReleaseAllCards();
            
            GameFlowController.Instance.ReStartGame();
            
            //HideWithDuration(0.5f);
            gameObject.SetActive(false);
            SoundManager.Instance.PlayButtonClick();
        }
    }
}
