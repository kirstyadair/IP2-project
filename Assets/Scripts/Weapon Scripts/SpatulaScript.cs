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
        if (hitCountdown > 0) return;

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, spatulaRadiusHit);
        foreach (Collider sushi in hitEnemies)
        {
            if (sushi.tag == "Zombie")
            {
                sushi.gameObject.GetComponent<ZombieScript>().Hit(gameData.spatulaDamage);
            }
        }
        hitCountdown = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        hitCountdown -= Time.deltaTime;
    }
}
