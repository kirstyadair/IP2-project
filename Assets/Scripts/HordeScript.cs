﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HordeScript : MonoBehaviour
{
    public delegate void SpawningEvent();
    public event SpawningEvent OnSpawnComplete;

    public GameObject zombiePrefab;
    public GameObject crosshair;
    public Vector3 centerPoint;
    public Animator crosshairAnimator;
    public AudioClip spawnSound;
    // Used for keeping stuff inside the screen
    Vector3 minScreenBounds;
    Vector3 maxScreenBounds;

    public float reticleSlowness; // How slow the reticle moves
    GameData gameData;

    // Where to locate the crosshair when we are respawning the horde
    public Transform crosshairDefault;

    [Header("Horde settings")]
    public float wiggleMultiplier;
    public float forceMultiplier;

    [Header("Sushi sprites")]
    public Sprite eyesSushi;
    public Sprite tentacleSushi;

    public int zombiesAlive;
    public int zombiesTotal;
    public bool isSpawning = false;

    // Start is called before the first frame update
    void Awake()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        gameData.OnStateChange += OnStateChange;
    }

    public void OnStateChange(GameState oldState, GameState newState)
    {

        if (newState == GameState.PREP)
        {
            // Kill off all old zombies
            foreach (Transform child in transform) child.gameObject.GetComponent<ZombieScript>().Kill();
        }

        if (newState == GameState.PLAY)
        {
            // Start spawning
            SpawnPointScript spawnPoint = gameData.currentSpawnPoint;

            // Find out how many zombies for this wave for this map
            SushiType sushiType = gameData.currentMap.waves[gameData.wave].sushiType;
            int count = gameData.currentMap.waves[gameData.wave].sushiCount;
            float timeBetweenSpawns = gameData.currentMap.waves[gameData.wave].timeBetweenSpawns;
            zombiesTotal = count;

            StartCoroutine(Spawn(sushiType, count, timeBetweenSpawns, spawnPoint));
        }
    }

    IEnumerator Spawn(SushiType sushiType, int count, float timeBetweenSpawns, SpawnPointScript spawnPoint)
    {
        isSpawning = true;

        for (int x = 0; x < count; x++)
        {
            Vector3 spawnPos = spawnPoint.gameObject.transform.position;
            spawnPos.y = this.transform.position.y;
            GameObject zombie = Instantiate(zombiePrefab, transform);
            zombie.transform.position = spawnPos;
           

            Sprite sushiSprite = eyesSushi;

            switch (sushiType)
            {
                case SushiType.EYES:
                    sushiSprite = eyesSushi;
                    break;
                case SushiType.TENTACLES:
                    sushiSprite = tentacleSushi;
                    break;
                default: break;
            }

            zombie.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sushiSprite;

            Vector3 force = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
            zombie.GetComponent<Rigidbody>().AddForce(force);
            zombie.GetComponent<AudioSource>().PlayOneShot(spawnSound);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        isSpawning = false;
        OnSpawnComplete();
    }

    public void MoveZombieTowardsTarget(GameObject zombie, Vector3 point)
    {
        //xSum += zombie.transform.position.x;
       // ySum += zombie.transform.position.y;

        Vector3 force = new Vector3(0, 0, 0);
        force = point - zombie.transform.position;
        force.Normalize();
        force.y = 0;
        force *= forceMultiplier;

        Vector3 wiggle = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        wiggle *= wiggleMultiplier;
        force += wiggle;
        //force /= 10f;
        zombie.GetComponent<Rigidbody>().AddForce(force);

  
        //if (force.x < -30f) zombie.GetComponent<SpriteRenderer>().flipX = true;
        //if (force.x > 30f) zombie.GetComponent<SpriteRenderer>().flipX = false;
    }

    // Update is called once per frame
    void Update()
    {
        //minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        //maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(-Screen.width, -Screen.height, Camera.main.transform.position.z));

        Ray ray1 = Camera.main.ScreenPointToRay(new Vector3(0, 0));
        Ray ray2 = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height));

        RaycastHit hit1;
        RaycastHit hit2;

        Physics.Raycast(ray1, out hit1, LayerMask.GetMask("Ground"));
        Physics.Raycast(ray2, out hit2, LayerMask.GetMask("Ground"));

        minScreenBounds = hit1.point;
        maxScreenBounds = hit2.point;

        int alive = 0;
        foreach (Transform child in transform)
        {
            alive++;
            GameObject zombie = child.gameObject;
            Vector3 target = centerPoint;
            MoveZombieTowardsTarget(zombie, target);
        }

        zombiesAlive = alive;

        centerPoint = crosshair.transform.position;

        // Move crosshair and constrain within world
        crosshair.transform.position += (new Vector3(Input.GetAxis("HorizontalHorde"), 0, Input.GetAxis("VerticalHorde"))) / 10;// distanceFromCenter / reticleSlowness;
        crosshair.transform.position = new Vector3(Mathf.Clamp(crosshair.transform.position.x, minScreenBounds.x + 1, maxScreenBounds.x - 1), crosshair.transform.position.y, Mathf.Clamp(crosshair.transform.position.z, minScreenBounds.z + 1, maxScreenBounds.z - 1));
    }


    /*
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(centerPoint, 0.2f);
    }*/
}