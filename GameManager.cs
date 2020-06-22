using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    [SerializeField]
    UserController player2;
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject[] spawnPoints;
    [SerializeField]
    GameObject Troll;
    [SerializeField]
    GameObject Dragon;
    [SerializeField]
    GameObject Skeleton;
    [SerializeField]
    Text levelText;
    [SerializeField]
    Text endGameText;
    [SerializeField]
    int finalLevel = 15;
    public GameObject SoundStop;
    public GameObject SoundStart;
    [SerializeField]
    public Mute mute;

    private int currentLevel;
    private float generatedSpawnTime = 1;
    private float currentSpawnTime = 0;
    private GameObject newEnemy;

    public GameObject PauseBtn;
    public GameObject PlayBtn;
    private bool gamepause;


    private List<EnemyHealth> enemies = new List<EnemyHealth> ();
    private List<EnemyHealth> killedEnemies = new List<EnemyHealth> ();

    public void RegisterEnemy(EnemyHealth enemy) {
        enemies.Add (enemy);
    }

    public void KilledEnemy(EnemyHealth enemy) {
        killedEnemies.Add (enemy);
    }


    private bool gameOver = false;

    public bool GamePause
    {
        get { return gamepause; }
    }

    public bool GameOver
    {
        get { return gameOver; }
    }

    public GameObject Player
    {
        get { return player; }
    }

    public UserController Player2
    {
        get { return player2; }
    }
    void Awake () {
        if(instance == null) {
            instance = this;
        }else if(instance != this) {
            Destroy (gameObject);
        }
       
    }

    // Use this for initialization
    void Start () {
        endGameText.GetComponent<Text> ().enabled = false;
        StartCoroutine (spawn ());
        currentLevel = 1;
        gamepause = false;
        if( AudioListener.pause==true) {
            SoundStop.SetActive (false);
            SoundStart.SetActive (true);
        }

        if ( AudioListener.pause == false) {
            SoundStop.SetActive (true);
            SoundStart.SetActive (false);
        }
        gamepause = false;
        PlayBtn.SetActive (false);
        PauseBtn.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
        currentSpawnTime += Time.deltaTime;
	}
    public void PlayerHit(int currentHP) {
        if(currentHP > 0) {
                gameOver = false;
            }
            else {
                gameOver = true;
            StartCoroutine (endGame ("Defeat"));
            }
        }

    IEnumerator spawn () {
        if (currentSpawnTime > generatedSpawnTime) {
            currentSpawnTime = 0;
            if(enemies.Count < currentLevel) {
                int randomNumber = Random.Range (0, spawnPoints.Length - 1);
                GameObject spawnLocation = spawnPoints[randomNumber];
                int randomEnemy = Random.Range (0, 3);
                if(randomEnemy == 0) {
                    newEnemy = Instantiate (Skeleton) as GameObject;
                }
                if (randomEnemy == 1) {
                    newEnemy = Instantiate (Troll) as GameObject;
                }
                if (randomEnemy == 2) {
                    newEnemy = Instantiate (Dragon) as GameObject;
                }
                newEnemy.transform.position = spawnLocation.transform.position;
            }
            if(killedEnemies.Count == currentLevel && currentLevel!=finalLevel) {
                enemies.Clear ();
                killedEnemies.Clear ();
                yield return new WaitForSeconds (3f);
                currentLevel++;
                levelText.text = "Level " + currentLevel;
            }
            if(killedEnemies.Count == finalLevel) {
                StartCoroutine (endGame ("Victory!"));
            }
        }
        yield return null;
        StartCoroutine (spawn ());

    }

    IEnumerator endGame (string outcome) {
        endGameText.text = outcome;
        endGameText.GetComponent<Text> ().enabled = true;
        yield return new WaitForSeconds (5f);

        SceneManager.LoadScene ("StartScene");
  
    }

    public void SoundStopDissaper() {
        SoundStop.SetActive (false);
        SoundStart.SetActive (true);
    }

    public void SoundStartDissaper () {
        SoundStop.SetActive (true);
        SoundStart.SetActive (false);
    }

    public void IsGamePause () {
        PlayBtn.SetActive (true);
        PauseBtn.SetActive (false);
        endGameText.text = "Pause";
        endGameText.GetComponent<Text> ().enabled = true;
        gamepause = true;
    }

    public void IsGameStart () {
        endGameText.GetComponent<Text> ().enabled = false;
        PlayBtn.SetActive (false);
        PauseBtn.SetActive (true);
        gamepause = false;
    }
}

