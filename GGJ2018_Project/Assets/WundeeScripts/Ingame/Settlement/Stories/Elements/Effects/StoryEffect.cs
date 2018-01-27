

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
			parentStoryNode.parentStory.parentLocation.storyHolder.AddStory(_storyKey);
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
			parentStoryNode.parentStory.parentLocation.storyHolder.RemoveStory(_storyKey);
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


    public class TransmitTextEffect: Effect
    {
        private string _textToTransmit;

        public override void ParseParams(JsonData parameters)
        {
            ContentHelper.VerifyKey(parameters, D.TEXT, definition.definitionKey);
            _textToTransmit = parameters[D.TEXT].ToString();
        }

        public override void ExecuteEffect()
        {
            Logger.Log(_textToTransmit);
        }
    }

    public class GoldEffect: Effect
    {
        private int _amount;

        public override void ParseParams(JsonData parameters)
        {
            _amount = ContentHelper.ParseInt(parameters, D.AMOUNT, 0);

            if (_amount == 0)
            {
                Logger.Warning("Only 0 money? " + parentStoryNode.definition.definitionKey);
            }
            
        }

        public override void ExecuteEffect()
        {
            WundeeUnity.GameEntry.gameInstance.world.gold += _amount;
        }
    }

}
