using InControl;
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
    public GameObject crosshair;
    public Vector3 centerPoint;
    public Animator crosshairAnimator;
    public AudioClip spawnSound;
    // Used for keeping stuff inside the screen
    Vector3 minScreenBounds;
    Vector3 maxScreenBounds;

    GameData gameData;

    // Where to locate the crosshair when we are respawning the horde
    public Transform crosshairDefault;

    [Header("Horde settings")]
    public float wiggleMultiplier;
    public float forceMultiplier;
    public float crosshairBounds;
    public float crosshairSpeed;

    [Header("Sushi sprites")]
    public Sprite eyesSushi;
    public Sprite tentacleSushi;

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

        GameObject selectiomDataObj = GameObject.Find("PlayerSelectionData");
        if (selectiomDataObj != null)
        {
            controller = selectiomDataObj.GetComponent<PlayerSelectionData>().GetHordeController();
        }
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
            Transform spawnPoint = gameData.currentMap.waves[gameData.wave].spawnPoint;


            // Find out how many zombies for this wave for this map
            SushiType sushiType = gameData.currentMap.waves[gameData.wave].sushiType;
            int count = gameData.currentMap.waves[gameData.wave].sushiCount;
            int hitpoints = gameData.currentMap.waves[gameData.wave].hitpoints;
            float timeBetweenSpawns = gameData.currentMap.waves[gameData.wave].timeBetweenSpawns;
            zombiesTotal = count;

            StartCoroutine(Spawn(sushiType, count, hitpoints, timeBetweenSpawns, spawnPoint));
        }
    }

    IEnumerator Spawn(SushiType sushiType, int count, int hitpoints, float timeBetweenSpawns, Transform spawnPoint)
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
            zombie.GetComponent<ZombieScript>().maxHealth = hitpoints;
            zombie.GetComponent<ZombieScript>().health = hitpoints;
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
        /*
        Ray ray1 = Camera.main.ScreenPointToRay(new Vector3(0, 0));
        Ray ray2 = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height));

        RaycastHit hit1;
        RaycastHit hit2;

        Physics.Raycast(ray1, out hit1, LayerMask.GetMask("Ground"));
        Physics.Raycast(ray2, out hit2, LayerMask.GetMask("Ground"));

        minScreenBounds = hit1.point;
        maxScreenBounds = hit2.point;
        */
        if (state != HordeState.OFFENSIVE)
        {
            if (offenseModeTimer < 5)
            {
                offenseModeTimer += Time.deltaTime / 2;
            }
            
        }

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

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (offenseModeTimer > 0)
            {
                state = HordeState.OFFENSIVE;
                StartCoroutine(OffenseCountdown());
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
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

        int alive = 0;
        foreach (Transform child in transform)
        {
            alive++;
            GameObject zombie = child.gameObject;
            Vector3 target = centerPoint;
            MoveZombieTowardsTarget(zombie, target);
        }

        zombiesAlive = alive;

        Vector3 centerOfScreen = new Vector3(center.transform.position.x, crosshair.transform.position.y, center.transform.position.z);
        centerPoint = crosshair.transform.position;

        Vector3 moveBy = Vector3.zero;

        // Move crosshair with either the controller or keyboard
        if (controller == null) moveBy += (new Vector3(Input.GetAxis("HorizontalHorde"), 0, Input.GetAxis("VerticalHorde")));// distanceFromCenter / reticleSlowness;
        else moveBy += (new Vector3(controller.LeftStick.Vector.x, 0, controller.LeftStick.Vector.y));

        moveBy *= Time.deltaTime;
        moveBy *= crosshairSpeed;

        crosshair.transform.position += moveBy;
        if (Vector3.Distance(crosshair.transform.position, centerOfScreen) > crosshairBounds) crosshair.transform.position -= (crosshair.transform.position - centerOfScreen) / 100;
        //crosshair.transform.position = new Vector3(Mathf.Clamp(crosshair.transform.position.x, minScreenBounds.x + 1, maxScreenBounds.x - 1), crosshair.transform.position.y, Mathf.Clamp(crosshair.transform.position.z, minScreenBounds.z + 1, maxScreenBounds.z - 1));
    }


    /*
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(centerPoint, 0.2f);
    }*/
}