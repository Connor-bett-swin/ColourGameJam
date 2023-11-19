using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorized : MonoBehaviour
{
	[SerializeField]
	private List<SpriteRenderer> m_Sprites;
	[SerializeField]
	private ColorScheme m_Colors;

	public int ColorIndex;

	private void Start()
	{
		var color = ColorIndex >= 0 ? m_Colors[ColorIndex] : Color.white;

		foreach (var sprite in m_Sprites)
		{
			sprite.color = color;
		}
	}
}
