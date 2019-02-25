using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HordeScript : MonoBehaviour
{
    public GameObject zombiePrefab;
    public GameObject crosshair;
    public Vector3 centerPoint;
    public Animator crosshairAnimator;

    // Where to locate the crosshair when we are respawning the horde
    public Transform crosshairDefault;

    //public Vector2 offset;

    public float wiggleMultiplier;
    public float forceMultiplier;
    public float cozynessMultiplier; // Only used for horde type B
    public float closenessBetweenZombies;
    public int hordeType = 0;

    public Text wiggleText;
    public Text forceText;
    public Text cozynessText;
    public Slider hordeTypeSlider;
    public GameObject cozynessSetting; // Hide when we're on type A cause it doesn't apply then

    // Used for keeping stuff inside the screen
    Vector3 minScreenBounds;
    Vector3 maxScreenBounds;

    public float reticleSlowness; // How slow the reticle moves
    GameData gameData;

    // If this is true, we are currently spawning the horde and bringing it to the cursor (so disable controls)
    bool resettingHorde = false;

     // 0 = all zombies move towards reticule, 1 = all zombies move independently
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
            ResetHordeCursor();
            resettingHorde = true;
            // Kill off all old zombies
            foreach (Transform child in transform) child.gameObject.GetComponent<ZombieScript>().Kill();

            // Find out how many zombies for this wave for this map
            int count = gameData.currentMap.waves[gameData.wave].sushiCount;

            for (int x = 0; x < count; x++)
            {
                Vector2 spawnPos = crosshairDefault.position + new Vector3(Random.Range(10, 20), 0, Random.Range(-10, 10));
                GameObject zombie = Instantiate(zombiePrefab, transform);
                zombie.transform.position = spawnPos;
            }

            StartCoroutine(ResumeCrosshairControl());
        }
    }

    void ResetHordeCursor()
    {
        crosshair.transform.position = crosshairDefault.position;
        crosshairAnimator.SetBool("enabled", false);
    }

    IEnumerator ResumeCrosshairControl()
    {
        yield return new WaitForSeconds(3f);
        crosshairAnimator.SetBool("enabled", true);
        resettingHorde = false;
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
        //maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        foreach (Transform child in transform)
        {
            GameObject zombie = child.gameObject;
            Vector3 target = centerPoint;
            bool targettedToEntryPoint = false;

            // if this zombie has not entered the building yet, move towards entry point
            if (gameData.state == GameState.PLAY && !zombie.GetComponent<ZombieScript>().hasEnteredBuilding)
            {
                target = gameData.currentEntryPoint.windowCollider.transform.position;
                targettedToEntryPoint = true;
            }

            MoveZombieTowardsTarget(zombie, target);

            // Only restrict zombies within boundary if they are in the building
           // if (!targettedToEntryPoint && !resettingHorde) zombie.transform.position = new Vector3(Mathf.Clamp(zombie.transform.position.x, minScreenBounds.x + 1, maxScreenBounds.x - 1), Mathf.Clamp(zombie.transform.position.y, minScreenBounds.y + 1, maxScreenBounds.y - 1), zombie.transform.position.z);
        }

        centerPoint = crosshair.transform.position;

        // Move crosshair if not resetting horde
        if (!resettingHorde) crosshair.transform.position += (new Vector3(Input.GetAxis("HorizontalHorde"), 0, Input.GetAxis("VerticalHorde"))) / 10;// distanceFromCenter / reticleSlowness;
       // crosshair.transform.position = new Vector3(Mathf.Clamp(crosshair.transform.position.x, minScreenBounds.x + 1, maxScreenBounds.x - 1), Mathf.Clamp(crosshair.transform.position.y, minScreenBounds.y + 1, maxScreenBounds.y - 1), crosshair.transform.position.z);

        wiggleText.text = "Wiggle (" + wiggleMultiplier + ")";
        forceText.text = "Force (" + forceMultiplier + ")";
        cozynessText.text = "Cozyness (" + cozynessMultiplier + ")";
    }
    public GameObject FindClosestZombieTo(Transform position)
    {
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;
        foreach (Transform child in transform)
        {
            if (child.position == position.position) continue; // I will be mad if the closest zombie is itself

            float distance = Vector2.Distance(child.position, position.position);

            if (distance < closenessBetweenZombies) continue;
            if (distance < closestDistance)
            {
                closest = child.gameObject;
                closestDistance = distance;
            }
        }

        if (closestDistance < closenessBetweenZombies) closest = null; // Don't return a closest zombie if it's already super close to one

        return closest;
    }

    public void SpawnZombie()
    {
        GameObject zomb = Instantiate<GameObject>(zombiePrefab);
        zomb.transform.localPosition = new Vector2(0, 0);
        zomb.transform.parent = this.transform;
        zomb.transform.GetComponent<Rigidbody>().AddForce(new Vector3(0.1f, 0, 0.1f));
    }

    public void DespawnZombie()
    {
        if (transform.GetChild(0) != null) Destroy(transform.GetChild(0).gameObject);
    }

    public void IncreaseWiggle()
    {
        wiggleMultiplier++;
    }

    public void DecreaseWiggle()
    {
        wiggleMultiplier--;
    }

    public void IncreaseForce()
    {

        forceMultiplier++;
    }

    public void DecreaseForce()
    {
        forceMultiplier--;
    }

    public void IncreaseCozyness()
    {
        cozynessMultiplier += 0.5f;
    }

    public void DecreaseCozyness()
    {
        cozynessMultiplier -= 0.5f;
    }

    /*
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(centerPoint, 0.2f);
    }*/
}