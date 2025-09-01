using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackwhenstand : MonoBehaviour
{
    public GameObject attackObject;

    public List<SpriteRenderer> runes;
    public float lerpSpeed;

    private Color curColor;
    private Color targetColor;

    private bool playerOnPlatform = false;
    private float timeOnPlatform = 0.0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerOnPlatform = true;
            targetColor = new Color(1, 1, 1, 1);
        }
    }

    private void ActivateAttackObject()
    {
        attackObject.SetActive(true);
        playerOnPlatform = false;
        timeOnPlatform = 0.0f;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            targetColor = new Color(1, 1, 1, 0);
            timeOnPlatform = 0;
            playerOnPlatform = false;
        }

    }
    private void Start()
    {
        attackObject = GameObject.FindGameObjectWithTag("aoe");
        attackObject.SetActive(false);
    }

    private void Update()
    {
        curColor = Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);

        foreach (var r in runes)
        {
            r.color = curColor;
        }

        if (playerOnPlatform)
        {
            timeOnPlatform += Time.deltaTime;
            if (timeOnPlatform >= lerpSpeed)
            {
                ActivateAttackObject();
            }
        }
        else
        {
            timeOnPlatform = 0.0f;
        }
    }
}
