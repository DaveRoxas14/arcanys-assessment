using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts.Runtime.Scenes
{
    public class SettingsMenu : MonoBehaviour
    {
        public static SettingsMenu Instance { get; private set; }
        
        [Header(ArcanysConstants.INSPECTOR.REFERENCES)] [SerializeField]
        private Button _defaultButton;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }
        
        public void ShowSettingsMenu()
        {
            
        }
        
        public void HideettingsMenu()
        {
            
            StartCoroutine(SelectDefaultButton());
        }
        
        private IEnumerator SelectDefaultButton()
        {
            yield return null;
            EventSystem.current.SetSelectedGameObject(_defaultButton.gameObject);
        }
    }
}