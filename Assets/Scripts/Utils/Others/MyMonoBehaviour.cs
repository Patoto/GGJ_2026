using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GGJ_2026
{
    public class MyMonoBehaviour : MonoBehaviour
    {
        private bool initialized;

        private const float INSTANTIATE_AND_DESTROY_AFTER_SECONDS_DEFAULT_SECONDS = 3f;

        public static Action<MyMonoBehaviour> onInitialized;
        public static Action<MyMonoBehaviour> onEnabled;
        public static Action<MyMonoBehaviour> onDisabled;
        public static Action<MyMonoBehaviour> onAboutToBeDestroyed;

        protected virtual void Initialize() 
        {
            initialized = true;
            onInitialized?.Invoke(this);
        }

        public void TryToInitialize()
        {
            if (!initialized)
            {
                Initialize();
            }
        }

        protected virtual void Awake()
        {
            TryToInitialize();
        }

        protected virtual void OnEnable()
        {
            onEnabled?.Invoke(this);
        }

        protected virtual void OnDisable()
        {
            onDisabled?.Invoke(this);
        }

        protected virtual void OnDestroy()
        {
            onAboutToBeDestroyed?.Invoke(this);
        }

        public new IEnumerator StartCoroutine(IEnumerator coroutine)
        {
            base.StartCoroutine(coroutine);
            return coroutine;
        }

        public new void StopCoroutine(IEnumerator coroutine)
        {
            if (coroutine != null)
            {
                base.StopCoroutine(coroutine);
            }
        }

        public void StopCoroutinesList(List<IEnumerator> coroutinesList)
        {
            foreach (IEnumerator iCoroutine in coroutinesList)
            {
                StopCoroutine(iCoroutine);
            }
        }

        public IEnumerator InvokeActionAfterSeconds(Action action, float seconds, bool useScaledTime = true)
        {
            return StartCoroutine(InvokeActionAfterSecondsCoroutine(action, seconds, useScaledTime));
        }

        private IEnumerator InvokeActionAfterSecondsCoroutine(Action action, float seconds, bool useScaledTime)
        {
            if (useScaledTime)
            {
                yield return new WaitForSeconds(seconds);
            }
            else
            {
                yield return new WaitForSecondsRealtime(seconds);
            }
            action?.Invoke();
        }

        public IEnumerator InvokeActionAtTheEndOfTheFrame(Action action)
        {
            return StartCoroutine(InvokeActionAtTheEndOfTheFrameCoroutine(action));
        }

        private IEnumerator InvokeActionAtTheEndOfTheFrameCoroutine(Action action)
        {
            yield return new WaitForEndOfFrame();
            action?.Invoke();
        }

        public IEnumerator InvokeActionAfterNextFixedUpdate(Action action)
        {
            return StartCoroutine(InvokeActionAfterNextFixedUpdateCoroutine(action));
        }

        private IEnumerator InvokeActionAfterNextFixedUpdateCoroutine(Action action)
        {
            yield return new WaitForFixedUpdate();
            action?.Invoke();
        }

        public IEnumerator InvokeActionNextFrame(Action action)
        {
            return InvokeActionAfterNFrames(action, 1);
        }

        public IEnumerator InvokeActionAtTheEndOfNFrames(Action action, int framesAmount)
        {
            return StartCoroutine(InvokeActionAtTheEndOfNFramesCoroutine(action, framesAmount));
        }

        private IEnumerator InvokeActionAtTheEndOfNFramesCoroutine(Action action, int framesAmount)
        {
            yield return WaitNFramesCoroutine(framesAmount);
            yield return InvokeActionAtTheEndOfTheFrameCoroutine(action);
        }

        public IEnumerator WaitNFramesCoroutine(int framesAmount)
        {
            for (int i = 0; i < framesAmount; i++)
            {
                yield return null;
            }
        }

        public IEnumerator InvokeActionAfterNFrames(Action action, int framesAmount)
        {
            return StartCoroutine(InvokeActionAfterNFramesCoroutine(action, framesAmount));
        }

        private IEnumerator InvokeActionAfterNFramesCoroutine(Action action, int framesAmount)
        {
            yield return WaitNFramesCoroutine(framesAmount);
            action?.Invoke();
        }

        public T InstantiateAndDestroyAfterSeconds<T>(T myObject, Vector3 position, Quaternion rotation = default, Transform parent = null, float seconds = INSTANTIATE_AND_DESTROY_AFTER_SECONDS_DEFAULT_SECONDS) where T : Object
        {
            T objectInstance = Instantiate(myObject, position, rotation, parent);
            InvokeActionAfterSeconds(() => Destroy(objectInstance), seconds);
            return objectInstance;
        }

        public ParticleSystem InstantiateParticleSystemAndDestroyAfterDone(ParticleSystem particleSystemPrefab, Vector3 position, Transform parent = null)
        {
            ParticleSystem particleSystem = InstantiateAndDestroyAfterSeconds(particleSystemPrefab.gameObject, position, particleSystemPrefab.transform.rotation, parent, particleSystemPrefab.main.duration).GetComponent<ParticleSystem>();
            return particleSystem;
        }

        public new void Destroy(Object myObject)
        {
            myObject.Destroy();
        }
    }
}