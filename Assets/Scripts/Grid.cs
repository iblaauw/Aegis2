using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Aegis
{
	public class Grid
	{
		public const int GRID_SIZE = 9;
		public const float SQUARE_SIZE = 1;

		private static Grid instance;
		public static Grid Current { get { return instance; } }

		private List<GameObject>[,] gridData = 
			new List<GameObject>[GRID_SIZE,GRID_SIZE];

		private Grid()
		{}

		//TODO: eventually this function will take arguments for the constructor: grid size, etc.
		public static void CreateGrid()
		{
			instance = new Grid();
		}

		public IGridSquare Get(int x, int y)
		{
			if (!IsValid(x,y))
				throw new ArgumentOutOfRangeException();

			return GetInternal(x, y);
		}

		public IGridSquare TryGet(int x, int y)
		{
			if (!IsValid(x,y))
				return null;
			return GetInternal(x,y);
		}

		public IEnumerable<GameObject> GetObjects(int x, int y)
		{
			if (!IsValid(x,y))
				throw new ArgumentOutOfRangeException();

			if (gridData[x,y] == null)
				gridData[x,y] = new List<GameObject>();

			return gridData[x,y];
		}

		public bool IsValid(int x, int y)
		{
			if (x < 0 || y < 0 || x >= GRID_SIZE || y >= GRID_SIZE)
				return false;
			return true;
		}

		public bool IsValid(IntVector2 ivec)
		{
			return IsValid(ivec.x, ivec.y);
		}

		public IGridSquare this[int x, int y]
		{
			get { return Get(x,y); }
		}

		/// <summary>
		/// Entry point for getting a GridSquare after all error checking
		/// </summary>
		private GridSquare GetInternal(int x, int y)
		{
			if (gridData[x,y] == null)
				gridData[x,y] = new List<GameObject>();
			
			return new GridSquare(x,y, this);
		}


		private class GridSquare : IGridSquare
		{
			private int x;
			private int y;
			private Grid grid;

			public GridSquare(int x, int y, Grid grid)
			{
				this.x = x;
				this.y = y;
				this.grid = grid;
			}

			public int XIndex { get { return x; } }
			public int YIndex { get { return y; } }
			
			public float Left { get { return x * Grid.SQUARE_SIZE; } }
			public float Right { get { return (x + 1) * Grid.SQUARE_SIZE; } }
			public float Up { get { return (y + 1) * Grid.SQUARE_SIZE; } }
			public float Down { get { return y * Grid.SQUARE_SIZE; } }

			public Vector2 Center
			{
				get { return new Vector2((x + 0.5f) * Grid.SQUARE_SIZE, (y + 0.5f) * Grid.SQUARE_SIZE); }
			}

			public IEnumerable<GameObject> GetObjects()
			{
				return grid.gridData[x,y];
			}

			public bool IsAtPosition(GameObject obj)
			{
				return grid.gridData[x,y].Contains(obj);
			}

			/// <summary>
			/// Returns the adjacent square in the given direction. If there is no
			/// 	square in that direction (edge of the grid), this will return null.
			/// </summary>
			public IGridSquare GetNext(Direction direction)
			{
				Vector2 vec = new Vector2(x,y);
				vec += direction.ToVector();

				return grid.TryGet((int)vec.x, (int)vec.y);
			}
		}
	}

}
