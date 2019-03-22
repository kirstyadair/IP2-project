using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScript : MonoBehaviour
{
    public PlayerScript player;

    public Image filler;
    public Animator animator;
    public float lerpSpeed = 0.1f;

    bool dead = false;

    float max = 1f;
    float start = 1f;
    float t = 0;

    public void Initialize(PlayerScript player)
    {
        this.player = player;
        filler.color = player.playerColor;
        player.OnHealthChanged += OnHealthChanged;
        Lerp(1f);
    }

    public void FixedUpdate()
    {
        t += lerpSpeed;

        if (t >= 1f)
        {
            filler.fillAmount = max;
        } else
        {
            filler.fillAmount = Mathf.Lerp(start, max, t * t * t);
        }
    }

    public void OnHealthChanged(float health)
    {
        if (health <= 0)
        {
            animator.Play("dead");
            Lerp(0f);
            dead = true;
        } else
        {
            Lerp(health / player.maxHealth);

            if (dead)
            {
                dead = false;
                animator.Play("respawn");
            }
        }
    }

    public void Lerp(float to)
    {
        start = filler.fillAmount;
        max = to;
        t = 0;
    }
}
