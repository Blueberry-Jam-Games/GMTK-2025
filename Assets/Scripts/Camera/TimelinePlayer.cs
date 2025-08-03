using UnityEngine;
using UnityEngine.Playables;
public class TimelinePlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private PlayableDirector director = new PlayableDirector();

    public bool startAnimation = false;

    void Start()
    {
        director = GetComponent<PlayableDirector>();
        GameObject gameplayManager = GameObject.FindGameObjectWithTag("GameManager");
        if (gameplayManager == null && startAnimation)
        {
            PlayTimeline();
        }
    }


    public void PlayTimeline()
    {
        director.Play();
    }
}
