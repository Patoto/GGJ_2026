using UnityEngine;
using DG.Tweening;
using System.Collections;
using NaughtyAttributes;

namespace GGJ_2026
{
    public class TransformChanger : MyMonoBehaviour
    {
        public enum TransformChangeMode
        {
            Difference,
            Speed
        }

        [SerializeField] private TransformChangeMode transformChangeMode;
        [ShowIf(nameof(transformChangeMode), TransformChangeMode.Difference)][SerializeField] private Ease easeMode = Ease.InOutSine;
        [ShowIf(nameof(transformChangeMode), TransformChangeMode.Difference)][SerializeField] private bool teleportBack = false;
        [ShowIf(nameof(transformChangeMode), TransformChangeMode.Difference)][SerializeField] private bool waitFirstTime = true;
        [ShowIf(nameof(transformChangeMode), TransformChangeMode.Difference)][SerializeField] private float moveSeconds = 1;
        [ShowIf(nameof(transformChangeMode), TransformChangeMode.Difference)][SerializeField] private float waitSeconds = 1;
        [ShowIf(nameof(transformChangeMode), TransformChangeMode.Difference)][SerializeField] private Vector3 endPositionDifference;
        [ShowIf(nameof(transformChangeMode), TransformChangeMode.Difference)][SerializeField] private Vector3 endEulerRotationDifference;
        [ShowIf(nameof(transformChangeMode), TransformChangeMode.Difference)][SerializeField] private Vector3 endLossyScaleDifference;
        [ShowIf(nameof(transformChangeMode), TransformChangeMode.Difference)][SerializeField] private Color gizmoMeshColor = Color.green;
        [ShowIf(nameof(transformChangeMode), TransformChangeMode.Difference)][SerializeField] private MeshFilter meshFilter;
        [ShowIf(nameof(transformChangeMode), TransformChangeMode.Difference)][SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [ShowIf(nameof(transformChangeMode), TransformChangeMode.Speed)][SerializeField] private Vector3 positionSpeed;
        [ShowIf(nameof(transformChangeMode), TransformChangeMode.Speed)][SerializeField] private Vector3 eulerRotationSpeed;
        [ShowIf(nameof(transformChangeMode), TransformChangeMode.Speed)][SerializeField] private Vector3 scaleSpeed;

        private bool movedFirstTime;
        private Vector3 startLocalPosition;
        private Vector3 startLocalEulerAngles;
        private Vector3 startLocalScale;
        private Vector3 meshStartPosition;
        private Vector3 meshStartEulerRotation;
        private Vector3 meshStartLossyScale;

        protected override void Awake()
        {
            base.Awake();
            SetupReferences();
        }

        private void Start()
        {
            StartTransformChange();
        }

        private void StartTransformChange()
        {
            switch (transformChangeMode)
            {
                case TransformChangeMode.Difference:
                    StartCoroutine(ChangeTransformWithDifferenceCoroutine());
                    break;
                case TransformChangeMode.Speed:
                    StartCoroutine(ChangeTransformWithSpeedCoroutine());
                    break;
            }
        }

        private void OnDrawGizmos()
        {
            CheckForDrawGizmoMesh();
        }

        private void SetupReferences()
        {
            startLocalPosition = transform.localPosition;
            startLocalEulerAngles = transform.localRotation.eulerAngles;
            startLocalScale = transform.localScale;
            if (transformChangeMode == TransformChangeMode.Difference)
            {
                Transform meshTransform = GetMeshTransform();
                meshStartPosition = meshTransform.position;
                meshStartEulerRotation = meshTransform.rotation.eulerAngles;
                meshStartLossyScale = meshTransform.lossyScale;
            }
        }

        private IEnumerator ChangeTransformWithDifferenceCoroutine()
        {
            while (true)
            {
                bool performWait = true;
                if (!movedFirstTime && !waitFirstTime)
                {
                    performWait = false;
                }
                if (performWait)
                {
                    yield return new WaitForSeconds(waitSeconds);
                }
                if (endPositionDifference != Vector3.zero)
                {
                    Vector3 endLocalPosition = startLocalPosition + endPositionDifference;
                    transform.DOLocalMove(endLocalPosition, moveSeconds).SetEase(easeMode);
                }
                if (endEulerRotationDifference != Vector3.zero)
                {
                    Vector3 endLocalEulerRotation = startLocalEulerAngles + endEulerRotationDifference;
                    transform.DOLocalRotate(endLocalEulerRotation, moveSeconds).SetEase(easeMode);
                }
                if (endLossyScaleDifference != Vector3.zero)
                {
                    Vector3 endLocalScale = startLocalScale + endLossyScaleDifference;
                    transform.DOScale(endLocalScale, moveSeconds).SetEase(easeMode);
                }
                yield return new WaitForSeconds(moveSeconds);
                movedFirstTime = true;
                yield return new WaitForSeconds(waitSeconds);
                if (teleportBack)
                {
                    transform.localPosition = startLocalPosition;
                    transform.localEulerAngles = startLocalEulerAngles;
                    transform.localScale = startLocalScale;
                }
                else
                {
                    if (endPositionDifference != Vector3.zero)
                    {
                        transform.DOLocalMove(startLocalPosition, moveSeconds).SetEase(easeMode);
                    }
                    if (endEulerRotationDifference != Vector3.zero)
                    {
                        transform.DOLocalRotate(startLocalEulerAngles, moveSeconds).SetEase(easeMode);
                    }
                    if (endLossyScaleDifference != Vector3.zero)
                    {
                        transform.DOScale(startLocalScale, moveSeconds).SetEase(easeMode);
                    }
                    yield return new WaitForSeconds(moveSeconds);
                }
            }
        }

        private IEnumerator ChangeTransformWithSpeedCoroutine()
        {
            while (true)
            {
                transform.position += positionSpeed * Time.deltaTime;
                transform.eulerAngles += eulerRotationSpeed * Time.deltaTime;
                transform.localScale += scaleSpeed * Time.deltaTime;
                yield return null;
            }
        }

        private void CheckForDrawGizmoMesh()
        {
            if (transformChangeMode == TransformChangeMode.Difference && AnyEndValueIsDifferentThanZero())
            {
                DrawGizmoMesh();
            }
        }

        private bool AnyEndValueIsDifferentThanZero()
        {
            return endPositionDifference != Vector3.zero || endEulerRotationDifference != Vector3.zero || endLossyScaleDifference != Vector3.zero;
        }

        private void DrawGizmoMesh()
        {
            UpdateGizmoMeshColor();
            Mesh gizmoMesh = GetMesh();
            const int GIZMO_MESH_SUBMESH_INDEX = -1;
            Transform meshTransform = GetMeshTransform();
            Vector3 gizmoMeshPosition = meshStartPosition;
            Vector3 gizmoMeshEulerRotation = meshStartEulerRotation;
            Vector3 gizmoMeshLossyScale = meshStartLossyScale;
            if (Utils.IsInEditorAndNotPlaying())
            {
                gizmoMeshPosition = meshTransform.position;
                gizmoMeshEulerRotation = meshTransform.rotation.eulerAngles;
                gizmoMeshLossyScale = meshTransform.lossyScale;
            }
            gizmoMeshPosition += endPositionDifference;
            gizmoMeshEulerRotation += endEulerRotationDifference;
            gizmoMeshLossyScale += endLossyScaleDifference;
            Quaternion gizmoMeshRotation = Quaternion.Euler(gizmoMeshEulerRotation);
            Gizmos.DrawMesh(gizmoMesh, GIZMO_MESH_SUBMESH_INDEX, gizmoMeshPosition, gizmoMeshRotation, gizmoMeshLossyScale);
        }

        private void UpdateGizmoMeshColor()
        {
            Color gizmoMeshColor = this.gizmoMeshColor;
            const float GIZMO_MESH_ALPHA = 0.75f;
            gizmoMeshColor.a = GIZMO_MESH_ALPHA;
            Gizmos.color = gizmoMeshColor;
        }

        private Mesh GetMesh()
        {
            Mesh mesh = null;
            if (meshFilter != null)
            {
                mesh = meshFilter.sharedMesh;
            }
            else if (skinnedMeshRenderer)
            {
                mesh = skinnedMeshRenderer.sharedMesh;
            }
            return mesh;
        }

        private Transform GetMeshTransform()
        {
            Transform meshTransform = transform;
            if (meshFilter != null)
            {
                meshTransform = meshFilter.transform;
            }
            else if (skinnedMeshRenderer)
            {
                meshTransform = skinnedMeshRenderer.rootBone.transform;
            }
            return meshTransform;
        }
    }
}