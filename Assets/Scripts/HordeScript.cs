using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HordeScript : MonoBehaviour
{
    public GameObject zombiePrefab;
    public GameObject crosshair;
    public Vector2 centerPoint;
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

     // 0 = all zombies move towards reticule, 1 = all zombies move independently
    // Start is called before the first frame update
    void Start()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
    }

    public void MoveZombieTowardsTarget(GameObject zombie, Vector2 point)
    {
        //xSum += zombie.transform.position.x;
       // ySum += zombie.transform.position.y;

        Vector2 force = new Vector2(0, 0);
        force = point - (Vector2)zombie.transform.position;
        force.Normalize();
        force *= forceMultiplier;

        Vector2 wiggle = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        wiggle *= wiggleMultiplier;
        force += wiggle;
        //force /= 10f;
        zombie.GetComponent<Rigidbody2D>().AddForce(force);

  
        if (force.x < -30f) zombie.GetComponent<SpriteRenderer>().flipX = true;
        if (force.x > 30f) zombie.GetComponent<SpriteRenderer>().flipX = false;
    }

    // Update is called once per frame
    void Update()
    {
        minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

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
            // TODO: force zombie through hole if it gets stuck (use an empty gameobject near the spawn point to denate where to spawn zombie)

            MoveZombieTowardsTarget(zombie, target);

            // Only restrict zombies within boundary if they are in the bui
            if (!targettedToEntryPoint) zombie.transform.position = new Vector3(Mathf.Clamp(zombie.transform.position.x, minScreenBounds.x + 1, maxScreenBounds.x - 1), Mathf.Clamp(zombie.transform.position.y, minScreenBounds.y + 1, maxScreenBounds.y - 1), zombie.transform.position.z);
  
        }

        //centerPoint = new Vector2(xSum / zombies.Count, ySum / zombies.Count);
        centerPoint = crosshair.transform.position;

        // Slow down crosshair movement depending on how far from center
        //float distanceFromCenter = (crosshair.transform.position - gameData.currentMap.centerPoint.position).magnitude;
        crosshair.transform.position += (new Vector3(Input.GetAxis("HorizontalHorde"), Input.GetAxis("VerticalHorde"), 0)) / 10;// distanceFromCenter / reticleSlowness;
        crosshair.transform.position = new Vector3(Mathf.Clamp(crosshair.transform.position.x, minScreenBounds.x + 1, maxScreenBounds.x - 1), Mathf.Clamp(crosshair.transform.position.y, minScreenBounds.y + 1, maxScreenBounds.y - 1), crosshair.transform.position.z);
        /*
        Vector2 position = crosshair.transform.position;
        if (position.x < -Screen.width/2) position.x = -Screen.width/2;
        if (position.y < -Screen.height/2) position.y = -Screen.height/2;
        if (position.x > Screen.width) position.x = Screen.width;
        if (position.y > Screen.height) position.y = Screen.height;
        crosshair.transform.position = position;*/
        //crosshair.transform.position = centerPoint;

        /*
        else if (hordeType == 1)
        {
            crosshair.SetActive(false);
            cozynessSetting.SetActive(true);

            // Direction from 
            Vector2 direction = new Vector2(Input.GetAxis("HorizontalHorde"), Input.GetAxis("VerticalHorde"));
            direction *= forceMultiplier;

            foreach (Transform child in transform)
            {
                Vector2 attraction = new Vector2(0, 0);
                // Calculate attraction to other zombies

                GameObject nearestZomb = FindClosestZombieTo(child);

                Vector2 wiggle = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                wiggle *= wiggleMultiplier;

                if (nearestZomb != null) attraction = (nearestZomb.transform.position - child.position) * cozynessMultiplier;

                Vector2 force = direction + attraction + wiggle;

                Vector2 line = force;
                line.Normalize();
                line /= 2;

                Debug.DrawLine(child.transform.position, (Vector2)child.transform.position + line, new Color(1, 1, 1, 0.5f));

                child.gameObject.GetComponent<Rigidbody2D>().AddForce(force);

                Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
                Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

                child.position = new Vector3(Mathf.Clamp(child.position.x, minScreenBounds.x + 1, maxScreenBounds.x - 1), Mathf.Clamp(child.position.y, minScreenBounds.y + 1, maxScreenBounds.y - 1), child.position.z);
            }
        }*/

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
        zomb.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.1f, 0.1f));
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