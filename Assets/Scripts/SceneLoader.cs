using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 5초 뒤에 Main 씬으로 전환
        StartCoroutine(LoadMainSceneAfterDelay(9.0f));
    }

    IEnumerator LoadMainSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Intro 씬에서 Main 씬으로 전환
        SceneManager.LoadScene("Main");

        // 씬 전환 후 EventSystem이 없으면 생성
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        // Intro 씬에서 Main 씬으로 전환할 때 TogglePause 호출하여 마우스 커서 처리
        GameManager.instance.TogglePause();
    }
}
