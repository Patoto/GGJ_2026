using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ_2026
{
	public class DefaultButtonPressVisualizer : MyMonoBehaviour, IEventSubscriberDeclarator
    {
        [SerializeField] private Button button;
        [SerializeField] private RectTransform faceRectTransform;
        [SerializeField] private RectTransform outlineRectTransform;
        [SerializeField] private RectTransform shadowRectTransform;

        private float originalFaceRectTransformBottom;

        public void SubscribeToEnableEvents()
        {
            Button.onPointerDown += OnButtonPointerDown;
            Button.onPointerUp += OnButtonPointerUp;
        }

        public void UnsubscribeFromEnableEvents()
        {
            Button.onPointerDown -= OnButtonPointerDown;
            Button.onPointerUp -= OnButtonPointerUp;
        }

        protected override void Initialize()
        {
            base.Initialize();
            SetupReferences();
        }

        private void SetupReferences()
        {
            originalFaceRectTransformBottom = faceRectTransform.GetBottom();
        }

        private void TogglePressedVisuals(bool on)
        {
            if (PressedVisualsAreOn() != on)
            {
                float difference = on ? originalFaceRectTransformBottom : -originalFaceRectTransformBottom;
                faceRectTransform.AddBottom(-difference);
                outlineRectTransform.AddTop(difference);
                shadowRectTransform.AddTop(difference);
            }
        }

        private bool PressedVisualsAreOn()
        {
            return faceRectTransform.GetBottom() != originalFaceRectTransformBottom;
        }

        private void OnButtonPointerDown(PointerListener pointerListener, PointerEventData pointerEventData)
        {
            if (pointerListener == button)
            {
                TogglePressedVisuals(true);
            }
        }

        private void OnButtonPointerUp(PointerListener pointerListener, PointerEventData pointerEventData)
        {
            if (pointerListener == button)
            {
                TogglePressedVisuals(false);
            }
        }
    }
}