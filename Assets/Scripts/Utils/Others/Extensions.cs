using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;
using static UnityEngine.UI.Button;
using AnimatorControllerParameterType = UnityEngine.AnimatorControllerParameterType;
using BigInteger = System.Numerics.BigInteger;
using BindingFlags = System.Reflection.BindingFlags;
using FieldInfo = System.Reflection.FieldInfo;
using Object = UnityEngine.Object;
using PropertyInfo = System.Reflection.PropertyInfo;
using Random = UnityEngine.Random;
using HorizontalDirection = PortalRollerCoaster.Utils.HorizontalDirection;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;
using UnityEngine.UI;

namespace PortalRollerCoaster
{
    public static class Extensions
    {
        private const string BEING_DESTROYED_STRING = "(BeingDestroyed)";

        #region int
        public static string ToMinutesFormattedString(this int seconds)
        {
            int minutes = Mathf.FloorToInt(seconds / 60);
            if (minutes > 0)
            {
                seconds -= (minutes * 60);
            }
            string modifiedMinutes = Utils.AddZeroBeforeOneDigitNumber(minutes.ToString());
            string modifiedSeconds = Utils.AddZeroBeforeOneDigitNumber(seconds.ToString());
            string minutesFormat = modifiedMinutes + ":" + modifiedSeconds;
            return minutesFormat;
        }

        public static bool IsEven(this int number)
        {
            return (number % 2) == 0;
        }

        public static int GetValueTruncatedBetweenValues(this int value, int minValue = 0, int maxValue = 1)
        {
            float floatValue = value;
            return (int)floatValue.GetValueTruncatedBetweenValues(minValue, maxValue);
        }

        public static string ToStringWithLetter(this int intValue, int maxDigitsWithoutSuffixLetter = 4)
        {
            return ((BigInteger)intValue).ToStringWithLetter(maxDigitsWithoutSuffixLetter);
        }
        #endregion

        #region Behaviour
        public static void DisableAndEnable(this Behaviour behaviour)
        {
            behaviour.enabled = false;
            behaviour.enabled = true;
        }

        public static void DisableWaitAFrameAndEnable(this Behaviour behaviour)
        {
            GameManager.instance.StartCoroutine(DisableWaitAFrameAndEnableCoroutine(behaviour));
        }

        private static IEnumerator DisableWaitAFrameAndEnableCoroutine(Behaviour behaviour)
        {
            behaviour.enabled = false;
            yield return null;
            behaviour.enabled = true;
        }
        #endregion

        #region GameObject
        public static List<GameObject> GetChildren(this GameObject gameObject, bool includeInactive = true, bool includeMe = false)
        {
            List<GameObject> childrenGameObjectsList = new();
            if (includeMe)
            {
                childrenGameObjectsList.Add(gameObject);
            }
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                GameObject childGameObject = gameObject.transform.GetChild(i).gameObject;
                bool shouldInclude = true;
                if (!includeInactive && !childGameObject.activeSelf)
                {
                    shouldInclude = false;
                }
                if (shouldInclude)
                {
                    childrenGameObjectsList.Add(childGameObject);
                }
            }
            return childrenGameObjectsList;
        }

        public static List<GameObject> GetAllChildren(this GameObject gameObject, bool includeInactive = true, bool includeMe = false)
        {
            List<GameObject> allChildrenGameObjectsList = new();
            List<GameObject> childrenGameObjectsList = gameObject.GetChildren(includeInactive, includeMe);
            allChildrenGameObjectsList.AddRange(childrenGameObjectsList);
            foreach (GameObject iChildGameObject in childrenGameObjectsList)
            {
                allChildrenGameObjectsList.AddRange(iChildGameObject.GetAllChildren(includeInactive));
            }
            return allChildrenGameObjectsList;
        }

        public static List<T> GetAllChildrenWithComponent<T>(this GameObject gameObject, bool includeInactive = true, bool includeMe = false) where T : Component
        {
            return gameObject.GetAllChildren(includeInactive, includeMe).GetGameObjectsWithComponent<T>();
        }

        public static List<T> GetChildrenWithComponent<T>(this GameObject gameObject, bool includeInactive = true, bool includeMe = false) where T : Component
        {
            return gameObject.GetChildren(includeInactive, includeMe).GetGameObjectsWithComponent<T>();
        }

        public static T GetFirstChildWithComponent<T>(this GameObject gameObject, bool includeInactive = true, bool includeMe = false) where T : Component
        {
            return gameObject.GetAllChildrenWithComponent<T>(includeInactive, includeMe).FirstOrDefault();
        }

        public static T GetComponentOfTypeOnMeOrAnyChildren<T>(this GameObject gameObject, bool includeInactive = true) where T : Component
        {
            T componentOfTypeOnMeOrAnyChildren = gameObject.GetAllChildrenWithComponent<T>(includeInactive, true).FirstOrDefault();
            return componentOfTypeOnMeOrAnyChildren;
        }

        public static List<GameObject> GetAllDirectParents(this GameObject gameObject, bool incluedMe = false)
        {
            List<GameObject> allDirectParentsList = new();
            if (incluedMe)
            {
                allDirectParentsList.Add(gameObject);
            }
            Transform parentTransform = gameObject.transform.parent;
            if (parentTransform != null)
            {
                GameObject parentGameObject = parentTransform.gameObject;
                allDirectParentsList.Add(parentGameObject);
                allDirectParentsList.AddRange(parentGameObject.GetAllDirectParents());
            }
            return allDirectParentsList;
        }

        public static List<T> GetAllDirectParentsWithComponent<T>(this GameObject gameObject, bool includeMe = false) where T : Component
        {
            return gameObject.GetAllDirectParents(includeMe).GetGameObjectsWithComponent<T>();
        }

        public static T GetFirstDirectParentWithComponent<T>(this GameObject gameObject, bool includeMe = false) where T : Component
        {
            return gameObject.GetAllDirectParentsWithComponent<T>(includeMe).FirstOrDefault();
        }

        public static bool HasComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponents<T>().Length > 0;
        }

        public static List<Collider> GetAllColliderComponents(this GameObject gameObject)
        {
            List<Collider> allColliderComponents = new();
            BoxCollider[] boxColliders = gameObject.GetComponents<BoxCollider>();
            SphereCollider[] sphereColliders = gameObject.GetComponents<SphereCollider>();
            CapsuleCollider[] capsuleColliders = gameObject.GetComponents<CapsuleCollider>();
            MeshCollider[] meshColliders = gameObject.GetComponents<MeshCollider>();
            TerrainCollider[] terrainColliders = gameObject.GetComponents<TerrainCollider>();
            allColliderComponents.AddRange(boxColliders);
            allColliderComponents.AddRange(sphereColliders);
            allColliderComponents.AddRange(capsuleColliders);
            allColliderComponents.AddRange(meshColliders);
            allColliderComponents.AddRange(terrainColliders);
            return allColliderComponents;
        }

        public static bool HasTag(this GameObject gameObject, Tag tag)
        {
            bool hasTag = false;
            List<TagHolder> tagHoldersList = new(gameObject.GetComponents<TagHolder>());
            foreach (TagHolder iTagHolder in tagHoldersList)
            {
                if (iTagHolder?.tag != null && iTagHolder.tag.name.Equals(tag.name))
                {
                    hasTag = true;
                    break;
                }
            }
            return hasTag;
        }

        public static bool HasAnyOfTheseTags(this GameObject gameObject, List<Tag> tagsList)
        {
            return tagsList.Count(iTag => gameObject.HasTag(iTag)) > 0;
        }

        public static bool HasTagWithReferencedComponent<T>(this GameObject gameObject, Tag tag, out T referencedComponent) where T : Component
        {
            bool hasTagWithReferencedComponent = false;
            referencedComponent = null;
            if (gameObject.HasTag(tag))
            {
                TagHolder tagHolder = gameObject.GetComponent<TagHolder>();
                referencedComponent = tagHolder.GetReferencedComponent<T>();
                if (referencedComponent != null)
                {
                    hasTagWithReferencedComponent = true;
                }
            }
            return hasTagWithReferencedComponent;
        }

        public static void AddIdToName(this GameObject gameObject)
        {
            string instanceID = gameObject.GetInstanceID().ToString();
            if (!gameObject.name.Contains(instanceID))
            {
                gameObject.name += " (ID:" + instanceID + ")";
            }
        }

        public static void ChangeLayerOnMeAndAllMyChildren(this GameObject gameObject, string layerName)
        {
            List<GameObject> meAndAllMyChildrenList = gameObject.GetAllChildren(includeMe: true);
            foreach (GameObject iGameObject in meAndAllMyChildrenList)
            {
                iGameObject.layer = LayerMask.NameToLayer(layerName);
            }
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        public static bool IsChildOf(this GameObject gameObjectA, GameObject gameObjectB, bool includeMe = false)
        {
            return gameObjectB.GetAllChildren(includeMe: includeMe).Contains(gameObjectA);
        }

        public static Bounds GetAllChildrenRenderersCombinedBounds(this GameObject gameObject)
        {
            return gameObject.GetAllChildrenWithComponent<Renderer>().GetCombinedBounds();
        }

        public static void SetParentCanvasAsParent(this GameObject gameObject)
        {
            gameObject.SetParent(gameObject.GetFirstDirectParentWithComponent<Canvas>().gameObject);
        }

        public static void SetParent(this GameObject gameObject, GameObject parentGameObject)
        {
            gameObject.transform.SetParent(parentGameObject);
        }
        #endregion

        #region Animator
        public static void PlayAnimationFromNormalizedTime(this Animator animator, string stateName, float normalizedTime, int layer = 0)
        {
            animator.Play(stateName, layer, normalizedTime);
        }

        public static void PlayAnimationFromNormalizedTime(this Animator animator, int stateNameHash, float normalizedTime, int layer = 0)
        {
            animator.Play(stateNameHash, layer, normalizedTime);
        }

        public static void PlayAnimationFromStart(this Animator animator, string stateName, int layer = 0)
        {
            animator.PlayAnimationFromNormalizedTime(stateName, 0, layer);
        }

        public static void PlayAnimationFromStart(this Animator animator, int stateNameHash, int layer = 0)
        {
            animator.PlayAnimationFromNormalizedTime(stateNameHash, 0, layer);
        }

        public static void PlayAnimationFromRandomNormalizedTime(this Animator animator, string stateName, int layer = 0)
        {
            animator.PlayAnimationFromNormalizedTime(stateName, RandomExtensions.Range01(), layer);
        }

        public static void PlayAnimationFromRandomNormalizedTime(this Animator animator, int stateHash, int layer = 0)
        {
            animator.PlayAnimationFromNormalizedTime(stateHash, RandomExtensions.Range01(), layer);
        }

        public static void ResetAllTriggers(this Animator animator)
        {
            animator.parameters
                .Where(iAnimatorControllerParameter => iAnimatorControllerParameter.type == AnimatorControllerParameterType.Trigger)
                .ToList().ForEach(iAnimatorControllerParameter => animator.ResetTrigger(iAnimatorControllerParameter.name));
        }

        public static AnimationClip GetAnimationClip(this Animator animator, string animationClipName)
        {
            return animator.runtimeAnimatorController.animationClips.Where(iAnimationClip => iAnimationClip.name.Equals(animationClipName)).FirstOrDefault();
        }

        public static void SetTriggerAndResetOthers(this Animator animator, string triggerName)
        {
            animator.ResetAllTriggers();
            animator.SetTrigger(triggerName);
        }

        public static string GetCurrentAnimationClipName(this Animator animator, int layerIndex = 0)
        {
            return animator.GetCurrentAnimatorClipInfo(layerIndex)[0].clip.name;
        }

        public static IEnumerator PlayAnimationFromStartAndWaitForCompletionCoroutine(this Animator animator, string stateName, int layerIndex = 0)
        {
            animator.PlayAnimationFromStart(stateName, layerIndex);
            animator.Update(0f);
            while (true)
            {
                if (animator.CurrentStateIsState(stateName) && !animator.IsPlayingAnAnimation())
                {
                    break;
                }
                yield return null;
            }
        }

        public static bool CurrentStateIsState(this Animator animator, string stateName, int layerIndex = 0)
        {
            return animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
        }

        public static bool IsPlayingAnAnimation(this Animator animator, int layerIndex = 0)
        {
            return animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime < 1f || animator.IsInTransition(layerIndex);
        }
        #endregion

        #region List<T>
        public class UnusedElementsListData<T>
        {
            public enum GetOrderType
            {
                FirstToLast,
                LastToFirst,
                Random
            }

            public List<T> unusedElementsList = new();

            public readonly List<T> originalList = new();

            public UnusedElementsListData(List<T> originalList)
            {
                this.originalList = originalList;
                ResetUnusedElementsList();
            }

            public void ResetUnusedElementsList()
            {
                unusedElementsList = new(originalList);
            }

            public T GetUnusedElement(GetOrderType getOrderType = GetOrderType.FirstToLast)
            {
                if (unusedElementsList.IsEmpty())
                {
                    ResetUnusedElementsList();
                }
                T unusedElement = unusedElementsList.FirstOrDefault();
                switch (getOrderType)
                {
                    case GetOrderType.LastToFirst:
                        unusedElement = unusedElementsList.LastOrDefault();
                        break;
                    case GetOrderType.Random:
                        unusedElement = unusedElementsList.GetRandom();
                        break;
                }
                unusedElementsList.RemoveIfIsInList(unusedElement);
                return unusedElement;
            }
        }

        private static readonly List<UnusedElementsListData<object>> unusedElementsListDatasList = new();

        public static T GetRandom<T>(this List<T> list)
        {
            int randomIndex = Random.Range(0, list.Count);
            T randomInList = default;
            if (list.ValueAtIndexExists(randomIndex))
            {
                randomInList = list[randomIndex];
            }
            return randomInList;
        }

        public static bool HasNull<T>(this List<T> objectsList) where T : Object
        {
            return objectsList.Count(iObject => iObject == null) > 0;
        }

        public static bool IndexIsBeyondHalfIndex<T>(this List<T> list, int index)
        {
            bool indexIsBeyondHalfIndex = false;
            int lastIndex = list.Count - 1;
            int halfIndex = lastIndex / 2;
            if (index > halfIndex)
            {
                indexIsBeyondHalfIndex = true;
            }
            return indexIsBeyondHalfIndex;
        }

        public static void AddIfNotInList<T>(this List<T> list, T t)
        {
            if (!list.Contains(t))
            {
                list.Add(t);
            }
        }

        public static void RemoveIfIsInList<T>(this List<T> list, T t)
        {
            if (list.Contains(t))
            {
                list.Remove(t);
            }
        }

        public static bool IsEqualToList<T>(this List<T> list, List<T> otherList)
        {
            return list.Count == otherList.Count && list.All(otherList.Contains);
        }

        public static T GetUnusedElement<T>(this List<T> list, UnusedElementsListData<object>.GetOrderType getOrderType = UnusedElementsListData<object>.GetOrderType.FirstToLast) where T : class
        {
            UnusedElementsListData<object> unusedElementsListData = list.GetOrCreateUnusedElementsListData();
            unusedElementsListDatasList.AddIfNotInList(unusedElementsListData);
            return unusedElementsListData.GetUnusedElement(getOrderType) as T;
        }

        public static void ResetUnusedElementsList<T>(this List<T> list) where T : class
        {
            list.GetOrCreateUnusedElementsListData().ResetUnusedElementsList();
        }

        private static UnusedElementsListData<object> GetOrCreateUnusedElementsListData<T>(this List<T> list) where T : class
        {
            List<object> objectsList = list.Select(iT => iT as object).ToList();
            UnusedElementsListData<object> unusedElementsListData = unusedElementsListDatasList
            .FirstOrDefault(iUnusedElementsListDatasList => iUnusedElementsListDatasList.originalList != null && iUnusedElementsListDatasList.originalList.IsEqualToList(objectsList));
            unusedElementsListData ??= new UnusedElementsListData<object>(objectsList);
            return unusedElementsListData;
        }

        public static List<T> WithTag<T>(this List<T> tsList, Tag tag) where T : Component
        {
            return tsList.WithoutNulls().Select(iT => iT.gameObject).ToList().WithTag(tag)
            .Select(iGameObject => iGameObject.GetComponent<T>()).ToList();
        }

        public static List<T> WithAnyOfTheseTags<T>(this List<T> tsList, List<Tag> tagsList) where T : Component
        {
            List<T> tsListWithAnyOfTheseTags = new();
            tagsList.ForEach(iTag => tsListWithAnyOfTheseTags.AddRange(tsList.WithTag(iTag)));
            tsListWithAnyOfTheseTags.RemoveDuplicates();
            return tsListWithAnyOfTheseTags;
        }

        public static void PrintNames<T>(this List<T> tsList) where T : Object
        {
            tsList.GetNamesList().PrintValues();
        }

        public static void PrintValues<T>(this List<T> tsList)
        {
            Debug.Log(tsList.ToReadableString());
        }

        public static string ToReadableString<T>(this List<T> tsList, char separationChar = ',')
        {
            string readableString = "";
            for (int i = 0; i < tsList.Count; i++)
            {
                T iT = tsList[i];
                if (i > 0)
                {
                    readableString += separationChar;
                }
                readableString += iT;
            }
            return readableString;
        }

        public static List<string> GetNamesList<T>(this List<T> tsList) where T : Object
        {
            return tsList.Select(iT => iT.name).ToList();
        }

        public static void RemoveDuplicates<T>(this List<T> tsList)
        {
            HashSet<T> hashSet = new();
            tsList.RemoveAll(iT => !hashSet.Add(iT));
        }

        public static List<T> WithoutNulls<T>(this List<T> tsList) where T : Object
        {
            return tsList.Where(iT => iT != null).ToList();
        }
        
        public static List<T> GetClone<T>(this List<T> tsList)
        {
            List<T> tsCloneList = new(tsList);
            return tsCloneList;
        }

        public static bool OnlyHasThisElement<T>(this List<T> tsList, T t) where T : Object
        {
            return tsList.Count == 1 && tsList.FirstOrDefault() == t;
        }
        #endregion

        #region List<GameObject>
        public static List<T> GetGameObjectsWithComponent<T>(this List<GameObject> gameObjectsList) where T : Component
        {
            return gameObjectsList.Where(iGameObject => iGameObject.HasComponent<T>())
                .Select(iGameObject => iGameObject.GetComponent<T>())
                .ToList();
        }

        public static IEnumerator ScaleCoroutine(this List<GameObject> gameObjectsList, float startScale = 0f, float endScale = 1f, float seconds = 0.5f, Ease ease = Ease.OutSine)
        {
            gameObjectsList.ForEach(iGameObject => iGameObject.transform.localScale = Vector3.one * startScale);
            yield return ScaleCoroutine(gameObjectsList, endScale, seconds, ease);
        }

        public static IEnumerator ScaleCoroutine(this List<GameObject> gameObjectsList, float endScale = 1f, float seconds = 0.5f, Ease ease = Ease.OutSine)
        {
            gameObjectsList.ForEach(iGameObject => iGameObject.transform.DOScale(endScale, seconds).SetEase(ease));
            yield return new WaitForSeconds(seconds);
        }

        public static IEnumerator DistributeAsCircleCoroutine(this List<GameObject> gameObjectsList, Vector3 centerPosition, Vector3 dimension1Direction, Vector3 dimension2Direction, float radius = 0.15f, float radiusVariance = 0f, float seconds = 0.5f, Ease ease = Ease.OutSine)
        {
            int gameObjectsListCount = gameObjectsList.Count;
            for (int i = 0; i < gameObjectsListCount; i++)
            {
                GameObject iGameObject = gameObjectsList[i];
                float sinCosValue = i / (float)gameObjectsListCount * Mathf.PI * 2f;
                radius += Random.Range(-radiusVariance, radiusVariance);
                float dimension1Distance = Mathf.Sin(sinCosValue) * radius;
                float dimension2Distance = Mathf.Cos(sinCosValue) * radius;
                Vector3 offset = (dimension1Direction * dimension1Distance) + (dimension2Direction * dimension2Distance);
                Vector3 targetPosition = centerPosition + offset;
                iGameObject.transform.DOMove(targetPosition, seconds).SetEase(ease);
            }
            yield return new WaitForSeconds(seconds);
        }

        public static IEnumerator MoveToPositionSequentiallyCoroutine(this List<GameObject> gameObjectsList, Vector3 position, float secondsToReachPosition = 0.5f, float secondsBetweenMove = 0.1f, Ease ease = Ease.OutSine, Action<GameObject> onStartedMoving = null, Action<GameObject> onReachedPosition = null)
        {
            foreach (GameObject iGameObject in gameObjectsList)
            {
                onStartedMoving?.Invoke(iGameObject);
                iGameObject.transform.DOMove(position, secondsToReachPosition).SetEase(ease).OnComplete(() => onReachedPosition?.Invoke(iGameObject));
                yield return new WaitForSeconds(secondsBetweenMove);
            }
            yield return new WaitForSeconds(secondsToReachPosition);
        }

        public static List<GameObject> WithTag(this List<GameObject> gameObjectsList, Tag tag)
        {
            return gameObjectsList.Where(iGameObject => iGameObject.HasTag(tag)).ToList();
        }

        public static List<GameObject> WithoutTag(this List<GameObject> gameObjectsList, Tag tag)
        {
            return gameObjectsList.Where(iGameObject => !iGameObject.HasTag(tag)).ToList();
        }
        #endregion

        #region T[]
        public static bool ValueAtIndexExists<T>(this T[] array, int index)
        {
            return (index >= 0) && (index < array.Length);
        }

        public static T[] Remove<T>(this T[] array, T t)
        {
            List<T> tList = array.ToList();
            tList.Remove(t);
            return tList.ToArray();
        }

        public static T[] RemoveAtIndex<T>(this T[] array, int index)
        {
            List<T> tList = array.ToList();
            tList.RemoveAt(index);
            return tList.ToArray();
        }
        #endregion

        #region List<Vector3>
        public static Vector3 GetCenter(this List<Vector3> vector3sList)
        {
            return vector3sList.GetSum() / vector3sList.Count();
        }

        public static Vector3 GetSum(this List<Vector3> vector3sList)
        {
            Vector3 sum = Vector3.zero;
            vector3sList.ForEach(iVector3 => sum += iVector3);
            return sum;
        }
        #endregion

        #region Vector3
        public static Vector3 GetHorizontalDirectionToPosition(this Vector3 originPosition, Vector3 targetPosition)
        {
            return originPosition.GetDirectionToPosition(targetPosition, true, false, true);
        }

        public static Vector3 GetVerticalDirectionToPosition(this Vector3 originPosition, Vector3 targetPosition)
        {
            return originPosition.GetDirectionToPosition(targetPosition, false, true, false);
        }

        public static Vector3 GetDirectionToPosition(this Vector3 originPosition, Vector3 targetPosition, bool useX = true, bool useY = true, bool useZ = true)
        {
            if (!useX)
            {
                targetPosition.x = 0;
                originPosition.x = 0;
            }
            if (!useY)
            {
                targetPosition.y = 0;
                originPosition.y = 0;
            }
            if (!useZ)
            {
                targetPosition.z = 0;
                originPosition.z = 0;
            }
            Vector3 directionToPosition = (targetPosition - originPosition).normalized;
            return directionToPosition;
        }

        public static float GetBiggestDimensionValue(this Vector3 vector3)
        {
            float biggestDimensionValue = Mathf.Max(Mathf.Max(vector3.x, vector3.y), vector3.z);
            return biggestDimensionValue;
        }

        public static bool IsSimilarDirectionTo(this Vector3 directionA, Vector3 directionB, float maxAngleDifference = 1f)
        {
            return Vector3.Angle(directionA, directionB) < maxAngleDifference;
        }

        public static Vector3 GetCenterPositionBetweenOtherPosition(this Vector3 positionA, Vector3 positionB)
        {
            return (positionA + positionB) / 2f;
        }

        public static float GetDistanceToPosition(this Vector3 positionA, Vector3 positionB)
        {
            return Vector3.Distance(positionA, positionB);
        }
        #endregion

        #region Transform
        public static void ResetLocalValues(this Transform transform)
        {
            ResetLocalPosition(transform);
            ResetLocalRotation(transform);
            ResetLocalScale(transform);
        }

        public static void ResetLocalPosition(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
        }

        public static void ResetLocalRotation(this Transform transform)
        {
            transform.localRotation = Quaternion.identity;
        }

        public static void ResetLocalScale(this Transform transform)
        {
            transform.localScale = Vector3.one;
        }

        public static void SetPositionX(this Transform transform, float x)
        {
            Vector3 position = transform.position;
            position.x = x;
            transform.position = position;
        }

        public static void SetPositionY(this Transform transform, float y)
        {
            Vector3 position = transform.position;
            position.y = y;
            transform.position = position;
        }

        public static void SetPositionZ(this Transform transform, float z)
        {
            Vector3 position = transform.position;
            position.z = z;
            transform.position = position;
        }

        public static void SetHorizontalPosition(this Transform transform, float x, float z)
        {
            SetPositionX(transform, x);
            SetPositionZ(transform, z);
        }

        public static void SetLocalPositionX(this Transform transform, float localX)
        {
            Vector3 localPosition = transform.localPosition;
            localPosition.x = localX;
            transform.localPosition = localPosition;
        }

        public static void SetLocalPositionY(this Transform transform, float localY)
        {
            Vector3 localPosition = transform.localPosition;
            localPosition.y = localY;
            transform.localPosition = localPosition;
        }

        public static void SetLossyScaleX(this Transform transform, float targetLossyScaleX)
        {
            float localScaleX = targetLossyScaleX;
            float currentLossyScaleX = transform.lossyScale.x;
            if (currentLossyScaleX != 0f)
            {
                localScaleX = transform.localScale.x * targetLossyScaleX / currentLossyScaleX;
            }
            transform.SetLocalScaleX(localScaleX);
        }

        public static void SetLossyScaleY(this Transform transform, float targetLossyScaleY)
        {
            float localScaleY = targetLossyScaleY;
            float currentLossyScaleY = transform.lossyScale.y;
            if (currentLossyScaleY != 0f)
            {
                localScaleY = transform.localScale.y * targetLossyScaleY / currentLossyScaleY;
            }
            transform.SetLocalScaleY(localScaleY);
        }

        public static void SetLossyScaleZ(this Transform transform, float targetLossyScaleZ)
        {
            float localScaleZ = targetLossyScaleZ;
            float currentLossyScaleZ = transform.lossyScale.z;
            if (currentLossyScaleZ != 0f)
            {
                localScaleZ = transform.localScale.z * targetLossyScaleZ / currentLossyScaleZ;
            }
            transform.SetLocalScaleZ(localScaleZ);
        }

        public static void SetLocalScaleX(this Transform transform, float localX)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = localX;
            transform.localScale = localScale;
        }

        public static void SetLocalScaleY(this Transform transform, float localY)
        {
            Vector3 localScale = transform.localScale;
            localScale.y = localY;
            transform.localScale = localScale;
        }

        public static void SetLocalScaleZ(this Transform transform, float localZ)
        {
            Vector3 localScale = transform.localScale;
            localScale.z = localZ;
            transform.localScale = localScale;
        }

        public static void SetLocalPositionZ(this Transform transform, float localZ)
        {
            Vector3 localPosition = transform.localPosition;
            localPosition.z = localZ;
            transform.localPosition = localPosition;
        }

        public static void SetEulerAnglesX(this Transform transform, float eulerAnglesX)
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.x = eulerAnglesX;
            transform.eulerAngles = eulerAngles;
        }

        public static void SetEulerAnglesY(this Transform transform, float eulerAnglesY)
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.y = eulerAnglesY;
            transform.eulerAngles = eulerAngles;
        }

        public static void SetEulerAnglesZ(this Transform transform, float eulerAnglesZ)
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.z = eulerAnglesZ;
            transform.eulerAngles = eulerAngles;
        }

        public static void SetLocalEulerAnglesX(this Transform transform, float localEulerAnglesX)
        {
            Vector3 localEulerAngles = transform.localEulerAngles;
            localEulerAngles.x = localEulerAnglesX;
            transform.localEulerAngles = localEulerAngles;
        }

        public static void SetLocalEulerAnglesY(this Transform transform, float localEulerAnglesY)
        {
            Vector3 localEulerAngles = transform.localEulerAngles;
            localEulerAngles.y = localEulerAnglesY;
            transform.localEulerAngles = localEulerAngles;
        }

        public static void SetLocalEulerAnglesZ(this Transform transform, float localEulerAnglesZ)
        {
            Vector3 localEulerAngles = transform.localEulerAngles;
            localEulerAngles.z = localEulerAnglesZ;
            transform.localEulerAngles = localEulerAngles;
        }

        public static void AddLocalEulerAnglesX(this Transform transform, float localEulerAnglesX)
        {
            transform.SetLocalEulerAnglesX(transform.localEulerAngles.x + localEulerAnglesX);
        }

        public static void AddLocalEulerAnglesY(this Transform transform, float localEulerAnglesY)
        {
            transform.SetLocalEulerAnglesY(transform.localEulerAngles.y + localEulerAnglesY);
        }

        public static void AddLocalEulerAnglesZ(this Transform transform, float localEulerAnglesZ)
        {
            transform.SetLocalEulerAnglesZ(transform.localEulerAngles.z + localEulerAnglesZ);
        }

        public static void AddEulerAnglesX(this Transform transform, float eulerAnglesX)
        {
            transform.SetEulerAnglesX(transform.eulerAngles.x + eulerAnglesX);
        }

        public static void AddEulerAnglesY(this Transform transform, float eulerAnglesY)
        {
            transform.SetEulerAnglesY(transform.eulerAngles.y + eulerAnglesY);
        }

        public static void AddEulerAnglesZ(this Transform transform, float eulerAnglesZ)
        {
            transform.SetEulerAnglesZ(transform.eulerAngles.z + eulerAnglesZ);
        }

        public static void SetHorizontalLocalPosition(this Transform transform, float localX, float localZ)
        {
            SetLocalPositionX(transform, localX);
            SetLocalPositionZ(transform, localZ);
        }

        public static Tween DoElasticPunchInTween(this Transform transform, float endScale = 1f, float seconds = 1f)
        {
            transform.localScale = Vector3.zero;
            return transform.DOScale(endScale, seconds).SetEase(Ease.OutElastic);
        }

        public static Sequence DoPunchSequence(this Transform transform, float punchScaleDifference = 0.25f, float seconds = 0.2f)
        {
            Vector3 originalLocalScale = Vector3.one;
            Vector3 punchScaleDifferenceVector3 = new(punchScaleDifference, punchScaleDifference, punchScaleDifference);
            Vector3 punchScale = originalLocalScale + punchScaleDifferenceVector3;
            Sequence sequence = DOTween.Sequence();
            float halfSeconds = seconds / 2;
            sequence.Append(transform.DOScale(punchScale, halfSeconds));
            sequence.Append(transform.DOScale(originalLocalScale, halfSeconds));
            sequence.Play();
            return sequence;
        }

        public static Tween DoBouncyFallInTween(this Transform transform, float endY, float seconds = 1f)
        {
            float startY = Camera.main.pixelHeight;
            transform.SetPositionY(startY);
            return transform.DoBouncyFallTween(endY, seconds);
        }

        public static Tween DoBouncyFallTween(this Transform transform, float endY, float seconds = 1f, bool useLocalY = false)
        {
            Tween tween;
            if (useLocalY)
            {
                tween = transform.DOLocalMoveY(endY, seconds).SetEase(Ease.OutBounce);
            }
            else
            {
                tween = transform.DOMoveY(endY, seconds).SetEase(Ease.OutBounce);
            }
            return tween;
        }

        public static Sequence DoWaveSequence(this Transform transform, float waveScaleVaration = 0.1f, float waveSeconds = 1f)
        {
            const Ease EASE_MODE = Ease.InOutSine;
            Vector3 waveScaleVariationVector3 = Vector3.one * waveScaleVaration;
            Vector3 originalScale = transform.localScale;
            Vector3 waveScaleA = originalScale + waveScaleVariationVector3;
            Vector3 waveScaleB = originalScale - waveScaleVariationVector3;
            float halfWaveSeconds = waveSeconds / 2;
            transform.localScale = waveScaleB;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOScale(waveScaleA, halfWaveSeconds).SetEase(EASE_MODE));
            sequence.Append(transform.DOScale(waveScaleB, halfWaveSeconds).SetEase(EASE_MODE));
            sequence.SetLoops(-1);
            sequence.Play();
            return sequence;
        }

        public static Sequence DoHorizontalShakeSequence(this Transform transform, float shakeRadius = 10f, float shakeHalfMoveSeconds = 0.05f, int shakesAmount = 2)
        {
            Sequence sequence = DOTween.Sequence();
            Vector3 originalPosition = transform.position;
            Vector3 rightVectorDistance = Vector3.right * shakeRadius;
            Vector3 moveRightDestination = originalPosition + rightVectorDistance;
            Vector3 moveLeftDestination = originalPosition - rightVectorDistance;
            for (int i = 0; i < shakesAmount; i++)
            {
                sequence.Append(transform.DOMove(moveRightDestination, shakeHalfMoveSeconds));
                sequence.Append(transform.DOMove(moveLeftDestination, shakeHalfMoveSeconds));
            }
            sequence.Append(transform.DOMove(originalPosition, shakeHalfMoveSeconds));
            sequence.Play();
            return sequence;
        }

        public static IEnumerator MoveThroughPositionsListCoroutine(this Transform transform, List<Vector3> positionsList, float seconds)
        {
            int positionsListCount = positionsList.Count;
            float betweenPositionsMoveSeconds = seconds / positionsListCount;
            int lastIndex = positionsListCount - 1;
            for (int i = 0; i < positionsListCount; i++)
            {
                int extraIndexes = Mathf.FloorToInt(Time.deltaTime / betweenPositionsMoveSeconds);
                if (extraIndexes > 0)
                {
                    i = Mathf.Min(i + extraIndexes, lastIndex);
                }
                int nextIndex = Mathf.Min(i + 1, lastIndex);
                Vector3 nextIndexPosition = positionsList[nextIndex];
                yield return transform.DOMove(nextIndexPosition, betweenPositionsMoveSeconds).WaitForCompletion();
            }
        }

        public static IEnumerator MoveForwardCoroutine(this Transform transform, float speed)
        {
            while (true)
            {
                float currentFrameForwardSpeed = speed * Time.deltaTime;
                Vector3 currentFrameForwardVelocity = transform.forward * currentFrameForwardSpeed;
                transform.position += currentFrameForwardVelocity;
                yield return null;
            }
        }

        public static Tween DoMoveToNewParentTween(this Transform transform, GameObject newParent, float seconds = 0.5f, Ease ease = Ease.InOutSine, float newScale = 1f)
        {
            transform.SetParent(newParent);
            transform.DOScale(Vector3.one * newScale, seconds).SetEase(ease);
            transform.DOLocalRotate(Vector3.zero, seconds, RotateMode.FastBeyond360).SetEase(ease);
            return transform.DOLocalMove(Vector3.zero, seconds).SetEase(ease);
        }

        public static Sequence DoJumpAndLookAtDirectionSequence(this Transform transform, float jumpHeight = 1f, Vector3 lookAtDirection = default, float seconds = 0.5f)
        {
            float startHeight = transform.position.y;
            float topHeight = startHeight + jumpHeight;
            float halfSeconds = seconds / 2;
            Vector3 lookAtPoint = transform.position + lookAtDirection;
            transform.DOLookAt(lookAtPoint, seconds);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOMoveY(topHeight, halfSeconds).SetEase(Ease.OutSine));
            sequence.Append(transform.DOMoveY(startHeight, halfSeconds).SetEase(Ease.InSine));
            sequence.Play();
            return sequence;
        }

        public static Sequence DoSpinSequence(this Transform transform, Utils.Dimension dimension, float spinSeconds = 1f, Ease easeMode = Ease.Linear)
        {
            Sequence spinSequence = DOTween.Sequence();
            Vector3 endRotateValue = transform.localEulerAngles;
            switch (dimension)
            {
                case Utils.Dimension.X:
                    endRotateValue += new Vector3(360f, 0f, 0f);
                    break;
                case Utils.Dimension.Y:
                    endRotateValue += new Vector3(0f, 360f, 0f);
                    break;
                case Utils.Dimension.Z:
                    endRotateValue += new Vector3(0f, 0f, 360f);
                    break;
            }
            spinSequence.Append(transform.DOLocalRotate(endRotateValue, spinSeconds, RotateMode.FastBeyond360).SetEase(easeMode));
            spinSequence.SetLoops(-1);
            return spinSequence;
        }

        public static void ResetHasChanged(this Transform transform)
        {
            transform.hasChanged = false;
        }

        public static Vector3 GetWorldPositionWithLocalPosition(this Transform transform, Vector3 localPosition)
        {
            return transform.TransformPoint(localPosition);
        }

        public static Vector3 GetWorldDirectionWithLocalDirection(this Transform transform, Vector3 localDirection)
        {
            return transform.TransformDirection(localDirection);
        }

        public static Vector3 GetLocalPositionWithWorldPosition(this Transform transform, Vector3 worldPosition)
        {
            return transform.InverseTransformPoint(worldPosition);
        }

        public static Vector3 GetLocalDirectionWithWorldDirection(this Transform transform, Vector3 worldDirection)
        {
            return transform.InverseTransformDirection(worldDirection);
        }

        public static void SetParent(this Transform transform, GameObject parent)
        {
            transform.SetParent(parent.transform);
        }
        #endregion

        #region RectTransform
        public static void SetLeft(this RectTransform rectTransform, float left)
        {
            rectTransform.offsetMin = new Vector2(left, rectTransform.offsetMin.y);
        }

        public static void SetRight(this RectTransform rectTransform, float right)
        {
            rectTransform.offsetMax = new Vector2(-right, rectTransform.offsetMax.y);
        }

        public static void SetBottom(this RectTransform rectTransform, float bottom)
        {
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, bottom);
        }

        public static void SetTop(this RectTransform rectTransform, float top)
        {
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -top);
        }

        public static float GetLeft(this RectTransform rectTransform) 
        {
            return rectTransform.offsetMin.x;
        }

        public static float GetRight(this RectTransform rectTransform) 
        {
            return -rectTransform.offsetMax.x;
        }

        public static float GetBottom(this RectTransform rectTransform) 
        {
            return rectTransform.offsetMin.y;
        }

        public static float GetTop(this RectTransform rectTransform) 
        {
            return -rectTransform.offsetMax.y;
        }

        public static void AddLeft(this RectTransform rectTransform, float amount) 
        {
            rectTransform.SetLeft(rectTransform.GetLeft() + amount);
        }

        public static void AddRight(this RectTransform rectTransform, float amount) 
        {
            rectTransform.SetRight(rectTransform.GetRight() + amount);
        }

        public static void AddBottom(this RectTransform rectTransform, float amount) 
        {
            rectTransform.SetBottom(rectTransform.GetBottom() + amount);
        }

        public static void AddTop(this RectTransform rectTransform, float amount) 
        {
            rectTransform.SetTop(rectTransform.GetTop() + amount);
        }

        public static void AnchorToCorners(this RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
        }

        public static void SetWidth(this RectTransform rectTransform, float width)
        {
            rectTransform.sizeDelta = new(width, rectTransform.sizeDelta.y);
        }

        public static void SetHeight(this RectTransform rectTransform, float height)
        {
            rectTransform.sizeDelta = new(rectTransform.sizeDelta.x, height);
        }

        public static Vector3 GetLeftPosition(this RectTransform rectTransform)
        {
            Vector3 rectTransformPosition = rectTransform.position;
            return new Vector3(rectTransformPosition.x - (rectTransform.rect.width * rectTransform.pivot.x), rectTransformPosition.y, rectTransformPosition.z);
        }

        public static Vector3 GetUpPosition(this RectTransform rectTransform)
        {
            Vector3 rectTransformPosition = rectTransform.position;
            return new Vector3(rectTransformPosition.x, rectTransformPosition.y + (rectTransform.rect.height * rectTransform.pivot.y), rectTransformPosition.z);
        }

        public static Vector3 GetRightPosition(this RectTransform rectTransform)
        {
            Vector3 rectTransformPosition = rectTransform.position;
            return new Vector3(rectTransformPosition.x + (rectTransform.rect.width * rectTransform.pivot.x), rectTransformPosition.y, rectTransformPosition.z);
        }

        public static Vector3 GetBottomPosition(this RectTransform rectTransform)
        {
            Vector3 rectTransformPosition = rectTransform.position;
            return new Vector3(rectTransformPosition.x, rectTransformPosition.y - (rectTransform.rect.height * rectTransform.pivot.y), rectTransformPosition.z);
        }

        public static void SetAnchoredPositionX(this RectTransform rectTransform, float x)
        {
            Vector2 anchoredPosition = rectTransform.anchoredPosition;
            anchoredPosition.x = x;
            rectTransform.anchoredPosition = anchoredPosition;
        }

        public static void SetAnchoredPositionY(this RectTransform rectTransform, float y)
        {
            Vector2 anchoredPosition = rectTransform.anchoredPosition;
            anchoredPosition.y = y;
            rectTransform.anchoredPosition = anchoredPosition;
        }
        #endregion

        #region float
        public static bool IsBetweenTwoValues(this float value, float valueA, float valueB, bool aInclusive = true, bool bInclusive = true)
        {
            bool isBetweenTwoValues = false;
            if (aInclusive && bInclusive)
            {
                if (value >= valueA && value <= valueB)
                {
                    isBetweenTwoValues = true;
                }
            }
            else
            {
                if (aInclusive)
                {
                    if (value >= valueA && value < valueB)
                    {
                        isBetweenTwoValues = true;
                    }
                }
                else if (bInclusive)
                {
                    if (value > valueA && value <= valueB)
                    {
                        isBetweenTwoValues = true;
                    }
                }
                else
                {
                    if (value > valueA && value < valueB)
                    {
                        isBetweenTwoValues = true;
                    }
                }
            }
            return isBetweenTwoValues;
        }

        public static bool IsAlmostEqualToFloat(this float floatA, float floatB, float floatsDifferenceThreshold = 0.001f)
        {
            bool isAlmostEqualToFloat = false;
            floatsDifferenceThreshold = Mathf.Abs(floatsDifferenceThreshold);
            float floatsDifference = Mathf.Abs(floatA - floatB);
            if (floatsDifference <= floatsDifferenceThreshold)
            {
                isAlmostEqualToFloat = true;
            }
            return isAlmostEqualToFloat;
        }

        public static float ToNormalizedScreenDistance(this float unnormalizedScreenDistance, Canvas canvas)
        {
            return unnormalizedScreenDistance * canvas.scaleFactor;
        }

        public static float GetValueTruncatedBetweenValues(this float value, float minValue = 0f, float maxValue = 1f)
        {
            float valueTruncatedBetweenValues = minValue;
            if (minValue < maxValue)
            {
                float range = maxValue - minValue + 1;
                float truncatedValue = (value - minValue) % range;
                if (truncatedValue < 0)
                {
                    truncatedValue += range;
                }
                valueTruncatedBetweenValues += truncatedValue;
            }
            return valueTruncatedBetweenValues;
        }

        public static int ToInt(this float floatValue)
        {
            return (int)floatValue;
        }

        public static bool IsCloserToAThanB(this float floatValue, float a, float b)
        {
            return Math.Abs(floatValue - a) < Math.Abs(floatValue - b);
        }

        public static int GetSign(this float floatValue)
        {
            return floatValue >= 0f ? 1 : -1;
        }

        public static float GetClampedBetweenValues(this float floatValue, float a, float b)
        {
            return Mathf.Clamp(floatValue, a, b);
        }
        #endregion

        #region BigInteger
        public static string ToStringWithLetter(this BigInteger bigInteger, int maxDigitsWithoutSuffixLetter = 4)
        {
            string stringValue;
            string bigIntegerString = bigInteger.ToString();
            int bigIntegerDigitsAmount = bigInteger.GetDigitsAmount();
            if (bigIntegerDigitsAmount > maxDigitsWithoutSuffixLetter)
            {
                // Calculate the group of thousands (k, m, b, etc.)
                int charsToRemoveAmount = bigIntegerDigitsAmount - 3;
                BigInteger significantPart = bigInteger / BigInteger.Pow(10, charsToRemoveAmount);
                int charsCompensation = Mathf.FloorToInt((bigIntegerDigitsAmount - 1) / Utils.DIGIT_GROUP_DEFAULT_LENGTH) * Utils.DIGIT_GROUP_DEFAULT_LENGTH - charsToRemoveAmount;

                // Convert to string and format with significant digits
                stringValue = ((decimal)((float)significantPart / Mathf.Pow(10, charsCompensation))).ToString();

                // Add suffix letter
                string suffixLetter = Utils.GetSuffixLetterForDigitsAmount(bigIntegerDigitsAmount, maxDigitsWithoutSuffixLetter);
                stringValue += suffixLetter;
            }
            else
            {
                //Set unchanged string
                stringValue = bigIntegerString;
            }
            return stringValue;
        }

        public static int GetDigitsAmount(this BigInteger bigInteger)
        {
            string bigIntegerString = bigInteger.ToString();
            int digitsAmount = bigIntegerString.Length;
            if (bigInteger < 0)
            {
                digitsAmount -= 1;
            }
            return digitsAmount;
        }

        public static BigInteger GetAbsoluteValue(this BigInteger bigInteger)
        {
            return BigInteger.Abs(bigInteger);
        }
        #endregion

        #region Char
        public static int ToInt(this char charValue)
        {
            return (int)char.GetNumericValue(charValue);
        }

        public static bool IsANumber(this char charValue)
        {
            return char.IsNumber(charValue);
        }
        #endregion

        #region String
        public static BigInteger ToBigInteger(this string stringValue)
        {
            BigInteger bigInteger = new(0);
            int currentExponent = 0;
            int lastCharIndex = stringValue.Length - 1;
            for (int i = lastCharIndex; i >= 0; i--)
            {
                char tempChar = stringValue[i];
                int currentDigitInt = tempChar.ToInt();
                BigInteger currentDigitBigInteger = new(currentDigitInt);
                BigInteger digitPowerMultiplier = BigInteger.Pow(10, currentExponent);
                BigInteger bigIntegerToAdd = currentDigitBigInteger * digitPowerMultiplier;
                bigInteger += bigIntegerToAdd;
                currentExponent++;
            }
            return bigInteger;
        }

        public static float ToFloat(this string stringValue)
        {
            return float.Parse(stringValue, CultureInfo.InvariantCulture.NumberFormat);
        }

        public static TEnum ToEnum<TEnum>(this string stringValue) where TEnum : struct
        {
            Enum.TryParse(stringValue, out TEnum tEnum);
            return tEnum;
        }

        public static bool IsNullOrEmpty(this string stringValue)
        {
            return string.IsNullOrEmpty(stringValue);
        }

        public static string Remove(this string stringValue, string stringToRemove) 
        {
            return stringValue.Replace(stringToRemove, "");
        }

        public static string GetPathFileName(this string path, bool includeExtension = true)
        {
            string fileName = path.Split('/').LastOrDefault();
            if (!includeExtension)
            {
                fileName = fileName.Split('.').FirstOrDefault();
            }
            return fileName;
        }

        public static int ToInt(this string stringValue)
        {
            return stringValue.ToFloat().ToInt();
        }

        public static string GetNumbersString(this string stringValue)
        {
            return stringValue.Where(iChar => iChar.IsANumber()).CombineToString();
        }
        #endregion

        #region IEnumerable<char>
        public static string CombineToString(this IEnumerable<char> charsIEnumerable)
        {
            string stringValue = "";
            charsIEnumerable.ToList().ForEach(iChar => stringValue += iChar);
            return stringValue;
        }
        #endregion

        #region RaycastHit
        public static bool HitFrontFace(this RaycastHit raycastHit, Ray ray)
        {
            bool raycastHitHitFrontFace = true;
            float raycastHitDot = Vector3.Dot(raycastHit.normal, -ray.direction);
            if (raycastHitDot < 0)
            {
                raycastHitHitFrontFace = false;
            }
            return raycastHitHitFrontFace;
        }
        #endregion

        #region Collider
        public static void DisableAndEnable(this Collider collider)
        {
            collider.enabled = false;
            collider.enabled = true;
        }
        #endregion

        #region ParticleSystem
        public static void SetStartColorToMeAndChildren(this ParticleSystem particleSystem, Color color)
        {
            List<ParticleSystem> meAndChildrenParticleSystemsList = particleSystem.gameObject.GetAllChildrenWithComponent<ParticleSystem>(includeMe: true);
            foreach (ParticleSystem iParticleSystem in meAndChildrenParticleSystemsList)
            {
                iParticleSystem.SetStartColor(color);
            }
        }

        public static void SetStartColor(this ParticleSystem particleSystem, Color color)
        {
            ParticleSystem.MainModule mainModule = particleSystem.main;
            mainModule.startColor = color;
        }
        #endregion

        #region Bounds
        public static Vector3 GetCurrentLeftMostPosition(this Bounds bounds)
        {
            return bounds.center + new Vector3(-bounds.extents.x, 0f, 0f);
        }

        public static Vector3 GetCurrentRightMostPosition(this Bounds bounds)
        {
            return bounds.center + new Vector3(bounds.extents.x, 0f, 0f);
        }

        public static Vector3 GetCurrentBottomMostPosition(this Bounds bounds)
        {
            return bounds.center + new Vector3(0f, -bounds.extents.y, 0f);
        }

        public static Vector3 GetCurrentTopMostPosition(this Bounds bounds)
        {
            return bounds.center + new Vector3(0f, bounds.extents.y, 0f);
        }

        public static Vector3 GetCurrentBackMostPosition(this Bounds bounds)
        {
            return bounds.center + new Vector3(0f, 0f, -bounds.extents.z);
        }

        public static Vector3 GetCurrentFrontMostPosition(this Bounds bounds)
        {
            return bounds.center + new Vector3(0f, 0f, bounds.extents.z);
        }

        public static Bounds GetCombinedWithBounds(this Bounds boundsA, Bounds boundsB)
        {
            Bounds combinedBounds = new(boundsA.center, boundsA.size);
            combinedBounds.Encapsulate(boundsB);
            return combinedBounds;
        }
        #endregion

        #region List<Renderer>
        public static Bounds GetCombinedBounds(this List<Renderer> renderersList)
        {
            Bounds combinedBounds = default;
            for (int i = 0; i < renderersList.Count; i++)
            {
                Renderer iRenderer = renderersList[i];
                Bounds iBounds = iRenderer.bounds;
                if (i == 0)
                {
                    combinedBounds = iBounds;
                }
                else
                {
                    combinedBounds.Encapsulate(iBounds);
                }
            }
            return combinedBounds;
        }
        #endregion

        #region List<Collider>
        public static Bounds GetCombinedBounds(this List<Collider> collidersList)
        {
            Bounds combinedBounds = default;
            for (int i = 0; i < collidersList.Count; i++)
            {
                Collider iRenderer = collidersList[i];
                Bounds iBounds = iRenderer.bounds;
                if (i == 0)
                {
                    combinedBounds = iBounds;
                }
                else
                {
                    combinedBounds.Encapsulate(iBounds);
                }
            }
            return combinedBounds;
        }
        #endregion

        #region CanvasGroup
        public static void ToggleInteractable(this CanvasGroup canvasGroup, bool on)
        {
            canvasGroup.interactable = on;
            canvasGroup.blocksRaycasts = on;
        }

        public static Tween FadeInAndEnableInteractable(this CanvasGroup canvasGroup, float seconds = 0.5f)
        {
            canvasGroup.blocksRaycasts = true;
            if (!canvasGroup.gameObject.activeSelf)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.gameObject.SetActive(true);
            }
            Tween fadeTween = canvasGroup.DOFade(1f, seconds).OnComplete(() =>
            {
                canvasGroup.ToggleInteractable(true);
            });
            return fadeTween;
        }

        public static Tween FadeOutAndDisableInteractable(this CanvasGroup canvasGroup, float seconds = 0.5f, bool fullFade = false)
        {
            canvasGroup.ToggleInteractable(false);
            float targetAlpha = 0.5f;
            if (fullFade)
            {
                targetAlpha = 0f;
            }
            Tween fadeTween = canvasGroup.DOFade(targetAlpha, seconds);
            return fadeTween;
        }

        public static Tween DoFullFadeTween(this CanvasGroup canvasGroup, bool on, float seconds = 0.5f)
        {
            float targetAlpha = on ? 1f : 0f;
            return canvasGroup.DOFade(targetAlpha, seconds);
        }

        public static Tween DoToggleFadeAndInteractableTween(this CanvasGroup canvasGroup, bool on, float seconds = 0.5f, bool fullFadeOut = true)
        {
            Tween tween;
            if (on)
            {
                tween = canvasGroup.FadeInAndEnableInteractable(seconds: seconds);
            }
            else
            {
                tween = canvasGroup.FadeOutAndDisableInteractable(seconds: seconds, fullFade: fullFadeOut);
            }
            return tween;
        }

        public static void ToggleVisible(this CanvasGroup canvasGroup, bool on)
        {
            canvasGroup.alpha = on ? 1f : 0f;
        }
        #endregion

        #region AudioSource
        public static Tween FadePitchToFixedValue(this AudioSource audioSource, float endFixedValue, float seconds = 1f)
        {
            return DOTween.To(() => audioSource.pitch, currentPitch => audioSource.pitch = currentPitch, endFixedValue, seconds);
        }

        public static Tween FadePitchToValuePercentage(this AudioSource audioSource, float endValuePercentageMultiplier, float seconds = 1f)
        {
            float endFixedValue = audioSource.pitch * endValuePercentageMultiplier;
            return audioSource.FadePitchToFixedValue(endFixedValue, seconds);
        }
        #endregion

        #region Material
        public static void SetAlpha(this Material material, float alpha)
        {
            Color color = material.color;
            color.a = alpha;
            material.color = color;
        }

        public static Tween DoFade(this Material material, Color color, string propertyName, float seconds) 
        {
            return DOTween.To(() => material.GetColor(propertyName), x => material.SetColor(propertyName, x), color, seconds);
        }
        #endregion

        #region MeshFilter
        public static Vector3 GetClosestVertexPositionToPositon(this MeshFilter meshFilter, Vector3 position)
        {
            Transform transform = meshFilter.transform;
            Vector3 localPosition = transform.InverseTransformPoint(position);
            float minDistanceSquare = Mathf.Infinity;
            Vector3 nearestVertex = Vector3.zero;
            Vector3[] verticesArray = meshFilter.mesh.vertices;
            foreach (Vector3 iVertex in verticesArray)
            {
                Vector3 positionsDifference = localPosition - iVertex;
                float distanceSquare = positionsDifference.sqrMagnitude;
                if (distanceSquare < minDistanceSquare)
                {
                    minDistanceSquare = distanceSquare;
                    nearestVertex = iVertex;
                }
            }
            Vector3 closestVertexPostionToPosition = transform.TransformPoint(nearestVertex);
            return closestVertexPostionToPosition;
        }

        public static float GetSharedMeshScaledWidth(this MeshFilter meshFilter)
        {
            return meshFilter.GetSharedMeshScaledSize().x;
        }

        public static float GetSharedMeshScaledHeight(this MeshFilter meshFilter)
        {
            return meshFilter.GetSharedMeshScaledSize().y;
        }

        public static float GetSharedMeshScaledDepth(this MeshFilter meshFilter)
        {
            return meshFilter.GetSharedMeshScaledSize().z;
        }

        public static Vector3 GetSharedMeshScaledSize(this MeshFilter meshFilter)
        {
            return Vector3.Scale(meshFilter.sharedMesh.bounds.size, meshFilter.transform.lossyScale);
        }

        public static Bounds GetWorldBounds(this MeshFilter meshFilter)
        {
            Bounds localBounds = meshFilter.sharedMesh.bounds;
            Vector3 localBoundsExtents = localBounds.extents;
            Vector3[] localCorners = new Vector3[8]
            {
                new(-localBoundsExtents.x, -localBoundsExtents.y, -localBoundsExtents.z),
                new(localBoundsExtents.x, -localBoundsExtents.y, -localBoundsExtents.z),
                new(-localBoundsExtents.x, localBoundsExtents.y, -localBoundsExtents.z),
                new(localBoundsExtents.x, localBoundsExtents.y, -localBoundsExtents.z),
                new(-localBoundsExtents.x, -localBoundsExtents.y, localBoundsExtents.z),
                new(localBoundsExtents.x, -localBoundsExtents.y, localBoundsExtents.z),
                new(-localBoundsExtents.x, localBoundsExtents.y, localBoundsExtents.z),
                new(localBoundsExtents.x, localBoundsExtents.y, localBoundsExtents.z)
            };
            Bounds worldBounds = new(meshFilter.transform.TransformPoint(localBounds.center), Vector3.zero);
            foreach (Vector3 iLocalCorner in localCorners)
            {
                worldBounds.Encapsulate(meshFilter.transform.TransformPoint(localBounds.center + iLocalCorner));
            }
            return worldBounds;
        }
        #endregion

        #region SkinnedMeshRenderer
        public static float GetSharedMeshScaledXWidth(this SkinnedMeshRenderer skinnedMeshRenderer)
        {
            return skinnedMeshRenderer.GetSharedMeshScaledSize().x;
        }

        public static float GetSharedMeshScaledHeight(this SkinnedMeshRenderer skinnedMeshRenderer)
        {
            return skinnedMeshRenderer.GetSharedMeshScaledSize().y;
        }

        public static float GetSharedMeshScaledZWidth(this SkinnedMeshRenderer skinnedMeshRenderer)
        {
            return skinnedMeshRenderer.GetSharedMeshScaledSize().z;
        }

        public static Vector3 GetSharedMeshScaledSize(this SkinnedMeshRenderer skinnedMeshRenderer)
        {
            return Vector3.Scale(skinnedMeshRenderer.sharedMesh.bounds.size, skinnedMeshRenderer.transform.lossyScale);
        }
        #endregion

        #region CinemachineVirtualCamera
        public static Tween DoFadeFieldOfViewTween(this CinemachineCamera cinemachineCamera, float targetFieldOfView, float seconds)
        {
            return DOTween.To(() => cinemachineCamera.Lens.FieldOfView, x => cinemachineCamera.Lens.FieldOfView = x, targetFieldOfView, seconds);
        }
        #endregion

        #region ICollection
        public static bool IsEmpty(this ICollection collection)
        {
            return collection.Count <= 0;
        }

        public static bool ValueAtIndexExists<T>(this ICollection<T> collection, int index)
        {
            return (index >= 0) && (index < collection.Count);
        }

        public static T GetValueAtIndexOrDefault<T>(this ICollection<T> collection, int index)
        {
            return collection.ValueAtIndexExists(index) ? collection.ElementAt(index) : default;
        }
        #endregion

        #region ConfigurabeJoint
        public static void SetXDrivePositionSpring(this ConfigurableJoint configurableJoint, float positionSpring)
        {
            JointDrive xDrive = configurableJoint.xDrive;
            xDrive.positionSpring = positionSpring;
            configurableJoint.xDrive = xDrive;
        }

        public static void SetXDriveMaximumForce(this ConfigurableJoint configurableJoint, float maximumForce)
        {
            JointDrive xDrive = configurableJoint.xDrive;
            xDrive.maximumForce = maximumForce;
            configurableJoint.xDrive = xDrive;
        }

        public static void SetYDrivePositionSpring(this ConfigurableJoint configurableJoint, float positionSpring)
        {
            JointDrive yDrive = configurableJoint.yDrive;
            yDrive.positionSpring = positionSpring;
            configurableJoint.yDrive = yDrive;
        }

        public static void SetYDriveMaximumForce(this ConfigurableJoint configurableJoint, float maximumForce)
        {
            JointDrive yDrive = configurableJoint.yDrive;
            yDrive.maximumForce = maximumForce;
            configurableJoint.yDrive = yDrive;
        }

        public static void SetZDrivePositionSpring(this ConfigurableJoint configurableJoint, float positionSpring)
        {
            JointDrive zDrive = configurableJoint.zDrive;
            zDrive.positionSpring = positionSpring;
            configurableJoint.zDrive = zDrive;
        }

        public static void SetZDriveMaximumForce(this ConfigurableJoint configurableJoint, float maximumForce)
        {
            JointDrive zDrive = configurableJoint.zDrive;
            zDrive.maximumForce = maximumForce;
            configurableJoint.zDrive = zDrive;
        }
        #endregion

        #region object
        public class ObjectData
        {
            public const BindingFlags BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default;

            public Dictionary<string, object> propertiesDictionary { get; private set; }
            public Dictionary<string, object> fieldInfosDictionary { get; private set; }

            public ObjectData(Dictionary<string, object> propertiesDictionary, Dictionary<string, object> fieldInfosDictionary)
            {
                this.propertiesDictionary = propertiesDictionary;
                this.fieldInfosDictionary = fieldInfosDictionary;
            }
        }

        public static Dictionary<string, object> GetPropertiesDictionary(this object myObject, BindingFlags bindingFlags = ObjectData.BINDING_FLAGS)
        {
            Dictionary<string, object> propertiesDictionary = new();
            PropertyInfo[] propertyInfosArray = myObject.GetType().GetProperties(bindingFlags);
            foreach (PropertyInfo iPropertyInfo in propertyInfosArray)
            {
                if (iPropertyInfo.CanRead && iPropertyInfo.CanWrite)
                {
                    propertiesDictionary[iPropertyInfo.Name] = iPropertyInfo.GetValue(myObject);
                }
            }
            return propertiesDictionary;
        }

        public static Dictionary<string, object> GetFieldInfosDictionary(this object myObject, BindingFlags bindingFlags = ObjectData.BINDING_FLAGS)
        {
            Dictionary<string, object> fieldsDictionary = new();
            FieldInfo[] fieldInfosArray = myObject.GetType().GetFields(bindingFlags);
            foreach (FieldInfo field in fieldInfosArray)
            {
                fieldsDictionary[field.Name] = field.GetValue(myObject);
            }
            return fieldsDictionary;
        }

        public static void SetPropertiesDictionary(this object myObject, Dictionary<string, object> propertiesDictionary, BindingFlags bindingFlags = ObjectData.BINDING_FLAGS)
        {
            foreach (KeyValuePair<string, object> iProperty in propertiesDictionary)
            {
                PropertyInfo propertyInfo = myObject.GetType().GetProperty(iProperty.Key, bindingFlags);
                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(myObject, iProperty.Value);
                }
            }
        }

        public static void SetFieldInfosDictionary(this object myObject, Dictionary<string, object> fieldInfosDictionary, BindingFlags bindingFlags = ObjectData.BINDING_FLAGS)
        {
            foreach (KeyValuePair<string, object> iFieldInfo in fieldInfosDictionary)
            {
                myObject.GetType().GetField(iFieldInfo.Key, bindingFlags)?.SetValue(myObject, iFieldInfo.Value);
            }
        }

        public static ObjectData GetObjectData(this object myObject)
        {
            return new(myObject.GetPropertiesDictionary(), myObject.GetFieldInfosDictionary());
        }

        public static void SetObjectData(this object myObject, ObjectData objectData)
        {
            myObject.SetPropertiesDictionary(objectData.propertiesDictionary);
            myObject.SetFieldInfosDictionary(objectData.fieldInfosDictionary);
        }
        #endregion

        #region Rigidbody
        public static void Stop(this Rigidbody rigidbody)
        {
            if (!rigidbody.isKinematic)
            {
                rigidbody.linearVelocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }
        }
        #endregion

        #region ButtonClickedEvent
        public static bool IsListeningToAction(this ButtonClickedEvent buttonClickedEvent, Action action)
        {
            bool isListeningToAction = false;
            if (action != null)
            {
                int persistentEventCount = buttonClickedEvent.GetPersistentEventCount();
                for (int i = 0; i < persistentEventCount; i++)
                {
                    if (buttonClickedEvent.GetPersistentTarget(i) == (Object)action.Target
                        && buttonClickedEvent.GetPersistentMethodName(i) == action.Method.Name)
                    {
                        isListeningToAction = true;
                    }
                }
                if (!isListeningToAction)
                {
                    FieldInfo fieldInfo = typeof(UnityEventBase).GetField("m_Calls", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (fieldInfo != null)
                    {
                        object calls = fieldInfo.GetValue(buttonClickedEvent);
                        if (calls != null)
                        {
                            FieldInfo callsField = calls.GetType().GetField("m_RuntimeCalls", BindingFlags.NonPublic | BindingFlags.Instance);
                            if (callsField != null)
                            {
                                if (callsField.GetValue(calls) is IList runtimeCalls)
                                {
                                    isListeningToAction = runtimeCalls.Cast<object>().Any(call => call.GetType()
                                        .GetField("Delegate", BindingFlags.NonPublic | BindingFlags.Instance)?
                                        .GetValue(call) is Delegate del && del.GetInvocationList().Contains(action));
                                }
                            }
                        }
                    }
                }
            }
            return isListeningToAction;
        }
        #endregion

        #region SplineContainer
        public static float GetTAtWorldPosition(this SplineContainer splineContainer, Vector3 worldPosition, int splineIndex = 0)
        {
            SplineUtility.GetNearestPoint(splineContainer.Splines[splineIndex], splineContainer.transform.InverseTransformPoint(worldPosition), out _, out float t, resolution: 32, iterations: 8);
            return t;
        }

        public static float GetDistanceAtWorldPosition(this SplineContainer splineContainer, Vector3 worldPosition, int splineIndex = 0)
        {
            return splineContainer.Splines[splineIndex].GetDistanceWithT(splineContainer.GetTAtWorldPosition(worldPosition, splineIndex));
        }

        public static Vector3 GetWorldPositionAtDistance(this SplineContainer splineContainer, float distance, int splineIndex = 0)
        {
            Spline spline = splineContainer.Splines[splineIndex];
            float t = SplineUtility.GetNormalizedInterpolation(spline, distance, PathIndexUnit.Distance);
            return splineContainer.transform.GetWorldPositionWithLocalPosition(spline.EvaluatePosition(t));
        }

        public static Vector3 GetWorldForwardDirectionAtDistance(this SplineContainer splineContainer, float distance, int splineIndex = 0)
        {
            Spline spline = splineContainer.Splines[splineIndex];
            Vector3 tangent = spline.EvaluateTangent(spline.GetTWithDistance(distance));
            return splineContainer.transform.GetWorldDirectionWithLocalDirection(tangent.normalized);
        }

        public static Vector3 GetWorldUpDirectionAtDistance(this SplineContainer splineContainer, float distance, int splineIndex = 0)
        {
            Spline spline = splineContainer.Splines[splineIndex];
            Vector3 upVector = spline.EvaluateUpVector(spline.GetTWithDistance(distance));
            return splineContainer.transform.GetWorldDirectionWithLocalDirection(upVector.normalized);
        }

        public static Quaternion GetWorldRotationAtDistance(this SplineContainer splineContainer, float distance, int splineIndex = 0)
        {
            return Quaternion.LookRotation(splineContainer.GetWorldForwardDirectionAtDistance(distance, splineIndex), splineContainer.GetWorldUpDirectionAtDistance(distance, splineIndex));
        }

        public static Vector3 GetWorldPositionAtT(this SplineContainer splineContainer, float t, int splineIndex = 0)
        {
            return splineContainer.transform.GetWorldPositionWithLocalPosition(splineContainer.Splines[splineIndex].EvaluatePosition(t));
        }

        public static Vector3 GetWorldUpDirectionAtT(this SplineContainer splineContainer, float t, int splineIndex = 0)
        {
            return splineContainer.transform.GetWorldDirectionWithLocalDirection(splineContainer.Splines[splineIndex].EvaluateUpVector(t)).normalized;
        }

        public static Vector3 GetBezierKnotWorldPosition(this SplineContainer splineContainer, BezierKnot bezierKnot)
        {
            return splineContainer.transform.GetWorldPositionWithLocalPosition(bezierKnot.Position);
        }
        #endregion

        #region Spline
        public static BezierKnot GetLastReachedBezierKnotAtT(this Spline spline, float t)
        {
            List<BezierKnot> bezierKnotsList = spline.Knots.ToList();
            BezierKnot lastReachedBezierKnotAtPosition = bezierKnotsList.FirstOrDefault();
            foreach (BezierKnot iBezierKnot in bezierKnotsList)
            {
                if (spline.GetTWithBezierKnot(iBezierKnot) <= t)
                {
                    lastReachedBezierKnotAtPosition = iBezierKnot;
                }
                else
                {
                    break;
                }
            }
            return lastReachedBezierKnotAtPosition;
        }

        public static BezierKnot GetBezierKnotAfterBezierKnot(this Spline spline, BezierKnot bezierKnot) 
        {
            return spline.Next(spline.GetBezierKnotIndex(bezierKnot));
        }

        public static int GetBezierKnotIndex(this Spline spline, BezierKnot bezierKnot)
        {
            return spline.Knots.ToList().IndexOf(bezierKnot);
        }

        public static SplineData<Object> GetSplineData(this Spline spline, string key) 
        {
            spline.TryGetObjectData(key, out SplineData<Object> splineData);
            return splineData;
        }

        public static float GetDistanceBetweenTs(this Spline spline, float tA, float tB)
        {
            float distanceBetweenTs = 0f;
            if (!Mathf.Approximately(tA, tB))
            {
                const int SAMPLES_PER_CURVE = 24;
                int knotsAmount = spline.Count;
                int curvesAmount = Mathf.Max(1, spline.Closed ? knotsAmount : knotsAmount - 1);
                int stepsAmount = Mathf.Max(2, Mathf.CeilToInt(Mathf.Abs(tB - tA) * curvesAmount * SAMPLES_PER_CURVE));
                Vector3 previousPosition = spline.EvaluatePosition(tA);
                for (int i = 1; i <= stepsAmount; i++)
                {
                    float iNormalizedTime = Mathf.Lerp(tA, tB, (float)i / stepsAmount);
                    Vector3 iPosition = spline.EvaluatePosition(iNormalizedTime);
                    distanceBetweenTs += Vector3.Distance(previousPosition, iPosition);
                    previousPosition = iPosition;
                }
            }
            return distanceBetweenTs;
        }

        public static float GetDistanceWithT(this Spline spline, float t)
        {
            return SplineUtility.ConvertIndexUnit(spline, t, PathIndexUnit.Normalized, PathIndexUnit.Distance);
        }

        public static float GetTWithDistance(this Spline spline, float distance)
        {
            return SplineUtility.ConvertIndexUnit(spline, distance, PathIndexUnit.Distance, PathIndexUnit.Normalized);
        }

        public static float GetTWithBezierKnot(this Spline spline, int bezierKnotIndex)
        {
            return SplineUtility.ConvertIndexUnit(spline, bezierKnotIndex, PathIndexUnit.Knot, PathIndexUnit.Normalized);
        }

        public static float GetTWithBezierKnot(this Spline spline, BezierKnot bezierKnot)
        {
            return spline.GetTWithBezierKnot(spline.GetBezierKnotIndex(bezierKnot));
        }

        public static float GetDistanceWithBezierKnot(this Spline spline, int bezierKnotIndex)
        {
            return SplineUtility.ConvertIndexUnit(spline, bezierKnotIndex, PathIndexUnit.Knot, PathIndexUnit.Distance);
        }

        public static float GetDistanceWithBezierKnot(this Spline spline, BezierKnot bezierKnot)
        {
            return spline.GetDistanceWithBezierKnot(spline.GetBezierKnotIndex(bezierKnot));
        }

        public static BezierKnot GetClosestBezierKnotAtT(this Spline spline, float t)
        {
            return spline.Knots.OrderBy(iBezierKnot => Mathf.Abs(t - spline.GetTWithBezierKnot(iBezierKnot))).FirstOrDefault();
        }
        #endregion

        #region DataPoint<Object>
        public static DataPoint<Object> GetSplineDataDataPoint(this SplineData<Object> splineData, int index)
        {
            return splineData[index];
        }
        #endregion

        #region BezierKnot
        public static bool IsLastBezierKnotInSpline(this BezierKnot bezierKnot, Spline spline) 
        {
            return spline.Knots.LastOrDefault().Equals(bezierKnot);
        }
        #endregion

        #region CinemachineBrain
        public static void SetDefaultBlendSeconds(this CinemachineBrain cinemachineBrain, float seconds)
        {
            cinemachineBrain.DefaultBlend = new(cinemachineBrain.DefaultBlend.Style, seconds);
        }
        #endregion

        #region Random
        public static class RandomExtensions
        {
            public static float Range01()
            {
                return Random.Range(0f, 1f);
            }
        }
        #endregion

        #region Renderer
        public static void SetSharedMaterial(this Renderer renderer, Material sharedMaterial, int index = 0)
        {
            Material[] sharedMaterialsArray = renderer.sharedMaterials;
            sharedMaterialsArray[index] = sharedMaterial;
            renderer.sharedMaterials = sharedMaterialsArray;
        }

        public static void RemoveMaterialAtIndex(this Renderer renderer, int index)
        {
            renderer.materials = renderer.materials.RemoveAtIndex(index);
        }

        public static void RemoveMaterial(this Renderer renderer, Material material)
        {
            renderer.materials = renderer.materials.Remove(material);
        }
        #endregion

        #region AssetDatabase
        public static class AssetDatabaseExtensions
        {
            #if UNITY_EDITOR
            public static List<T> LoadAssetsOfTypeAtPathAndItsSubpathsList<T>(string path) where T : Object
            {
                return AssetDatabase.FindAssets("", new[] { path }).Select(iGUID => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(iGUID))).ToList();
            }

            public static T LoadFirstAssetOfTypeAtPathAndItsSubpaths<T>(string path) where T : Object
            {
                return LoadAssetsOfTypeAtPathAndItsSubpathsList<T>(path).FirstOrDefault();
            }
            #endif
        }
        #endregion

        #region HorizontalDirection
        public static Vector3 GetVector3(this HorizontalDirection horizontalDirection)
        {
            Vector3 vector3 = Vector3.right;
            switch (horizontalDirection)
            {
                case HorizontalDirection.Forward:
                    vector3 = Vector3.forward;
                    break;
                case HorizontalDirection.Left:
                    vector3 = Vector3.left;
                    break;
                case HorizontalDirection.Back:
                    vector3 = Vector3.back;
                    break;
            }
            return vector3;
        }

        public static bool IsXDirection(this HorizontalDirection horizontalDirection)
        {
            return horizontalDirection == HorizontalDirection.Right || horizontalDirection == HorizontalDirection.Left;
        }

        public static bool IsZDirection(this HorizontalDirection horizontalDirection)
        {
            return horizontalDirection == HorizontalDirection.Forward || horizontalDirection == HorizontalDirection.Back;
        }

        public static HorizontalDirection GetOppositeDirection(this HorizontalDirection horizontalDirection)
        {
            HorizontalDirection oppositeHorizontalDirection = HorizontalDirection.Right;
            switch (horizontalDirection)
            {
                case HorizontalDirection.Right:
                    oppositeHorizontalDirection = HorizontalDirection.Left;
                    break;
                case HorizontalDirection.Forward:
                    oppositeHorizontalDirection = HorizontalDirection.Back;
                    break;
                case HorizontalDirection.Back:
                    oppositeHorizontalDirection = HorizontalDirection.Forward;
                    break;
            }
            return oppositeHorizontalDirection;
        }
        #endregion
        
        #region Color
        public static Color WithAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
        #endregion

        #region List<Bounds>
        public static Bounds GetCombined(this List<Bounds> boundsList)
        {
            Bounds combinedBounds = default;
            for (int i = 0; i < boundsList.Count; i++)
            {
                Bounds iBounds = boundsList[i];
                if (i == 0)
                {
                    combinedBounds = iBounds;
                }
                else
                {
                    combinedBounds = combinedBounds.GetCombinedWithBounds(iBounds);
                }
            }
            return combinedBounds;
        }

        public static Mesh ToMesh(this List<Bounds> boundsList)
        {
            List<CombineInstance> combineInstancesList = new();
            foreach (Bounds iBounds in boundsList)
            {
                Mesh cube = Utils.CreateCubeMesh();
                Vector3 scale = iBounds.size;
                Matrix4x4 matrix = Matrix4x4.TRS(iBounds.center, Quaternion.identity, scale);
                combineInstancesList.Add(new CombineInstance { mesh = cube, transform = matrix });
            }
            Mesh finalMesh = new() { indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 };
            finalMesh.CombineMeshes(combineInstancesList.ToArray(), true, true);
            finalMesh.RecalculateNormals();
            finalMesh.RecalculateBounds();
            return finalMesh;
        }
        #endregion

        #region BoxCollider
        public static void SetBounds(this BoxCollider boxCollider, Bounds bounds)
        {
            Transform transform = boxCollider.transform;
            boxCollider.center = transform.InverseTransformPoint(bounds.center);
            boxCollider.size = transform.InverseTransformVector(bounds.size).Abs();
        }
        #endregion

        #region Camera
        public static List<RaycastHit> GetRaycastHitsListAtScreenPosition(this Camera camera, Vector3 screenPosition)
        {
            return Physics.RaycastAll(camera.ScreenPointToRay(screenPosition), maxDistance: Mathf.Infinity).Reverse().ToList();
        }
        #endregion

        #region bool
        public static int ToSign(this bool on)
        {
            return on ? 1 : -1;
        }
        #endregion

        #region Object
        public static void Destroy(this Object myObject)
        {
            myObject.name += $" {BEING_DESTROYED_STRING}";
            if (Utils.IsInEditorAndNotPlaying())
            {
                Object.DestroyImmediate(myObject);
            }
            else
            {
                Object.Destroy(myObject);
            }
        }

        public static bool IsNullOrBeingDestroyed(this Object myObject)
        {
            return myObject == null || myObject.IsBeingDestroyed();
        }

        public static bool IsBeingDestroyed(this Object myObject)
        {
            return myObject.name.Contains(BEING_DESTROYED_STRING);
        }

        public static T GetTagHolderReferencedComponent<T>(this Object myObject) where T : Component
        {
            return myObject.GetComponent<TagHolder>().GetReferencedComponent<T>();
        }
        #endregion

        #region LayoutGroup
        public static void Update(this LayoutGroup layoutGroup)
        {
            layoutGroup.DisableWaitAFrameAndEnable();
        }
        #endregion
    }
}