using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
    public GameObject buttonReturn;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        buttonReturn.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisplayButton()
    {
        buttonReturn.SetActive(true);
    }

    public void ActiveDisplay()
    {
        animator.SetTrigger("IsDead");
    }
}
