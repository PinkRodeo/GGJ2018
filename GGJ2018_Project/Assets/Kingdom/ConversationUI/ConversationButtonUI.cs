using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using Wundee;
using Wundee.Stories;

namespace Kingdom
{
    public class ConversationButtonUI : MonoBehaviour
    {
        public enum State
        {
            Visible,
            Disabled,
            Hidden
        }

        public Text choiceText;
        public State state;
        private ConversationUI m_ConversationUI;

        [SerializeField]
        private Button m_Button;

        private StoryChoice m_CurrentStoryChoice;

        public void SetToStoryChoice(ConversationUI conversationUI, StoryChoice storyChoice)
        {
            m_ConversationUI = conversationUI;
            m_CurrentStoryChoice = storyChoice;

            if ((storyChoice.conditions.CheckConditions() == false && storyChoice.conditions.Length > 0))
            {
                choiceText.text = "Should not be visible, son";
                state = State.Hidden;

                m_Button.onClick.AddListener(OnPressedDisabled);


                gameObject.SetActive(false);
            }
            else if (storyChoice.enabledConditions.CheckConditions() == false && storyChoice.enabledConditions.Length > 0)
            {
                choiceText.text = "Disabled" +  storyChoice.definition.choiceText;
                state = State.Disabled;

                gameObject.SetActive(true);
            }
            else
            {
                choiceText.text = storyChoice.definition.choiceText;
                state = State.Visible;

                m_Button.onClick.AddListener(OnPressedValid);

                gameObject.SetActive(true);
            }

            
        }

        private void OnPressedValid()
        {
            Debug.Log("Button is pressed");

            m_CurrentStoryChoice.rewards.ExecuteEffects<Effect>();

            m_ConversationUI.SetVisible(false);
        }

        private void OnPressedDisabled()
        {
            Debug.Log("Button is disabled");
        }
    }

}
