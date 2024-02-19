
using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;
using System;
using Newtonsoft.Json;

namespace EAPS.Models
{
    public class EAPSUserData: ObservableObject
    {
		public bool IsLightTheme { get; set; }

		public static EAPSUserData LoadEAPSUserData(string dirName)
		{
			EAPSUserData EAPSUserData = null;

			string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			path = Path.Combine(path, dirName);
			if (Directory.Exists(path) == false)
			{
				return EAPSUserData;
			}
			path = Path.Combine(path, "EAPSUserData.json");
			if (File.Exists(path) == false)
			{
				return EAPSUserData;
			}


			string jsonString = File.ReadAllText(path);
			JsonSerializerSettings settings = new JsonSerializerSettings();
			settings.Formatting = Formatting.Indented;
			settings.TypeNameHandling = TypeNameHandling.All;
			EAPSUserData = JsonConvert.DeserializeObject(jsonString, settings) as EAPSUserData;


			return EAPSUserData;
		}



		public static void SaveEAPSUserData(
			string dirName,
			EAPSUserData EAPSUserData)
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			path = Path.Combine(path, dirName);
			if (Directory.Exists(path) == false)
				Directory.CreateDirectory(path);
			path = Path.Combine(path, "EAPSUserData.json");

			JsonSerializerSettings settings = new JsonSerializerSettings();
			settings.Formatting = Formatting.Indented;
			settings.TypeNameHandling = TypeNameHandling.All;
			var sz = JsonConvert.SerializeObject(EAPSUserData, settings);
			File.WriteAllText(path, sz);
		}
	}
}
