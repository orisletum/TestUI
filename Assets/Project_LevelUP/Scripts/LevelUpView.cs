using DG.Tweening;
using LevelUP.Service;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelUP.View
{
    public class LevelUpView : MonoBehaviour
    {
        #region UIElements
        [Header("Panel")]

        [SerializeField] private Transform panel;
        public Transform Panel => panel;

        [SerializeField] private CanvasGroup canvasGroup;
        public CanvasGroup canvas => canvasGroup;

        [Header("Panel elements")]
        [SerializeField] private Image[] fogImages;
        public Image[] FogImages => fogImages;

        [SerializeField] private Image[] rayImages;
        public Image[] RayImages => rayImages;

        [SerializeField] private Image[] starImages;
        public Image[] StarImages => starImages;

        [SerializeField] private Transform[] uiElements;
        public Transform[] UIElements => uiElements;

        [SerializeField] private Transform panelHeader;
        public Transform PanelHeader => panelHeader;

        [Header("Level")]

        [SerializeField] private Transform level;
        public Transform Level => level;

        [SerializeField] private TMP_Text levelText;
        public TMP_Text LevelText => levelText;

        [Header("Buttons")]

        [SerializeField] private Button claimBackgroundButton;
        public Button ClaimBackgroundButton => claimBackgroundButton;

        [SerializeField] private Button claimButton;
        public Button ClaimButton => claimButton;

        [SerializeField] private Button claimX2Button;
        public Button ÑlaimX2Button => claimX2Button;
        #endregion
        #region 

        [Header("Rewards")]
        private LevelUpService levelUpService;
        public static Action LevelUpAction = delegate { };
        public Transform rewardContainerDefault;
        public Transform rewardContainerX2;

        [SerializeField] private RewardView[] rewards;
        public RewardView[] Rewards => rewards;
       
        private void Start()
        {
            levelUpService = new LevelUpService(this);
        }
        
        private void OnDestroy() => levelUpService.Deinitialize();
        public void ShowLevelUpPanel() => LevelUpAction.Invoke();
        public void CreateReward(RewardView reward, Transform rewardTransform, string rewardText) => Instantiate(reward, rewardTransform).GetComponent<RewardView>().Txt.text = rewardText;
        
        #endregion
        #region cleaning
        public void CleanGarbage()
        {
            KillDotweens(fogImages);
            KillDotweens(rayImages);
            KillDotweens(starImages);

            var count = rewardContainerDefault.childCount;
            for (int i = 0; i < count; i++)
            {
                Destroy(rewardContainerX2.GetChild(i).gameObject, 0);
                Destroy(rewardContainerDefault.GetChild(i).gameObject, 0);
            }
        }

        private void KillDotweens(Image[] images)
        {

            foreach (var img in images)
            {
                img.DOKill();
            }
        }
        #endregion 
    }
}