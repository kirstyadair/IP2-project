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

    public float reticleSlowness; // How slow the reticle moves
    GameData gameData;

    // Where to locate the crosshair when we are respawning the horde
    public Transform crosshairDefault;

    [Header("Horde settings")]
    public float wiggleMultiplier;
    public float forceMultiplier;
    public float crosshairBounds;

    [Header("Sushi sprites")]
    public Sprite eyesSushi;
    public Sprite tentacleSushi;

    public int zombiesAlive;
    public int zombiesTotal;
    public bool isSpawning = false;
    public Transform center;

    
    [Header("Offensive/Defensive stats")]
    public float defensiveStat = 1;
    public float offensiveStat = 1;
    public HordeState state;
    public const int baseStat = 1;

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
            defensiveStat = baseStat + 0.5f;
            offensiveStat = baseStat - 0.5f;
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("DefensiveStat = " + defensiveStat + " OffensiveStat = " + offensiveStat);
            }
        }

        if (state == HordeState.DEFENSIVE)
        {
            defensiveStat = baseStat - 0.5f;
            offensiveStat = baseStat + 0.5f;
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
        // Move crosshair and constrain within world
        if (controller == null) crosshair.transform.position += (new Vector3(Input.GetAxis("HorizontalHorde"), 0, Input.GetAxis("VerticalHorde"))) / 10;// distanceFromCenter / reticleSlowness;
        else crosshair.transform.position += (new Vector3(controller.LeftStick.Vector.x, 0, controller.LeftStick.Vector.y)) / 10;
        if (Vector3.Distance(crosshair.transform.position, centerOfScreen) > crosshairBounds) crosshair.transform.position -= (crosshair.transform.position - centerOfScreen) / 100;
        //crosshair.transform.position = new Vector3(Mathf.Clamp(crosshair.transform.position.x, minScreenBounds.x + 1, maxScreenBounds.x - 1), crosshair.transform.position.y, Mathf.Clamp(crosshair.transform.position.z, minScreenBounds.z + 1, maxScreenBounds.z - 1));
    }


    /*
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(centerPoint, 0.2f);
    }*/
}