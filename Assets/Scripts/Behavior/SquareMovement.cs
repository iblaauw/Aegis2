﻿using UnityEngine;
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

    private Coroutine currentRoutine;

    #region Unity Messages

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

	void OnDestroy()
	{
		this.Position.RemoveObject(this.gameObject);
	}

    #endregion

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
        if (!Grid.Current.IsValid(x,y))
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

        this.currentRoutine = this.StartCoroutine(MoveCoroutine());
	}

	public void CancelMove()
	{
		if (!this.IsMoving)
			return;

        StopCoroutine(currentRoutine);
        DoStopMove();
	}

    private IEnumerator MoveCoroutine()
    {
        Vector2 offset = moveTarget.Center - (Vector2)this.transform.position;

        while (offset.magnitude >= currentSpeed)
        {
            // Check if canceled
            if (!IsMoving) 
                yield break;

            Vector2 newPos = Vector2.MoveTowards(this.transform.position, this.moveTarget.Center, this.currentSpeed);
            this.transform.position = newPos;

            yield return new WaitForFixedUpdate();

            offset = moveTarget.Center - (Vector2)this.transform.position;
        }

        this.SetPosition(moveTarget);
        this.DoStopMove();
        this.FireMoveFinished();
    }

    private void DoStopMove()
    {
        this.moveTarget = null;
        this.currentSpeed = 0;
        this.currentRoutine = null;
    }

	private void FireMoveFinished()
	{
		if (this.LongMoveFinished != null)
			this.LongMoveFinished(this.Position);
	}
}
