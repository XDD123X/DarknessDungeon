using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	private GameObject player;
	private PlayerController playerController;
	[SerializeField] private float distanceFromPlayer = 1.5f;

	[SerializeField] public GameObject SwordPrelab;
	[SerializeField] public GameObject BowPreLab;
	[SerializeField] public GameObject StaffPrelab;

	[SerializeField] public GameObject slashPrelab;
	[SerializeField] public GameObject arrowPreLab;
	[SerializeField] public GameObject magicBallPrelab;

	private GameObject weaponAnim;
	private IWeapon weapon;
	private GameObject usingWeapon;

	private WeaponClass Iweapon;

	// Start is called before the first frame update
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		if (player != null)
		{
			playerController = player.GetComponent<PlayerController>();
		}

		if (playerController == null)
		{
			Debug.LogError("PlayerController not found on player.");
		}
	}

	// Update is called once per frame
	void Update()
	{
        if (Iweapon == null) return;
        if ((playerController.IsAttacking && Iweapon.weaponType == WeaponClass.WeaponType.Sword))
        {
            return ;
		}
		else
		{
	        MoveByMouse();
		}
	}

	public void MoveByMouse()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 0;
		Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(player.transform.position);
		Vector3 direction = (mousePos - playerScreenPoint).normalized;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		Vector3 newWeaponPosition = transform.parent.position + direction * distanceFromPlayer;
		transform.position = newWeaponPosition;

		transform.rotation = Quaternion.Euler(0, 0, angle);
	}

	public WeaponClass GetWeapon()
	{
		return this.Iweapon;
	}

	public void Attack()
	{
		if (Iweapon == null) return;

		switch (Iweapon.weaponType)
		{
			case WeaponClass.WeaponType.Sword:
				Sword sword = usingWeapon.GetComponent<Sword>();
				if (sword != null)
				{
					playerController.PlaySound("Sword");
					sword.Attack();
					Slash slashAnim = weaponAnim.GetComponent<Slash>();
					if (slashAnim != null)
					{
						slashAnim.sword = sword;
						slashAnim.Slashing();
					}
				}
				break;
			case WeaponClass.WeaponType.Bow:
				Bow bow = usingWeapon.GetComponent<Bow>();
				if (bow != null)
                {
                    playerController.PlaySound("Bow");
                    weaponAnim = Instantiate(arrowPreLab, transform.position, Quaternion.identity);
					weaponAnim.name = "Arrow";
					Arrow arrowAnim = weaponAnim.GetComponent<Arrow>();
					if (arrowAnim != null)
					{
						arrowAnim.bow = bow;
						arrowAnim.Shoot();
					}
				}
				break;
			case WeaponClass.WeaponType.Staff:
				Staff staff = usingWeapon.GetComponent<Staff>();
				if (staff != null)
                {
                    playerController.PlaySound("Staff");
                    weaponAnim = Instantiate(magicBallPrelab, transform.position, Quaternion.identity);
					weaponAnim.name = "Magic";
					Magic magicAnim = weaponAnim.GetComponent<Magic>();
					if (magicAnim != null)
					{
						magicAnim.staff = staff;
						magicAnim.Shoot();
					}
				}
				break;
			default:
				break;
		}
	}

	public void ChangeWeapon(WeaponClass thisIweapon)
	{
		Iweapon = thisIweapon;
		if (Iweapon == null)
		{
			RemoveAllChildren();
			return;
		}
		switch (Iweapon.weaponType)
		{
			case WeaponClass.WeaponType.Sword:
				RemoveAllChildren();
				usingWeapon = Instantiate(SwordPrelab, transform.position, Quaternion.identity);
				usingWeapon.transform.parent = this.transform;
				Sword sword = usingWeapon.GetComponent<Sword>();
					weaponAnim = Instantiate(slashPrelab, transform.position, Quaternion.identity);
				if (sword != null && weaponAnim !=null)
				{
					weaponAnim = Instantiate(slashPrelab, transform.position, Quaternion.identity);
					weaponAnim.name = "Slashing";
					weaponAnim.transform.parent = this.transform;
					weaponAnim.transform.localRotation = Quaternion.identity;
				}
				break;
			case WeaponClass.WeaponType.Bow:
                RemoveAllChildren();
				usingWeapon = Instantiate(BowPreLab, transform.position, Quaternion.identity);
				usingWeapon.transform.parent = this.transform;
				break;
			case WeaponClass.WeaponType.Staff:
                RemoveAllChildren();
				usingWeapon = Instantiate(StaffPrelab, transform.position, Quaternion.identity);
				usingWeapon.transform.parent = this.transform;
				break;
			default:
				RemoveAllChildren();
				break;

		}


	}

	private void RemoveAllChildren()
	{
		foreach (Transform child in transform)
		{
			GameObject.Destroy(child.gameObject);
		}
	}
}
