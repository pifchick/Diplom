using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChanger : MonoBehaviour
{
    private Animator animator;
    public int LevelToLoad;
    // Start is called before the first frame update
    public Slider slider;
    public GameObject LevelNext;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void FadeToLevel()
    {
        animator.SetTrigger("fade");
    }
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(LevelToLoad);
        StartCoroutine(LoadingSceneOnFade());
    }
    IEnumerator LoadingSceneOnFade()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(LevelToLoad);
        LevelNext.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
        }
    }
}
