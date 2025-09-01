using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour, IWeaponDmgDeal
{
	public Sword sword;
	private void Start()
	{
		this.gameObject.transform.Rotate(180, 0, 0);
		this.gameObject.SetActive(false);
	}
	private void Update()
	{
	}
	public void Slashing()
	{
		this.gameObject.SetActive(true);
		this.gameObject.transform.Rotate(180, 0, 0);
	}
	public void DoneSlash()
	{
		this.gameObject.SetActive(false);
	}

}
