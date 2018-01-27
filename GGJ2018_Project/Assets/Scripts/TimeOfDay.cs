using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfDay : MonoBehaviour
{
	[SerializeField]
	private TextMesh m_TextMesh;
	[SerializeField, Tooltip("Seconds real time")]
	private float m_DayDuration = 10f;

	private float m_StartTime = 0;
	private float m_EndTime = 7;

	private float m_CurrentTime;
	private bool m_TimerPaused;

	public System.Action OnDayPassed = delegate { };

	public virtual void Update()
	{
		if (!m_TimerPaused)
		{
			m_CurrentTime = Mathf.Clamp01(m_CurrentTime + Time.deltaTime / m_DayDuration);
			m_TextMesh.text = GetTimeOfDayFormatted();
			if (m_CurrentTime == 1)
			{
				m_TextMesh.text = "Day over";
				m_TimerPaused = true;
			}

		}
	}

	public float GetTimeProgress()
	{
		return m_CurrentTime;
	}

	public float GetTimeOfDay()
	{
		float timeDiff = m_EndTime < m_StartTime ? m_EndTime + 24 - m_StartTime : m_EndTime - m_StartTime;
		float timeOfDay = timeDiff * m_CurrentTime + m_StartTime;
		return timeOfDay >= 24 ? timeOfDay - 24 : timeOfDay;
	}

	public string GetTimeOfDayFormatted()
	{
		float time = GetTimeOfDay();
		int hours = (int) time;
		int minutes = (int)((time - hours) * 60);
		return String.Format("{0}{1}:{2}{3}", hours < 10 ? "0" : "", hours, minutes < 10 ? "0" : "", minutes);
	}
}
