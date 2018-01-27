using System;
using System.Collections.Generic;

namespace Wundee
{
    public class World
	{
		private HashSet<ushort> _worldFlags = new HashSet<ushort>();

        public int gold
        {
            get
            {
                return _gold;
            }
            set
            {
                int previousValue = gold;
                _gold = Math.Max(0, value);

                OnGoldChanged(previousValue, _gold);
            }
        }

        private int _gold;

        public Dictionary<string, Location> locations = new Dictionary<string, Location>();

        /// <summary>
        /// previousAmount, newAmount
        /// </summary>
        public System.Action<int, int> OnGoldChanged = delegate { };

        private Game _game;
        
        public World(Game game)
		{
            _game = game;
		}

        public void Initialize()
        {
            var locationDefinitions = _game.definitions.locationDefinitions.GetCopy();
            foreach (var locationKey in locationDefinitions.Keys)
            {
                locations.Add(locationKey, locationDefinitions[locationKey].GetConcreteType() as Location);
            }


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

