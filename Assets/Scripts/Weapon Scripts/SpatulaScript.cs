using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatulaScript : MonoBehaviour
{
    float hitCountdown = 0.2f;
    public float spatulaRadiusHit;
    PlayerScript playerScript;
    GameData gameData;
    // Start is called before the first frame update
    void Start()
    {
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
        playerScript = GetComponent<PlayerScript>();
        playerScript.OnFire += Fire;
    }

    void Fire()
    {
        // If the cooldown isn't at 0, don't allow a hit
        if (hitCountdown > 0) return;

        // Create an array of hit enemy colliders to apply damage to
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, spatulaRadiusHit);
        foreach (Collider sushi in hitEnemies)
        {
            if (sushi.tag == "Zombie")
            {
                if (sushi.gameObject.GetComponent<ZombieScript>().Hit(gameData.spatulaDamage)) playerScript.stats.kills++;
            }
        }
        // Reset the cooldown, which decreases in Update()
        hitCountdown = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        hitCountdown -= Time.deltaTime;
    }
}
