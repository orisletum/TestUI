using LevelUP.View;
using System;
using UnityEngine;
using UnityEngine.UI;
namespace LevelUP.CanvasUI
{
    public class CanvasUI : MonoBehaviour
    {
        [SerializeField] private Button levelUpButton;
        [SerializeField] private LevelUpView levelUpView;
        public void OnEnable()
        {
            levelUpButton.onClick.AddListener(ShowLevelUpPanel);
        }

        private void ShowLevelUpPanel() => levelUpView.ShowLevelUpPanel();

        private void OnDisable()
        {
            levelUpButton.onClick.RemoveAllListeners();
        }
    }
}
