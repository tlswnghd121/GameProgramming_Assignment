using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 5�� �ڿ� Main ������ ��ȯ
        StartCoroutine(LoadMainSceneAfterDelay(9.0f));
    }

    IEnumerator LoadMainSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Intro ������ Main ������ ��ȯ
        SceneManager.LoadScene("Main");

        // �� ��ȯ �� EventSystem�� ������ ����
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        // Intro ������ Main ������ ��ȯ�� �� TogglePause ȣ���Ͽ� ���콺 Ŀ�� ó��
        GameManager.instance.TogglePause();
    }
}
