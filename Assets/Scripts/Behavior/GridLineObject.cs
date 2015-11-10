using UnityEngine;
using System.Collections;
using Aegis;

public class GridLineObject : MonoBehaviour {
	private int xpos = 0, ypos = 0;
	private Direction direction;
	
	public static GridLineObject Create(int xpos, int ypos, Direction direction)
	{
		GridLineObject obj = GameManager.PrefabCache.Create<GridLineObject>("GridLine");
		obj.xpos = xpos;
		obj.ypos = ypos;
		obj.direction = direction;
		return obj;
	}

	// Use this for initialization
	void Start () {

		Vector3 position = new Vector3();
		IGridSquare square = Grid.Current[xpos, ypos];

		position.z = 10; //Make it the background

		if (direction == Direction.DOWN || direction == Direction.LEFT)
		{
			position.x = square.Left;
			position.y = square.Down;
		}
		else if (direction == Direction.UP)
		{
			position.x = square.Left;
			position.y = square.Up;
		}
		else //RIGHT
		{
			position.x = square.Right;
			position.y = square.Down;
		}

		this.transform.position = position;

		if (direction == Direction.DOWN || direction == Direction.UP)
			this.transform.rotation = Quaternion.Euler(0, 0, -90);

		this.transform.localScale = new Vector3(1, Grid.SQUARE_SIZE, 1);


	}
}
