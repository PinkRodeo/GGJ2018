

namespace Wundee.Stories
{
	public class StoryNode
	{
		public enum StoryNodeState
		{
			Running,
			Finished
		}

		public StoryNodeDefinition definition;
		public Story parentStory;
		
		public Effect[] effects;

		public Effect[] onStartRewards;
		public Effect[] onCompleteRewards;

		public StoryTrigger[] storyTriggers;

		public StoryNodeState Tick()
		{
			for (int i = 0; i < storyTriggers.Length; i++)
			{
				if (storyTriggers[i].CheckTrigger())
				{
					return StoryNodeState.Finished;
				}
			}

			effects.ExecuteEffects();
			
			return StoryNodeState.Running;
		}

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