using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using Wundee;
using Wundee.Stories;

namespace Kingdom
{
	public class KingdomGameEntry : WundeeUnity.GameEntry
	{
		[System.Serializable]
		private struct TimelineEvent
		{
			public string RewardName;
			public float Time;
		}

        [SerializeField]
		private List<TimelineEvent> m_Events = new List<TimelineEvent>();

		private System.Action<object> m_TimelineCallback;
		private ConversationUI m_ConversationUI;
		private int m_visibleCount;

		public void Start()
		{
			Scene scene = SceneManager.GetSceneByName("ConversationUI");

			if (!scene.isLoaded)
			{
				SceneManager.LoadScene("ConversationUI", LoadSceneMode.Additive);
			}

            Scene scene2 = SceneManager.GetSceneByName("ConversationUI");

            if (!scene2.isLoaded)
            {
                SceneManager.LoadScene("backgrounds", LoadSceneMode.Additive);
            }

            m_TimelineCallback += ExecuteEffect;
			m_GameTimer.OnDayPassed += OnDayCompleted;

			foreach (var timelineEvent in m_Events)
			{
				if (!string.IsNullOrEmpty(timelineEvent.RewardName))
				{
					m_GameTimer.AddTimelineMarker(timelineEvent.Time, m_TimelineCallback, timelineEvent.RewardName);
				}
			}

			StartCoroutine("DelayedStart");
		}

		public virtual void Update()
		{
			
			if (m_ConversationUI != null && (m_ConversationUI.IsVisible() || m_ConversationUI.SealButton.gameObject.activeSelf))
			{
				m_GameTimer.SetPaused(true);
				m_visibleCount = 0;
			}
			else
			{
				m_visibleCount++;
				if (m_visibleCount > 30)
				{
					m_visibleCount = 0;
					m_GameTimer.SetPaused(false);
				}
			}
		}

		private void ExecuteEffect(object Name)
		{
			Game.instance.definitions.effectDefinitions[(string)Name].GetConcreteType().ExecuteEffect();
			m_GameTimer.SetPaused(true);
			m_visibleCount = 0;
		}

		private void OnDayCompleted()
		{
			//TODO what to do when day ends
			m_GameTimer.RestartDayTimer();
		}

		private IEnumerator DelayedStart()
		{
			yield return new WaitForEndOfFrame();
			m_ConversationUI = Game.instance.conversationUI;
		}
	}
}
