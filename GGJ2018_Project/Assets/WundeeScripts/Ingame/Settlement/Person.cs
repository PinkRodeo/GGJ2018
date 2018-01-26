
using System.Collections.Generic;
using Wundee.Stories;
using UnityEngine;

namespace Wundee
{
    public class Person
	{
		public readonly StoryHolder storyHolder;

        private double _timeOfPreviousUpdate;

		private HashSet<ushort> _settlementFlags = new HashSet<ushort>();
		public PersonDefinition definition;

		private AudioClip[] audioClips;

		public Person(PersonDefinition p_Definition)
		{
			this.definition = p_Definition;

			this.storyHolder = new StoryHolder(this);

			string voiceFolder = "Voices/" + definition.voiceType;
			Object[] assets = Resources.LoadAll (voiceFolder);
			int ii;

			audioClips = new AudioClip[assets.Length];

			for (ii = 0; ii < assets.Length; ii++){
				audioClips[ii] = assets[ii] as AudioClip;
			};

			
			var needParams = Game.instance.@params.needParams;

			/*
			this.needs = new Need[needParams.needs.Length];
			this.needsDictionary = new Dictionary<string, Need>(needParams.needs.Length);

			for (int i = 0; i < needParams.needs.Length; i++)
			{
				needs[i] = new Need(this, needParams.needs[i]);
				needsDictionary[needParams.needs[i]] = needs[i];
			}
			*/

			
		}

		public AudioClip[] GetAudioClips(){
			return audioClips;
		}

		public float GetAudioVolume(){
			return definition.voiceVolume;
		}

		public void TuneIn()
		{

			ExecuteEffectFromDefinition(ref this.definition._onTuneInRewardDefinitions);
		}
		
		public void Tick()
		{
			var deltaTime = Time.fixedGameTime - _timeOfPreviousUpdate;

			storyHolder.Tick();

			_timeOfPreviousUpdate = Time.fixedGameTime;
		}

		public void ExecuteEffectFromDefinition(ref Definition<Effect>[] effectDefinitions)
		{
			var effects = effectDefinitions.GetConcreteTypes(storyHolder.lifeStoryNode);
			effects.ExecuteEffects();
		}

		public bool CheckConditionFromDefinition(ref Definition<Condition>[] conditionDefinitions)
		{
			var conditions = conditionDefinitions.GetConcreteTypes(storyHolder.lifeStoryNode);
			return conditions.CheckConditions();
		}

		public void AddFlag(ushort flag)
		{
			_settlementFlags.Add(flag);
		}

		public void RemoveFlag(ushort flag)
		{
			_settlementFlags.RemoveWhere((ushort flagToTest) =>
			{
				if (flagToTest == flag)
					return true;

				return false;
			});
		}

		public bool HasFlag(ushort flag)
		{
			return _settlementFlags.Contains(flag);
		}
	}

}
