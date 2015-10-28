using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Aegis
{
	public class Projectile
	{
		private Targeter targeter;
		private Sprite sprite;

		private IList<IGridSquare> targets;


		//TODO: pass an "attack" object in here, to trigger
		public Projectile(Targeter targeter, Sprite visualSprite)
		{
			if (targeter == null)
				throw new ArgumentNullException("targeter");

			if (targeter.IsVisible)
				throw new ArgumentException("The given targeter must not already be active.");

			this.targeter = targeter;
			this.targeter.Selected += HandleTargetSelected;
			this.sprite = visualSprite;
		}

		public event Action Finished;

		public void BeginTargeting()
		{
			this.targeter.Show();
		}

		private void HandleTargetSelected(IList<IGridSquare> locations)
		{
			this.targets = locations;

			IGridSquare to = Utilities.GetMouseSquare();
			ProjectileVisual visual = ProjectileVisual.Create(5, Grid.Current[0,0], to);
			visual.Finished += this.HandleVisualDone;
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
