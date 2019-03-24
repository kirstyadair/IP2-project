using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupScript : MonoBehaviour
{
    float timeToDestroy = 3.0f;
    Rigidbody rb;
    public GameObject audioObject;
    public KetchupBeamScript ketchupBeamScript;
    GameData gameData;
    public PlayerStats stats;

    bool firing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ketchupBeamScript.playerScript.OnFire += Fire;
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
    }

    void Fire()
    {
        firing = true;
    }


    void Update()
    {
        timeToDestroy -= Time.deltaTime;

        if (firing)
        {
            transform.localScale += ketchupBeamScript.ketchupSpread;
            firing = false;
        }


        if (timeToDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameData == null) return;
        if (other.name == "Ground")
        {
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
        }

        if (other.tag == "Zombie" && other.gameObject.GetComponent<ZombieScript>() != null)
        {
            if (other.gameObject.GetComponent<ZombieScript>().Hit(gameData.ketchupDamage)) stats.kills++;
        }
    }
}
