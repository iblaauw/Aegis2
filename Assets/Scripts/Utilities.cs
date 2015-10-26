using UnityEngine;
using System;
using System.Collections;

namespace Aegis
{
	public static class Utilities {
		/// <summary>
		/// Extension method for converting a direction enum into the corresponding vector.
		/// The vector returned will always be a unit vector.
		/// </summary>
		public static Vector2 ToVector(this Direction dir)
		{
			Vector2 vec;
			switch (dir)
			{
			case Direction.LEFT: 
				vec = Vector2.right * -1;
				break;
			case Direction.RIGHT: 
				vec = Vector2.right;
				break;
			case Direction.UP: 
				vec = Vector2.up;
				break;
			case Direction.DOWN: 
				vec = Vector2.up * -1;
				break;
			default:
				throw new Exception("Die horribly.");
			}

			return vec;
		}

		/// <summary>
		/// Gets the square that the mouse is currently in. Returns null if there is none.
		/// </summary>
		public static IGridSquare GetMouseSquare()
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			//TODO: this is assuming that the grid starts at 0,0
			int xpos = Mathf.FloorToInt(mousePos.x / Grid.SQUARE_SIZE);
			int ypos = Mathf.FloorToInt(mousePos.y / Grid.SQUARE_SIZE);

			return Grid.Current.TryGet(xpos, ypos);
		}
	}

	public struct IntVector2
	{
		public int x;
		public int y;

		public IntVector2(int x, int y)
		{
			this.x = x; this.y = y;
		}

		public static IntVector2 operator+(IntVector2 ivec1, IntVector2 ivec2)
		{
			return new IntVector2(ivec1.x + ivec2.x, ivec2.y + ivec2.y);
		}

		public static IntVector2 operator-(IntVector2 ivec1, IntVector2 ivec2)
		{
			return new IntVector2(ivec1.x - ivec2.x, ivec2.y - ivec2.y);
		}

		public static IntVector2 operator*(int mult, IntVector2 ivec)
		{
			return new IntVector2(mult * ivec.x, mult * ivec.y);
		}

		public static IntVector2 operator*(IntVector2 ivec, int mult)
		{
			return mult * ivec;
		}

		public static implicit operator Vector2(IntVector2 ivec)
		{
			return new Vector2(ivec.x, ivec.y);
		}

		public static explicit operator IntVector2(Vector2 vec)
		{
			return new IntVector2((int)vec.x, (int)vec.y);
		}
	}
}

