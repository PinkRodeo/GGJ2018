

using LitJson;

namespace Wundee.Stories
{

	public interface IStoryEffect
	{

	}

	public class AddStoryEffect : Effect, IStoryEffect
	{
		private string _storyKey;

		public override void ParseParams(JsonData parameters)
		{
			ContentHelper.VerifyKey(parameters, D.STORY_KEY, definition.definitionKey);
			_storyKey = parameters[D.STORY_KEY].ToString();
		}

		public override void ExecuteEffect()
		{
			parentStoryNode.parentStory.parentPerson.storyHolder.AddStory(_storyKey);
		}
	}

	public class RemoveStoryEffect : Effect, IStoryEffect
	{
		private string _storyKey;

		public override void ParseParams(JsonData parameters)
		{
			ContentHelper.VerifyKey(parameters, D.STORY_KEY, definition.definitionKey);
			_storyKey = parameters[D.STORY_KEY].ToString();
		}

		public override void ExecuteEffect()
		{
			parentStoryNode.parentStory.parentPerson.storyHolder.RemoveStory(_storyKey);
		}
	}

	public class StartStoryNodeEffect : Effect, IStoryEffect
	{
		private string _storyNode;


		public override void ParseParams(JsonData parameters)
		{
			
			throw new System.NotImplementedException();
		}

		public override void ExecuteEffect()
		{
			//parentStoryNode.parentStory.SetCurrentStoryNode();
		}
	}


	public class CompleteStoryNodeEffect : Effect, IStoryEffect
	{
		public override void ParseParams(JsonData parameters)
		{

		}

		public override void ExecuteEffect()
		{
			parentStoryNode.parentStory.SetCurrentStoryNode(null);
		}
	}



}
