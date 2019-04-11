using InControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HordeState
{
    NEUTRAL, OFFENSIVE, DEFENSIVE
}

public class HordeScript : MonoBehaviour
{
    public delegate void SpawningEvent();
    public event SpawningEvent OnSpawnComplete;

    public GameObject zombiePrefab;

    public HordeCrosshairScript crosshairA;
    public HordeCrosshairScript crosshairB;

    List<GameObject> zombiesAttachedToCrosshairA = new List<GameObject>();
    List<GameObject> zombiesAttachedToCrosshairB = new List<GameObject>();

    public AudioClip spawnSound;
    // Used for keeping stuff inside the screen
    Vector3 minScreenBounds;
    Vector3 maxScreenBounds;

    GameData gameData;


    [Header("Horde settings")]
    public float wiggleMultiplier;
    public float forceMultiplier;
    public float crosshairBounds;
    public float crosshairSpeed;

    [Header("Sushi sprites")]
    public Sprite eyesSushi;
    public Sprite teethSushi;
    public Sprite tentaclesSushi;
    public Sprite squidSushi;
    public float regenerationSpeed;

    public int zombiesAlive;
    public int zombiesTotal;
    public bool isSpawning = false;
    public Transform center;


    [Header("Offensive/Defensive system stats")]
    public float defensiveStat = 1;
    public float offensiveStat = 1;
    public HordeState state;
    public const int baseStat = 1;
    public float defenseModeTimer;
    public float offenseModeTimer;

    InputDevice controller;

    // Start is called before the first frame update
    void Awake()
    {
        state = HordeState.NEUTRAL;
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        gameData.OnStateChange += OnStateChange;

        GameObject selectionDataObj = GameObject.Find("PlayerSelectionData");
        if (selectionDataObj != null)
        {
            controller = selectionDataObj.GetComponent<PlayerSelectionData>().GetHordeController();
        }
    }

    public void OnStateChange(GameState oldState, GameState newState)
    {
        Transform spawnA = gameData.currentMap.hordeSpawnA;
        Transform spawnB = gameData.currentMap.hordeSpawnB;

        if (newState == GameState.PREP)
        {
            // Kill off all old zombies
            foreach (Transform child in transform) child.gameObject.GetComponent<ZombieScript>().Kill();

            if (crosshairA.showing) crosshairA.Hide();
            if (crosshairB.showing) crosshairB.Hide();

            // Reset crosshair positions
            crosshairA.transform.position = new Vector3(spawnA.position.x, crosshairA.transform.position.y, spawnA.position.z);
            crosshairB.transform.position = new Vector3(spawnB.position.x, crosshairB.transform.position.y, spawnB.position.z);
        }

        if (newState == GameState.PLAY)
        {
            zombiesAttachedToCrosshairA.Clear();
            zombiesAttachedToCrosshairB.Clear();

            // Show the crosshairs
            crosshairA.Show();
            crosshairB.Show();

            // Grab the relevant data for this map and wave
            SushiType sushiType = gameData.currentMap.waves[gameData.wave].sushiType;
            int count = gameData.currentMap.waves[gameData.wave].sushiCount;
            int hitpoints = gameData.currentMap.waves[gameData.wave].hitpoints;
            float timeBetweenSpawns = gameData.currentMap.waves[gameData.wave].timeBetweenSpawns;
            zombiesTotal = count;

            // Start spawning
            StartCoroutine(Spawn(sushiType, count, hitpoints, timeBetweenSpawns, spawnA, spawnB));
        }
    }

    public void ZombieDied(ZombieScript zombieScript)
    {
        if (zombiesAttachedToCrosshairA.Contains(zombieScript.gameObject))
        {
            zombiesAttachedToCrosshairA.Remove(zombieScript.gameObject);
            crosshairA.currentZombies--;
        } else if (zombiesAttachedToCrosshairB.Contains(zombieScript.gameObject))
        {
            zombiesAttachedToCrosshairB.Remove(zombieScript.gameObject);
            crosshairB.currentZombies--;
        }
    }

    IEnumerator Spawn(SushiType sushiType, int count, int hitpoints, float timeBetweenSpawns, Transform spawnA, Transform spawnB)
    {
        isSpawning = true;

        bool isSpawnA = true;
        for (int x = 0; x < count; x++)
        {
            Vector3 spawnPos = isSpawnA ? spawnA.position : spawnB.position;
            spawnPos.y = this.transform.position.y;
            GameObject zombie = Instantiate(zombiePrefab, transform);
            zombie.transform.position = spawnPos;

            Sprite sushiSprite = eyesSushi;

            switch (sushiType)
            {
                case SushiType.EYES:
                    sushiSprite = eyesSushi;
                    break;
                case SushiType.TEETH:
                    sushiSprite = teethSushi;
                    break;
                case SushiType.SQUID:
                    sushiSprite = squidSushi;
                    break;
                case SushiType.TENTACLES:
                    sushiSprite = tentaclesSushi;
                    break;
                default: break;
            }

            zombie.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sushiSprite;

            Vector3 force = new Vector3(UnityEngine.Random.Range(-100, 100), 0, UnityEngine.Random.Range(-100, 100));
            zombie.GetComponent<Rigidbody>().AddForce(force);
            zombie.GetComponent<ZombieScript>().maxHealth = hitpoints;
            zombie.GetComponent<ZombieScript>().health = hitpoints;

            if (isSpawnA)
            {
                zombiesAttachedToCrosshairA.Add(zombie);
                crosshairA.maxZombies++;
                crosshairA.currentZombies++;
            }

            if (!isSpawnA)
            {
                crosshairB.maxZombies++;
                crosshairB.currentZombies++;
                zombiesAttachedToCrosshairB.Add(zombie);
            }

            
            isSpawnA = !isSpawnA;
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        isSpawning = false;
        OnSpawnComplete();
    }

    IEnumerator OffenseCountdown()
    {
        do
        {
            offenseModeTimer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
            if (offenseModeTimer <= 0) break;
            if (state != HordeState.OFFENSIVE) break;
        } while (offenseModeTimer > 0);

        if (state != HordeState.DEFENSIVE) state = HordeState.NEUTRAL;
    }

    IEnumerator DefenceCountdown()
    {
        do
        {
            defenseModeTimer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
            if (defenseModeTimer <= 0) break;
            if (state != HordeState.DEFENSIVE) break;
        } while (defenseModeTimer > 0);

        if (state != HordeState.OFFENSIVE) state = HordeState.NEUTRAL;
    }

    public void MoveZombieTowardsTarget(GameObject zombie, Vector3 point)
    {
        Vector3 force = new Vector3(0, 0, 0);
        force = point - zombie.transform.position;
        force.Normalize();
        force.y = 0;
        force *= forceMultiplier;

        Vector3 wiggle = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
        wiggle *= wiggleMultiplier;
        force += wiggle;
        zombie.GetComponent<Rigidbody>().AddForce(force);
    }

    private void CalcOffensiveDefensive()
    {
        // If the state is not offensive
        if (state != HordeState.OFFENSIVE)
        {
            // If there are less than 5 seconds left (i.e. the timer is not maxed out)
            if (offenseModeTimer < 5)
            {
                // Increase the amount of time left, but at half speed
                // This way, using the mode for 1 seconds takes 2 seconds to regenerate
                offenseModeTimer += Time.deltaTime / 2;
            }

        }

        // Do the same for the defensive mode
        if (state != HordeState.DEFENSIVE)
        {
            if (defenseModeTimer < 5)
            {
                defenseModeTimer += Time.deltaTime / 2;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            state = HordeState.NEUTRAL;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) || (controller != null && controller.RightBumper.IsPressed))
        {
            if (offenseModeTimer > 0)
            {
                state = HordeState.OFFENSIVE;
                StartCoroutine(OffenseCountdown());
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha3) || (controller != null && controller.LeftBumper.IsPressed))
        {
            if (defenseModeTimer > 0)
            {
                state = HordeState.DEFENSIVE;
                StartCoroutine(DefenceCountdown());
            }
        }

        if (state == HordeState.NEUTRAL)
        {
            defensiveStat = baseStat;
            offensiveStat = baseStat;
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("DefensiveStat = " + defensiveStat + " OffensiveStat = " + offensiveStat);
            }
        }

        if (state == HordeState.OFFENSIVE)
        {
            defensiveStat = baseStat + 0.75f;
            offensiveStat = baseStat + 0.75f;
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("DefensiveStat = " + defensiveStat + " OffensiveStat = " + offensiveStat);
            }
        }

        if (state == HordeState.DEFENSIVE)
        {
            defensiveStat = baseStat - 0.75f;
            offensiveStat = baseStat - 0.75f;
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("DefensiveStat = " + defensiveStat + " OffensiveStat = " + offensiveStat);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalcOffensiveDefensive();

        MoveZombies();

        MoveCrosshairs();
    }

    private void MoveZombies()
    {
        int alive = 0;

        foreach (GameObject zombie in zombiesAttachedToCrosshairA)
        {
            if (zombie == null) return;
            alive++;
            MoveZombieTowardsTarget(zombie, crosshairA.transform.position);
        }

        foreach (GameObject zombie in zombiesAttachedToCrosshairB)
        {
            if (zombie == null) return;

            alive++;
            MoveZombieTowardsTarget(zombie, crosshairB.transform.position);
        }

        zombiesAlive = alive;
    }

    private void MoveCrosshairs()
    {

        Vector3 centerOfScreen = new Vector3(center.transform.position.x, this.transform.position.y, center.transform.position.z);

        // figure out how we will move both the crosshairs
        Vector3 moveByA = Vector3.zero;
        Vector3 moveByB = Vector3.zero;

        if (controller == null)
        {
            moveByA = (new Vector3(Input.GetAxis("HorizontalHordeA"), 0, Input.GetAxis("VerticalHordeA")));
            moveByB = (new Vector3(Input.GetAxis("HorizontalHordeB"), 0, Input.GetAxis("VerticalHordeB")));
        }
        else
        {
            moveByA = (new Vector3(controller.LeftStick.Vector.x, 0, controller.LeftStick.Vector.y));
            moveByB = (new Vector3(controller.RightStick.Vector.x, 0, controller.RightStick.Vector.y));
        }

        moveByA *= Time.deltaTime * crosshairSpeed;
        moveByB *= Time.deltaTime * crosshairSpeed;

        crosshairA.transform.position += moveByA;
        crosshairB.transform.position += moveByB;

        KeepCrosshairWithinLimits(crosshairA, centerOfScreen);
        KeepCrosshairWithinLimits(crosshairB, centerOfScreen);
    }

    private void KeepCrosshairWithinLimits(HordeCrosshairScript crosshair, Vector3 centerOfScreen)
    {
        if (Vector3.Distance(crosshairA.transform.position, centerOfScreen) > crosshairBounds) crosshair.transform.position -= (crosshair.transform.position - centerOfScreen) / 100;
    }
}