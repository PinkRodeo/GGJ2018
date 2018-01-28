
using LitJson;

namespace Wundee.Stories
{
	public class AddLocationFlagEffect : Effect
	{
		private ushort _locationFlag;

		public override void ParseParams(JsonData parameters)
		{
			_locationFlag = ContentHelper.ParseLocationFlag(parameters);
		}

		public override void ExecuteEffect()
		{
			parentStoryNode.parentLocation.AddFlag(_locationFlag);
		}
	}

	public class RemoveLocationFlagEffect : Effect
	{
		private ushort _locationFlag;

		public override void ParseParams(JsonData parameters)
		{
			_locationFlag = ContentHelper.ParseLocationFlag(parameters);
		}

		public override void ExecuteEffect()
		{
			parentStoryNode.parentLocation.RemoveFlag(_locationFlag);
		}
	}

	public class AddWorldFlagEffect : Effect
	{
		private ushort _worldFlag;

		public override void ParseParams(JsonData parameters)
		{
			_worldFlag = ContentHelper.ParseLocationFlag(parameters);
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
			_worldFlag = ContentHelper.ParseLocationFlag(parameters);
		}

		public override void ExecuteEffect()
		{
			Game.instance.world.RemoveFlag(_worldFlag);
		}
	}
}

