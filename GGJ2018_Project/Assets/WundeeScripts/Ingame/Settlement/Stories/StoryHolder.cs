using System.Collections.Generic;

namespace Wundee.Stories
{

    public class StoryHolder
	{
		private Location owner;

		private List<StoryNode> activeStories;

        public readonly StoryNode lifeStoryNode;

		public StoryHolder(Location owner)
		{
			this.owner = owner;

            lifeStoryNode = new StoryNode();
            lifeStoryNode.parentLocation = owner;

            activeStories = new List<StoryNode>();
		}

		public void Tick()
		{

        }

		public StoryNode AddStoryNode(string definitionKey)
		{
			var storyDefinition = Game.instance.definitions.storyNodeDefinitions[definitionKey];
            var storyNode = storyDefinition.GetConcreteType(owner);
            activeStories.Add(storyNode);

            return storyNode;
		}

		public void RemoveStory(string definitionKey)
		{
            StoryNode storyNodeToRemove = null;

            foreach (var storyNode in activeStories)
            {
                if (storyNode.definition.definitionKey == definitionKey)
                {
                    storyNodeToRemove = storyNode;
                }
                
            }

            if (storyNodeToRemove != null)
            {
                activeStories.Remove(storyNodeToRemove);
            }
		}

		public bool IsStoryActive(string storyDefinitionKey)
		{
			for (int i = 0; i < activeStories.Count; i++)
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