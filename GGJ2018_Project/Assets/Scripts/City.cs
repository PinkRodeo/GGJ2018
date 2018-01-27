using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour, iClickObject
{
	public System.Action<City> OnCityClicked = delegate {};

	public void OnClicked()
	{
		OnCityClicked(this);
	}
}
