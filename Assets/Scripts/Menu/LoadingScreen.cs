using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public Image progressBar; 
    public TextMeshProUGUI loadingText;
    public float minShowTime = 0.1f;  

    public void StartLoading(string sceneName)
    {
        progressBar.fillAmount = 0f;
        StartCoroutine(LoadScene(sceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        float timer = 0f; 

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);

            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, progress, Time.deltaTime * 6f);

            loadingText.text = "Loading... " + Mathf.RoundToInt(progressBar.fillAmount * 100f) + "%";

            timer += Time.deltaTime;

            if (progress >= 0.99f && timer >= minShowTime) 
            {
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }

}