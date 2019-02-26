using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupBeamScript : MonoBehaviour
{
    public GameObject ketchupPrefab;
    Vector3 direction1 = new Vector3(0, 0, 1);
    Vector3 direction2 = new Vector3(0.1f, 0, 1);
    Vector3 direction3 = new Vector3(-0.1f, 0, 1);
    bool fireAgain = true;
    float timeUntilDisappear = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (fireAgain)
            {
                GameObject ketchupDir1 = (GameObject)Instantiate(ketchupPrefab, transform.position, Quaternion.identity);
                ketchupDir1.GetComponent<Rigidbody>().AddForce(direction1, ForceMode.Impulse);
                StartCoroutine(StartRapidFire());
            }
        }
    }

    IEnumerator StartRapidFire()
    {
        fireAgain = false;
        yield return new WaitForSeconds(0.1f);
        fireAgain = true;
    }
}
