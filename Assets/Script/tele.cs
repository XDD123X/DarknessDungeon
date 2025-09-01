using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tele : MonoBehaviour
{
    public List<SpriteRenderer> runes;

    private Color curColor;
    private Color targetColor;

    private bool playerOnPlatform = false;
    private float timeOnPlatform = 0.0f;

    public float lerpSpeed = 0.0f;

    public int MapPortal;

    public bool isCanGoThrough;

    private InventoryManagement im;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerOnPlatform = true;
        }
    }

    private void teleToMap()
    {
        this.SetActive(false);
        im.GoToMap(MapPortal + 1);
        return;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            timeOnPlatform = 0;
            playerOnPlatform = false;
        }

    }
    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("UI").GetComponent<InventoryManagement>();
    }

    private void Update()
    {
        int Map = PlayerPrefs.GetInt("Map");

        if (Map >= MapPortal)
        {
            isCanGoThrough = true;
        }

        if (!isCanGoThrough) return;

        if (playerOnPlatform)
        {
            timeOnPlatform += Time.deltaTime;
            if (timeOnPlatform >= lerpSpeed)
            {
                teleToMap();
            }
        }
        else
        {
            timeOnPlatform = 0.0f;
        }
    }
}
