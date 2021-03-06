﻿

using System.Collections;
using System.Collections.Generic;

namespace Wundee
{
	public class DefinitionLoader<TDefinition, TConcrete> where TDefinition : Definition<TConcrete>, new()
	{
		private readonly Dictionary<string, TDefinition> _definitions;
		private readonly DataLoader _loader;
        private readonly string _typeName;

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

        public Dictionary<string, TDefinition> GetCopy()
        {
            return new Dictionary<string, TDefinition>(_definitions); ;
        }

		public TDefinition this[string definitionKey]
		{
			get
			{

#if UNITY_EDITOR
                if (_definitions.ContainsKey(definitionKey) == false)
                {
                    Logger.Error("Cannot find key: " + definitionKey + " of type " + this.ToString());
                }
#endif
                return _definitions[definitionKey];
			}
		}



	}

}
