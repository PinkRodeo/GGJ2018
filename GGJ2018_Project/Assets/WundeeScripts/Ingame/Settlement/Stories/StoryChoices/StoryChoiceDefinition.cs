using LitJson;

namespace Wundee.Stories
{
    public class StoryChoiceDefinition : Definition<StoryChoice>
	{
        public string choiceText;

		private Definition<Condition>[] _conditionDefinitions;
        private Definition<Condition>[] _disabledConditionDefinitions;

        private Definition<Effect>[] _rewardDefinitions;

		public override void ParseDefinition(string definitionKey, JsonData jsonData)
		{
            choiceText = ContentHelper.ParseString(jsonData, D.TEXT, "No Choice Text");

            var keys = jsonData.Keys;

            if (keys.Contains(D.CONDITIONS))
                _conditionDefinitions = ConditionDefinition.ParseDefinitions(jsonData[D.CONDITIONS], definitionKey);
            else
                _conditionDefinitions = new Definition<Condition>[0];

            if (keys.Contains(D.ENABLED_CONDITIONS))
                _disabledConditionDefinitions = ConditionDefinition.ParseDefinitions(jsonData[D.ENABLED_CONDITIONS], definitionKey);
            else
                _disabledConditionDefinitions = new Definition<Condition>[0];

            if (keys.Contains(D.REWARDS))
                _rewardDefinitions = EffectDefinition.ParseDefinitions(jsonData[D.REWARDS], definitionKey);
            else
                _rewardDefinitions = new Definition<Effect>[0];
        }

		public override StoryChoice GetConcreteType(object parent = null)
		{
			var newStoryChoice = new StoryChoice();

			newStoryChoice.definition = this;
			
			newStoryChoice.parentStoryNode = parent as StoryNode;
			if (newStoryChoice.parentStoryNode == null)
				Logger.Log("[StoryChoiceDefinition] Invalid parent StoryNode provided for new StoryTrigger");

			newStoryChoice.conditions = _conditionDefinitions.GetConcreteTypes(newStoryChoice.parentStoryNode);
            newStoryChoice.enabledConditions = _disabledConditionDefinitions.GetConcreteTypes(newStoryChoice.parentStoryNode);
			newStoryChoice.rewards = _rewardDefinitions.GetConcreteTypes(newStoryChoice.parentStoryNode);

			return newStoryChoice;
		}

		public static Definition<StoryChoice>[] ParseDefinitions(JsonData storyTriggerData, string definitionKey)
		{
			return ContentHelper.GetDefinitions<StoryChoiceDefinition, StoryChoice>
				(storyTriggerData, definitionKey, KEYS.STORYTRIGGER);
		}
	}
}

