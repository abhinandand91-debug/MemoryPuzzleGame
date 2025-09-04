using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CyberSpeed.MemoryPuzzleGame
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button loadSavedGameButton;
        [SerializeField] private TMP_Dropdown gameLayoutDropdown;

        [Header("Assets & Popups")]
        [SerializeField] private GameLayoutScriptableObject gameLayoutAsset;
        [SerializeField] private LoadGameConfirmationPopup loadGameConfirmationPopup;

        private int currentRows;
        private int currentColumns;

        private void Awake()
        {
            PopulateGameLayoutDropdown();
            Debug.Log($"Persistent Data Path: {Application.persistentDataPath}");
        }

        private void OnEnable()
        {
            gameLayoutDropdown.onValueChanged.AddListener(HandleDropdownValueChanged);
            playButton.onClick.AddListener(HandlePlayButtonClicked);
            loadSavedGameButton.onClick.AddListener(HandleLoadSavedGameButtonClicked);
        }

        private void OnDisable()
        {
            gameLayoutDropdown.onValueChanged.RemoveListener(HandleDropdownValueChanged);
            playButton.onClick.RemoveListener(HandlePlayButtonClicked);
            loadSavedGameButton.onClick.RemoveListener(HandleLoadSavedGameButtonClicked);
        }

        private void PopulateGameLayoutDropdown()
        {
            gameLayoutDropdown.ClearOptions();

            List<string> options = new List<string>();
            foreach (var grid in gameLayoutAsset.gridLevels)
            {
                options.Add(grid.ToString()); // Assuming grid is like "4x4"
            }
            gameLayoutDropdown.AddOptions(options);
            gameLayoutDropdown.value = 0;
            gameLayoutDropdown.RefreshShownValue();
            HandleDropdownValueChanged(0);
        }

        private void HandleDropdownValueChanged(int value)
        {
            (currentRows, currentColumns) = ParseRowColumn(gameLayoutDropdown.options[value].text);
            Debug.Log($"Layout selected: {currentRows}x{currentColumns}");

            UpdateSavedGameButtonVisibility();
        }

        private (int, int) ParseRowColumn(string input)
        {
            string[] parts = input.Split('x');
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int rows) &&
                int.TryParse(parts[1], out int cols))
            {
                return (rows, cols);
            }

            Debug.LogWarning($"Invalid layout format: {input}. Defaulting to 2x2.");
            return (2, 2);
        }

        private void HandlePlayButtonClicked()
        {
            GameFlowController.Instance.StartGame(currentRows, currentColumns);
            gameObject.SetActive(false);
        }

        private void HandleLoadSavedGameButtonClicked()
        {
            string saveKey = $"{currentRows}x{currentColumns}";
            if (SaveLoadManager.HasSave(saveKey))
            {
                GameFlowController.Instance.LoadGame(saveKey);
                //HideWithDuration(0.5f);
                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("No saved game found for this layout.");
                loadSavedGameButton.gameObject.SetActive(false);
            }
        }

        private void UpdateSavedGameButtonVisibility()
        {
            string saveKey = $"{currentRows}x{currentColumns}";
            loadSavedGameButton.gameObject.SetActive(SaveLoadManager.HasSave(saveKey));
        }
    }
}
