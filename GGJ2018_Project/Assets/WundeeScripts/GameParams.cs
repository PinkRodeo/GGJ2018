﻿// ReSharper disable InconsistentNaming


using System.Collections.Generic;
using LitJson;

namespace Wundee
{
	public class GameParams
	{
		public bool parseDefinitions = true;

		public Dictionary<string, System.Object> constants = new Dictionary<string, System.Object>(100); 

		public NeedParams needParams = new NeedParams();
		public double timeMultiplier = 1d;

        public int startingPawns;
        public int startingGold;

        public void InitializeFromData(JsonData gameParamData)
		{
			var constantsData = gameParamData[P.CONSTANTS];
			var constantKeys = constantsData.Keys;

            startingPawns = ContentHelper.ParseInt(gameParamData, P.STARTING_PAWNS, 9001);
            startingGold = ContentHelper.ParseInt(gameParamData, P.STARTING_GOLD, 9001);


            foreach (var constantKey in constantKeys)
			{
				constants[constantKey] = ContentHelper.ParseDouble(constantsData, constantKey, 0d);
			}
			
			timeMultiplier = ContentHelper.ParseDouble(gameParamData, P.TIME_MULTIPLIER, this.timeMultiplier);

			if (gameParamData.Keys.Contains(P.NEED_PARAMS))
			{
				var needParamData = gameParamData[P.NEED_PARAMS];

				if (needParamData.Keys.Contains(P.NEEDS))
				{
					var needsData = needParamData[P.NEEDS];

					if (needsData.IsArray)
					{
						var count = needsData.Count;
						needParams.needs = new string[count];

						for (int i = 0; i < count; i++)
						{
							needParams.needs[i] = needsData[i].ToString();
						}
					}
				}
			}

		}

		public bool IsValidNeed(string potentialNeed)
		{
			for (int i = 0; i < needParams.needs.Length; i++)
			{
				if (needParams.needs[i] == potentialNeed)
					return true;
			}

			return false;
		}
	}

	public class NeedParams
	{
		public string[] needs = new string[]
		{
			"Population",
			"Goods",
			"Defense",
			"Satisfaction",
		};
	}

	public static class P
	{
        public const string STARTING_PAWNS = "startingPawns";
        public const string STARTING_GOLD = "startingGold";

        public const string TIME_MULTIPLIER = "timeMultiplier";
		public const string CONSTANTS = "constants";

		public const string RANDOM = "RANDOM";


		public const string NEED_PARAMS = "needParams";
		public const string NEEDS = "needs";

	}

	// Definition keys, for lookup in Json files
	public static class D
	{
        public static string NAME = "name";

		public static string TEXT = "text";
		public static string PORTRAITKEY = "portraitKey";
		public static string UNLOCKAXIS = "unlockAxis";

		// Stories
		public const string START_NODE = "startNode";
		public const string STORYCHOICES = "storyChoices";

		public const string EFFECTS = "effects";
		public const string CONDITIONS = "conditions";
        public const string ENABLED_CONDITIONS = "enabledConditions";
        public const string REWARDS = "rewards";

		public const string REWARDS_ON_START = "onStartRewards";
		public const string REWARDS_ON_COMPLETE = "onCompleteRewards";

		public const string EFFECTS_ON_COMPLETE = "onCompleteEffects";
		public const string EFFECTS_ON_BREAKOFF = "onBreakoffEffects";



		public const string REWARDS_ON_TUNE = "onTuneRewards";




		// StoryElements
		public const string TYPE = "type";
		public const string PARAMS = "params";

		public const string OPERATOR = "operator";
		public const string AMOUNT = "amount";

		public const string RATE = "rate";

		// Effects

		public const string STATEMENTS = "statements";

		public const string SPEED = "speed";

		public const string NEED = "need";

		public const string RANGE = "range";


		// StoryEffects
		public const string STORY_KEY = "storyKey";
        public const string CONVERSATION = "conversation";
        public const string LOCATION = "location";
        // Conditions
        public const string FLAG = "flag";


        // Location
		public const string HAS_FLAG = "hasFlag";


        // Factions
        public const string STARTING_REPUTATION = "startingReputation";
        public const string FACTION_KEY = "factionKey";

    }

    // Strings that are used to extend the definitionKey for definitions that are declared within its parent
    public static class KEYS
	{
		public const string STORYNODE = "_STORYNODE_";
		public const string STORYTRIGGER = "_STORYTRIGGER_";

		public const string EFFECT = "_EFFECT_";
		public const string CONDITION = "_CONDITION_";
		public const string REWARD = "_REWARD_";


	}
}
