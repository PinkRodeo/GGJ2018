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

        public Image GuardIcon;
        public Image ThiefIcon;
        public Image WitchIcon;

        public State state;
        private ConversationUI m_ConversationUI;

        [SerializeField]
        private Button m_Button;

        private StoryChoice m_CurrentStoryChoice;

	    public Button Button
	    {
		    get { return m_Button; }
	    }

        public void SetToStoryChoice(ConversationUI conversationUI, StoryChoice storyChoice)
        {
            m_ConversationUI = conversationUI;
            m_CurrentStoryChoice = storyChoice;

            var factionIcon = GetCurrentFactionIcon(storyChoice);

            if ((storyChoice.conditions.CheckConditions() == false && storyChoice.conditions.Length > 0))
            {
                choiceText.text = "Should not be visible, son";
                state = State.Hidden;

                gameObject.SetActive(false);
            }
            else if (storyChoice.enabledConditions.CheckConditions() == false && storyChoice.enabledConditions.Length > 0)
            {
                var rawMainText = storyChoice.definition.choiceText;
                rawMainText = rawMainText.Replace("%FACTIONMEMBER", storyChoice.parentStoryNode.parentLocation.owningFaction.definition.memberName);

                rawMainText = rawMainText.Replace("%FACTION", storyChoice.parentStoryNode.parentLocation.owningFaction.definition.name);

                rawMainText = rawMainText.Replace("%RIVALFACTIONMEMBER", Game.instance.definitions.factionDefinitions[storyChoice.parentStoryNode.parentLocation.owningFaction.definition.rivalFaction].memberName);
                rawMainText = rawMainText.Replace("%RIVALFACTION", Game.instance.definitions.factionDefinitions[storyChoice.parentStoryNode.parentLocation.owningFaction.definition.rivalFaction].name);


                rawMainText = rawMainText.Replace("%LOCATION", storyChoice.parentStoryNode.parentLocation.definition.name);
                rawMainText = rawMainText.Replace("%GOLD", Game.instance.world.gold.ToString());
                rawMainText = rawMainText.Replace("%PAWNS", Game.instance.world.pawns.ToString());
                rawMainText = rawMainText.Replace("%REPUTATION", storyChoice.parentStoryNode.parentLocation.owningFaction.reputation.ToString());

                choiceText.text = rawMainText;
                state = State.Disabled;

                choiceText.fontStyle = FontStyles.Strikethrough;
                choiceText.color = disabledTextColor;

                m_Button.onClick.AddListener(OnPressedDisabled);

                gameObject.SetActive(true);

                if (factionIcon != null)
                {
                    factionIcon.color = Color.black;
                    factionIcon.gameObject.SetActive(true);
                }
            }
            else
            {
                var rawMainText = storyChoice.definition.choiceText;

                rawMainText = rawMainText.Replace("%FACTIONMEMBER", storyChoice.parentStoryNode.parentLocation.owningFaction.definition.memberName);

                rawMainText = rawMainText.Replace("%FACTION", storyChoice.parentStoryNode.parentLocation.owningFaction.definition.name);

                rawMainText = rawMainText.Replace("%RIVALFACTIONMEMBER", Game.instance.definitions.factionDefinitions[storyChoice.parentStoryNode.parentLocation.owningFaction.definition.rivalFaction].memberName);
                rawMainText = rawMainText.Replace("%RIVALFACTION", Game.instance.definitions.factionDefinitions[storyChoice.parentStoryNode.parentLocation.owningFaction.definition.rivalFaction].name);


                rawMainText = rawMainText.Replace("%LOCATION", storyChoice.parentStoryNode.parentLocation.definition.name);
                rawMainText = rawMainText.Replace("%GOLD", Game.instance.world.gold.ToString());
                rawMainText = rawMainText.Replace("%PAWNS", Game.instance.world.pawns.ToString());
                rawMainText = rawMainText.Replace("%REPUTATION", storyChoice.parentStoryNode.parentLocation.owningFaction.reputation.ToString());

                choiceText.text = rawMainText;
                state = State.Visible;
                choiceText.color = regularTextColor;

                m_Button.onClick.AddListener(OnPressedValid);

                gameObject.SetActive(true);

                if (factionIcon != null)
                {
                    factionIcon.gameObject.SetActive(true);
                }
            }

            
        }

        public Image GetCurrentFactionIcon(StoryChoice storyChoice)
        {
            GuardIcon.gameObject.SetActive(false);
            ThiefIcon.gameObject.SetActive(false);
            WitchIcon.gameObject.SetActive(false);

            var factionKey = storyChoice.definition.factionKey;

            if (factionKey == "FACTION_GUARDS")
                return GuardIcon;
            else if (factionKey == "FACTION_THIEVES")
                return ThiefIcon;
            else if (factionKey == "FACTION_WITCHES")
                return WitchIcon;
            else
                return null;

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
