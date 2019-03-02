using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnivesScript : MonoBehaviour
{
    public GameObject knifePrefab;
    Vector3 direction = new Vector3(0, 0, 1);
    public float fireCooldown;
    float fireTimeout = 0;
    public GameObject reticle;
    Vector3 reticlePos;
    GameData gameData;

    // Start is called before the first frame update
    void Start()
    {
        // this is just for the AI script, change this when actual players are added
        reticlePos = new Vector3(0, 0, 0);

        gameData = GameObject.Find("GameData").GetComponent<GameData>();
    }

    // Update is called once per frame
    void Update()
    {
        fireTimeout -= Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && fireCooldown <= 0)
        {
            GameObject knife = (GameObject)Instantiate(knifePrefab, transform.position, Quaternion.identity);
            knife.AddComponent<ThrowingKnifeScript>();
            knife.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
            fireCooldown = 0.5f;
        }

        if (Input.GetMouseButton(0) == true && fireTimeout <= 0)
        {
            Vector3 bulletDirection = reticlePos - transform.position;
            bulletDirection.Normalize();

            Shoot(bulletDirection);

            fireTimeout = fireCooldown;
        }
    }

    public void Shoot(Vector3 direction)
    {
        if (fireTimeout > 0) return;
        if (gameData.state == GameState.PREP) return; // Don't shoot whilst we're preparing
        GameObject knife = (GameObject)Instantiate(knifePrefab, transform.position, Quaternion.identity);

        knife.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
        knife.transform.up = direction;
    }


}
