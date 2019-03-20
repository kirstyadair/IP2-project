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

    bool firing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
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
        } else
        {
            //transform.localScale -= new Vector3(ketchupBeamScript.ketchupSpread.x * 3, 0f, ketchupBeamScript.ketchupSpread.z * 2);
        }


        if (timeToDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Ground")
        {
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
            //transform.localScale = ketchupBeamScript.ketchupPuddle;
        }

        if (other.tag == "Zombie")
        {
            other.gameObject.GetComponent<ZombieScript>().Hit(gameData.ketchupDamage);
        }
    }
}
