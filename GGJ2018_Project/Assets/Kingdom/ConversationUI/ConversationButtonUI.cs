using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

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

        [SerializeField]
        public TextMeshProUGUI choiceText;

        [SerializeField]
        private Color regularTextColor;
        [SerializeField]
        private Color disabledTextColor;


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

                gameObject.SetActive(false);
            }
            else if (storyChoice.enabledConditions.CheckConditions() == false && storyChoice.enabledConditions.Length > 0)
            {
                choiceText.text = storyChoice.definition.choiceText;
                state = State.Disabled;

                choiceText.fontStyle = FontStyles.Strikethrough;
                choiceText.color = disabledTextColor;

                m_Button.onClick.AddListener(OnPressedDisabled);

                gameObject.SetActive(true);
            }
            else
            {
                choiceText.text = storyChoice.definition.choiceText;
                state = State.Visible;
                choiceText.color = regularTextColor;

                m_Button.onClick.AddListener(OnPressedValid);

                gameObject.SetActive(true);
            }

            
        }

        private void OnPressedValid()
        {
            m_ConversationUI.SetVisible(false);

            m_CurrentStoryChoice.rewards.ExecuteEffects<Effect>();

        }

        private void OnPressedDisabled()
        {

        }
    }

}
