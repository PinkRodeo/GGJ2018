

namespace Wundee.Stories
{
	public class StoryNode
	{
		public StoryNodeDefinition definition;
		public Story parentStory;
		
		public Effect[] effects;

		public Effect[] onStartRewards;
		public Effect[] onCompleteRewards;

		public StoryChoice[] storyTriggers;

		public void OnStart()
		{
			onStartRewards.ExecuteEffects();
		}

		public void OnComplete()
		{
			onCompleteRewards.ExecuteEffects();
		}
	}
}