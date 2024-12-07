using Managers;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Misc
{
    public class IntroController : MonoBehaviour
    {
        private PlayableDirector _playableDirector;

        private void Start()
        {
            _playableDirector = GetComponent<PlayableDirector>();
            _playableDirector.stopped += _ => { GameManager.LoadLevel(3); };
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _playableDirector.Stop();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                var time = _playableDirector.time;
                var timelineAsset = _playableDirector.playableAsset as TimelineAsset;

                foreach (var trackAsset in timelineAsset!.GetRootTracks())
                {
                    if (trackAsset.start > time)
                    {
                        _playableDirector.time = trackAsset.start;
                        return;
                    }
                }

                _playableDirector.Stop();
            }
        }
    }
}