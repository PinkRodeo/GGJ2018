﻿using System;
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
                int previousValue = _gold;
                _gold = Math.Max(0, value);

                OnGoldChanged(previousValue, _gold);
            }
        }

        private int _gold;

        /// <summary>
        /// previousAmount, newAmount
        /// </summary>
        public System.Action<int, int> OnGoldChanged = delegate { };

        public int pawns
        {
            get
            {
                return _pawns;
            }
            set
            {
                int previousValue = _pawns;
                _pawns = Math.Max(0, value);

                OnPawnsChanged(previousValue, _pawns);
            }
        }
        private int _pawns;
        /// <summary>
        /// previousAmount, newAmount
        /// </summary>
        public System.Action<int, int> OnPawnsChanged = delegate { };


        public Dictionary<string, Location> locations = new Dictionary<string, Location>();
        public Dictionary<string, Faction> factions = new Dictionary<string, Faction>();


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
                var location = locationDefinitions[locationKey].GetConcreteType() as Location;
                locations.Add(locationKey, location);
                location.SetWorld(this);
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


            _gold = _game.@params.startingGold;
            _pawns = _game.@params.startingPawns;
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

