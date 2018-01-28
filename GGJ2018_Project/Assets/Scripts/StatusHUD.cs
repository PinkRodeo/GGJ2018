using System;
using TMPro;
using UnityEngine;
using Wundee;

public class StatusHUD : MonoBehaviour
{
	[SerializeField]
	private enum HUDType
	{
		Days,
		Gold,
		Pawns,
		Reputation
	}

	[SerializeField]
	private HUDType m_HUDType;
	[SerializeField]
	private string m_Faction;
	[SerializeField]
	private TextMeshPro m_DigitMesh;

	private float m_Value;

	public virtual void Start()
	{
		switch (m_HUDType)
		{
			case HUDType.Days:
				break;
			case HUDType.Gold:
				Game.instance.world.OnGoldChanged += SetValue;
				SetValue(0, Game.instance.world.gold);
				break;
			case HUDType.Pawns:
				Game.instance.world.OnPawnsChanged += SetValue;
				SetValue(0, Game.instance.world.pawns);
				break;
			case HUDType.Reputation:
				Game.instance.world.factions[m_Faction].OnReputationChanged += SetValue;
				SetValue(0, Game.instance.world.factions[m_Faction].reputation);
				break;
		}
	}

	private void UpdateText()
	{
		switch (m_HUDType)
		{
			case HUDType.Days:
			case HUDType.Gold:
			case HUDType.Pawns:
			{
				m_DigitMesh.text = Mathf.RoundToInt(m_Value) + "";
				break;
			}
			case HUDType.Reputation:
			{
				m_DigitMesh.text = m_Value + "%";
				break;
			}
		}
	}

	public void SetValue(int a_Old, int a_New)
	{
		m_Value = a_New;
		UpdateText();
	}

	public void SetValue(int a_Old, int a_New, Wundee.Faction a_Faction)
	{
		SetValue(a_Old, a_New);
	}
}
