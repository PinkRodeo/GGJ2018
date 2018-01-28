using System;
using System.Collections.Generic;

using UnityEngine;

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
        public Dictionary<string, Faction> factions = new Dictionary<string, Faction>();

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
            // instantiate the locations
            var locationDefinitions = _game.definitions.locationDefinitions.GetCopy();
            foreach (var locationKey in locationDefinitions.Keys)
            {
                locations.Add(locationKey, locationDefinitions[locationKey].GetConcreteType() as Location);
            }

            var cities = GameObject.FindObjectsOfType<City>();

            var locationsCopy = new Dictionary<string, Location>(locations);

            foreach (var city in cities)
            {
                Location location;
                if (locations.TryGetValue(city.locationDefinitionKey, out location))
                {
                    location.SetCity(city);
                    locationsCopy.Remove(city.locationDefinitionKey);
                }
                else
                {
                    Logger.Error("Couldn't find locationdefinition with key " + city.locationDefinitionKey);
                }
            }

            foreach (var unusedLocationKey in locationsCopy.Keys)
            {
                Logger.Warning("Unused location: " + unusedLocationKey);
            }

            // Instantiate the factions
            var factionDefinitions = _game.definitions.factionDefinitions.GetCopy();
            foreach (var factionKey in factionDefinitions.Keys)
            {
                factions.Add(factionKey, factionDefinitions[factionKey].GetConcreteType() as Faction);
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

