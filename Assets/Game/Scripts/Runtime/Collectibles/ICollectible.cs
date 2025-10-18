using UnityEngine;

namespace Arcanys.Collectibles
{
    public interface ICollectible
    {
        void UseCollectible(PlayerController collector);
    }
}