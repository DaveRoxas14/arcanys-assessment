using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Runtime.Enemies
{
    /// <summary>
    /// This spawns the enemies randomly.
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        [Header(ArcanysConstants.INSPECTOR.REFERENCES)]
        [SerializeField] private EnemyBase _enemyPrefab;
        
        [Header(ArcanysConstants.INSPECTOR.SETTINGS)]
        [SerializeField] private int _numberOfEnemiesToSpawn = 5;
        [SerializeField] private float _spawnRadius = 10f;
        [SerializeField] private int _spawnDelay = 3;
        [SerializeField] private int _initialSpawnDelay = 3;

        private List<EnemyBase> _enemyList = new List<EnemyBase>();

        private async void Start()
        {
            try
            {
                await Task.Delay(_initialSpawnDelay * ArcanysConstants.INTEGERS.MILLISECOND);
                
                for (var i = 0; i < _numberOfEnemiesToSpawn; i++)
                {
                    var delay = _spawnDelay * ArcanysConstants.INTEGERS.MILLISECOND;
                    await Task.Delay(delay);

                    while (GameManager.Instance.IsGamePaused)
                        await Task.Yield();
                    
                    if (!transform) break;

                    var pos = transform.position + Random.insideUnitSphere * _spawnRadius;
                    pos.y = transform.position.y;

                    var enemy = Instantiate(_enemyPrefab, pos, Quaternion.identity, parent:transform);
                    
                    _enemyList.Add(enemy); 
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1, 1, 0, 0.75F);
            Gizmos.DrawSphere(transform.position, _spawnRadius);
        }
    }
}