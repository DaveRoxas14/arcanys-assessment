using UnityEngine;

namespace Arcanys.Collectibles
{
    [CreateAssetMenu(menuName = "Gem/Score Gem Effect")]
    public class ScoreGemEffectSo : CollectibleEffectSo
    {
        [SerializeField] private int scoreValue = 10;
        public override void UseCollectible(GameObject collector)
        {
            ScoringManager.Instance.AddScore(scoreValue);
        }
    }
}