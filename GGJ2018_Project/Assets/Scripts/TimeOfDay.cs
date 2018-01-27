using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct TimeMarker
{
	public float Time;
	public System.Action<object> Callback;
	public object Argument;
}

public class TimeOfDay : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro m_TextMesh;
	[SerializeField, Tooltip("Seconds real time")]
	private float m_DayDuration = 10f;

	private float m_StartTime = 0;
	private float m_EndTime = 7;

	private float m_CurrentTime;
	private bool m_TimerPaused;

	public System.Action OnDayPassed = delegate { };

	private readonly List<TimeMarker> m_TimelineMarkers = new List<TimeMarker>();

	public virtual void Update()
	{
		if (!m_TimerPaused)
		{
			m_CurrentTime = Mathf.Clamp01(m_CurrentTime + Time.deltaTime / m_DayDuration);

			m_TextMesh.text = GetTimeOfDayFormatted();
			UpdateTimelineMarkers(GetTimeOfDay());

			if (m_CurrentTime == 1)
			{
				OnDayPassed();
				m_TextMesh.text = "Day over";
				m_TimerPaused = true;
			}

		}
	}

	private void UpdateTimelineMarkers(float a_NewTime)
	{
		int markerCalls = 0;

		foreach (var marker in m_TimelineMarkers)
		{
			if (marker.Time < a_NewTime)
			{
				marker.Callback(marker.Argument);
				++markerCalls;
			}
			else
			{
				//List is sorted
				break;
			}
		}

		if (markerCalls > 0)
		{
			m_TimelineMarkers.RemoveRange(0, markerCalls);
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
		return string.Format("{0}{1}:{2}{3}", hours < 10 ? "0" : "", hours, minutes < 10 ? "0" : "", minutes);
	}

	public void AddTimelineMarker(float a_Time, System.Action<object> a_Callback, object a_Argument = null)
	{
		TimeMarker marker;
		marker.Time = a_Time;
		marker.Callback = a_Callback;
		marker.Argument = a_Argument;

		m_TimelineMarkers.Add(marker);
		m_TimelineMarkers.Sort((a, b) => a.Time.CompareTo(b.Time));
	}
}
