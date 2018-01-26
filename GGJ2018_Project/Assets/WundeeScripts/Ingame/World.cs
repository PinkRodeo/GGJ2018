using System;
using System.Collections.Generic;

namespace Wundee
{
    public class World
	{
		private HashSet<ushort> _worldFlags = new HashSet<ushort>();

		public World()
		{
			var gameParams = Game.instance.@params;
		}


		public void AddFlag(ushort flag)
		{
			_worldFlags.Add(flag);
		}

		public void RemoveFlag(ushort flag)
		{
			_worldFlags.RemoveWhere((ushort flagToTest) =>
			{
				if (flagToTest == flag)
					return true;

				return false;
			});
		}

		public bool HasFlag(ushort flag)
		{
			return _worldFlags.Contains(flag);
		}
	}
}

