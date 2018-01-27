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
        


        public void SetToStoryChoice(StoryChoice storyChoice)
        {
            if ((storyChoice.conditions.CheckConditions() == false && storyChoice.conditions.Length > 0))
            {
                choiceText.text = "Should not be visible, son";
                state = State.Hidden;

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

                gameObject.SetActive(true);
            }
        }
    }

}
