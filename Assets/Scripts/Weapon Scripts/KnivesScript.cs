using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnivesScript : MonoBehaviour
{
    public GameObject knifePrefab;
    Vector3 direction = new Vector3(0, 0, 1);
    public float fireCooldown = 0.5f;
    float fireTimeout = 0;
    public GameObject reticle;
    GameData gameData;
    PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        gameData = GameObject.Find("GameData").GetComponent<GameData>();
    }

    // Update is called once per frame
    void Update()
    {
        fireTimeout -= Time.deltaTime;

        if (Input.GetMouseButton(0) == true && fireTimeout <= 0)
        {
            Vector3 bulletDirection = playerScript.reticlePos - transform.position;
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
