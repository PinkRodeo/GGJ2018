using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class City : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro m_TextMesh;

	public System.Action<City> OnCityClicked = delegate {};

	private void OnMouseDown()
	{
		OnCityClicked(this);
	}

	private void OnMouseEnter()
	{
		m_TextMesh.gameObject.SetActive(true);
	}

	private void OnMouseExit()
	{
		m_TextMesh.gameObject.SetActive(false);
	}

}
