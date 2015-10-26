using UnityEngine;
using System.Collections;
using Aegis;

public class GenerateGrid : MonoBehaviour {

	void Awake()
	{
		Grid.CreateGrid();
	}

	// Use this for initialization
	void Start () {
		CreateGrid();
		AlignCamera();
	}

	private void CreateGrid()
	{
		for (int i = 0; i < Grid.GRID_SIZE; i++)
		{
			for (int j = 0; j < Grid.GRID_SIZE; j++)
			{
				//Fill in majority of grid
				CreateLine(i,j,Direction.LEFT);
				CreateLine(i,j,Direction.DOWN);
			}
			
			//Fill in right side
			CreateLine(Grid.GRID_SIZE - 1, i, Direction.RIGHT);
			//Fill in top
			CreateLine(i, Grid.GRID_SIZE - 1, Direction.UP);
		}
	}

	private void AlignCamera()
	{
		GameObject camera = GameObject.FindWithTag("MainCamera");

		Vector3 cameraPos = new Vector3();

		cameraPos.z = camera.transform.position.z;
		float halfPoint = Grid.GRID_SIZE * Grid.SQUARE_SIZE / 2;
		cameraPos.x = halfPoint;
		cameraPos.y = halfPoint;

		camera.transform.position = cameraPos;
	}

	private void CreateLine(int x, int y, Direction dir)
	{
		GridLineObject line = GridLineObject.Create(x,y,dir);
		line.transform.SetParent(this.transform);
	}
}
