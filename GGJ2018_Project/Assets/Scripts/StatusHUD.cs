using System;
using TMPro;
using UnityEngine;

public class StatusHUD : MonoBehaviour
{
	[SerializeField]
	private enum DigitType
	{
		Integer,
		Percentage
	}

	[SerializeField]
	private DigitType m_DigitType;
	[SerializeField]
	private TextMeshPro m_DigitMesh;

	private float m_Value;
	

	private void UpdateText()
	{
		switch (m_DigitType)
		{
			case DigitType.Integer:
			{
				m_DigitMesh.text = Mathf.RoundToInt(m_Value) + "";
				break;
			}
			case DigitType.Percentage:
			{
				m_DigitMesh.text = m_Value + "%";
				break;
			}
		}
	}

	public void SetValue(float a_Value)
	{
		m_Value = a_Value;
	}
}
