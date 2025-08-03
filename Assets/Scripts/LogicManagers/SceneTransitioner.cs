using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tweens;
using UnityEngine.EventSystems;

using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    private static SceneTransitioner instance;
    public static SceneTransitioner Instance { get => instance; }

    [Header("Game Object References")]
    public CanvasGroup crossfade;

    [Header("Animation Configuration")]
    public float transitionTime = 1f;
    private WaitForSeconds waitForTransition;

    private HashSet<string> loadReasons = new HashSet<string>();

    public delegate void SceneTransitionEvent(string scene);
    /**
     * \brief An event fired when any new scene is requested to be loaded, but before the animation starts.
     * \param scene The scene that will be loaded.
     */
    public SceneTransitionEvent OnSceneLoadRequested;
    /**
     * \brief An event fired when a new scene is loaded, but the blackout is still active. Can be used to add load reasons or for initialization.
     * \param scene The scene that was loaded.
     */
    public SceneTransitionEvent OnSceneLoaded;
    /**
     * \brief An event fired when a new scene has loaded, all load reasons are cleared, and the transition out animation has played.
     * \param scene The scene that was loaded.
     */
    public SceneTransitionEvent OnBlackoutLifted;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
            waitForTransition = new WaitForSeconds(transitionTime);
            crossfade.alpha = 0f;
            crossfade.gameObject.SetActive(false);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /**
     * \brief Loads the requested scene, doing a fade to black between scenes.
     * \param sceneName The name of the scene to load.
     *        Note the scene must be in the 'Scenes to Build' setting to work.
     */
    public void LoadNewScene(string sceneName, GameManager caller)
    {
        StartCoroutine(LoadLevelAnim(sceneName, caller));
    }

    /**
     * \brief Can be used by anywhere to hold the screen at black between sceens to allow extra time for initialization and configuration.
     * \param newLoadReason The name of the reason to hold the screen to black for. Should be used in the CompleteLoadReason call as well.
     */
    public void AddLoadReason(string newLoadReason)
    {
        if (!loadReasons.Contains(newLoadReason))
        {
            loadReasons.Add(newLoadReason);
        }
        else
        {
            Debug.LogError("Issue in scene loading, reason " + newLoadReason + " already present");
        }
    }

    /**
     * \brief Marks the given load reason as completed, once all load reasons are completed the scene will be faded in to.
     * \param completedReason The name of the reason to mark completed. Must be the same as what was used in AddLoadReason.
     */
    public void CompleteLoadReason(string completedReason)
    {
        if (loadReasons.Contains(completedReason))
        {
            loadReasons.Remove(completedReason);
        }
        else
        {
            Debug.LogError("Issue in scene loading, reason " + completedReason + " already removed.");
        }
    }

    /**
     * \brief Does the scene loading and animation.
     */
    private IEnumerator LoadLevelAnim(string sceneName, GameManager caller)
    {
        Debug.Log("Starting scene loading");

        OnSceneLoadRequested?.Invoke(sceneName);

        crossfade.gameObject.SetActive(true);

        FloatTween fadeOutTween = new FloatTween
        {
            from = 0.0f,
            to = 1.0f,
            duration = transitionTime,
            easeType = EaseType.SineIn,
            onUpdate = (_, value) => crossfade.alpha = value
        };
        // crossfade.TweenCanvasGroupAlpha(1f, transitionTime);
        crossfade.gameObject.AddTween(fadeOutTween);

        yield return waitForTransition;

        Debug.Log("Animation done, executing load");

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);

        Debug.Log("Loading done, waiting for other reasons");

        // Gives the activated scene 1 frame to register loading reasons
        yield return null;

        OnSceneLoaded?.Invoke(sceneName);

        yield return waitForTransition;

        while (loadReasons.Count != 0)
        {
            Debug.Log(string.Join(",", loadReasons));
            yield return null;
        }

        Debug.Log("Playing scene open animation");

        FloatTween fadeInTween = new FloatTween
        {
            from = 1.0f,
            to = 0.0f,
            duration = transitionTime,
            easeType = EaseType.SineIn,
            onUpdate = (_, value) => crossfade.alpha = value
        };
        // crossfade.TweenCanvasGroupAlpha(0f, transitionTime);
        crossfade.gameObject.AddTween(fadeInTween);

        yield return waitForTransition;

        crossfade.gameObject.SetActive(false);

        OnBlackoutLifted?.Invoke(sceneName);

        caller.FinishedTransition();

        Debug.Log("Scene loading complete");
    }
}