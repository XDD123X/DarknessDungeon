using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    private Animator myAnimator;

	private float attackCoolDown;
	private float attackDamage;

	public float AttackCoolDown
	{
		get { return attackCoolDown; }
		set { attackCoolDown = value; }
	}

	public float AttackDamage
	{
		get { return attackDamage; }
		set { attackDamage = value; }
	}

	private void Awake() {
        myAnimator = GetComponent<Animator>();
	}

    private void Start() {
	}

    private void Update() {
    }

    public void Attack() {
		myAnimator.SetTrigger("Attack");
	}




}
