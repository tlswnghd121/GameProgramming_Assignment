using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public List<Transform> enemyPoints; // Enemy Location
    public GameObject[] enemies;    // Enemy Prefab
    public float createTime = 2.0f;
    public int maxEnemy = 4;

    public int killEnemy = 0;
    public int goal = 5;            // goal -> Fin Game
    

    public bool isGameOver = false;
    public GameObject PauseUI;
    public bool isGameLose = false;
    public bool isPaused = false;

    private float gameTimer = 0.0f;
    public float gameDuration = 30.0f; // ���� ���� �ð� (30��)

    public static GameManager instance = null;

    public TextMeshProUGUI killCountText; // UI Text ������Ʈ�� ���� ����
    public TextMeshProUGUI timerText;

    public static GameManager Instance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

       
    }

    void Start()
    {
        StartCoroutine(InitCoroutine());
        //Init();                         // GameManager Init
     
    }
    private IEnumerator InitCoroutine()
    {
        yield return Init(); // Init �ڷ�ƾ�� �Ϸ�� ������ ��ٸ�
                             // ���Ŀ� �ʿ��� �ʱ�ȭ ������ �߰��� �� ����
    }
    public IEnumerator Init()
    {
        enemyPoints = new List<Transform>(GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>());

        if (enemyPoints.Count > 0)
        {
            yield return StartCoroutine(CreateEnemy()); // CreateEnemy �ڷ�ƾ�� �Ϸ�� ������ ��ٸ�
        }
    }

    IEnumerator CreateEnemy()
    {
        while (!isGameOver)
        {
            int enemyCount = (int)GameObject.FindGameObjectsWithTag("ENEMY").Length;

            if (enemyCount < maxEnemy)
            {
                yield return new WaitForSeconds(createTime);

                int pointIdx = UnityEngine.Random.Range(1, enemyPoints.Count);
                int enemyIdx = UnityEngine.Random.Range(0, enemies.Length);

                GameObject newEnemy = Instantiate(enemies[enemyIdx], enemyPoints[pointIdx].position, enemyPoints[pointIdx].rotation, GameObject.Find("Enemies").transform);

                // 10�� �ڿ� ���� �ڵ� �ı�
                //Destroy(newEnemy, 10.0f);
            }
            else
            {
                yield return null;
            }
        }
    }

    /*
    public void Init()
    {
        enemyPoints = new List<Transform>(GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>());

        if (enemyPoints.Count > 0)
        {
            StartCoroutine(this.CreateEnemy());
        }
    }

    IEnumerator CreateEnemy()
    {
        while (!isGameOver)
        {
            int enemyCount = (int)GameObject.FindGameObjectsWithTag("ENEMY").Length;

            if (enemyCount < maxEnemy)
            {
                yield return new WaitForSeconds(createTime);

                int pointIdx = UnityEngine.Random.Range(1, enemyPoints.Count);
                int enemyIdx = UnityEngine.Random.Range(0, enemies.Length);

                GameObject newEnemy = Instantiate(enemies[enemyIdx], enemyPoints[pointIdx].position, enemyPoints[pointIdx].rotation, GameObject.Find("Enemies").transform);

                // 10�� �ڿ� ���� �ڵ� �ı�
                //Destroy(newEnemy, 10.0f);

            }
            else yield return null;
        }
    }
    */



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
    {
        TogglePause();
    }

        if (!isGameOver)
        {
            gameTimer += Time.deltaTime; // ���� ��� �ð� ����

            float remainingTime = Mathf.Max(0, gameDuration - gameTimer); // ���� �ð��� ���

            if (timerText != null)
            {
                timerText.text = Mathf.FloorToInt(remainingTime).ToString(); // UI Text�� ���� �ð��� ǥ��
            }

            if (gameTimer >= gameDuration)
            {
                // ���� ���� �ð��� 30�� �̻��̸� 'LoseMenu' ������ ��ȯ
                isGameLose = true;
                SceneManager.LoadScene("LoseMenu");

                // ���� ���� UI�� ���
                UnityEngine.Cursor.visible = true;
                UnityEngine.Cursor.lockState = CursorLockMode.None;

            }
        }

        if (killCountText != null)
        {
            killCountText.text = killEnemy.ToString();
        }

        if (killEnemy >= goal && isGameOver == false)
        {
            isGameOver = true;
            if (isGameOver)
            {
                SceneManager.LoadScene("WinMenu");
            }


            
            // ���� ���� UI�� ���
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            //Instantiate(GameFinUI, GameObject.Find("Canvas").transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
        
        }

        if (Input.GetKeyDown(KeyCode.X))
        //if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            // ������ ���� ������ ������ ���� UI�� ���
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            GameObject UIPrefab = Resources.Load<GameObject>("Prefabs/ExitUI");
            //GameObject UI = Instantiate(UIPrefab, new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2), Quaternion.identity, GameObject.Find("Canvas").transform);
            GameObject UI = Instantiate(UIPrefab,
                new Vector3(GameObject.Find("Canvas").transform.position.x, GameObject.Find("Canvas").transform.position.y + 3.0f, GameObject.Find("Canvas").transform.position.z),
                Quaternion.identity,
                GameObject.Find("Canvas").transform);
        }

    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Main");

    }

    public void StartGame()
    {
        //StartCoroutine(StartGameCoroutine());
        
        SceneManager.LoadScene("Assignment1");

        killEnemy = 0;
        isGameOver = false;
        isGameLose = false;
        gameTimer = 0.0f; // ���� �ð� �ʱ�ȭ
        // Manager Init
        enemyPoints.Clear();
        instance.Init();
        //ItemManager.instance.Init();
        
    }


    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // �ð��� ����
            ShowPauseMenu(); // �Ͻ� ���� �޴� ǥ��

            if (SceneManager.GetActiveScene().name != "Intro") // Intro ���� �ƴ� ��쿡�� ���콺 Ŀ�� ó��
            {
                UnityEngine.Cursor.visible = true;
                UnityEngine.Cursor.lockState = CursorLockMode.None;
            }
            //Cursor.visible = true;
            // Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1f; // �ð��� �ٽ� ����
            HidePauseMenu(); // �Ͻ� ���� �޴� ����

            if (SceneManager.GetActiveScene().name != "Intro") // Intro ���� �ƴ� ��쿡�� ���콺 Ŀ�� ó��
            {
                UnityEngine.Cursor.visible = false;
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            }
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void ShowPauseMenu()
    {
        // TODO: �Ͻ� ���� �޴� UI�� Ȱ��ȭ�ϴ� �ڵ� �ۼ�
        if (PauseUI != null)
        {
            PauseUI.SetActive(true);
            Debug.Log("�Ͻ�����");

            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            // �̺�Ʈ �ý��ۿ��� ���� ���õ� ��ü�� ����
            EventSystem.current.SetSelectedGameObject(null);

        }
    }

    private void HidePauseMenu()
    {
        // TODO: �Ͻ� ���� �޴� UI�� ��Ȱ��ȭ�ϴ� �ڵ� �ۼ�
        if (PauseUI != null)
        {
            PauseUI.SetActive(false);

            // ���콺 Ŀ�� �����
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        }
    }

    public void ResumeGame()
    {
        HidePauseMenu();
        Time.timeScale = 1f;
        isPaused = false;
    }


    public bool IsPaused
    {
        get { return isPaused; }
    }
}
