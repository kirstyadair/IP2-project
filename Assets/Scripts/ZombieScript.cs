using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    public bool hasEnteredBuilding = true;
    public bool isBeingYeeted = false;


    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Animator>().speed = UnityEngine.Random.Range(0.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryKill()
    {
        if (hasEnteredBuilding && !isBeingYeeted) Kill();
    }

    public void Kill()
    {
        GetComponent<Animator>().SetTrigger("death");
        GetComponent<ParticleSystem>().Emit(10);
        Destroy(gameObject, 2f);
    }
}
