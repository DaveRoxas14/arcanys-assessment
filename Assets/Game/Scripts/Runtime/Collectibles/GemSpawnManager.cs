using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Scripts.Runtime;
using UnityEngine;

namespace Arcanys.Collectibles
{
    public class GemSpawnManager : MonoBehaviour
    {
        [Header(ArcanysConstants.INSPECTOR.REFERENCES)] [SerializeField]
        private List<CollectibleSpawner> _gemSpawners = new List<CollectibleSpawner>();

        public async Task StartSpawningGems()
        {
            foreach (var spawner in _gemSpawners)
            {
                await spawner.SpawnCollectible(0);
                
                await Task.Yield(); 
            }
        }
    }
}