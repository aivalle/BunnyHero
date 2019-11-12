using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Lean.Localization
{
    // This component will update a Text component with localized text, or use a fallback if none is found
    [ExecuteInEditMode]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LeanLocalizedTextMeshPro : LeanLocalizedBehaviour
    {
        [Tooltip("If PhraseName couldn't be found, this text will be used")]
        public string FallbackText;
        // This gets called every time the translation needs updating
        public override void UpdateTranslation(LeanTranslation translation)
        {
            // Get the TextMeshUI component attached to this GameObject
            var textMesh = GetComponent<TextMeshProUGUI>();

            // Use translation?
            if (translation != null)
            {
                textMesh.text = translation.Text;
            }
            // Use fallback?
            else
            {
                textMesh.text = FallbackText;
            }
        }

        protected virtual void Awake()
        {
            // Should we set FallbackText?
            if (string.IsNullOrEmpty(FallbackText) == true)
            {
                // Get the TextMeshUI component attached to this GameObject
                var textMesh = GetComponent<TextMeshProUGUI>();

                // Copy current text to fallback
                FallbackText = textMesh.text;

            }
        }

    }
}