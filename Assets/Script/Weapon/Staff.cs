using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
	[SerializeField]
	private float attackCoolDown;
	[SerializeField]
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

	[SerializeField] public float MagicSpeed;
	[SerializeField] public float maxDistance = 10f;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
