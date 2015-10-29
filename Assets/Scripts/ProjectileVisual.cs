using UnityEngine;
using System.Collections;

namespace Aegis
{
	/// <summary>
	/// A visual for a projectile. Wraps an actual game object and simply fires it at a certain square.
	/// </summary>
	public class ProjectileVisual
	{
		private GameObject wrappedObject;

		private ProjectileVisual()
		{}

		public event System.Action Finished;

		public static ProjectileVisual Create(float speed, IGridSquare start, IGridSquare end)
		{
			return ProjectileVisual.Create(speed, start, end, null);
		}

		public static ProjectileVisual Create(float speed, IGridSquare start, IGridSquare end, Sprite sprite)
		{
			GameObject g = new GameObject("projectile");

			if (sprite != null)
			{
				SpriteRenderer render = g.AddComponent<SpriteRenderer>();
				render.sprite = sprite;
			}

			SquareMovement moveControl = g.AddComponent<SquareMovement>();
			moveControl.SetPosition(start);
			moveControl.StartMove(end, speed);

			g.transform.Rotate(new Vector3(0,0, moveControl.Rotation));

			ProjectileVisual visual = new ProjectileVisual();
			visual.wrappedObject = g;
			moveControl.LongMoveFinished += visual.MoveDoneCallback;
			return visual;
		}

		private void MoveDoneCallback(IGridSquare square)
		{
			if (Finished != null)
			{
				this.Finished();
				this.Finished = null;
			}

			Object.Destroy(this.wrappedObject);
		}
	}
}
