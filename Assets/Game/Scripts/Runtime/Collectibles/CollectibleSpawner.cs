using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arcanys.Collectibles
{
    public class CollectibleSpawner : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField]
        private List<GameObject> _collectiblePrefabs = new ();

        [Header("Debug")] 
        [SerializeField] private bool _showGizmos;

        [SerializeField] private Color _gizmoColor = new (1, 1, 0, 0.75F);

        private void Start()
        {
            SpawnCollectible(0);
        }

        public async void SpawnCollectible(int delay)
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