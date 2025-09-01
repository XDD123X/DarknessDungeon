using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
	[SerializeField] public float arrowSpeed;
	[SerializeField] public float maxDistance = 10f;

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
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void Attack()
	{
		throw new NotImplementedException();
	}
}
