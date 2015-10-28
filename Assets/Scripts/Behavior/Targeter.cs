using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aegis
{
	public class Targeter : MonoBehaviour
	{
		List<GameObject> highlightObjs = new List<GameObject>();
		List<IntVector2> highlightPositions = new List<IntVector2>();

		public static Targeter Create()
		{
			GameObject g = new GameObject("targeting root", typeof(Targeter));
			Targeter t = g.GetComponent<Targeter>();
			t.IsVisible = false;
			return t;
		}

		public bool IsVisible { get; private set; }

		public delegate void TargetSelectedCallback(IList<IGridSquare> locations);

		public event TargetSelectedCallback Selected;

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
			if (IsVisible)
				return;

			IsVisible = true;

			foreach (IntVector2 ivec in highlightPositions)
			{
				var highlight = HighlightFollow.Create(ivec.x, ivec.y);
				highlight.transform.SetParent(this.transform);
				highlightObjs.Add(highlight.gameObject);
			}
		}

		public void Hide()
		{
			if (!IsVisible)
				return;

			IsVisible = false;

			foreach (Transform child in this.transform)
			{
				Destroy(child.gameObject);
			}
		}

		void Update()
		{
			if (!IsVisible)
				return;

			if (Input.GetMouseButtonDown(0))
			{
				FireSelected();
				this.Selected = null;
				Destroy(this.gameObject);
			}
		}

		private void FireSelected()
		{
			if (Selected == null)
				return;

			IGridSquare mouseSquare = Utilities.GetMouseSquare();
			if (mouseSquare == null)
				return;

			IntVector2 mousePos = new IntVector2(mouseSquare.XIndex, mouseSquare.YIndex);

			var gridSquares = this.highlightPositions.Select(h => h + mousePos)
				.Where(v => Grid.Current.IsValid(v))
				.Select(v => Grid.Current[v.x, v.y]);

			Selected(gridSquares.ToList());
		}
	}
}
