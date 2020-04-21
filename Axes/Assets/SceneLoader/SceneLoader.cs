﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneTransition {
    None,
    Fade,
    CircleWipe,
}

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    Animator anim;

    bool loading;

    private void Awake()
    {
        if (Instance != null && Instance == this) {
            Destroy(gameObject);
            return;
        } else {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void SetAnimator (SceneTransition transition) {
        switch (transition) {
            case SceneTransition.Fade:
                anim = transform.Find("BasicFade").GetComponent<Animator>();
                break;
            case SceneTransition.CircleWipe:
                anim = transform.Find("CircleWipe").GetComponent<Animator>();
                break;
            case SceneTransition.None:
                anim = null;
                break;
            default:
                break;
        }
    }

    public static void LoadNextScene(SceneTransition transition = SceneTransition.Fade) {
        LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings, transition);
    }

    public static void ResetScene(SceneTransition transition = SceneTransition.Fade) {
        LoadScene(SceneManager.GetActiveScene().buildIndex, transition);
    }

    public static void LoadScene(int buildIndex, SceneTransition transition = SceneTransition.Fade) {
        if (Instance.loading) return;
        Instance.StartCoroutine(Instance.LoadSceneAsync(buildIndex, transition));
    }

    private IEnumerator LoadSceneAsync(int buildIndex, SceneTransition transition) {
        loading = true;
        Instance.SetAnimator(transition);
        if (anim != null) {
            anim.SetTrigger("Out");
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        }

        AsyncOperation async = SceneManager.LoadSceneAsync(buildIndex);
        while (!async.isDone)
        {
            yield return null;
        }
        if (anim != null) anim.SetTrigger("In");
        loading = false;
    }
}
