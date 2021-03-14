using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading1 : MonoBehaviour
{
    [SerializeField] private Image _progressBar;
    // Start is called before the first frame update
    void Start()
    {
        // Start async operation
        Time.timeScale = 1;
        StartCoroutine(LoadAsyncOperation());

    }
    IEnumerator LoadAsyncOperation()
    {
        // create an async operation
        yield return new WaitForSeconds(3f);
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(2);
        while (gameLevel.progress < 1)
        {
            // take the progress bar fill = async operation progress
            _progressBar.fillAmount = gameLevel.progress;
            // when finished, load the game scene
            yield return new WaitForEndOfFrame();
        }
    }
}
