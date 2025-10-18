using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Scripts.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arcanys.Collectibles
{
    public class CollectibleSpawner : MonoBehaviour
    {
        [Header(ArcanysConstants.INSPECTOR.REFERENCES)] 
        [SerializeField]
        private List<GameObject> _collectiblePrefabs = new ();

        [Header(ArcanysConstants.INSPECTOR.DEBUG)] 
        [SerializeField] private bool _showGizmos;

        [SerializeField] private Color _gizmoColor = new (1, 1, 0, 0.75F);
        

        public async Task SpawnCollectible(int delay)
        {
            try
            {
                var indexToSpawn = Random.Range(0, _collectiblePrefabs.Count);

                await Task.Delay(delay * 1000);

                Instantiate(_collectiblePrefabs[indexToSpawn], transform.position, transform.rotation, transform);
            }
            catch (Exception e)
            {
                Debug.LogError("[Collectible Spawner] Something went wrong with spawning the collectible");
            }
        }

        private void OnDrawGizmos()
        {
            if(!_showGizmos) return;
            
            Gizmos.color = _gizmoColor;
            Gizmos.DrawSphere(transform.position, 1.5f);
        }
    }
}