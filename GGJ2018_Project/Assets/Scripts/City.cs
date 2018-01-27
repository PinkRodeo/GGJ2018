using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Wundee;

public class City : MonoBehaviour
{
    [SerializeField]
    private string m_LocationDefinitionKey;

    public string locationDefinitionKey
    {
        get
        {
            return m_LocationDefinitionKey;
        }
    }

    private Location m_Location;
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

    public void SetLocation(Location location)
    {
        m_Location = location;
        m_TextMesh.text = location.definition.name;
    }

}
