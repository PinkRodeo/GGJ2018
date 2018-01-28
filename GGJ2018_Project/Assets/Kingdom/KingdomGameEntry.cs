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
		private List<ConversationButtonUI> m_Buttons;

		public void Start()
		{
			/*
		    var location = new Location(game.definitions.locationDefinitions["PERSON_DEFAULT_01"]);

		    var spyMessageDefinition = game.definitions.storyNodeDefinitions["SPY_TEST_1"];

		    Debug.Log(spyMessageDefinition.nodeText);

		    var spyMessage = spyMessageDefinition.GetConcreteType(null) as StoryNode;

		    foreach (var choice in spyMessage.storyChoices)
		    {
		        Debug.Log(choice.definition.choiceText);
		    }
		    */

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
			//Game.instance.definitions.effectDefinitions[""].GetConcreteType(); //TODO: need some kind of callback to unpause game

			foreach (var timelineEvent in m_Events)
			{
				m_GameTimer.AddTimelineMarker(timelineEvent.Time, m_TimelineCallback, timelineEvent.RewardName);
			}

			StartCoroutine("DelayedStart");
		}

		private void ExecuteEffect(object Name)
		{
			Game.instance.definitions.effectDefinitions[(string)Name].GetConcreteType().ExecuteEffect();
			m_GameTimer.SetPaused(true);
			BindButtonEvents();
		}

		private void BindButtonEvents()
		{
			m_Buttons = Game.instance.conversationUI.GetChoiceButtons();
			m_Buttons.ForEach(button => button.Button.onClick.AddListener(ChoiceButtonClicked));
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

		private void ChoiceButtonClicked()
		{
			StartCoroutine("DelayedButtonClick");
		}

		private IEnumerator DelayedButtonClick()
		{
			yield return new WaitForSeconds(1f);
			if (!m_ConversationUI.IsVisible())
			{
				m_Buttons.ForEach(button => button.Button.onClick.RemoveListener(ChoiceButtonClicked));
				m_GameTimer.SetPaused(false);
			}
			else
			{
				BindButtonEvents();
			}
		}
	}
}
