

namespace Wundee.Stories
{
	public class StoryNode
	{
		public StoryNodeDefinition definition;
		public Location parentLocation;
		
		public Effect[] effects;

		public Effect[] onStartRewards;
		public Effect[] onCompleteRewards;

		public StoryChoice[] storyChoices;

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