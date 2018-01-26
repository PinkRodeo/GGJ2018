namespace Wundee.Stories
{

    public class StoryHolder
	{
		private Person owner;

		private Story[] activeStories;

		public readonly Story lifeStory;
		public readonly StoryNode lifeStoryNode;


		public StoryHolder(Person owner)
		{
			this.owner = owner;

			lifeStory = new Story();
			lifeStoryNode = new StoryNode();

			lifeStory.currentNode = lifeStoryNode;
			lifeStory.parentPerson = owner;
			lifeStoryNode.parentStory = lifeStory;


			activeStories = new Story[0];
		}

		public void Tick()
		{
			for (int index = 0; index < activeStories.Length; index++)
			{
				activeStories[index].Tick();
			}
		}

		public void AddStory(string definitionKey)
		{
			var storyDefinition = Game.instance.definitions.storyDefinitions[definitionKey];

			var newStories = new Story[activeStories.Length + 1];


			activeStories.CopyTo(newStories, 0);
			newStories[activeStories.Length] = storyDefinition.GetConcreteType(owner);

			activeStories = newStories;
		}

		public void RemoveStory(string definitionKey)
		{
			for (int i = 0; i < activeStories.Length; i++)
			{
				if (activeStories[i].definition.definitionKey == definitionKey)
				{
					RemoveStory(i);
					return;
				}
			}
		}

		public void RemoveStory(int index)
		{
			activeStories = activeStories.RemoveAt(index);
		}

		public bool IsStoryActive(string storyDefinitionKey)
		{
			for (int i = 0; i < activeStories.Length; i++)
			{
				if (activeStories[i].definition.definitionKey == storyDefinitionKey)
				{
					return true;
				}
			}

			return false;
		}
	}


}