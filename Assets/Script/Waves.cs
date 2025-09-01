using Assets.Scripts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Waves;

public class Waves : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        [System.Serializable]
        public class Monster
        {
            public GameObject enemyPrefab;
            public int amount;
            public Vector3 position;
        }
        public Monster[] monsters;
    }

    public enum State
    {
        OnLoading,
        OnFighting,
        Finished
    }

    public State state;

    public Wave[] waves;
    private ObjectPool pooler;
    public float timeEachWaves = 10f;

    // Way to get the number of total waves 
    public int TotalWavesNumber
    {
        get { return waves.Length; }
    }

    public int CurrentWave
    {
        get { return waveCurrent; }
    }

    public Dictionary<GameObject, int> waveLeastMonster;

    // This is current wave number
    private int waveCurrent;
    // This is the remains amount of monster of the current wave
    public int leftOver;

    private float timer = 0f;

    private InventoryManagement UIScript;

    // Start is called before the first frame update
    void Start()
    {
        UIScript = GameObject.FindGameObjectWithTag("UI").GetComponent<InventoryManagement>();
        int waveNumber = 0;
        // Start
        waveCurrent = 0;
        leftOver = 0;

        waveLeastMonster = new Dictionary<GameObject, int>();
        pooler = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();

        Invoke("InsertPool", 2f);

        timer = timeEachWaves;
        if (waves.Length == 0) state = State.Finished;
    }

    public void InsertPool()
    {
        // Find the maximum amount of monster for each types
        foreach (Wave wave in waves)
        {
            foreach (Wave.Monster monster in wave.monsters)
            {
                if (waveLeastMonster.ContainsKey(monster.enemyPrefab))
                {
                    if (monster.amount > waveLeastMonster[monster.enemyPrefab])
                    {
                        waveLeastMonster[monster.enemyPrefab] = monster.amount;
                    }
                    else { }
                }
                else
                {
                    waveLeastMonster.Add(monster.enemyPrefab, monster.amount);
                }
            }
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Create an object pooler for each monster type
        foreach (GameObject key in waveLeastMonster.Keys)
        {
            Ghost ghost = key.GetComponent<Ghost>();
            if (ghost != null)
            {
                ghost.SetTarget(player);
            }
            pooler.Add(key.ToString(), key, waveLeastMonster[key]);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (state)
        {
            case State.OnLoading:
                {
                    UIScript.DisplayTimer();
                    UIScript.SetTimer(timer);
                    timer -= Time.deltaTime;

                    if (timer <= 0)
                    {
                        // Summon the next wave
                        foreach (Wave.Monster monster in waves[waveCurrent].monsters)
                        {
                            for (int i = 0; i < monster.amount; i++)
                            {
                                GameObject adding = pooler.SpawnFromPool(monster.enemyPrefab.ToString(), monster.position, Quaternion.identity);
                                WaveSupport scriptWave = adding.GetComponent<WaveSupport>();
                                scriptWave.SetActive(true);
                            }
                            leftOver += monster.amount;
                        }

                        // Increase counter for current wave
                        waveCurrent++;

                        state = State.OnFighting;

                    }
                    break;
                }
            case State.OnFighting:
                {
                    UIScript.HideTimer();
                    if (leftOver == 0)
                    {
                        // If it the last wave
                        if (CurrentWave >= TotalWavesNumber)
                        {
                            // Turn into finish round
                            state = State.Finished;
                        }
                        else
                        {
                            // Move to next wave
                            timer = timeEachWaves;
                            state = State.OnLoading;
                        }
                    }
                    break;
                }
            case State.Finished:
                {
                    ItemManagement.Save(ItemManagement.USER_DATA);
                    ItemManagement.LoadRoundNumber();
                    int buildIndex = PlayerPrefs.GetInt("Map");
                    int currBuildIndex = SceneManager.GetActiveScene().buildIndex - 2;
                    if (currBuildIndex > buildIndex) ItemManagement.SaveRoundNumber(currBuildIndex);
                    UIScript.FinishMap();
                    break;
                }
        }
    }
}
