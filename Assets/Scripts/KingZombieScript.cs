using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingZombieScript : MonoBehaviour
{
    public HordeScript hordeScript;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Zombie" && collision.gameObject.transform.parent != this.transform)
        {
            hordeScript.AttachZombie(collision.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
