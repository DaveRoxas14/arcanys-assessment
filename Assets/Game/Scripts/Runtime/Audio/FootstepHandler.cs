using System;
using UnityEngine;

namespace Game.Scripts.Runtime.Audio
{
    public class FootstepHandler : MonoBehaviour
    {
        [Header(ArcanysConstants.INSPECTOR.REFERENCES)]
        [SerializeField] private SoundEffect _footstep1Sfx;
        [SerializeField] private SoundEffect _footstep2Sfx;
        
        [Header(ArcanysConstants.INSPECTOR.SETTINGS)]
        [SerializeField] private int _footstepCooldown = 1;

        private GameManager _gm;
        
        private void Start()
        {
            _gm = GameManager.Instance;
        }

        public void LeftFoot()
        {
            if(!_gm.IsPlayerGrounded()) return;
            
            if(_footstep1Sfx)
                AudioManager.Instance.PlayFootsteps(_footstep1Sfx.clip);
        }

        public void RightFoot()
        {
            if(!_gm.IsPlayerGrounded()) return;
            
            if(_footstep2Sfx)
                AudioManager.Instance.PlayFootsteps(_footstep2Sfx.clip);
        }
    }
}