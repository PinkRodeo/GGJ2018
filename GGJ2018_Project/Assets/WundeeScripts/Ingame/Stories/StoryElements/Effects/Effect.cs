using System.Collections.Generic;
using LitJson;

namespace Wundee.Stories
{
    public abstract class Effect : StoryElement<Effect>
	{
		public abstract void ExecuteEffect();
	}

	public abstract class CollectionEffect : Effect
	{
		protected Definition<Effect>[] _effectDefinitions;
		protected Effect[] effects;
	}

	public class NullEffect : Effect
	{
		public override void ParseParams(JsonData parameters)
		{
		}

		public override void ExecuteEffect()
		{

		}
	}

	public class ConditionalEffect : CollectionEffect
	{
		private enum ConditionalType
		{
			SinglePair,
			MultiplePairs
		}

		private ConditionalType _type;
		
		protected Condition[] conditions;
		private Definition<Condition>[] _conditionDefinitions;

		protected Effect[] elseEffects;
		private Definition<Effect>[] _elseEffectDefinitions;


		private KeyValuePair<Definition<Condition>[], Definition<Effect>[]>[] _conditionEffectPairsDefinition;
		private KeyValuePair<Condition[], Effect[]>[] _conditionEffectPairs;

		public override void ParseParams(JsonData parameters)
		{
			var keys = parameters.Keys;

			if (!keys.Contains(D.STATEMENTS))
			{
				_type = ConditionalType.SinglePair;

				_effectDefinitions = EffectDefinition.ParseDefinitions(parameters[D.EFFECTS], definition.definitionKey);
				_conditionDefinitions = ConditionDefinition.ParseDefinitions(parameters[D.CONDITIONS], definition.definitionKey);

				if (parameters.Keys.Contains("elseEffects"))
				{
					_elseEffectDefinitions = EffectDefinition.ParseDefinitions(parameters["elseEffects"], definition.definitionKey);
				}
				else
				{
					_elseEffectDefinitions = new Definition<Effect>[0];
				}
			}
			else
			{
				_type = ConditionalType.MultiplePairs;

				var conditionEffectData = parameters[D.STATEMENTS];
				var numConditionEffects = conditionEffectData.Count;
				_conditionEffectPairsDefinition =
					new KeyValuePair<Definition<Condition>[], Definition<Effect>[]>[numConditionEffects];

				for (int i = 0; i < numConditionEffects; i++)
				{
					_conditionEffectPairsDefinition[i] = new KeyValuePair<Definition<Condition>[], Definition<Effect>[]>(
						ConditionDefinition.ParseDefinitions(conditionEffectData[i][D.CONDITIONS], definition.definitionKey),
						EffectDefinition.ParseDefinitions(conditionEffectData[i][D.EFFECTS], definition.definitionKey)
					);
				}
			}
		}

		public override Effect GetClone(StoryNode parent)
		{
			var retValue = base.GetClone(parent) as ConditionalEffect;

			if (_type == ConditionalType.SinglePair)
			{
				retValue.conditions = _conditionDefinitions.GetConcreteTypes(parent);
				retValue.effects = _effectDefinitions.GetConcreteTypes(parent);
				retValue.elseEffects = _elseEffectDefinitions.GetConcreteTypes(parent);
			}
			else if (_type == ConditionalType.MultiplePairs)
			{
				retValue._conditionEffectPairs = new KeyValuePair<Condition[], Effect[]>[_conditionEffectPairsDefinition.Length];
				
				for (int i = 0; i < _conditionEffectPairsDefinition.Length; i++)
				{
					retValue._conditionEffectPairs[i] = new KeyValuePair<Condition[], Effect[]>(
						_conditionEffectPairsDefinition[i].Key.GetConcreteTypes(parent),
						_conditionEffectPairsDefinition[i].Value.GetConcreteTypes(parent)
					);
				}
			}
			return retValue;
		}

		public override void ExecuteEffect()
		{
			if (_type == ConditionalType.SinglePair)
			{
				if (conditions.CheckConditions())
				{
					effects.ExecuteEffects();
				}
				else
				{
					elseEffects.ExecuteEffects();
				}
			}
			else
			{
				for (int i = 0; i < _conditionEffectPairs.Length; i++)
				{
					if (_conditionEffectPairs[i].Key.CheckConditions())
					{
						_conditionEffectPairs[i].Value.ExecuteEffects();
						break;
					}
				}
			}

		}
	}

	public class BreakLoopEffect : Effect
	{
		protected Definition<Effect>[] _effectDefinitions;
		public BreakLoopEffect original;
		


		public override void ParseParams(JsonData parameters)
		{
			_effectDefinitions = EffectDefinition.ParseDefinitions(parameters[D.EFFECTS], definition.definitionKey);
		}

		public override void ExecuteEffect()
		{
			var effects = original._effectDefinitions.GetConcreteTypes(parentStoryNode);
			effects.ExecuteEffects();
		}

		public override Effect GetClone(StoryNode parent)
		{
			var clone =  base.GetClone(parent) as BreakLoopEffect;

			clone.original = this;

			return clone;
		}

		
	}

	public class RandomEffect : CollectionEffect
	{
		public override void ParseParams(JsonData parameters)
		{
			_effectDefinitions= EffectDefinition.ParseDefinitions(parameters[D.EFFECTS], definition.definitionKey);

		}

		public override Effect GetClone(StoryNode parent)
		{
			var retValue = base.GetClone(parent) as RandomEffect;

			retValue.effects = _effectDefinitions.GetConcreteTypes(parent);

			return retValue;
		}

		public override void ExecuteEffect()
		{
			var randomNumber = R.Content.Next(effects.Length);
			
			effects[randomNumber].ExecuteEffect();
		}
	}

	public class PrintEffect : Effect
	{
		public string messageToPrint;

		public override void ParseParams(JsonData parameters)
		{
			messageToPrint = parameters.ToString();
		}

		public override void ExecuteEffect()
		{
			Logger.Print(messageToPrint);
		}
	}
}