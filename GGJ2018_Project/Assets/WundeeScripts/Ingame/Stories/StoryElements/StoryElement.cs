using LitJson;


namespace Wundee.Stories
{
    public abstract class StoryElement<TChild> where TChild : StoryElement<TChild>
	{
		public StoryElementDefinition<TChild> definition;
		public StoryNode parentStoryNode;
		
		public abstract void ParseParams(JsonData parameters);

		public virtual TChild GetClone(StoryNode parent)
		{
			this.parentStoryNode = parent;
			return (TChild) MemberwiseClone();
		}
		
	}
}