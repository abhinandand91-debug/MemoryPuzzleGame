using UnityEngine;
using UnityEngine.UI;

namespace CyberSpeed.MemoryPuzzleGame
{
    public class SaveGameConfirmationPopup : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button saveButton;
        [SerializeField] private Button cancelButton;

        /// <summary>
        /// Show or hide the popup.
        /// </summary>
        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }

        private void OnEnable()
        {
            saveButton.onClick.AddListener(HandleSaveButtonClicked);
            cancelButton.onClick.AddListener(HandleCancelButtonClicked);
        }

        private void OnDisable()
        {
            saveButton.onClick.RemoveListener(HandleSaveButtonClicked);
            cancelButton.onClick.RemoveListener(HandleCancelButtonClicked);
        }

        private void HandleSaveButtonClicked()
        {
            GameFlowController.Instance.GameSave();
            SetVisible(false);
            SoundManager.Instance.PlayButtonClick();
        }

        private void HandleCancelButtonClicked()
        {
            SetVisible(false);
            SoundManager.Instance.PlayButtonClick();
        }
    }
}