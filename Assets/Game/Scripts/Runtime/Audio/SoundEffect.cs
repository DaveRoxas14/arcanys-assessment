using UnityEngine;

namespace Game.Scripts.Runtime.Audio
{
    [CreateAssetMenu(menuName = ArcanysConstants.MENU_NAME.SOUND_EFFECT)]
    public class SoundEffect : ScriptableObject
    {
        public AudioClip clip;
    }
}