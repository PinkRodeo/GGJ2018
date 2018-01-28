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
	    private TimeOfDay m_GameTimer;
		[SerializeField]
	    private List<TimelineEvent> m_Events = new List<TimelineEvent>();

	    private System.Action<object> m_TimelineCallback;

		// Use this for initialization
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
	    }

	    private void OnDayCompleted()
	    {
			//TODO what to do when day ends
		    m_GameTimer.RestartDayTimer();
		}

	    private IEnumerator DelayedStart()
	    {
		    yield return new WaitForEndOfFrame();
		    //Debug.Log(Game.instance.conversationUI);

		}
	}
}
