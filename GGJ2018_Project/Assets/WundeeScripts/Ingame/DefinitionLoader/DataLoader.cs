

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

using LitJson;
using Wundee.Stories;

using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Wundee
{
	public class DataLoader
	{
		public DefinitionLoader<PersonDefinition, Location> personDefinitions; 

		public DefinitionLoader<StoryDefinition, Story> storyDefinitions;
		public DefinitionLoader<StoryNodeDefinition, StoryNode> storyNodeDefinitions;
		public DefinitionLoader<StoryChoiceDefinition, StoryChoice> storyTriggerDefinitions;

		public DefinitionLoader<EffectDefinition, Effect> effectDefinitions;
		public DefinitionLoader<ConditionDefinition, Condition> conditionDefinitions;


		public Dictionary<Type, object> definitionLoaderMapper;

		private Serializer jsonSerializer;
		private Deserializer yamlDeserializer;

		private FileSystemWatcher _fileSystemWatcher;

		public DataLoader()
		{
			personDefinitions = new DefinitionLoader<PersonDefinition, Location>(this);

			storyDefinitions = new DefinitionLoader<StoryDefinition, Story>(this);
			storyNodeDefinitions = new DefinitionLoader<StoryNodeDefinition, StoryNode>(this);
			storyTriggerDefinitions = new DefinitionLoader<StoryChoiceDefinition, StoryChoice>(this);

			effectDefinitions = new DefinitionLoader<EffectDefinition, Effect>(this);
			conditionDefinitions = new DefinitionLoader<ConditionDefinition, Condition>(this);
            
			this.definitionLoaderMapper = new Dictionary<Type, object>(10);

			
			definitionLoaderMapper[typeof(PersonDefinition)] = personDefinitions;

			definitionLoaderMapper[typeof (StoryDefinition)] = storyDefinitions;
			definitionLoaderMapper[typeof (StoryNodeDefinition)] = storyNodeDefinitions;
			definitionLoaderMapper[typeof (StoryChoiceDefinition)] = storyTriggerDefinitions;

			definitionLoaderMapper[typeof (EffectDefinition)] = effectDefinitions;
			definitionLoaderMapper[typeof (ConditionDefinition)] = conditionDefinitions;
            


			jsonSerializer = new Serializer(SerializationOptions.JsonCompatible);
			yamlDeserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());

			yamlDeserializer.TypeResolvers.Add(new ScalarYamlNodeTypeResolver());

			_fileSystemWatcher = new FileSystemWatcher(GetContentFilePath());
			_fileSystemWatcher.IncludeSubdirectories = true;
			_fileSystemWatcher.Changed += (object sender, FileSystemEventArgs e) =>
			{
				Logger.Log("File changed " + e.FullPath);
				Game.instance.reloadGame = true;
			};

			_fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
				| NotifyFilters.FileName | NotifyFilters.DirectoryName;
			_fileSystemWatcher.Filter = "*.yaml";

			_fileSystemWatcher.EnableRaisingEvents = true;
		}

		public void ParseDefinitions()
		{
			personDefinitions.AddFolder("Person");

			storyDefinitions.AddFolder("Story");
			storyNodeDefinitions.AddFolder("StoryNode");
			storyTriggerDefinitions.AddFolder("StoryTrigger");

			effectDefinitions.AddFolder("Effect");
			effectDefinitions.AddFolder("Reward");

			conditionDefinitions.AddFolder("Condition");
		}

		public JsonData GetJsonDataFromFile(string filePath)
		{
			var jsonString = File.ReadAllText(filePath);

			return GetJsonDataFromString(jsonString);
		}

		public JsonData GetJsonDataFromString(string jsonString)
		{
			var reader = new JsonReader(jsonString)
			{
				AllowComments = true
			};

			return JsonMapper.ToObject(reader);
		}

		public JsonData GetJsonDataFromYamlFile(string filePath)
		{
			// convert string/file to YAML object
			var reader = new StreamReader(filePath);
			var yamlObject = yamlDeserializer.Deserialize(reader);
			reader.Close();

			var writer = new StringWriter();
			jsonSerializer.Serialize(writer, yamlObject);
			var jsonString = writer.ToString();
			writer.Close();

			return GetJsonDataFromString(jsonString);
		}

		public static string GetContentFilePath()
		{
			return Application.streamingAssetsPath + Path.DirectorySeparatorChar 
				+ "Definitions" + Path.DirectorySeparatorChar;
		}

		public string[] GetAllContentFilePaths(string relativePath, string extension)
		{
			var fullPath = GetContentFilePath() + relativePath;

			return GetAllFilePathsRecursively(fullPath, "*." + extension).ToArray();
		}

		private static List<string> GetAllFilePathsRecursively(string absolutePath, string extension)
		{
			var subDirectories = Directory.GetDirectories(absolutePath);

			var jsonFilePaths = new List<string>();

			for (int i = 0; i < subDirectories.Length; i++)
			{
				var directoryFilePath = subDirectories[i];

				var filesFromSubdirectory = GetAllFilePathsRecursively(directoryFilePath, extension);
				for (int j = 0; j < filesFromSubdirectory.Count; j++)
				{
					var jsonFilePath = filesFromSubdirectory[j];
					jsonFilePaths.Add(jsonFilePath);
				}
			}

			var files = Directory.GetFiles(absolutePath, extension);
			for (int i = 0; i < files.Length; i++)
			{
				var jsonFilePath = files[i];
				jsonFilePaths.Add(jsonFilePath);
			}

			return jsonFilePaths;
		}
	}



	internal class ScalarYamlNodeTypeResolver : INodeTypeResolver
	{
		// Expressions taken from https://github.com/aaubry/YamlDotNet/blob/feat-schemas/YamlDotNet/Core/Schemas/JsonSchema.cs
		private Regex isIntRegex = new Regex(@"^-?(0|[1-9][0-9]*)$", RegexOptions.IgnorePatternWhitespace);
		private Regex isDoubleRegex = new Regex(@"^-?(0|[1-9][0-9]*)(\.[0-9]*)?([eE][-+]?[0-9]+)?$", RegexOptions.IgnorePatternWhitespace);

		private const string falseString = "false";
		private const string trueString = "true";

		public bool Resolve(NodeEvent nodeEvent, ref System.Type currentType)
		{
			var scalar = nodeEvent as Scalar;
			if ((scalar != null) && (scalar.Style == ScalarStyle.Plain))
			{
				var value = scalar.Value;

				if (value == falseString || value == trueString)
				{
					currentType = typeof(bool);
					return true;
				}

				if (isIntRegex.IsMatch(value))
				{
					currentType = typeof(int);
					return true;
				}

				if (isDoubleRegex.IsMatch(value))
				{
					currentType = typeof(double);
					return true;
				}
			}

			return false;
		}
	}
}
