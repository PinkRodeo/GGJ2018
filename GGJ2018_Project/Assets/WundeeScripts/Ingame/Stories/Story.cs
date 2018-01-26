namespace Wundee.Stories
{
	public class Story
	{
		public StoryNode currentNode;
		public StoryDefinition definition;

		public Person parentPerson;

		public void Tick()
		{

			if (currentNode != null)
			{
				var result = currentNode.Tick();

				if (result == StoryNode.StoryNodeState.Finished)
				{
					SetCurrentStoryNode(null);
				}
			}

		}

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