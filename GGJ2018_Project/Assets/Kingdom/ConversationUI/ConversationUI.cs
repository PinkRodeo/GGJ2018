using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Wundee;
using Wundee.Stories;

using DG.Tweening;

namespace Kingdom
{
    public class ConversationUI : MonoBehaviour
    {
        [Header("Self")]
        public Text conversationText;

        [Header("Choices")]
        public RectTransform buttonLayout;

        public GameObject choiceButtonPrefab;

        [SerializeField]
        private CanvasGroup m_CanvasGroup;

        // Use this for initialization
        void Start()
        {
            var game = KingdomGameEntry.gameInstance;

            /*
            var spyMessage = location.storyHolder.AddStoryNode("SPY_TEST_1");

            Debug.Log(spyMessage.definition.nodeText);
            foreach (var choice in spyMessage.storyChoices)
            {
                Debug.Log(choice.definition.choiceText);
            }

            SetToStoryNode(spyMessage);
            */
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



        public void SetVisible(bool p_IsVisible)
        {
            m_CanvasGroup.blocksRaycasts = p_IsVisible;
            m_CanvasGroup.interactable = p_IsVisible;

            var sequence = DOTween.Sequence();
        }
    }
}