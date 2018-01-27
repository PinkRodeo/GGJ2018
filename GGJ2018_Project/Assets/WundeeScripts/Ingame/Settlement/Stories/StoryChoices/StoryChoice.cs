
namespace Wundee.Stories
{
	public class StoryChoice
	{
		public StoryChoiceDefinition definition;

		public Condition[] conditions;
        public Condition[] enabledConditions;
        public StoryNode parentStoryNode;
		public Effect[] rewards;
	}

}
