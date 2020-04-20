using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
   

    public Text winText;
    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    public Text shieldText;
    public Text slomoText;
    public Rigidbody shieldgen;
    public GameObject energyshield;
    public GameObject player;
    public Transform shieldSpawn;

    private bool gameOver;
    private bool restart;
    private int score;
    private int shield;
    private int slo;
    public float currentAmount = 0f;
    public float maxAmount = 5f;
   
    public AudioSource musicSource;
    public AudioClip winClip;
    public AudioClip lossClip;
    public AudioClip backgroundClip;

    void Start()
    {
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        winText.text = "";
        score = 0;
        UpdateScore();
        UpdateSlo();
        StartCoroutine(SpawnWaves());
        musicSource.clip = backgroundClip;
        musicSource.Play();
    }

    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                SceneManager.LoadSceneAsync(0);
            }
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        UpdateSlo();
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "Press 'T' for Restart";
                restart = true;
                break;
            }
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }
    public void AddShield(int newShieldValue)
    {
        shield += newShieldValue;
        UpdateShield();
    }
    public void AddSlo(int newSloValue)
    {
        slo += newSloValue;
        UpdateSlo();
    }

    public void UpdateShield()
    {
        shieldText.text = "ENERGY SHIELD: " + shield;

        if (shield>=100)
        {
            shieldText.text = "PRESS R TO USE ENERGY SHIELD!";
            if (Input.GetKeyDown("r"))
            {
                Instantiate(energyshield, shieldSpawn.position, shieldSpawn.rotation);
                shield = 0;
            }
        }
        
    }
    public void UpdateSlo()
    {
        slomoText.text = "SLOW MOTION: " + slo;

        if (slo >= 100)
        {
            slomoText.text = "PRESS E TO SLOW DOWN TIME!";

            if (Input.GetKeyDown("e"))
            {
                StartCoroutine(SlowMotion());
            }
        }

        else
        {
            Time.timeScale = 1.0f;
        }
    }


    void UpdateScore()
    {
        scoreText.text = "POINTS: " + score;
        if (score >= 500)
        {
            musicSource.Stop();
            musicSource.clip = winClip;
            musicSource.Play();

            winText.text = "You win! Game created by Ian Montilla!";
            gameOver = true;
            restart = true;

            
        }

    }
    public void GameOver()
    {
        musicSource.Stop();
        musicSource.clip = lossClip;
        musicSource.Play();

        gameOverText.text = "Game Over! Game created by Ian Montilla!";
        gameOver = true;

      
    }

    public IEnumerator SlowMotion()
    {
            if (Time.timeScale == 1.0f)
                Time.timeScale = 0.3f;

            else

                Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        


        if (Time.timeScale == 0.03f)
        {

            currentAmount += Time.deltaTime;
        }

        if (currentAmount > maxAmount)
        {

            currentAmount = 0f;
            Time.timeScale = 1.0f;

        }

        yield return new WaitForSeconds(2f);

        slo = 0;
    }
}