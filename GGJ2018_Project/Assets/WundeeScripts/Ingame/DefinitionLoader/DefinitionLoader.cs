
using System.Collections.Generic;

namespace Wundee
{
	public class DefinitionLoader<TDefinition, TConcrete> where TDefinition : Definition<TConcrete>, new()
	{
		private readonly Dictionary<string, TDefinition> _definitions;
		private readonly DataLoader _loader;
		
		public DefinitionLoader(DataLoader loader)
		{
			this._loader = loader;
			this._definitions = new Dictionary<string, TDefinition>(100);
		}

		public void AddFolder(string relativePath)
		{
			var yamlFilePaths = _loader.GetAllContentFilePaths(relativePath, "yaml");
			for (var index = 0; index < yamlFilePaths.Length; index++)
			{
				var filePath = yamlFilePaths[index];
				var jsonData = _loader.GetJsonDataFromYamlFile(filePath);

				foreach (var dataKey in jsonData.Keys)
				{
					var newDefinition = new TDefinition();
					newDefinition.ParseDefinition(dataKey, jsonData[dataKey]);
					_definitions[dataKey] = newDefinition;
				}
			}

			var jsonFilePaths = _loader.GetAllContentFilePaths(relativePath, "json");
			for (int index = 0; index < jsonFilePaths.Length; index++)
			{
				var filePath = jsonFilePaths[index];
				var jsonData = _loader.GetJsonDataFromFile(filePath);

				foreach (var dataKey in jsonData.Keys)
				{
					var newDefinition = new TDefinition();
					newDefinition.ParseDefinition(dataKey, jsonData[dataKey]);
					_definitions[dataKey] = newDefinition;
				}
			}
		}

		public TDefinition this[string definitionKey]
		{
			get
			{
				return _definitions[definitionKey];
			}
		}



	}

}
