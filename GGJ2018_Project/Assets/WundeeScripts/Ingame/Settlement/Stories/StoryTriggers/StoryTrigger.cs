
namespace Wundee.Stories
{
	public class StoryTrigger
	{
		public StoryTriggerDefinition definition;

		public Condition[] conditions;
		public StoryNode parentStoryNode;
		public Effect[] rewards;

		private bool _hasBeenTriggeredBefore = false;

		public bool CheckTrigger()
		{
			if (_hasBeenTriggeredBefore)
				return false;

			if (conditions.CheckConditions() == false)
				return false;

			rewards.ExecuteEffects();
			
			_hasBeenTriggeredBefore = true;

			return true;
		}
	}

}
