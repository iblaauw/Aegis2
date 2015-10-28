using UnityEngine;
using System.Collections;
using System;

public class Stats : MonoBehaviour
{
	public int Health { get; set; }
	public int MaxHealth { get; set; }

	public virtual void Hit(int amount)
	{
		Health -= amount;
		if (Health <= 0 && Death != null)
			Death();
	}

	public event Action Death;

	void Start()
	{
		Debug.Log("started!");
	}
}

/************** Just a note that inheritance works with GetComponent: *********************/
public class InvincibleStats : Stats
{
	public override void Hit (int amount)
	{
		return; //Invincible!! :)
	}
}