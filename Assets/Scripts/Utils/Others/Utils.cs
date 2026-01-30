using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Canvas = UnityEngine.Canvas;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace GGJ_2026
{
    public static class Utils
    {
        public enum Dimension
        {
            X,
            Y,
            Z
        }

        public enum HorizontalDirection
        {
            Right,
            Forward,
            Left,
            Back
        }

        private const float DEFAULT_RAY_MAX_DISTANCE = 100f;

        //Shader property names
        public const string EMISSION_COLOR_SHADER_PROPERTY_NAME = "_EmissionColor";
        public const string EMISSION_SHADER_PROPERTY_NAME = "_EMISSION";
        //Others
        public const string PROJECT_NAME = nameof(GGJ_2026);
        public const string NEW_LINE_CHARACTER = "\n";
        public const int DIGIT_GROUP_DEFAULT_LENGTH = 3;

        public static Vector3 GetVectorBetween2Points(Vector3 originPoint, Vector3 targetPoint)
        {
            return new(targetPoint.x - originPoint.x, targetPoint.y - originPoint.y, targetPoint.z - originPoint.z);
        }

        public static T InstantiateUIElement<T>(T prefab, Transform parent, Vector2 localPosition) where T : Component
        {
            return InstantiateUIElement(prefab.gameObject, parent, localPosition).GetComponent<T>();
        }

        public static GameObject InstantiateUIElement(GameObject prefab, Transform parent, Vector2 localPosition)
        {
            GameObject uiElement = Object.Instantiate(prefab, parent, false);
            uiElement.transform.localPosition = localPosition;
            return uiElement;
        }

        public static bool GameObjectIsPrefab(GameObject gameObject)
        {
            return gameObject.scene.name == null;
        }

        public static string AddZeroBeforeOneDigitNumber(string number)
        {
            string modifiedNumber = number;
            if (modifiedNumber.Length == 1)
            {
                modifiedNumber = "0" + modifiedNumber;
            }
            return modifiedNumber;
        }

        public static float GetAngleBetweenTwoPoints(Vector3 pointA, Vector3 pointB)
        {
            return Mathf.Atan2(pointB.x - pointA.x, pointB.y - pointA.y) * Mathf.Rad2Deg;
        }

        public static bool IsInEditorAndNotPlaying()
        {
            return Application.isEditor && !Application.isPlaying;
        }

        public static bool ProbabilityPasses(float probability)
        {
            return Random.Range(0f, 1f) <= probability;
        }

        public static bool CoinTossPasses()
        {
            return ProbabilityPasses(0.5f);
        }

        public static Vector3 GetRandomDirection()
        {
            return Random.insideUnitSphere.normalized;
        }

        public static Vector3 GetRandomHorizontalDirection()
        {
            Vector3 randomDirection = GetRandomDirection();
            Vector3 randomHorizontalDirection = randomDirection;
            randomHorizontalDirection.y = 0f;
            randomHorizontalDirection.Normalize();
            return randomHorizontalDirection;
        }

        public static Vector3 GetRandomVerticalDirection()
        {
            Vector3 randomDirection = GetRandomDirection();
            Vector3 randomVerticalDirection = randomDirection;
            randomVerticalDirection.z = 0f;
            randomVerticalDirection.Normalize();
            return randomVerticalDirection;
        }

        public static float GetPercentageBetweenTwoValues(float valueA, float valueB, float valueBetweenValues, bool clampPercentageBetweenZeroAndOne = true)
        {
            float percentageBetweenTwoValues = Mathf.InverseLerp(valueA, valueB, valueBetweenValues);
            if (clampPercentageBetweenZeroAndOne)
            {
                percentageBetweenTwoValues = Mathf.Clamp(percentageBetweenTwoValues, 0, 1);
            }
            return percentageBetweenTwoValues;
        }

        public static string GetSuffixLetterForDigitsAmount(int digitsAmount, int maxDigitsWithoutSuffixLetter = 0)
        {
            string suffixLetter = "";
            if (digitsAmount > maxDigitsWithoutSuffixLetter)
            {
                int currentGroup = Mathf.CeilToInt(digitsAmount / (float)DIGIT_GROUP_DEFAULT_LENGTH);
                switch (currentGroup)
                {
                    case 0:
                    case 1:
                        suffixLetter = "";
                        break;
                    case 2:
                        suffixLetter = "K";
                        break;
                    case 3:
                        suffixLetter = "M";
                        break;
                    case 4:
                        suffixLetter = "B";
                        break;
                    case 5:
                        suffixLetter = "T";
                        break;
                    case 6:
                        suffixLetter = "Q";
                        break;
                    default:
                        suffixLetter = "Z";
                        break;
                }
            }
            return suffixLetter;
        }

        public static Vector2 GetRandomPositionOnCircleBorder(float radius, float offset)
        {
            Vector2 randomDirectionOnCircleBorder = Random.insideUnitCircle.normalized;
            float randomOffset = Random.Range(-offset, offset);
            Vector2 randomOffsetVector = randomDirectionOnCircleBorder * randomOffset;
            Vector2 randomPositionOnCircleBorder = (randomDirectionOnCircleBorder * radius) + randomOffsetVector;
            return randomPositionOnCircleBorder;
        }

        public static Vector2 GetRandomPositionInCircle(float radius)
        {
            Vector2 randomPositionInCircle = Random.insideUnitCircle * radius;
            return randomPositionInCircle;
        }

        public static bool PositionIsInsideMeshCollider(Vector3 position, MeshCollider meshCollider)
        {
            bool positionIsInsideCollider = false;
            bool originalQueriesHitBackfaces = Physics.queriesHitBackfaces;
            Physics.queriesHitBackfaces = true;
            Vector3 raycastDirection = Vector3.forward;
            Ray ray = new(position, raycastDirection);
            const float RAYCAST_MAX_DISTANCE = Mathf.Infinity;
            List<RaycastHit> raycastHitsList = new(Physics.RaycastAll(ray, RAYCAST_MAX_DISTANCE));
            List<RaycastHit> meshColliderRaycastHitsList = raycastHitsList.Where(iRaycastHit => iRaycastHit.triangleIndex != -1 && iRaycastHit.collider == meshCollider).ToList();
            if (meshColliderRaycastHitsList.Count > 0)
            {
                RaycastHit firstMeshColliderRaycastHit = meshColliderRaycastHitsList[0];
                if (!firstMeshColliderRaycastHit.HitFrontFace(ray) && !PositionIsInBoundsBorder(position, meshCollider.bounds))
                {
                    positionIsInsideCollider = true;
                }
            }
            Physics.queriesHitBackfaces = originalQueriesHitBackfaces;
            return positionIsInsideCollider;
        }

        public static bool PositionIsInBoundsBorder(Vector3 position, Bounds bounds)
        {
            bool positionIsInBoundsBorder = false;
            Vector3 boundsMin = bounds.min;
            Vector3 boundsMax = bounds.max;
            float positionX = position.x;
            float positionY = position.y;
            float positionZ = position.z;
            if (positionX.IsAlmostEqualToFloat(boundsMin.x) || positionX.IsAlmostEqualToFloat(boundsMax.x) ||
                positionY.IsAlmostEqualToFloat(boundsMin.y) || positionY.IsAlmostEqualToFloat(boundsMax.y) ||
                positionZ.IsAlmostEqualToFloat(boundsMin.z) || positionZ.IsAlmostEqualToFloat(boundsMax.z))
            {
                positionIsInBoundsBorder = true;
            }
            return positionIsInBoundsBorder;
        }

        public static void PauseEditor()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPaused = true;
#endif
        }

        public static void EditorUtilitySetDirty(Object target)
        {
#if UNITY_EDITOR
            if (IsInEditorAndNotPlaying())
            {
                EditorUtility.SetDirty(target);
            }
#endif
        }

        public static void CopySerializedFieldsFromComponentToComponent(Component componentA, Component componentB)
        {
#if UNITY_EDITOR
            SerializedObject componentASerializedObject = new(componentA);
            SerializedObject componentBSerializedObject = new(componentB);
            SerializedProperty serializedPropertyIterator = componentASerializedObject.GetIterator();
            //Jump into serialized object. This will skip script type so that we dont override the destination component's type
            if (serializedPropertyIterator.NextVisible(true))
            {
                //Iterate through all serialized properties
                while (serializedPropertyIterator.NextVisible(true))
                {
                    //Try obtaining the property in the destination component
                    SerializedProperty iSerializedProperty = componentBSerializedObject.FindProperty(serializedPropertyIterator.name);
                    //Validate that the properties are present in both components, and that they're the same type
                    if (iSerializedProperty != null && iSerializedProperty.propertyType == serializedPropertyIterator.propertyType)
                    {
                        //Copy value from source to destination component
                        componentBSerializedObject.CopyFromSerializedProperty(serializedPropertyIterator);
                    }
                }
            }
            componentBSerializedObject.ApplyModifiedProperties();
#endif
        }

        public static RaycastHit GetClosestRaycastHitWithTag(Ray ray, Tag tag, float maxDistance = DEFAULT_RAY_MAX_DISTANCE)
        {
            return GetRaycastHitsWithTagList(ray, tag, maxDistance)
            .OrderBy(iRaycastHit => Vector3.Distance(iRaycastHit.point, ray.origin)).FirstOrDefault();
        }

        public static List<RaycastHit> GetRaycastHitsWithTagList(Ray ray, Tag tag, float maxDistance = DEFAULT_RAY_MAX_DISTANCE)
        {
            return Physics.RaycastAll(ray, maxDistance).Where(iRaycastHit => iRaycastHit.collider.gameObject.HasTag(tag)).ToList();
        }

        public static Vector3 EulerAnglesToDirection(Vector3 eulerAngles)
        {
            Quaternion rotation = Quaternion.Euler(eulerAngles);
            Vector3 direction = rotation * Vector3.forward;
            return direction;
        }

        public static float GetNegativeAngleToPositive(float angle)
        {
            return (angle % 360f + 360f) % 360f;
        }

        public static float GetPositiveAngleToNegative(float angle)
        {
            return (angle % 360f) - 360f;
        }

        public static Vector3 ScreenSpaceCameraPositionToWorldPosition(Vector3 screenSpaceCameraPosition, Camera screenSpaceCamera, Canvas screenSpaceCanvas)
        {
            Vector3 worldToScreenPoint = screenSpaceCamera.WorldToScreenPoint(screenSpaceCameraPosition);
            worldToScreenPoint.z = (screenSpaceCanvas.transform.position - screenSpaceCamera.transform.position).magnitude;
            return Camera.main.ScreenToWorldPoint(worldToScreenPoint);
        }

        public static Vector2 WorldPositionToScreenSpaceCameraPosition(Vector3 worldPosition, Camera worldCamera, Camera screenSpaceCamera, Canvas screenSpaceCanvas)
        {
            Vector3 screenPosition = worldCamera.WorldToScreenPoint(worldPosition);
            screenPosition.z = (screenSpaceCanvas.transform.position - screenSpaceCamera.transform.position).magnitude;
            return screenSpaceCamera.ScreenToWorldPoint(screenPosition);
        }

        public static bool GameObjectsHaveSamePrefabName(GameObject gameObjectA, GameObject gameObjectB)
        {
            const string CLONE_STRING = "(Clone)";
            const string UNITY_ENGINE_GAME_OBJECT_STRING = " (UnityEngine.GameObject)";
            string prefabNameA = gameObjectA.name.Replace(CLONE_STRING, "").Replace(UNITY_ENGINE_GAME_OBJECT_STRING, "");
            string prefabNameB = gameObjectB.name.Replace(CLONE_STRING, "").Replace(UNITY_ENGINE_GAME_OBJECT_STRING, "");
            return prefabNameA.Equals(prefabNameB);
        }

        public static bool AngleIsBetweenTwoAngles(float angle, float angleA, float angleB)
        {
            bool angleIsBetweenTwoAngles;
            angle = NormalizeAngleBetween0And360(angle);
            angleA = NormalizeAngleBetween0And360(angleA);
            angleB = NormalizeAngleBetween0And360(angleB);
            if (angleA < angleB)
            {
                angleIsBetweenTwoAngles = angleA <= angle && angle <= angleB;
            }
            else
            {
                angleIsBetweenTwoAngles = angle >= angleA || angle <= angleB;
            }
            return angleIsBetweenTwoAngles;
        }

        public static float NormalizeAngleBetween0And360(float angle)
        {
            angle %= 360f;
            if (angle < 0f)
            {
                angle += 360f;
            }
            return angle;
        }

        public static string ScenePathToSceneName(string levelScenePath)
        {
            return levelScenePath.Split('/').LastOrDefault().Split(".unity").FirstOrDefault();
        }

        public static List<T> InstantiateTList<T>(T tObject, int amount, Transform parent) where T : Object
        {
            List<T> tList = new();
            for (int i = 0; i < amount; i++)
            {
                tList.Add(Object.Instantiate(tObject, parent));
            }
            return tList;
        }

        public static float MetersPerSecondToKilometersPerHour(float metersPerSecond)
        {
            return metersPerSecond / 1000f * 3600f;
        }

        public static float KilometersPerHourToMetersPerSecond(float kilometersPerHour)
        {
            return kilometersPerHour * 1000f / 3600f;
        }

        public static bool IsConnectedToTheInternet()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        public static Vector3 GetScreenPointToWorldPointAtDimensionPosition(Vector2 screenPoint, Dimension dimension, float dimensionPosition, Camera camera = null)
        {
            camera = camera == null ? Camera.main : camera;
            Ray ray = camera.ScreenPointToRay(screenPoint);
            float cameraDimensionPosition = camera.transform.position.x;
            float rayDirectionDimensionPosition = ray.direction.x;
            switch (dimension)
            {
                case Dimension.Y:
                    cameraDimensionPosition = camera.transform.position.y;
                    rayDirectionDimensionPosition = ray.direction.y;
                    break;
                case Dimension.Z:
                    cameraDimensionPosition = camera.transform.position.z;
                    rayDirectionDimensionPosition = ray.direction.z;
                    break;
            }
            float rayDistance = (dimensionPosition - cameraDimensionPosition) / rayDirectionDimensionPosition;
            Vector3 worldPoint = camera.transform.position + ray.direction * rayDistance;
            return worldPoint;
        }

        public static List<T> GetEnumValuesList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static bool DirectionsAreParallel(Vector3 directionA, Vector3 directionB, float anglesThreshold = 0.1f)
        {
            float anglesBetweenDirections = GetAnglesBetweenDirections(directionA, directionB);
            return anglesBetweenDirections <= anglesThreshold || Mathf.Abs(anglesBetweenDirections - 180f) <= anglesThreshold;
        }

        public static float GetAnglesBetweenDirections(Vector3 direcitonA, Vector3 directionB)
        {
            return Vector3.Angle(direcitonA, directionB);
        }

        public static Mesh CreateCubeMesh()
        {
            Vector3[] verticesArray = 
            {
                new(-0.5f, -0.5f,  0.5f),
                new( 0.5f, -0.5f,  0.5f),
                new( 0.5f,  0.5f,  0.5f),
                new(-0.5f,  0.5f,  0.5f),
                new(-0.5f, -0.5f, -0.5f),
                new( 0.5f, -0.5f, -0.5f),
                new( 0.5f,  0.5f, -0.5f),
                new(-0.5f,  0.5f, -0.5f)
            };
            int[] trianglesArray = 
            {
                0, 2, 1, 0, 3, 2,
                5, 6, 7, 5, 7, 4,
                4, 7, 3, 4, 3, 0,
                1, 2, 6, 1, 6, 5,
                3, 7, 6, 3, 6, 2,
                4, 0, 1, 4, 1, 5
            };
            Mesh mesh = new()
            {
                vertices = verticesArray,
                triangles = trianglesArray
            };
            mesh.RecalculateNormals();
            return mesh;
        }

        public static bool ScreenPositionPointsToGameObjectWithTag(Vector2 screenPosition, Tag tag)
        {
            return GetFirstGameObjectWithTagPointedAtScreenPosition(screenPosition, tag) != null;
        }

        public static GameObject GetFirstGameObjectWithTagPointedAtScreenPosition(Vector2 screenPosition, Tag tag)
        {
            return GetFirstRaycastHitWithTagPointedAtScreenPosition(screenPosition, tag).collider?.gameObject;
        }

        public static RaycastHit GetFirstRaycastHitWithTagPointedAtScreenPosition(Vector2 screenPosition, Tag tag)
        {
            Camera camera = Camera.main;
            return camera.GetRaycastHitsListAtScreenPosition(screenPosition)
            .Where(iRaycast => iRaycast.collider.gameObject.HasTag(tag))
            .OrderBy(iRaycastHit => iRaycastHit.point.GetDistanceToPosition(camera.transform.position))
            .FirstOrDefault();
        }

        public static List<RaycastResult> GetRaycastResultsListAtScreenPosition(Vector2 screenPosition)
        {
            List<RaycastResult> raycastResultsList = new();
            EventSystem.current.RaycastAll(GetPointerEventDataWithScreenPosition(screenPosition), raycastResultsList);
            return raycastResultsList;
        }

        public static PointerEventData GetPointerEventDataWithScreenPosition(Vector2 screenPosition)
        {
            return new(EventSystem.current) { position = screenPosition };
        }

        public static PointerEventData GetPointerEventDataWithCurrentMousePosition()
        {
            return GetPointerEventDataWithScreenPosition(Input.mousePosition);
        }

        public static List<GameObject> GetCurrentHoveredUIGameObjectsList()
        {
            return GetRaycastResultsListAtScreenPosition(Input.mousePosition).Select(iRaycastResult => iRaycastResult.gameObject).ToList();
        }

        public static List<Graphic> GetCurrentHoveredGraphicsList()
        {
            return GetCurrentHoveredUIGameObjectsList()
            .Select(iGameObject => iGameObject.GetComponent<Graphic>())
            .ToList().WithoutNulls();
        }

        public static bool IsInDevelopmentBuild()
        {
            return IsInBuild() && Debug.isDebugBuild;
        }

        public static bool IsInBuild()
        {
            bool isInBuild = true;
            #if UNITY_EDITOR
            isInBuild = false;
            #endif
            return isInBuild;
        }
    }
}