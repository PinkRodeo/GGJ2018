
using LitJson;

namespace Wundee
{
	public abstract class Definition<TConcrete>
	{
		public string definitionKey;

		public abstract void ParseDefinition(string definitionKey, JsonData jsonData);
		public abstract TConcrete GetConcreteType(System.Object parent = null); 
	}
}