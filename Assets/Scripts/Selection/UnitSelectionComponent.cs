using UnityEngine;

namespace Selection
{
    public class UnitSelectionComponent : MonoBehaviour, ISelectionComponent
    {
        private GameObject _highLight;
    
        private void Start()
        {
            _highLight = transform.Find("Highlight").gameObject;
            _highLight.SetActive(false);
        }

        public void Enable()
        {
            _highLight.SetActive(true);
        }

        public void Disable()
        {
            _highLight.SetActive(false);
        }
    }
}