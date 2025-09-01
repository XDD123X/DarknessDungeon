using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject  potion;

    public void DropItems() {
        int randomNum = Random.Range(1, 5);

        switch (randomNum)
        {
            case 0:
                break;
            case 1:
                break;
        }
    }
}
