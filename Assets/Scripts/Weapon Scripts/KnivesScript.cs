using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnivesScript : MonoBehaviour
{
    public GameObject knifePrefab;
    public float fireCooldown = 0.5f;
    float fireTimeout = 0;
    public GameObject reticle;
    GameData gameData;
    PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        playerScript.OnFire += Shoot;
    }

    // Update is called once per frame
    void Update()
    {
        fireTimeout -= Time.deltaTime;
    }

    public void Shoot()
    {
        // Don't shoot if the cooldown isn't finished
        if (fireTimeout > 0) return;
        // Don't shoot while we're preparing
        if (gameData.state == GameState.PREP) return; 

        // Find the direction, but reset the Y position to prevent firing over the sushi
        Vector3 bulletDirection = playerScript.reticlePos - transform.position;
        bulletDirection.Normalize();
        bulletDirection.y = 0;

        // Instantitate the knife 
        GameObject knife = (GameObject)Instantiate(knifePrefab, transform.position, Quaternion.identity);
        // Add relevant damage stats to the attached script
        knife.GetComponent<ThrowingKnifeScript>().stats = playerScript.stats;
        // Fire the knife in the required direction
        knife.GetComponent<Rigidbody>().AddForce(bulletDirection * 2, ForceMode.Impulse);
        knife.transform.up = bulletDirection;
        // fireCooldown acts as the maximum fire timeout, assigned in the inspector as 0.5 seconds
        fireTimeout = fireCooldown;
    }


}
