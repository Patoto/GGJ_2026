using UnityEngine;
using UnityEngine.Playables;

namespace PortalRollerCoaster
{
    [RequireComponent(typeof(PlayableDirector))]
    public class PlayableDirectorEditor : MyMonoBehaviour
    {
        [SerializeField] private float speed = 1f;

        protected override void Awake()
        {
            base.Awake();
            Setup();
        }

        private void OnValidate()
        {
            PlayableDirector playableDirector = GetPlayableDirector();
            SetSpeed(playableDirector);
        }

        private void Setup()
        {
            PlayableDirector playableDirector = GetPlayableDirector();
            playableDirector.played += SetSpeed;
            SetSpeed(playableDirector);
        }

        private void SetSpeed(PlayableDirector playableDirector)
        {
            PlayableGraph playableGraph = playableDirector.playableGraph;
            if (playableGraph.IsValid())
            {
                playableGraph.GetRootPlayable(0).SetSpeed(speed);
            }
        }

        private PlayableDirector GetPlayableDirector()
        {
            return GetComponent<PlayableDirector>();
        }
    }
}