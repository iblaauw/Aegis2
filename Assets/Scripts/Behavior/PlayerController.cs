using System.Collections;
using System.Collections.Generic;
using Aegis;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public int startX;
	public int startY;
	public float speed;

	public GameObject projectilePrefab;

	private SquareMovement moveControl;
	private Projectile projectile;

	// Use this for initialization
	void Start () {
		moveControl = this.GetComponentForce<SquareMovement>();
		moveControl.SetPosition(startX, startY);
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
			if (this.projectile == null)
			{
				this.projectile = CreateProjectile();
				this.projectile.Finished += () => 
				{
					this.projectile = null;
				};
				this.projectile.BeginTargeting();
			}
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
		Projectile proj = new Projectile(CreateTargeter(), this.moveControl, this.projectilePrefab);
		return proj;
	}
}
