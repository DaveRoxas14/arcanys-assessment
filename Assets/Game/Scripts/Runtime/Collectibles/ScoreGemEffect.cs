using Game.Scripts.Runtime;
using UnityEngine;

namespace Arcanys.Collectibles
{
    [CreateAssetMenu(menuName = ArcanysConstants.MENU_NAME.SCORE_GEM_EFFECT)]
    public class ScoreGemEffectSo : CollectibleEffectSo
    {
        [SerializeField] private int scoreValue = 10;
        public override void UseCollectible(GameObject collector)
        {
            ScoringManager.Instance.AddScore(scoreValue);
        }
    }
}