﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public SpriteRenderer sprt;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Animator>().speed = UnityEngine.Random.Range(0.5f, 1.5f);    
    }

    // Update is called once per frame
    void Update()
    {
        Color clr = sprt.color;
        clr.g = (health / maxHealth);
        clr.b = (health / maxHealth);
        sprt.color = clr;
    }

    public void Hit(int hp)
    {
        health -= hp;
        if (health <= 0) Kill();
    }

    public void Kill()
    {
        GetComponent<Animator>().SetTrigger("death");
        GetComponent<ParticleSystem>().Emit(10);
        Destroy(gameObject, 2f);
    }
}
