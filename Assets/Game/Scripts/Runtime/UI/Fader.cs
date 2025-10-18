using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Runtime.UI
{
    public class Fader : MonoBehaviour
    {
        [Header(ArcanysConstants.INSPECTOR.REFERENCES)]
        [SerializeField] 
        private Image _imageToFade;

        public async Task FadeImage(float from, float to, float duration, CancellationToken token = default)
        {
            var image = _imageToFade;
            
            if (image == null) return;

            var color = image.color;
            var elapsed = 0f;

            while (elapsed < duration)
            {
                if (token.IsCancellationRequested) return;
                
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);
                color.a = Mathf.Lerp(from, to, t);
                if (image == null) break;
                image.color = color;
                await Task.Yield(); 
            }


            color.a = to;
            image.color = color;
        }
    }
}