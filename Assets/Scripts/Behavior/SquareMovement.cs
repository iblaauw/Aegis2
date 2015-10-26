using UnityEngine;
using System.Collections;
using Aegis;

public class SquareMovement : MonoBehaviour {
	private int xpos = 0;
	private int ypos = 0;
	private float currentSpeed = 0;
	private IGridSquare moveTarget = null;
			
	void FixedUpdate()
	{
		if (!this.IsMoving)
			return;

		Vector2 offset = moveTarget.Center - (Vector2)this.transform.position;
		
		if (offset.magnitude < currentSpeed)
		{
			this.SetPosition(moveTarget);
			this.CancelMove();
		}
		else
		{
			Vector3 dir = offset.normalized;
			this.transform.position += dir * currentSpeed;
		}
	}

	/**************** Non-Unity Functions *****************/

	public IGridSquare Position
	{
		get { return Grid.Current[xpos, ypos]; }
	}

	public IGridSquare Target { get { return moveTarget; } }

	public bool IsMoving { get { return currentSpeed != 0; } }

	public void SnapToGrid()
	{
		var square = Grid.Current[xpos, ypos];
		this.transform.position = square.Center;
	}

	public void SetPosition(int x, int y)
	{
		if (x < 0 || x >= Grid.GRID_SIZE || y < 0 || y >= Grid.GRID_SIZE)
			throw new System.IndexOutOfRangeException();

		this.xpos = x;
		this.ypos = y;
		this.SnapToGrid();
	}

	public void SetPosition(IGridSquare square)
	{
		this.SetPosition(square.XIndex, square.YIndex);
	}

	public void StartMove(IGridSquare target, float speed)
	{
		if (target == null)
			throw new System.ArgumentNullException();

		if (speed == 0)
			throw new System.ArgumentException("Speed cannot be zero.");

		if (this.IsMoving)
			return;

		this.moveTarget = target;
		this.currentSpeed = speed;
	}

	public void CancelMove()
	{
		if (!this.IsMoving)
			return;

		this.moveTarget = null;
		this.currentSpeed = 0;
	}
}
