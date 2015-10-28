using UnityEngine;
using System.Collections;
using Aegis;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public int startX;
	public int startY;
	public float speed;

	private SquareMovement moveControl;
	private Targeter targeter = null;

	// Use this for initialization
	void Start () {
		moveControl = GetComponent<SquareMovement>();
		if (moveControl == null)
			throw new MissingComponentException("Player does not have SquareMovement.");

		moveControl.SetPosition(startX, startY);

		this.gameObject.AddComponent<InvincibleStats>();
		Stats stats = this.GetComponent<Stats>();
		Debug.Log(stats);
	}
	
	// Update is called once per frame
	void Update () {
		UpdateTarget();
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

	private void UpdateTarget()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (this.targeter == null)
			{
				this.targeter = CreateTargeter();
			}
			this.targeter.Show();
		}
		
		if (Input.GetMouseButtonDown(1))
		{
			if (this.targeter != null)
				this.targeter.Hide();
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

		t.Selected += OnTargetSelect;

		return t;
	}

	private void OnTargetSelect(IList<IGridSquare> squares)
	{
		Debug.Log("Fired!");
	}
}
