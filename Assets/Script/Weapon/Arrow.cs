using Assets.Scripts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, IWeaponDmgDeal
{
	private Vector3 startPosition;
	[SerializeField] public float lifetime = 3f;

	public Bow bow;

	// Start is called before the first frame update
	void Start()
	{
		startPosition = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		if (Vector3.Distance(startPosition, transform.position) > bow.maxDistance)
		{
			Destroy(gameObject);
		}
	}

	public void Shoot()
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = 0f;
		Vector3 direction = (mousePosition - transform.position).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, angle);

		Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
		rb.velocity = direction * bow.arrowSpeed;
	}

	public void DoneDealDmg(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			return;
		}
		Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
		rb.velocity = Vector2.zero;
		Collider2D c = this.GetComponent<Collider2D>();
		c.enabled = false;
		if(collision.GetComponent<EnemyHealth>() != null)
		{
			//dinh dinh vao quai n ko dc
			Destroy(gameObject);
		}
		Destroy(gameObject, lifetime);
	}
}
