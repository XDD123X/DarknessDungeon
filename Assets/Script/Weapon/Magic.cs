using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Magic : MonoBehaviour, IWeaponDmgDeal
{
	private Vector3 startPosition;
	[SerializeField] public float lifetime = 3f;

	[SerializeField] public float magic;
	[SerializeField] public float maxDistance = 10f;
	private Animator animator;

    public Staff staff;
	// Start is called before the first frame update
	void Start()
    {
		animator = GetComponent<Animator>();
		startPosition = transform.position;
	}

	// Update is called once per frame
	void Update()
    {
		if (Vector3.Distance(startPosition, transform.position) > staff.maxDistance)
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
		rb.velocity = direction * staff.MagicSpeed;
	}

	public void DoneDealDmg(Collider2D collision)
	{
		if (collision.tag == "Player")
        {
            return;
        }
        else
		{
            Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            animator.SetTrigger("hit");
		}
	}
	public void selfDestroy()
	{
        Destroy(gameObject); 
	}
}
