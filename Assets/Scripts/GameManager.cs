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
    public float gameDuration = 30.0f; // 게임 지속 시간 (30초)

    public static GameManager instance = null;

    public TextMeshProUGUI killCountText; // UI Text 엘리먼트에 대한 참조
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
        yield return Init(); // Init 코루틴이 완료될 때까지 기다림
                             // 이후에 필요한 초기화 로직을 추가할 수 있음
    }
    public IEnumerator Init()
    {
        enemyPoints = new List<Transform>(GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>());

        if (enemyPoints.Count > 0)
        {
            yield return StartCoroutine(CreateEnemy()); // CreateEnemy 코루틴이 완료될 때까지 기다림
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

                // 10초 뒤에 적을 자동 파괴
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

                // 10초 뒤에 적을 자동 파괴
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
            gameTimer += Time.deltaTime; // 게임 경과 시간 증가

            float remainingTime = Mathf.Max(0, gameDuration - gameTimer); // 남은 시간을 계산

            if (timerText != null)
            {
                timerText.text = Mathf.FloorToInt(remainingTime).ToString(); // UI Text에 남은 시간을 표시
            }

            if (gameTimer >= gameDuration)
            {
                // 게임 지속 시간이 30초 이상이면 'LoseMenu' 씬으로 전환
                isGameLose = true;
                SceneManager.LoadScene("LoseMenu");

                // 게임 종료 UI를 띄움
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


            
            // 게임 종료 UI를 띄움
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            //Instantiate(GameFinUI, GameObject.Find("Canvas").transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
        
        }

        if (Input.GetKeyDown(KeyCode.X))
        //if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            // 게임을 정말 종료할 것인지 묻는 UI를 띄움
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
        gameTimer = 0.0f; // 게임 시간 초기화
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
            Time.timeScale = 0f; // 시간을 멈춤
            ShowPauseMenu(); // 일시 정지 메뉴 표시

            if (SceneManager.GetActiveScene().name != "Intro") // Intro 씬이 아닌 경우에만 마우스 커서 처리
            {
                UnityEngine.Cursor.visible = true;
                UnityEngine.Cursor.lockState = CursorLockMode.None;
            }
            //Cursor.visible = true;
            // Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1f; // 시간을 다시 진행
            HidePauseMenu(); // 일시 정지 메뉴 숨김

            if (SceneManager.GetActiveScene().name != "Intro") // Intro 씬이 아닌 경우에만 마우스 커서 처리
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
        // TODO: 일시 정지 메뉴 UI를 활성화하는 코드 작성
        if (PauseUI != null)
        {
            PauseUI.SetActive(true);
            Debug.Log("일시정지");

            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            // 이벤트 시스템에서 현재 선택된 객체를 해제
            EventSystem.current.SetSelectedGameObject(null);

        }
    }

    private void HidePauseMenu()
    {
        // TODO: 일시 정지 메뉴 UI를 비활성화하는 코드 작성
        if (PauseUI != null)
        {
            PauseUI.SetActive(false);

            // 마우스 커서 숨기기
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
