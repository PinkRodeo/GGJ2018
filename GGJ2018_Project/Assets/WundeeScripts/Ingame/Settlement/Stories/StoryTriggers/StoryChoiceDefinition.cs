using LitJson;

namespace Wundee.Stories
{
    public class StoryChoiceDefinition : Definition<StoryChoice>
	{
        public string choiceText;

		private Definition<Condition>[] _conditionDefinitions;
		private Definition<Effect>[] _rewardDefinitions;

		public override void ParseDefinition(string definitionKey, JsonData jsonData)
		{
            choiceText = ContentHelper.ParseString(jsonData, D.TEXT, "No Choice Text");

			_conditionDefinitions = ConditionDefinition.ParseDefinitions(jsonData[D.CONDITIONS], definitionKey);
			_rewardDefinitions = EffectDefinition.ParseDefinitions(jsonData[D.REWARDS], definitionKey);
		}

		public override StoryChoice GetConcreteType(object parent = null)
		{
			var newStoryTrigger = new StoryChoice();

			newStoryTrigger.definition = this;
			
			newStoryTrigger.parentStoryNode = parent as StoryNode;
			if (newStoryTrigger.parentStoryNode == null)
				Logger.Log("[StoryChoiceDefinition] Invalid parent StoryNode provided for new StoryTrigger");

			newStoryTrigger.conditions = _conditionDefinitions.GetConcreteTypes(newStoryTrigger.parentStoryNode);
			newStoryTrigger.rewards = _rewardDefinitions.GetConcreteTypes(newStoryTrigger.parentStoryNode);

			return newStoryTrigger;
		}

		public static Definition<StoryChoice>[] ParseDefinitions(JsonData storyTriggerData, string definitionKey)
		{
			return ContentHelper.GetDefinitions<StoryChoiceDefinition, StoryChoice>
				(storyTriggerData, definitionKey, KEYS.STORYTRIGGER);
		}
	}
}

