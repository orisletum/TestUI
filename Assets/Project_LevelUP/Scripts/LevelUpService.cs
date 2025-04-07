using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using System.Linq;
using LevelUP.View;

namespace LevelUP.Service
{
    public class LevelUpService
    {
        private Vector2 startScale = Vector2.zero;
        private Vector2 endScale = Vector2.one;
        private float startAlpha = 0f;
        private float endAlpha = 1f;
        private float duration = 0.5f;
        private int playerLevel = 1;
        private LevelUpView view;
        public LevelUpService(LevelUpView _view)
        {
            view = _view;
            playerLevel = PlayerPrefs.GetInt("PlayerLevel", 0);

            view.ClaimBackgroundButton.onClick.AddListener(ClaimReward);
            view.ClaimButton.onClick.AddListener(ClaimReward);
            view.ÑlaimX2Button.onClick.AddListener(ClaimX2Reward);
            LevelUpView.LevelUpAction += ShowLevelUpPanel;
        }
        public void Deinitialize()
        {
            view.ClaimBackgroundButton.onClick.RemoveAllListeners();
            view.ClaimButton.onClick.RemoveAllListeners();
            view.ÑlaimX2Button.onClick.RemoveAllListeners();
            LevelUpView.LevelUpAction -= ShowLevelUpPanel;
        }
        private void ShowLevelUpPanel() => StartTask().Forget();

        private async UniTask StartTask()
        {
            SetLevel();
            ResetUI();
            await ShowPanel();
            await ShowHeader();
            ShowUIElements();
            CreateRewards();
            PlayAnimations().Forget();
        }

        private void ShowUIElements()
        {
            view.UIElements.ToList().ForEach(x =>
            {
                x.DOScale(endScale, duration * 0.5f).SetEase(Ease.OutBack);
            });
        }

        private async UniTask ShowHeader() => await view.PanelHeader.DOScale(endScale, 0.4f).SetEase(Ease.OutBack);


        private void ResetUI()
        {
            view.FogImages.ToList().ForEach(x => x.DOFade(startAlpha, 0));
            view.RayImages.ToList().ForEach(x => x.DOFade(startAlpha, 0));
            view.StarImages.ToList().ForEach(x => x.DOFade(startAlpha, 0));
            view.UIElements.ToList().ForEach(x => x.localScale = startScale);
            view.Panel.localScale = startScale;
            view.PanelHeader.localScale = startScale;
            view.canvas.alpha = startAlpha;
        }

        private void SetLevel()
        {
            playerLevel++;
            view.LevelText.text = $"- LEVEL {playerLevel} -";
        }

        private void CreateRewards()
        {
            view.Rewards.ToList().ForEach(reward =>
            {
                int rewardValue = Random.Range(1, 5);
                view.CreateReward(reward, view.rewardContainerDefault, GetText(reward.Type, rewardValue));
                view.CreateReward(reward, view.rewardContainerX2, GetText(reward.Type, rewardValue*2));
            });
        }
        public string GetText(RewardType type,int rewardValue)
        {
            string textValue = "";
            switch (type)
            {
                case RewardType.Money:
                    textValue = $"<color=#FEC752>+{rewardValue * 10000}</color>";
                    break;
                case RewardType.TimeBender:
                    textValue = $"<color=#CDD2CF>Time Bender</color> <color=#FEC752>õ{rewardValue}</color>";
                    break;
                case RewardType.DepositOverdrive:
                    textValue = $"<color=#CDD2CF>Deposit Overdrive</color> <color=#FEC752>õ{rewardValue}</color>";
                    break;
                case RewardType.Coins:
                    textValue= $"<color=#FEC752>+{rewardValue * 10}</color>";
                    break;
                default:
                    break;
            }
            return textValue;
        }
        public async UniTask ShowPanel()
        {
            Sequence sequence = DOTween.Sequence()
                .PrependInterval(0.5f)
                .Join(view.Panel.DOScale(endScale, duration * 0.5f).SetEase(Ease.OutBack))
                .Join(view.canvas.DOFade(endAlpha, duration * 0.5f))
                .PrependInterval(0.1f);
            await sequence.Play().ToUniTask();
            view.canvas.interactable = true;
            view.canvas.blocksRaycasts = true;
        }
        public async UniTask PlayAnimations()
        {
            foreach (var fog in view.FogImages)
            {
                await UniTask.Delay(300);
                fog.DOFade(endAlpha, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            }
            foreach (var ray in view.RayImages)
            {
                await UniTask.Delay(150);
                ray.DOFade(endAlpha, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            }
            foreach (var star in view.StarImages)
            {
                await UniTask.Delay(200);
                star.DOFade(endAlpha, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            }
        }
        private void ClaimX2Reward()
        {
            HideCanvas();
            view.CleanGarbage();
            SaveLevel(2);
        }

        private void ClaimReward()
        {
            HideCanvas();
            view.CleanGarbage();
            SaveLevel();
        }

        private void HideCanvas()
        {
            view.canvas.interactable = false;
            view.canvas.blocksRaycasts = false;
            view.canvas.DOFade(startAlpha, duration * 0.5f);
            view.Panel.DOScale(startScale, duration * 0.5f);
        }
        private void SaveLevel(int multiplier = 1)
        {
            PlayerPrefs.SetInt("PlayerLevel", playerLevel);
        }
    }
}