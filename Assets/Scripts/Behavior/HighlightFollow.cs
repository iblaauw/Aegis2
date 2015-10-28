using UnityEngine;
using System.Collections;
using Aegis;

public class HighlightFollow : MonoBehaviour {

	private int offsetX = 0;
	private int offsetY = 0;

	private new SpriteRenderer renderer;

	public static HighlightFollow Create(int offsetX, int offsetY)
	{
		HighlightFollow script = PrefabCache.Instance.Create<HighlightFollow>("Highlight");
		script.offsetX = offsetX;
		script.offsetY = offsetY;
		return script;
	}

	void Start() {
		this.renderer = this.GetComponent<SpriteRenderer>();
		if (this.renderer == null)
			throw new MissingComponentException();

		this.renderer.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		var square = Utilities.GetMouseSquare();

		if (square != null)
		{
			if (!this.renderer.enabled)
				this.renderer.enabled = true;

			Vector2 offset = new Vector2(offsetX, offsetY) * Grid.SQUARE_SIZE;
			this.transform.position = square.Center + offset;
		}
	}
}
