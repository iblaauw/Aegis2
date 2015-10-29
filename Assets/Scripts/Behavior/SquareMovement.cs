using UnityEngine;
using System.Collections;
using Aegis;

public class SquareMovement : MonoBehaviour {
	/***
	 * Xpos and Ypos are the index that this square is currently at. Note that if they are -1 this means that the
	 * object hasn't been initialized yet.
	 ***/
	private int xpos = -1;
	private int ypos = -1;
	private float currentSpeed = 0;
	private IGridSquare moveTarget = null;
	private float rotation = 0;

	void Start()
	{
		if (this.xpos == -1 || this.ypos == -1)
		{
			this.xpos = 0;
			this.ypos = 0;
			Grid.Current[0,0].AddObject(this.gameObject);
			this.SnapToGrid();
		}
	}

	void FixedUpdate()
	{
		if (!this.IsMoving)
			return;

		Vector2 offset = moveTarget.Center - (Vector2)this.transform.position;
		
		if (offset.magnitude < currentSpeed)
		{
			this.SetPosition(moveTarget);
			this.CancelMove();
			this.FireMoveFinished();
		}
		else
		{
			Vector3 dir = offset.normalized;
			this.transform.position += dir * currentSpeed;
		}
	}

	void OnDestroy()
	{
		this.Position.RemoveObject(this.gameObject);
	}

	/**************** Non-Unity Functions *****************/

	public delegate void LongMoveFinishedCallback(IGridSquare position);

	public event LongMoveFinishedCallback LongMoveFinished;

	public IGridSquare Position
	{
		get { return Grid.Current[xpos, ypos]; }
	}

	public IGridSquare Target { get { return moveTarget; } }

	public bool IsMoving { get { return currentSpeed != 0; } }

	public float Rotation { get { return rotation; } }

	public void SnapToGrid()
	{
		var square = Grid.Current[xpos, ypos];
		this.transform.position = square.Center;
	}

	public void SetPosition(int x, int y)
	{
		if (x < 0 || x >= Grid.GRID_SIZE || y < 0 || y >= Grid.GRID_SIZE)
			throw new System.IndexOutOfRangeException();

		if (xpos != -1 && ypos != -1)
			Grid.Current[xpos,ypos].RemoveObject(this.gameObject);

		this.xpos = x;
		this.ypos = y;

		Grid.Current[xpos,ypos].AddObject(this.gameObject);

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

		if (speed < 0)
			throw new System.ArgumentException("Speed cannot be negative.");

		if (this.IsMoving)
			return;

		this.moveTarget = target;
		this.currentSpeed = speed;

		var quat = Quaternion.FromToRotation(Vector2.right, this.Target.Center - this.Position.Center);
		this.rotation = quat.eulerAngles.z;
	}

	public void CancelMove()
	{
		if (!this.IsMoving)
			return;

		this.moveTarget = null;
		this.currentSpeed = 0;
	}

	private void FireMoveFinished()
	{
		if (this.LongMoveFinished != null)
			this.LongMoveFinished(this.Position);
	}
}
