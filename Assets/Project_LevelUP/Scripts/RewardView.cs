using System;
using TMPro;
using UnityEngine;
namespace LevelUP.View
{
    public enum RewardType
    {
        Money,
        TimeBender,
        DepositOverdrive,
        Coins
    }
    public class RewardView : MonoBehaviour
    {
        [SerializeField] private RewardType type;
        public RewardType Type => type;
        [SerializeField] private TMP_Text text;
        public TMP_Text Txt => text;

       
    }
}