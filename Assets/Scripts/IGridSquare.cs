﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Aegis
{
	public interface IGridSquare
	{
		int XIndex { get; }
		int YIndex { get; }
		
		float Left { get; }
		float Right { get; }
		float Up { get; }
		float Down { get; }

		Vector2 Center { get; }
		IntVector2 Location { get; }
		
		IEnumerable<GameObject> GetObjects();
		
		bool IsAtPosition(GameObject obj);

		IGridSquare GetNext(Direction direction);

		void AddObject(GameObject obj);
		void RemoveObject(GameObject obj);
	}
	
	public enum Direction
	{
		LEFT, RIGHT, UP, DOWN
	}
}
