using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ_2026
{
    public class MaskingBaseHighlights : MyMonoBehaviour, IEventSubscriberDeclarator
    {
        [SerializeField] private RectTransform currentTopMaskingBaseHighlightsImageRectTransform;
        [SerializeField] private RectTransform currentBottomMaskingBaseHighlightsImageRectTransform;
        [SerializeField] private RectTransform bottomMaskingBaseHighlightsLimitPoint;

        private float maskingBaseHighlightsImageXDifference;
        private float maskingBaseHighlightsImageYDifference;
        private Vector2 baseHighlightsImagesDirection;

        public void SubscribeToEnableEvents()
        {
            MaskingStrip.onAddedY += OnMaskingStripAddedY;
            MaskingTape.onSetState += OnMaskingTapeSetState;
        }

        public void UnsubscribeFromEnableEvents()
        {            
            MaskingStrip.onAddedY -= OnMaskingStripAddedY;
            MaskingTape.onSetState -= OnMaskingTapeSetState;
        }

        private void Start()
        {
            maskingBaseHighlightsImageXDifference = currentTopMaskingBaseHighlightsImageRectTransform.position.x - currentBottomMaskingBaseHighlightsImageRectTransform.position.x;
            maskingBaseHighlightsImageYDifference = currentTopMaskingBaseHighlightsImageRectTransform.position.y - currentBottomMaskingBaseHighlightsImageRectTransform.position.y;
            baseHighlightsImagesDirection = currentTopMaskingBaseHighlightsImageRectTransform.position.GetDirectionToPosition(currentBottomMaskingBaseHighlightsImageRectTransform.position).normalized;
        }

        private void OnMaskingStripAddedY(float y)
        {
            transform.SetLocalPositionX(transform.localPosition.x - (baseHighlightsImagesDirection.x * y));
            transform.SetLocalPositionY(transform.localPosition.y - (baseHighlightsImagesDirection.y * y));
            if(currentBottomMaskingBaseHighlightsImageRectTransform.position.y < bottomMaskingBaseHighlightsLimitPoint.position.y)
            {
                currentBottomMaskingBaseHighlightsImageRectTransform.SetPositionX(currentBottomMaskingBaseHighlightsImageRectTransform.position.x + (maskingBaseHighlightsImageXDifference * 2f));
                currentBottomMaskingBaseHighlightsImageRectTransform.SetPositionY(currentBottomMaskingBaseHighlightsImageRectTransform.position.y + (maskingBaseHighlightsImageYDifference * 2f));
                RectTransform tempCurrentTopMaskingStripImageRectTransform = currentTopMaskingBaseHighlightsImageRectTransform;
                currentTopMaskingBaseHighlightsImageRectTransform = currentBottomMaskingBaseHighlightsImageRectTransform;
                currentBottomMaskingBaseHighlightsImageRectTransform = tempCurrentTopMaskingStripImageRectTransform;
            }
        }

        private void OnMaskingTapeSetState(MaskingTape.State state)
        {
            if (state == MaskingTape.State.Empty)
            {
                gameObject.SetActive(false);
            }
        }
    }
}