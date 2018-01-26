using LitJson;

namespace Wundee.Stories
{
    public class StoryTriggerDefinition : Definition<StoryTrigger>
	{
		private Definition<Condition>[] _conditionDefinitions;
		private Definition<Effect>[] _rewardDefinitions;

		public override void ParseDefinition(string definitionKey, JsonData jsonData)
		{
			_conditionDefinitions = ConditionDefinition.ParseDefinitions(jsonData[D.CONDITIONS], definitionKey);
			_rewardDefinitions = EffectDefinition.ParseDefinitions(jsonData[D.REWARDS], definitionKey);
		}

		public override StoryTrigger GetConcreteType(object parent = null)
		{
			var newStoryTrigger = new StoryTrigger();

			newStoryTrigger.definition = this;
			
			newStoryTrigger.parentStoryNode = parent as StoryNode;
			if (newStoryTrigger.parentStoryNode == null)
				Logger.Log("[StoryTriggerDefinition] Invalid parent StoryNode provided for new StoryTrigger");

			newStoryTrigger.conditions = _conditionDefinitions.GetConcreteTypes(newStoryTrigger.parentStoryNode);
			newStoryTrigger.rewards = _rewardDefinitions.GetConcreteTypes(newStoryTrigger.parentStoryNode);

			return newStoryTrigger;
		}

		public static Definition<StoryTrigger>[] ParseDefinitions(JsonData storyTriggerData, string definitionKey)
		{
			return ContentHelper.GetDefinitions<StoryTriggerDefinition, StoryTrigger>
				(storyTriggerData, definitionKey, KEYS.STORYTRIGGER);
		}
	}
}

