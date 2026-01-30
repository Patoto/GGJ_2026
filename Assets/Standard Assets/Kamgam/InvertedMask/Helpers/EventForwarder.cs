using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kamgam.InvertedMask
{
    [RequireComponent(typeof(Imaginary))]
    [RequireComponent(typeof(CanvasRenderer))]
    public class EventForwarder : MonoBehaviour
    {
        [Header("Hole")]
        [Tooltip("The hole you want to be able to click through.")]
        public RectTransform Target;

        protected Imaginary _imaginary;
        public Imaginary Imaginary
        {
            get
            {
                if (_imaginary == null)
                {
                    _imaginary = this.GetComponent<Imaginary>();
                }
                return _imaginary;
            }
        }

        protected GraphicRaycaster _raycaster;
        public GraphicRaycaster RayCaster
        {
            get
            {
                if (_raycaster == null)
                {
                    _raycaster = this.GetComponentInParent<GraphicRaycaster>();
                }
                return _raycaster;
            }
        }

        protected EventTrigger _trigger;
        public EventTrigger Trigger
        {
            get
            {
                if (_trigger == null)
                {
                    _trigger = this.GetComponent<EventTrigger>();
                    if(_trigger == null)
                    {
                        _trigger = gameObject.AddComponent<EventTrigger>();
                    }
                }
                return _trigger;
            }
        }

        protected RectTransform _rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = transform as RectTransform;
                }
                return _rectTransform;
            }
        }

        protected Canvas _canvas;
        public Canvas Canvas
        {
            get
            {
                if (_canvas == null)
                {
                    _canvas = this.GetComponentInParent<Canvas>();
                }
                return _canvas;
            }
        }

        protected List<RaycastResult> _raycastResults = new List<RaycastResult>();

        public void Start()
        {
            addEventTrigger(Trigger, EventTriggerType.PointerClick, onPointerClick);
            addEventTrigger(Trigger, EventTriggerType.Submit, onSubmit);
        }

        public void OnEnable()
        {
            UpdateTarget();
        }

        protected void addEventTrigger(EventTrigger eventTrigger, EventTriggerType type, UnityEngine.Events.UnityAction<BaseEventData> call)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback.AddListener(call);
            eventTrigger.triggers.Add(entry);
        }

        private void onSubmit(BaseEventData e)
        {
            UpdateTarget();

            foreach (var result in _raycastResults)
            {
                var target = result.gameObject;
                if (target.TryGetComponent<ISubmitHandler>(out var submitHandler))
                {
                    submitHandler.OnSubmit(e);
                }
                else if (target.TryGetComponent<EventTrigger>(out var trigger))
                {
                    var pointerData = new BaseEventData(EventSystem.current);
                    ExecuteEvents.Execute(target, pointerData, ExecuteEvents.submitHandler);
                }
            }
        }

        private void onPointerClick(BaseEventData e)
        {
            UpdateTarget(e as PointerEventData);

            foreach (var result in _raycastResults)
            {
                var target = result.gameObject;
                if (target.TryGetComponent<IPointerClickHandler>(out var pointerHandler))
                {
                    pointerHandler.OnPointerClick(e as PointerEventData);
                    
                }
                else if (target.TryGetComponent<EventTrigger>(out var trigger))
                {
                    var pointerData = new PointerEventData(EventSystem.current);
                    pointerData.pointerPress = target;
                    pointerData.position = (e as PointerEventData).position;
                    ExecuteEvents.Execute(target, pointerData, ExecuteEvents.pointerClickHandler);
                }
            }
        }

        public void UpdateTarget(PointerEventData e = null)
        {
            if (Target == null)
                return;

            // Match the target.
            RectTransform.position = Target.position;
            RectTransform.rotation = Target.rotation;
            RectTransform.localScale = Target.localScale;
            RectTransform.sizeDelta = Target.sizeDelta;

            PointerEventData data;
            if (e != null)
            {
                data = e;
            }
            else
            {
                data = new PointerEventData(EventSystem.current);
                data.position = RectTransformUtility.WorldToScreenPoint(Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Canvas.worldCamera, transform.position);
            }
            _raycastResults.Clear();
            RayCaster.Raycast(data, _raycastResults);

            bool remove = false;
            for (int i = _raycastResults.Count-1; i >=0 ; i--) 
            {
                // Remove forwarder.
                if (_raycastResults[i].gameObject == gameObject)
                {
                    _raycastResults.RemoveAt(i);
                    continue;
                }

                // Remove all above target.
                if (remove)
                {
                    _raycastResults.RemoveAt(i);
                    continue;
                }

                // If target found then remove all above.
                if (_raycastResults[i].gameObject == Target.gameObject)
                {
                    remove = true;
                    _raycastResults.RemoveAt(i);
                }
            }
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            UpdateTarget();
        }
#endif
    }
}