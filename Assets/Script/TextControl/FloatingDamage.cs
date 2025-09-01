using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    private TextMeshProUGUI displayText;
    private Animator animator;
    private ObjectPool op;
    // Start is called before the first frame update
    void Awake()
    {
        displayText = this.gameObject.GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
        op = GameObject.Find("ObjectPool").GetComponent<ObjectPool>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Show(float damage)
    {
        displayText.text = damage.ToString("0.0");
        animator.SetTrigger("display");
    }

    public void Break()
    {
        op.ReturnToPool("floatingdamage", this.gameObject);
    }
}
