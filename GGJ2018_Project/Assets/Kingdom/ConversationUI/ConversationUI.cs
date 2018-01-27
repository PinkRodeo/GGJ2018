using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Wundee;
using Wundee.Stories;

namespace Kingdom
{
    public class ConversationUI : MonoBehaviour
    {
        [Header("Self")]
        public Text conversationText;

        [Header("Choices")]
        public RectTransform buttonLayout;

        public GameObject choiceButtonPrefab;

        // Use this for initialization
        void Start()
        {
            var game = KingdomGameEntry.gameInstance;

            var location = new Location(game.definitions.locationDefinitions["PERSON_DEFAULT_01"]);

            var spyMessageDefinition = game.definitions.storyNodeDefinitions["SPY_TEST_1"];

            Debug.Log(spyMessageDefinition.nodeText);

            var spyMessage = spyMessageDefinition.GetConcreteType(null) as StoryNode;

            foreach (var choice in spyMessage.storyChoices)
            {
                Debug.Log(choice.definition.choiceText);
            }

            SetToStoryNode(spyMessage);
        }

        public void SetToStoryNode(StoryNode storyNode)
        {
            conversationText.text = storyNode.definition.nodeText;

            // choices
            
            // TODO: clean up these children
            buttonLayout.DetachChildren();
            foreach (var choice in storyNode.storyChoices)
            {
                var choiceButtonGO = Instantiate<GameObject>(choiceButtonPrefab, buttonLayout);
                var choiceButtonUI = choiceButtonGO.GetComponent<ConversationButtonUI>();
                choiceButtonUI.SetToStoryChoice(choice);
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                var game = KingdomGameEntry.gameInstance;

                var spyMessageDefinition = game.definitions.storyNodeDefinitions["SPY_TEST_1"];
                var spyMessage = spyMessageDefinition.GetConcreteType(null) as StoryNode;
                
                SetToStoryNode(spyMessage);
            }
        }
    }
}