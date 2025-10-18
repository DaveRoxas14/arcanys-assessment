using UnityEngine;

namespace Arcanys.Collectibles
{
    public abstract class CollectibleEffectSo : ScriptableObject, ICollectible
    {
        [TextArea] [SerializeField] private string _description;
        
        public abstract void UseCollectible(PlayerController collector);
    }
}