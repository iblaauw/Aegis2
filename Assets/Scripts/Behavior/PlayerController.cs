using UnityEngine;
using System.Collections;
using Aegis;

public class PlayerController : MonoBehaviour {

	public int startX;
	public int startY;
	public float speed;

	/*private int xpos, ypos;
	private float currentSpeed;
	private IGridSquare moveTarget;*/

	private SquareMovement moveControl;

	// Use this for initialization
	void Start () {
		/*this.xpos = startX;
		this.ypos = startY;

		SnapToGrid();
		currentSpeed = 0;*/

		moveControl = GetComponent<SquareMovement>();
		if (moveControl == null)
			throw new MissingComponentException("Player does not have SquareMovement.");

		moveControl.SetPosition(startX, startY);
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.C))
		{
			Targeter t = Targeter.Create();
			t.Add(0,0);
			t.Add(0,1);
			t.Add(0,-1);
			t.Add(1,0);
			t.Add(-1,0);
			t.Show();
		}


		/*if (moveTarget != null)
			return;*/
		if (Input.GetKeyDown(KeyCode.Space))
		{
			HighlightFollow.Create(0,0);
			HighlightFollow.Create(0,1);
			HighlightFollow.Create(0,-1);
			HighlightFollow.Create(1,0);
			HighlightFollow.Create(-1,0);
		}

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

	/*void FixedUpdate()
	{
		if (currentSpeed != 0)
		{
			Vector2 offset = moveTarget.Center - (Vector2)this.transform.position;

			if (offset.magnitude < currentSpeed)
			{
				transform.position = moveTarget.Center;
				currentSpeed = 0;
				xpos = moveTarget.XIndex;
				ypos = moveTarget.YIndex;
				moveTarget = null;
			}
			else
			{
				Vector3 dir = offset.normalized;
				this.transform.position += dir * currentSpeed;
			}
		}
	}

	private void SnapToGrid()
	{
		var square = Grid.Current[xpos, ypos];
		this.transform.position = square.Center;
	}*/
}
