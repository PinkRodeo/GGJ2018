namespace Wundee.Stories
{
	public class Story
	{
		public StoryNode currentNode;
		public StoryDefinition definition;
		public Location parentLocation;
        
		public void SetCurrentStoryNode(StoryNode storyNode)
		{
			if (currentNode != null)
				currentNode.OnComplete();

			currentNode = storyNode;

			if (currentNode != null)
				currentNode.OnStart();
		}
	}
}