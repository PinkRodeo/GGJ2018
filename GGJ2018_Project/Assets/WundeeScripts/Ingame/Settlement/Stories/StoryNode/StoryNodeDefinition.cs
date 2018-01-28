using LitJson;


namespace Wundee.Stories
{
    public class StoryNodeDefinition : Definition<StoryNode>
	{
        public string nodeText;

		private Definition<Effect>[] _effectDefinitions;
		private Definition<StoryChoice>[] _storyTriggerDefinitions;

		private Definition<Effect>[] _onStartRewardDefinitions;
		private Definition<Effect>[] _onCompleteRewardDefinitions;

		public override void ParseDefinition(string definitionKey, JsonData jsonData)
		{
			var keys = jsonData.Keys;

            nodeText = ContentHelper.ParseString(jsonData, D.TEXT, "No Conversation Text");  

			if (keys.Contains(D.EFFECTS))
				this._effectDefinitions = EffectDefinition.ParseDefinitions(jsonData[D.EFFECTS], definitionKey);
			else
				this._effectDefinitions = new Definition<Effect>[0];

            if (keys.Contains(D.STORYCHOICES))
                this._storyTriggerDefinitions = StoryChoiceDefinition.ParseDefinitions(jsonData[D.STORYCHOICES], definitionKey);
            else
            {
                var fallbackDefinition = new StoryChoiceDefinition();
                fallbackDefinition.ParseDefinition(definitionKey + "Dummy", new JsonData());
                this._storyTriggerDefinitions = new Definition<StoryChoice>[1] { fallbackDefinition };

             }
                  

			if (keys.Contains(D.REWARDS_ON_START))
				this._onStartRewardDefinitions = EffectDefinition.ParseDefinitions(jsonData[D.REWARDS_ON_START], definitionKey);
			else
				this._onStartRewardDefinitions = new Definition<Effect>[0];

			if (keys.Contains(D.REWARDS_ON_COMPLETE))
				this._onCompleteRewardDefinitions = EffectDefinition.ParseDefinitions(jsonData[D.REWARDS_ON_COMPLETE], definitionKey);
			else
				this._onCompleteRewardDefinitions = new Definition<Effect>[0];
		}

		public override StoryNode GetConcreteType(System.Object parent)
		{
			var newStoryNode = new StoryNode();
			newStoryNode.definition = this;

			newStoryNode.parentLocation = parent as Location;
			if (newStoryNode.parentLocation == null)
				Logger.Log("[StoryNodeDefinition] Invalid parent Location provided for new StoryNode");

			newStoryNode.effects = _effectDefinitions.GetConcreteTypes(newStoryNode);
			newStoryNode.storyChoices = _storyTriggerDefinitions.GetConcreteTypes(newStoryNode);
			newStoryNode.onStartRewards = _onStartRewardDefinitions.GetConcreteTypes(newStoryNode);
			newStoryNode.onCompleteRewards = _onCompleteRewardDefinitions.GetConcreteTypes(newStoryNode);

			return newStoryNode;
		}
	}
}