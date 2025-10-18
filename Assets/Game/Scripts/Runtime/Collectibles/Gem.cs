using System;
using System.Threading.Tasks;
using Game.Scripts.Runtime;
using Game.Scripts.Runtime.Audio;
using UnityEngine;

namespace Arcanys.Collectibles
{
    /// <summary>
    /// Handles the pickup logic of gems
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    public class Gem : MonoBehaviour
    {
        [Header(ArcanysConstants.INSPECTOR.REFERENCES)] [SerializeField]
        private GameObject _model;
        [SerializeField]
        private SoundEffect _sfx;

        [SerializeField]
        private GameObject _vfx;
        
        [Header(ArcanysConstants.INSPECTOR.EFFECT)]
        [SerializeField] 
        private CollectibleEffectSo _effectBehavior;

        [Header(ArcanysConstants.INSPECTOR.SETTINGS)]
        private int _delayBeforeDestroy = 1;
        
        private ICollectible _effect;

        #region Unity Functions

        private void Awake()
        {
            _effect = _effectBehavior as ICollectible;
            
            if(_effect == null)
                Debug.LogError($"{name} has invalid effect!!");
        }

        private async void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(ArcanysConstants.TAGS.PLAYER)) return;
            
            _effect.UseCollectible(other.GetComponentInChildren<PlayerController>());
            PlaySfx();
            PlayVfx();

            await Task.Delay(_delayBeforeDestroy * ArcanysConstants.INTEGERS.MILLISECOND);
            
            if(gameObject)
                Destroy(gameObject);
        }

        #endregion

        private void PlayVfx()
        {
            _model.SetActive(false);
            _vfx.SetActive(true);
        }

        private void PlaySfx()
        {
           AudioManager.Instance.PlaySFX(_sfx.clip);
        }
    }
}