
using LitJson;


namespace Wundee.Stories
{
	public class StoryDefinition : Definition<Story>
	{
		private Definition<StoryNode> _startNode;

		public override void ParseDefinition(string definitionKey, JsonData jsonData)
		{
			this.definitionKey = definitionKey;

				this._startNode = ContentHelper.GetDefinition<StoryNodeDefinition, StoryNode>(jsonData[D.START_NODE],
					definitionKey, KEYS.STORYNODE);
			}

		public override Story GetConcreteType(System.Object parent = null)
		{
			var newStory = new Story
			{
				definition = this,
				parentPerson = parent as Person
			};
			
			if (newStory.parentPerson == null)
				Logger.Log("[StoryDefinition] Invalid parent Settlement provided for new Story");

			newStory.SetCurrentStoryNode(_startNode.GetConcreteType(newStory));

			return newStory;
		}
	}

}

