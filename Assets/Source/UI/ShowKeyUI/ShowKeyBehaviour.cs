using TMPro;
using UnityEngine;

namespace MO.UI
{
    public class ShowKeyBehaviour : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private TextMeshProUGUI keyText;

        [SerializeField]
        private TextMeshProUGUI descriptionText;

        public void SetKey(KeyCode keyCode, string description = "")
        {
            keyText.text = keyCode.ToString();
            descriptionText.text = description;
        }
    }
}

