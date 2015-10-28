using UnityEngine;
using System.Collections;
using Aegis;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public int startX;
	public int startY;
	public float speed;

	private SquareMovement moveControl;
	private Projectile projectile;

	// Use this for initialization
	void Start () {
		moveControl = GetComponent<SquareMovement>();
		if (moveControl == null)
			throw new MissingComponentException("Player does not have SquareMovement.");

		moveControl.SetPosition(startX, startY);

		this.gameObject.AddComponent<InvincibleStats>();
		Stats stats = this.GetComponent<Stats>();

		this.projectile = CreateProjectile();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateProjectile();
		UpdateMove();
	}

	private void UpdateMove()
	{
		if (moveControl.IsMoving)
			return;
		
		IGridSquare current = moveControl.Position;
		IGridSquare target = null;
		
		if (Input.GetKey(KeyCode.UpArrow))
			target = current.GetNext(Direction.UP);
		else if (Input.GetKey(KeyCode.DownArrow))
			target = current.GetNext(Direction.DOWN);
		else if (Input.GetKey(KeyCode.LeftArrow))
			target = current.GetNext(Direction.LEFT);
		else if (Input.GetKey(KeyCode.RightArrow))
			target = current.GetNext(Direction.RIGHT);
		
		if (target != null)
		{
			moveControl.StartMove(target, this.speed);
		}
	}

	private void UpdateProjectile()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			this.projectile.BeginTargeting();
		}
	}

	private Targeter CreateTargeter()
	{
		Targeter t = Targeter.Create();
		t.Add(0,0);
		t.Add(0,1);
		t.Add(0,-1);
		t.Add(1,0);
		t.Add(-1,0);

		return t;
	}

	private Projectile CreateProjectile()
	{
		Projectile proj = new Projectile(CreateTargeter(), null);
		return proj;
	}
}
