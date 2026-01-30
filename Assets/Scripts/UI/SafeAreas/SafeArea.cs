using System.Collections.Generic;
using UnityEngine;

namespace GGJ_2026
{
    public class SafeArea : MyMonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private GameObject fullScreenReference;

        private ScreenOrientation currentScreenOrientation;
        private Rect currentSafeAreaRect;

        public void Start()
        {
            Setup();
        }

        private void Setup()
        {
            UpdateSafeArea();
        }

        private void Update()
        {
            if (SafeAreaNeedsUpdate())
            {
                UpdateSafeArea();
            }
        }

        private bool SafeAreaNeedsUpdate()
        {
            bool safeAreaNeedsUpdate = false;
            if ((currentScreenOrientation != Screen.orientation) || (currentSafeAreaRect != Screen.safeArea))
            {
                safeAreaNeedsUpdate = true;
            }
            return safeAreaNeedsUpdate;
        }

        private void UpdateSafeArea()
        {
            UpdateRectTransform();
            UpdateSafeAreaIgnorers();
            UpdateCurrentValues();
        }

        private void UpdateSafeAreaIgnorers()
        {
            List<SafeAreaIgnorer> safeAreaIgnorersList = gameObject.GetAllChildrenWithComponent<SafeAreaIgnorer>(true);
            if (safeAreaIgnorersList.Count > 0)
            {
                GameObject fullScreenReferenceClone = CreateFullScreenReferenceClone();
                RectTransform fullScreenReferenceCloneRectTransform = fullScreenReferenceClone.GetComponent<RectTransform>();
                foreach (SafeAreaIgnorer tempSafeAreaIgnorer in safeAreaIgnorersList)
                {
                    tempSafeAreaIgnorer.UpdateRectTransform(fullScreenReferenceCloneRectTransform);
                }
                DestroyImmediate(fullScreenReferenceClone);
            }
        }

        private GameObject CreateFullScreenReferenceClone()
        {
            GameObject fullScreenReferenceClone = Utils.InstantiateUIElement(fullScreenReference, fullScreenReference.transform.parent, fullScreenReference.transform.localPosition);
            fullScreenReferenceClone.transform.SetParent(gameObject.transform);
            return fullScreenReferenceClone;
        }

        private void UpdateRectTransform()
        {
            Rect canvasPixelRect = canvas.pixelRect;
            Rect safeAreaRect = Screen.safeArea;
            Vector2 safeAreaLeftBottomCornerPoint = safeAreaRect.position;
            Vector2 safeAreaRightTopCornerPoint = safeAreaRect.position + safeAreaRect.size;
            Vector2 anchorMin = safeAreaLeftBottomCornerPoint;
            Vector2 anchorMax = safeAreaRightTopCornerPoint;
            anchorMin.x /= canvasPixelRect.width;
            anchorMax.x /= canvasPixelRect.width;
            anchorMin.y /= canvasPixelRect.height;
            anchorMax.y /= canvasPixelRect.height;
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }

        private void UpdateCurrentValues()
        {
            currentScreenOrientation = Screen.orientation;
            currentSafeAreaRect = Screen.safeArea;
        }
    }
}