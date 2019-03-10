using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguisherTrapScript : MonoBehaviour
{
    // Variables
    float timeToDeactivate = 5.0f;
    float timeToActivate = 5.0f;
    bool activated = true;

    public ParticleSystem ps;
    BoxCollider bCollider;
    CapsuleCollider cCollider;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        timeToDeactivate = 5.0f;
        StartCoroutine(PSActive());
        bCollider = GetComponent<BoxCollider>();
        cCollider = GetComponent<CapsuleCollider>();
        bCollider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PSActive()
    {
        activated = true;
        if (!ps.isPlaying)
        {
            ps.Play();
        }

        timeToDeactivate = 5.0f;
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
        activated = false;
        if (ps.isPlaying)
        {
            ps.Stop();
        }

        timeToActivate = 5.0f;
        do
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timeToActivate -= Time.deltaTime;

        } while (timeToActivate >= 0);
    }

    void OnTriggerStay(Collider other)
    { 
        if (activated)
        {
            if (other.tag == "Zombie")
            {
                Rigidbody zombieRB = other.GetComponent<Rigidbody>();
                Vector3 dir = (bCollider.center - transform.position);
                zombieRB.AddForce(dir * 2, ForceMode.Impulse);
            }

            if (other.tag == "Player")
            {
                Vector3 dir = (bCollider.center - transform.position);
                other.GetComponent<PlayerScript>().pushForce = dir * 2;
            }
        }
        
        if (!activated)
        {
            if (other.tag == "Player")
            {
                Debug.Log("Collision");
                if (Input.GetKey(KeyCode.R))
                {
                    Debug.Log("R pressed");
                    StartCoroutine(PSActive());
                }
            }
        }
    }
}
