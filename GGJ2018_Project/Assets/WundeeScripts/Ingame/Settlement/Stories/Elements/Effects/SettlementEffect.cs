
using System.Collections.Generic;
using LitJson;

namespace Wundee.Stories
{
	public interface ISettlementEffect
	{
		
	}
	
	public abstract class SettlementEffect : Effect, ISettlementEffect
	{
		protected Definition<Condition>[] _conditionDefinitions;
		protected Definition<Effect>[] _effectDefinitions;  

		public override void ParseParams(JsonData parameters)
		{
			var keys = parameters.Keys;

			if (keys.Contains(D.CONDITIONS))
				_conditionDefinitions = ConditionDefinition.ParseDefinitions(parameters[D.CONDITIONS], definition.definitionKey);
			else
				_conditionDefinitions = new Definition<Condition>[0];

			if (keys.Contains(D.EFFECTS))
				_effectDefinitions = EffectDefinition.ParseDefinitions(parameters[D.EFFECTS], definition.definitionKey);
			else
				_effectDefinitions = new Definition<Effect>[0];
		}

		protected void ExecuteOnSettlements(List<Location> settlements)
		{
			for (int i = 0; i < settlements.Count; i++)
			{
				if (settlements[i].CheckConditionFromDefinition(ref _conditionDefinitions))
					settlements[i].ExecuteEffectFromDefinition(ref _effectDefinitions);
			}
		}
		protected void ExecuteOnSettlement(Location p_Person)
		{
			if (p_Person.CheckConditionFromDefinition(ref _conditionDefinitions))
				p_Person.ExecuteEffectFromDefinition(ref _effectDefinitions);
		}
	}


	/*
	public class NearbySettlementEffect : SettlementEffect
	{
		private float _effectRange = 50f;

		public override void ParseParams(JsonData parameters)
		{
			base.ParseParams(parameters);

			_effectRange = ContentHelper.ParseFloat(parameters, D.RANGE, _effectRange);
		}

		public override void ExecuteEffect()
		{
			var physicsWorld = Game.instance.world.physicsWorld;

			var testSettlement = parentStoryNode.parentStory.parentSettlement;
			var testPosition = testSettlement.habitat.position;

			var testSphere = new CircleShape(_effectRange, 1f);
			Transform testTransform;
			testSettlement.habitat.body.GetTransform(out testTransform);

			// Used to check for duplicates, if an entity has multiple physicsShapes,
			// multiple fixtures from the same body may be returned by the AABB query
			var fixturesTested = new List<Fixture>(8);

			var aaBBQuery = new AABB(testPosition, _effectRange*3f, _effectRange*3f);
			physicsWorld.QueryAABB((Fixture fixture) =>
			{
				var userData = fixture.Body.UserData;
				var habitat = userData as Habitat;

				if (habitat == null || fixturesTested.Contains(fixture))
					return true;

				var settlement = habitat.occupant as Settlement;
				if (settlement == null || settlement == testSettlement)
					return true;

				Transform bodyTransform;
				fixture.Body.GetTransform(out bodyTransform);

				var bodyFixtures = fixture.Body.FixtureList;
				var numFixtures = bodyFixtures.Count;

				for (int i = 0; i < numFixtures; i++)
				{
					if (Collision.TestOverlap(bodyFixtures[i].Shape, 0, 
                                              testSphere, 0, 
											  ref bodyTransform, ref testTransform))
					{
						ExecuteOnSettlement(settlement);
						fixturesTested.Add(fixture);
					}
				}

				return true;
			}, ref aaBBQuery);
		}
	}
	*/
}

