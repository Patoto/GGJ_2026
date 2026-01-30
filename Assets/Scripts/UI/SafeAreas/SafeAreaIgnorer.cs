using UnityEngine;

namespace GGJ_2026
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaIgnorer : MyMonoBehaviour
    {
        public void UpdateRectTransform(RectTransform fullScreenReferenceCloneRectTransform)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            float leftOffset = fullScreenReferenceCloneRectTransform.offsetMin.x;
            float rightOffset = -fullScreenReferenceCloneRectTransform.offsetMax.x;
            float bottomOffset = fullScreenReferenceCloneRectTransform.offsetMin.y;
            float topOffset = -fullScreenReferenceCloneRectTransform.offsetMax.y;
            rectTransform.SetLeft(leftOffset);
            rectTransform.SetRight(rightOffset);
            rectTransform.SetBottom(bottomOffset);
            rectTransform.SetTop(topOffset);
        }
    }
}