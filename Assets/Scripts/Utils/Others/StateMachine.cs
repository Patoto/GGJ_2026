using System;
using System.Collections;
using UnityEngine;

namespace PortalRollerCoaster
{
    public class StateMachine : MyMonoBehaviour
    {
        private IEnumerator currentStateCoroutine;
        private Func<IEnumerator> currentStateFunction;

        public void SetState(Func<IEnumerator> stateFunction)
        {
            StopCurrentState();
            currentStateFunction = stateFunction;
            currentStateCoroutine = StartCoroutine(currentStateFunction());
        }

        public void StopCurrentState()
        {
            StopCoroutine(currentStateCoroutine);
            currentStateFunction = null;

        }

        public bool CurrentStateIsState(Func<IEnumerator> stateFunction)
        {
            return stateFunction == currentStateFunction;
        }
    }
}