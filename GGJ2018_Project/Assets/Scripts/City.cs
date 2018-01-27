using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
	public System.Action<City> OnCityClicked = delegate {};

	private void OnMouseDown()
	{
		OnCityClicked(this);
	}
}
