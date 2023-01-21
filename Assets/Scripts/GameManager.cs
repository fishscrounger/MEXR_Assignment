using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreUI;
    public TextMeshProUGUI MultiplierUI;
    public TextMeshProUGUI TimeUI;

    private float totalScore;
    private float multiplier;
    private int combination;

    private bool isPlaying;
    private float currentGameTime;

    public float totalGameTime;
    public float baseHitScore;   //score given for hitting an object without the multiplier
    public GameObject[] SpawnObjects; //food prefabs, thrown at player

    void Start()
    {
        
    }

    void Update()
    {
        if (isPlaying)
        {
            currentGameTime -= Time.deltaTime;

            if(currentGameTime <= 0)
            {
                currentGameTime = 0.0f;
                EndGame();
            }

            TimeUI.text = "Time: " + currentGameTime;

        }
    }

    public void ObjectHit()
    {
        //increase score and multiplier
        totalScore += (baseHitScore * multiplier);

        multiplier = 1.0f + (1.0f - (float)combination) * 0.05f;
    }
    public void ObjectMissed()
    {
        //reset multiplier, combo broken
        combination = 0;
    }

    public void StartGame()
    {
        totalScore = 0.0f;
        multiplier = 0.0f;
        combination = 0;
    }

    public void EndGame()
    {
        isPlaying = false;
    }

    private void SpawnFruit()
    {
        int RandObj = Random.Range(0, 10);

        Vector3 position = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 50.0f);
        GameObject newSpawn = Instantiate(SpawnObjects[RandObj], position, Quaternion.identity);

        //add force
    }
}
