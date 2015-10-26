using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Aegis
{
	public class Targeter : MonoBehaviour
	{
		List<GameObject> highlightObjs = new List<GameObject>();
		List<IntVector2> highlightPositions = new List<IntVector2>();

		public static Targeter Create()
		{
			GameObject g = new GameObject("targeting root", typeof(Targeter));
			return g.GetComponent<Targeter>();
		}

		public void Add(IntVector2 offset)
		{
			highlightPositions.Add(offset);
		}

		public void Add(int x, int y)
		{
			this.Add(new IntVector2(x,y));
		}

		public void Show()
		{
			foreach (IntVector2 ivec in highlightPositions)
			{
				var highlight = HighlightFollow.Create(ivec.x, ivec.y);
				highlight.transform.SetParent(this.transform);
				highlightObjs.Add(highlight.gameObject);
			}
		}

		void Update()
		{
			if (Input.GetMouseButtonDown(0))
				Destroy(this.gameObject);
		}
	}
}
