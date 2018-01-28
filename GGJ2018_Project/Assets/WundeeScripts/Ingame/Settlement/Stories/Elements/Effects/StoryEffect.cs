﻿

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
			parentStoryNode.parentLocation.storyHolder.AddStoryNode(_storyKey);
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
			parentStoryNode.parentLocation.storyHolder.RemoveStory(_storyKey);
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

    public class PawnEffect : Effect
    {
        private int _amount;

        public override void ParseParams(JsonData parameters)
        {
            _amount = ContentHelper.ParseInt(parameters, D.AMOUNT, 0);

            if (_amount == 0)
            {
                Logger.Warning("Only 0 pawns effect? " + parentStoryNode.definition.definitionKey);
            }

        }

        public override void ExecuteEffect()
        {
            WundeeUnity.GameEntry.gameInstance.world.pawns += _amount;
        }
    }

    public class ReputationEffect: Effect
    {
        private int _amount;
        private string _targetFaction;

        public override void ParseParams(JsonData parameters)
        {
            _amount = ContentHelper.ParseInt(parameters, D.AMOUNT, 0);

            _targetFaction = ContentHelper.ParseString(parameters, D.FACTION_KEY, parentStoryNode.parentLocation.owningFaction.definition.definitionKey);
        }

        public override void ExecuteEffect()
        {
            parentStoryNode.parentLocation.world.factions[_targetFaction].reputation += _amount;
        }
    }


    public class ConversationEffect: Effect
    {
        protected string _conversationKey;
        protected string _locationKey;


        public override void ParseParams(JsonData parameters)
        {
            if (parameters.IsString)
            {
                _conversationKey = parameters.ToString();
                _locationKey = null;
            }
            else
            {
                _conversationKey = ContentHelper.ParseString(parameters, D.CONVERSATION, "CONVERSATION_FALLBACK");

                if (_conversationKey == "CONVERSATION_FALLBACK")
                {
                    Logger.Warning("Fallback conversation for: " + parentStoryNode.definition.definitionKey);
                }
                _locationKey = ContentHelper.ParseString(parameters, D.LOCATION, null);

            }
        }

        public override void ExecuteEffect()
        {
            if (_locationKey == null)
            {
                _locationKey = parentStoryNode.parentLocation.definition.definitionKey;
            }
            Game.instance.conversationUI.SetToStoryNode(Game.instance.world.locations[_locationKey].storyHolder.AddStoryNode(_conversationKey));
        }
    }

    public class StartSpyMessageEffect : ConversationEffect
    {
        public override void ExecuteEffect()
        {
            base.ExecuteEffect();

            Game.instance.conversationUI.SetSpyMessageMode();
            Game.instance.conversationUI.SetStartPosition();
        }
    }

    public class StartRegularMessageEffect : ConversationEffect
    {
        public override void ExecuteEffect()
        {
            base.ExecuteEffect();

            Game.instance.conversationUI.SetRegularMessageMode();
            Game.instance.conversationUI.SetStartPosition();
        }
    }

}
