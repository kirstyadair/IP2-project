using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguisherTrapScript : MonoBehaviour
{
    // Variables
    float timeToDeactivate = 10.0f;
    float timeToActivate = 60.0f;
    bool activated = true;
    bool playerHurt = false;
    bool canReactivate = true;

    public ParticleSystem ps;
    //public ParticleSystem sparksPS;
    BoxCollider bCollider;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        timeToDeactivate = 10.0f;
        StartCoroutine(PSActive());
        bCollider = GetComponent<BoxCollider>();
        bCollider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PSActive()
    {
        activated = true;
        canReactivate = false;
        if (!ps.isPlaying)
        {
            //sparksPS.Stop();
            ps.Play();
        }

        timeToDeactivate = 10.0f;
        do
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timeToDeactivate -= Time.deltaTime;

        } while (timeToDeactivate >= 0);

        if (timeToDeactivate <= 0)
        {
            StartCoroutine(PSInactive());
        }
    }

    IEnumerator PSInactive()
    {
        if (ps.isPlaying)
        {
            ps.Stop();
            //sparksPS.Play();
        }
        activated = false;

        // trap will not be able to activate again for the next 60 seconds
        timeToActivate = 5.0f;
        do
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timeToActivate -= Time.deltaTime;

        } while (timeToActivate >= 0);
        //sparksPS.Stop();
        canReactivate = true;
    }


    
    void OnTriggerStay(Collider other)
    { 
        // if the trap is activated, push away and apply damage
        if (activated)
        {
            if (other.tag == "Zombie")
            {
                Rigidbody zombieRB = other.GetComponent<Rigidbody>();
                Vector3 dir = (bCollider.center - transform.position);
                zombieRB.AddForce(dir, ForceMode.Impulse);
                other.GetComponent<ZombieScript>().Hit();
            }

            if (other.tag == "Player")
            {
                Vector3 dir = (bCollider.center - transform.position);
                other.GetComponent<PlayerScript>().pushForce = dir * 2;
                if (!playerHurt)
                {
                    playerHurt = true;
                    other.GetComponent<PlayerScript>().numOfHits--;
                }
            }
        }
        
        // if the trap is not activated, allow player to activate by pressing E
        if (!activated && canReactivate)
        {
            if (other.tag == "Player")
            {
                Debug.Log("Collision");
                if (Input.GetKey(KeyCode.E))
                {
                    Debug.Log("E pressed");
                    StartCoroutine(PSActive());
                }
            }
        }
    }
}
