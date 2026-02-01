using System;
using UnityEngine;

namespace GGJ_2026
{
	public class FloorTapesHandler : MyMonoBehaviour, IEventSubscriberDeclarator
	{
		[SerializeField] private GameObject floorTapes1GameObject;
		[SerializeField] private GameObject floorTapes2GameObject;
		[SerializeField] private GameObject floorTapes3GameObject;

        public void SubscribeToEnableEvents()
		{
			MaskingTape.onSetState += OnMaskingTapeSetState;
		}

        public void UnsubscribeFromEnableEvents()
        {
			MaskingTape.onSetState -= OnMaskingTapeSetState;
        }

        private void OnMaskingTapeSetState(MaskingTape.State state)
		{
			floorTapes1GameObject.SetActive(false);
			floorTapes2GameObject.SetActive(false);
			floorTapes3GameObject.SetActive(false);
            switch (state)
            {
                case MaskingTape.State.Full:
                    break;
                case MaskingTape.State.Middle:
					floorTapes1GameObject.SetActive(true);
                    break;
                case MaskingTape.State.Low:
					floorTapes1GameObject.SetActive(true);
					floorTapes2GameObject.SetActive(true);
                    break;
                case MaskingTape.State.Empty:
					floorTapes1GameObject.SetActive(true);
					floorTapes2GameObject.SetActive(true);
					floorTapes3GameObject.SetActive(true);
                    break;
            }
        }
    }
}