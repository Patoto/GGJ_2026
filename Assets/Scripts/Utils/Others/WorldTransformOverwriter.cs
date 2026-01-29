using NaughtyAttributes;
using System;
using UnityEngine;

namespace PortalRollerCoaster
{
    [ExecuteInEditMode]
	public class WorldTransformOverwriter : MyMonoBehaviour
	{
		[Header("Position")]
		[SerializeField, OnValueChanged(nameof(OverwriteWorldTransform))] private bool setPositionX;
        [SerializeField, ShowIf(nameof(setPositionX)), OnValueChanged(nameof(OverwriteWorldTransform))] private float positionX;
        [SerializeField, OnValueChanged(nameof(OverwriteWorldTransform))] private bool setPositionY;
        [SerializeField, ShowIf(nameof(setPositionY)), OnValueChanged(nameof(OverwriteWorldTransform))] private float positionY;
        [SerializeField, OnValueChanged(nameof(OverwriteWorldTransform))] private bool setPositionZ;
        [SerializeField, ShowIf(nameof(setPositionZ)), OnValueChanged(nameof(OverwriteWorldTransform))] private float positionZ;
        [Header("Euler Angles")]
        [SerializeField, OnValueChanged(nameof(OverwriteWorldTransform))] private bool setEulerAnglesX;
        [SerializeField, ShowIf(nameof(setEulerAnglesX)), OnValueChanged(nameof(OverwriteWorldTransform))] private float eulerAnglesX;
        [SerializeField, OnValueChanged(nameof(OverwriteWorldTransform))] private bool setEulerAnglesY;
        [SerializeField, ShowIf(nameof(setEulerAnglesY)), OnValueChanged(nameof(OverwriteWorldTransform))] private float eulerAnglesY;
        [SerializeField, OnValueChanged(nameof(OverwriteWorldTransform))] private bool setEulerAnglesZ;
        [SerializeField, ShowIf(nameof(setEulerAnglesZ)), OnValueChanged(nameof(OverwriteWorldTransform))] private float eulerAnglesZ;
        [Header("Scale")]
        [SerializeField, OnValueChanged(nameof(OverwriteWorldTransform))] private bool setScaleX;
        [SerializeField, ShowIf(nameof(setScaleX)), OnValueChanged(nameof(OverwriteWorldTransform))] private float scaleX;
        [SerializeField, OnValueChanged(nameof(OverwriteWorldTransform))] private bool setScaleY;
        [SerializeField, ShowIf(nameof(setScaleY)), OnValueChanged(nameof(OverwriteWorldTransform))] private float scaleY;
        [SerializeField, OnValueChanged(nameof(OverwriteWorldTransform))] private bool setScaleZ;
        [SerializeField, ShowIf(nameof(setScaleZ)), OnValueChanged(nameof(OverwriteWorldTransform))] private float scaleZ;

        protected override void Awake()
        {
            base.Awake();
            OverwriteWorldTransform();
        }

        private void Update()
        {
            if (transform.hasChanged)
            {
                OnTransformChanged();
            }
        }

        private void OnTransformChanged()
        {
            OverwriteWorldTransform();
            transform.ResetHasChanged();
        }

        [Button]
        private void OverwriteWorldTransform()
        {
            OverwritePosition();
            OverwriteEulerAngles();
            OverwriteScale();
        }

        private void OverwritePosition()
        {
            if (setPositionX)
            {
                transform.SetPositionX(positionX);
            }
            if (setPositionY)
            {
                transform.SetPositionY(positionY);
            }
            if (setPositionZ)
            {
                transform.SetPositionZ(positionZ);
            }
        }

        private void OverwriteEulerAngles()
        {
            if (setEulerAnglesX)
            {
                transform.SetEulerAnglesX(eulerAnglesX);
            }
            if (setEulerAnglesY)
            {
                transform.SetEulerAnglesY(eulerAnglesY);
            }
            if (setEulerAnglesZ)
            {
                transform.SetEulerAnglesZ(eulerAnglesZ);
            }
        }

        private void OverwriteScale()
        {
            if (setScaleX)
            {
                transform.SetLossyScaleX(scaleX);
            }
            if (setScaleY)
            {
                transform.SetLossyScaleY(scaleY);
            }
            if (setScaleZ)
            {
                transform.SetLossyScaleZ(scaleZ);
            }
        }
    }
}