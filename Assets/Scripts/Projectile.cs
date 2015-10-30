using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Aegis
{
	public class Projectile
	{
		private SquareMovement source;
		private Targeter targeter;
		private Sprite sprite;

		private IList<IGridSquare> targets;

		//TODO: make this not pass in a SquareMovement but rather an interface exposing only position
		//TODO: pass an "attack" object in here, to trigger
		public Projectile(Targeter targeter, SquareMovement source, Sprite visualSprite)
		{
			if (targeter == null)
				throw new ArgumentNullException("targeter");

			if (targeter.IsVisible)
				throw new ArgumentException("The given targeter must not already be active.");

			this.targeter = targeter;
			this.targeter.Selected += HandleTargetSelected;
			this.sprite = visualSprite;

			this.source = source;
		}

		public event Action Finished;

		public void BeginTargeting()
		{
			this.targeter.Show();
		}

		private void HandleTargetSelected(IList<IGridSquare> locations)
		{
			this.targets = locations;

			bool first = true;
			foreach (IGridSquare to in locations)
			{
				ProjectileVisual visual = ProjectileVisual.Create(0.15f, this.source.Position, to, this.sprite);
				if (first)
				{
					visual.Finished += this.HandleVisualDone;
					first = false;
				}
			}
		}

		private void HandleVisualDone()
		{
			foreach (IGridSquare square in this.targets)
			{
				foreach (GameObject obj in square.GetObjects())
				{
					Stats stats = obj.GetComponent<Stats>();
					if (stats != null)
						stats.Hit(5);
				}
			}

			if (this.Finished != null)
				this.Finished();
		}

	}
}
