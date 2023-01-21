using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreUI;
    public TextMeshProUGUI MultiplierUI;
    public TextMeshProUGUI TimeUI;

    private float TotalScore;
    private float Multiplier;
    private int Combo;

    private bool IsPlaying;
    private float CurrentGameTime;

    public float TotalGameTime;
    public float BaseHitScore;   //score given for hitting an object without the multiplier
    public GameObject[] SpawnObjects; //food prefabs, thrown at player

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

        SpawnFruit();
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
    }

    private void SpawnFruit()
    {
        int RandObj = Random.Range(0, 19);
        Vector3 RandPos = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(2.0f, 3.0f), 20.0f);

        GameObject newSpawn = Instantiate(SpawnObjects[RandObj], RandPos, Quaternion.identity);

        newSpawn.GetComponent<Projectile>().GManager = this;
    }
}
