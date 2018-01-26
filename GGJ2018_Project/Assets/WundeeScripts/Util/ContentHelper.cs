
using System;
using System.Collections.Generic;
using LitJson;
using Wundee.Stories;

using System.Diagnostics;
using System.Linq;
using NCalc;

namespace Wundee
{
	public static class ContentHelper
	{
		public static Definition<TConcrete> GetDefinition<TDefinition, TConcrete>(JsonData definitionData, string definitionKey, string definitionKeyExtension, int definitionCount = 0) where TDefinition : Definition<TConcrete>, new()
		{
			if (definitionData.IsString)
				return new DefinitionPromise<TDefinition, TConcrete>(definitionData.ToString());
			else
			{
				var definition = new TDefinition();
				definition.ParseDefinition(definitionKey + definitionKeyExtension + definitionCount, definitionData);

				return definition;
			}
		}

		public static Definition<TConcrete>[] GetDefinitions<TDefinition, TConcrete>(JsonData definitionData, string definitionKey, string definitionKeyExtension) where TDefinition : Definition<TConcrete>, new()
		{
			var returnValue = new Definition<TConcrete>[definitionData.Count];

			for (int i = 0; i < definitionData.Count; i++)
			{
				returnValue[i] = GetDefinition<TDefinition, TConcrete>(
					definitionData[i], definitionKey, definitionKeyExtension, i
				);
			}
			
			return returnValue;
		}

		public static T[] GetConcreteTypes<T>(this Definition<T>[] definitions, Object parent)
		{
			var concreteTypes = new T[definitions.Length];

			for (int i = 0; i < definitions.Length; i++)
			{
				concreteTypes[i] = definitions[i].GetConcreteType(parent);
			}

			return concreteTypes;
		}

		public static void ExecuteEffects<T>(this T[] effects) where T : Effect
		{
			for (int i = 0; i < effects.Length; i++)
			{
				effects[i].ExecuteEffect();
			}
		}

		public static bool CheckConditions<T>(this T[] conditions) where T : Condition
		{
			for (int i = 0; i < conditions.Length; i++)
			{
				if (conditions[i].Check() == false)
					return false;
			}

			return true;
		}

		public static bool CheckOrConditions<T>(this T[] conditions) where T : Condition
		{
			for (int i = 0; i < conditions.Length; i++)
			{
				if (conditions[i].Check() == true)
					return true;
			}

			return false;
		}

		public static double ParseDouble(JsonData jsonData, string key, double defaultValue)
		{
			if (jsonData.Keys.Contains(key))
			{
				var value = jsonData[key];
				if (value.IsDouble)
					return (double) value;
				else if (value.IsInt)
					return (int) value;
				else
				{
					var expression = new Expression(value.ToString());

					expression.Parameters = Game.instance.@params.constants ;
					expression.EvaluateParameter += (string name, ParameterArgs args) =>
					{
						if (name == P.RANDOM)
						{
							args.Result = R.Content.NextDouble();
							args.HasResult = true;
							return;
						}
						args.HasResult = false;
					};
					// TODO: exception handling
					// http://ncalc.codeplex.com/
					return (double) expression.Evaluate();
				}

			}

			return defaultValue;
		}

		public static float ParseFloat(JsonData jsonData, string key, float defaultValue)
		{
			return (float) ParseDouble(jsonData, key, defaultValue);
		}

		public static int ParseInt(JsonData jsonData, string key, int defaultValue)
		{
			if (jsonData.Keys.Contains(key))
			{
				var value = jsonData[key];
				if (value.IsInt)
					return (int)value;
				else if (value.IsDouble)
				{
					Logger.Error("Tried parsing double as an int with key " + key + " and defaultvalue " + defaultValue);
					return (int)(double)value;
				}
				else
				{
					var expression = new Expression(value.ToString());

					expression.Parameters = Game.instance.@params.constants;
					expression.EvaluateParameter += (string name, ParameterArgs args) =>
					{
						if (name == P.RANDOM)
						{
							args.Result = R.Content.NextDouble();
							args.HasResult = true;
							return;
						}
						args.HasResult = false;
					};
					// TODO: exception handling
					// http://ncalc.codeplex.com/
					return (int)(double)expression.Evaluate();
				}

			}

			return defaultValue;
		}

		public static bool ParseBool(JsonData jsonData, string key, bool defaultValue)
		{
			if (jsonData.Keys.Contains(key))
			{
				var value = jsonData[key];
				if (value.IsBoolean)
					return (bool) value;
				else
				{
					Logger.Error("Tried parsing invalid bool with key " + key + " and defaultvalue " + defaultValue);
					
					return defaultValue;
				}
			}
			return defaultValue;
		}

		public static string ParseString (JsonData jsonData, string key, string defaultValue)
		{
			if (jsonData.Keys.Contains(key))
			{
				var value = jsonData[key];
				if (value.IsString)
					return (string) value;
				else
				{
					Logger.Error("Tried parsing invalid string with key " + key + " and defaultvalue " + defaultValue);

					return defaultValue;
				}
			}
			return defaultValue;
		}


		public static int ParseNeedIndex(JsonData jsonData, string key)
		{
			if (!jsonData.Keys.Contains(key)) 
				return -1;

			var need = jsonData[key].ToString();

			for (int i = 0; i < Game.instance.@params.needParams.needs.Length; i++)
			{
				if (Game.instance.@params.needParams.needs[i] == need)
					return i;
			}

			return -1;
		}

		public static Operator ParseOperator(JsonData jsonData, string key = D.OPERATOR)
		{
			return OperatorExtensions.stringToOperator[jsonData[key].ToString()];
		}

		public static Operator ParseOperator(JsonData jsonData, Operator defaultValue, string key = D.OPERATOR)
		{
			if (! jsonData.Keys.Contains(key))
				return defaultValue;
			
			return OperatorExtensions.stringToOperator[jsonData[key].ToString()];
		}

		private static Dictionary<string, ushort> _settlementFlags = new Dictionary<string, ushort>(100); 
		public static ushort ParseSettlementFlag(JsonData jsonData, string key = D.FLAG)
		{
			if (!jsonData.Keys.Contains(key))
			{
				Logger.Warning("Couldn't parse flag with key " + key);
				return ushort.MaxValue;
			}
			var flag = jsonData[key].ToString();

			return ParseSettlementFlag(flag);

		}

		public static ushort ParseSettlementFlag(string flag)
		{
			if (_settlementFlags.ContainsKey(flag))
				return _settlementFlags[flag];

			var newFlagShort = (ushort)_settlementFlags.Count;

			_settlementFlags[flag] = newFlagShort;

			return newFlagShort;
		}

		private static Dictionary<string, ushort> _worldFlags = new Dictionary<string, ushort>(100);
		public static ushort ParseWorldFlag(JsonData jsonData, string key = D.FLAG)
		{
			if (!jsonData.Keys.Contains(key))
			{
				Logger.Warning("Couldn't parse flag with key " + key);
				return ushort.MaxValue;
			}
			var flag = jsonData[key].ToString();

			if (_worldFlags.ContainsKey(flag))
				return _settlementFlags[flag];

			var newFlagShort = (ushort)_worldFlags.Count;

			_worldFlags[flag] = newFlagShort;

			return newFlagShort;
		}

		[Conditional("DEBUG_CONTENT")]
		public static void VerifyKey(JsonData jsonData, string key, string ownerKey)
		{
			if (!jsonData.Keys.Contains(key))
			{
				Logger.Error("Missing key <b>" + key + "</b> in jsonData with key <b>" + ownerKey + "</b>", 1);
			}
		}

		[Conditional("DEBUG_CONTENT")]
		public static void VerifyArrayLength(System.Object[] array, int length, string ownerKey)
		{
			if (array.Length != length)
			{
				Logger.Error("Invalid length <b>" + length + "</b> in jsonData with key <b>" + ownerKey + "</b>", 1);
			}
		}

		[Conditional("DEBUG_CONTENT")]
		public static void VerifyMaxArrayLength(System.Object[] array, int length, string ownerKey)
		{
			if (array.Length > length)
			{
				Logger.Error("Invalid length <b>" + length + "</b> in jsonData with key <b>" + ownerKey + "</b>", 1);
			}
		}


		[Conditional("DEBUG_CONTENT")]
		public static void VerifyType(Dictionary<string, System.Type> typeDictionary, string type, string ownerKey)
		{
			if (!typeDictionary.ContainsKey(type))
			{
				Logger.Error("Invalid type <b>" + type + "</b> for effect with key <b>" + ownerKey + "</b>", 1);
			}
		}

		[Conditional("DEBUG_CONTENT")]
		public static void VerifyOperator(JsonData jsonData, string key, string ownerKey)
		{
			VerifyKey(jsonData, key, ownerKey);

			var enumValue = jsonData[key].ToString();

			if (!OperatorExtensions.stringToOperator.Keys.Contains(enumValue))
			{
				Logger.Error("Invalid operator <b>" + enumValue + "</b> in jsonData with key <b>" + ownerKey + "</b>", 1);
			}
		}

		[Conditional("DEBUG_CONTENT")]
		public static void VerifyDouble(JsonData jsonData, string key, string ownerKey)
		{
			VerifyKey(jsonData, key, ownerKey);

			var doubleValue = jsonData[key];

			if (!doubleValue.IsDouble && !doubleValue.IsInt)
			{
				Logger.Error("Invalid double <b>" + key + "</b>, " + doubleValue.ToString() + " in jsonData with key <b>" + ownerKey + "</b>", 1);
			}
		}
	}
}

