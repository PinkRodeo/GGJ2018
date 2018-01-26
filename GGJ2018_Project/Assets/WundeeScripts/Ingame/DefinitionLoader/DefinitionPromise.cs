using LitJson;

namespace Wundee
{
	public class DefinitionPromise<TDefinition, TConcrete> : Definition<TConcrete>
		where TDefinition : Definition<TConcrete>, new()
	{
		private readonly PromiseType _promiseType;

		private TDefinition _definition;

		public DefinitionPromise(string definitionKey)
		{
			_promiseType = PromiseType.HoldsKey;

			this.definitionKey = definitionKey;
		}

		public TDefinition GetDefinition()
		{
			if (_promiseType == PromiseType.HoldsData || _definition != null)
			{
				return _definition;
			}
			else
			{
				// this global misuse = best global misuse
				var newDefinition =
					(Game.instance.definitions.definitionLoaderMapper[typeof (TDefinition)] as
						DefinitionLoader<TDefinition, TConcrete>)[definitionKey];

				_definition = newDefinition;

				return newDefinition;
			}
		}

		public override void ParseDefinition(string definitionKey, JsonData jsonData)
		{
			throw new System.NotImplementedException();
		}

		public override TConcrete GetConcreteType(System.Object parent)
		{
			return GetDefinition().GetConcreteType(parent);
		}

		private enum PromiseType
		{
			HoldsKey,
			HoldsData
		}
	}
}