using UnityEngine;
using UnityEngine.Playables;
public class TimelinePlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private PlayableDirector director = new PlayableDirector();

    void Start()
    {
        director = GetComponent<PlayableDirector>();
    }


    public void PlayTimeline()
    {
        director.Play();
    }
}
