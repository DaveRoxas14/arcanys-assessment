using System;
using Game.Scripts.Runtime;
using UnityEngine;

namespace Arcanys.Collectibles
{
    /// <summary>
    /// Handles the pickup logic of gems
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    public class Gem : MonoBehaviour
    {
        [Header(ArcanysConstants.INSPECTOR.EFFECT)]
        [SerializeField] private CollectibleEffectSo _effectBehavior;
        
        private ICollectible _effect;

        #region Unity Functions

        private void Awake()
        {
            _effect = _effectBehavior as ICollectible;
            
            if(_effect == null)
                Debug.LogError($"{name} has invalid effect!!");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(ArcanysConstants.TAGS.PLAYER)) return;
            
            _effect.UseCollectible(other.GetComponentInChildren<PlayerController>());
            PlaySfx();
            PlayVfx();
            
            // todo - add sfx/vfx and delay the game object destroy...
            Destroy(gameObject);
        }

        #endregion

        private void PlayVfx()
        {
            
        }

        private void PlaySfx()
        {
           
        }
    }
}