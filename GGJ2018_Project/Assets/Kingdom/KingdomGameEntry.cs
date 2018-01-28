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
	    }
    }
}
