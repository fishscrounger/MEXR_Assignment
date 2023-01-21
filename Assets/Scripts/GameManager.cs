using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreUI;
    public TextMeshProUGUI MultiplierUI;
    public TextMeshProUGUI TimeUI;

    public TextMeshProUGUI GameOverUI;
    public Button QuitToMenuButton;
    public Button PlayAgainButton;

    public TextMeshProUGUI TipUI;

    private float TotalScore;
    private float Multiplier;
    private int Combo;

    private bool IsPlaying;
    private float CurrentGameTime;

    public float TotalGameTime;
    public float BaseHitScore;   //score given for hitting an object without the multiplier
    public GameObject[] SpawnObjects; //food prefabs, thrown at player

    public string[] MotivationTips;

    void Start()
    {
        
    }

    void Update()
    {
        if (IsPlaying)
        {
            CurrentGameTime -= Time.deltaTime;

            if(CurrentGameTime <= 0)
            {
                CurrentGameTime = 0.0f;
                EndGame();
            }

            TimeUI.text = "Time: " + (int)CurrentGameTime;

        }
    }

    public void ObjectHit()
    {
        //increase score and multiplier
        TotalScore += (BaseHitScore * Multiplier);
        Multiplier = 1.0f + (1.0f - (float)Combo) * 0.05f;

        ScoreUI.text = "Score: " + (int)TotalScore;
        MultiplierUI.text = "Multiplier: " + Multiplier.ToString("F2");
    }
    public void ObjectMissed()
    {
        //reset multiplier, combo broken
        Combo = 0;
        Multiplier = 1.0f;
        MultiplierUI.text = "Multiplier: 1";
    }

    public void StartGame()
    {
        TotalScore = 0.0f;
        Multiplier = 1.0f;
        Combo = 0;
        CurrentGameTime = TotalGameTime;

        ScoreUI.text = "Score: 0";
        MultiplierUI.text = "Multiplier: 1";

        IsPlaying = true;

        StartCoroutine(Spawn());
        StartCoroutine(ShowTips());
    }

    public void EndGame()
    {
        IsPlaying = false;

        //clean up the projectiles, game is over
        Projectile[] RemainingProjectiles = FindObjectsOfType<Projectile>();
        foreach (Projectile p in RemainingProjectiles)
        {
            Destroy(p.gameObject);
        }

        //show game over screen
        GameOverUI.gameObject.SetActive(true);
        QuitToMenuButton.gameObject.SetActive(true);
        PlayAgainButton.gameObject.SetActive(true);

        //hide tips
        TipUI.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator Spawn()
    {
        while (IsPlaying)
        {
            SpawnFood();
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
        }
    }

    IEnumerator ShowTips()
    {
        while (IsPlaying)
        {
            yield return new WaitForSeconds(10.0f); //show a new tip afetr ten seconds, and show it for five seconds to motivate the user

            int RandObj = Random.Range(0, MotivationTips.Length - 1);
            TipUI.gameObject.SetActive(true);
            TipUI.text = MotivationTips[RandObj];

            yield return new WaitForSeconds(5.0f);

            TipUI.gameObject.SetActive(false);

        }
    }

    private void SpawnFood()
    {
        int RandObj = Random.Range(0, SpawnObjects.Length - 1);
        Vector3 RandPos = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(1.0f, 3.5f), 20.0f);

        GameObject newSpawn = Instantiate(SpawnObjects[RandObj], RandPos, Quaternion.identity);

        newSpawn.GetComponent<Projectile>().GManager = this;
    }
}
