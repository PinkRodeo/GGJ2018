
using LitJson;

namespace Wundee.Stories
{
	public class AddSettlementFlagEffect : Effect
	{
		private ushort _settlementFlag;

		public override void ParseParams(JsonData parameters)
		{
			_settlementFlag = ContentHelper.ParseSettlementFlag(parameters);
		}

		public override void ExecuteEffect()
		{
			parentStoryNode.parentStory.parentPerson.AddFlag(_settlementFlag);
		}
	}

	public class RemoveSettlementFlagEffect : Effect
	{
		private ushort _settlementFlag;

		public override void ParseParams(JsonData parameters)
		{
			_settlementFlag = ContentHelper.ParseSettlementFlag(parameters);
		}

		public override void ExecuteEffect()
		{
			parentStoryNode.parentStory.parentPerson.RemoveFlag(_settlementFlag);
		}
	}

	public class AddWorldFlagEffect : Effect
	{
		private ushort _worldFlag;

		public override void ParseParams(JsonData parameters)
		{
			_worldFlag = ContentHelper.ParseSettlementFlag(parameters);
		}

		public override void ExecuteEffect()
		{
			Game.instance.world.AddFlag(_worldFlag);
		}
	}

	public class RemoveWorldFlagEffect : Effect
	{
		private ushort _worldFlag;

		public override void ParseParams(JsonData parameters)
		{
			_worldFlag = ContentHelper.ParseSettlementFlag(parameters);
		}

		public override void ExecuteEffect()
		{
			Game.instance.world.RemoveFlag(_worldFlag);
		}
	}
}

