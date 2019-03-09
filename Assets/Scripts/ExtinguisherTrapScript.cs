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

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        timeToDeactivate = 5.0f;
        StartCoroutine(PSActive());
        bCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // if active, enable the collider
        if (activated) bCollider.enabled = true;
        else bCollider.enabled = false;
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
            Debug.Log("Deactivating");
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

        if (timeToActivate <= 0)
        {
            Debug.Log("Activating");
            StartCoroutine(PSActive());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Zombie")
        {

        }

        if (other.tag == "Player")
        {

        }
    }
}
