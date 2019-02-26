using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    bool trapDeactivated = false;
    float timeToExplode;

    // Start is called before the first frame update
    void Start()
    {
        timeToExplode = 3.0f;
        transform.localScale = new Vector3(1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToExplode <= 0)
        {
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 5);
            Debug.Log("Number of hit enemies: " + hitEnemies.Length);
            foreach (Collider zombie in hitEnemies)
            {
                if (zombie.tag == "Zombie")
                {
                    zombie.gameObject.GetComponent<ZombieScript>().TryKill();
                }
            }
            gameObject.SetActive(false);
            trapDeactivated = true;
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Collided, press E to detonate");
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E pressed");
                if (!trapDeactivated)
                {
                    timeToExplode = 3.0f;
                    StartCoroutine(Grow());
                }
            }
        }
    }

    IEnumerator Grow()
    {
        do
        {
            transform.localScale += new Vector3(0.05f, 0.05f, 0);
            yield return new WaitForSeconds(Time.deltaTime);
            timeToExplode -= Time.deltaTime;
        }
        while (transform.localScale.x <= 1.5f);

        if (timeToExplode > 0)
        {
            StartCoroutine(Shrink());
        }
    }

    IEnumerator Shrink()
    {
        do
        {
            transform.localScale -= new Vector3(0.05f, 0.05f, 0);
            yield return new WaitForSeconds(Time.deltaTime);
            timeToExplode -= Time.deltaTime;
        }
        while (transform.localScale.x >= 1f);

        if (timeToExplode > 0)
        {
            StartCoroutine(Grow());
        }
    }
}
