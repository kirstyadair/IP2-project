using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerIndicatorScript : MonoBehaviour
{
    public Animator animator;
    public TextMeshPro text;
    public TextMeshPro killedNumber;
    public SpriteRenderer sprite;

    public void Show(int playerNumber, Color color, int kills, float seconds)
    {
        this.text.text = "P" + playerNumber;
        this.killedNumber.text = kills.ToString();
        sprite.color = color;
        animator.Play("show");
        StartCoroutine(HideAfter(seconds));
    }

    private IEnumerator HideAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        animator.Play("hide");
    }
}
