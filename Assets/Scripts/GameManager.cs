using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public List<Transform> enemyPoints; // Enemy Location
    public GameObject[] enemies;    // Enemy Prefab
    public float createTime = 2.0f;
    public int maxEnemy = 4;

    public int killEnemy = 0;
    public int goal = 5;            // goal -> Fin Game

    public bool isGameOver = false;
    public GameObject GameFinUI;
    public bool isGameLose = false;

    private float gameTimer = 0.0f;
    public float gameDuration = 30.0f; // 게임 지속 시간 (30초)

    public static GameManager instance = null;

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
        Init();                         // GameManager Init
        
    }

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



    void Update()
    {
        if (!isGameOver)
        {
            gameTimer += Time.deltaTime; // 게임 경과 시간 증가

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
}
