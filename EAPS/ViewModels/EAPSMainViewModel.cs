
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EAPS.Models;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace EAPS.ViewModels
{
	public class EAPSMainViewModel: ObservableObject
	{
		#region Properties

		public string Version { get; set; }

		public EAPSUserData EAPSUserData { get; set; }



		#endregion Properties
		#region Constructor

		public EAPSMainViewModel()
		{
			ChangeDarkLightCommand = new RelayCommand(ChangeDarkLight); 
			ClosingCommand = new RelayCommand<CancelEventArgs>(Closing);
			LoadedCommand = new RelayCommand(Loaded);


			Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

			LoadEAPSUserData();
		}

		#endregion Constructor

		#region Methods

		#region Load/Save EAPSUserData

		private void LoadEAPSUserData()
		{
			EAPSUserData = EAPSUserData.LoadEAPSUserData("EAPS");

			if (EAPSUserData == null)
			{
				EAPSUserData = new EAPSUserData();
				EAPSUserData.IsLightTheme = false;
				ChangeDarkLight();
				return;
			}
			else
				EAPSUserData.IsLightTheme = !EAPSUserData.IsLightTheme;


			ChangeDarkLight();
		}

		private void SaveEAPSUserData()
		{
			EAPSUserData.SaveEAPSUserData(
				"EAPS",
				EAPSUserData);
		}

		#endregion Load/Save EAPSUserData

		private void Closing(CancelEventArgs e)
		{
			SaveEAPSUserData();			
		}

		private void Loaded()
		{
		}

		private void ChangeDarkLight()
		{

			EAPSUserData.IsLightTheme = !EAPSUserData.IsLightTheme;
			App.ChangeDarkLight(EAPSUserData.IsLightTheme);
			
		}

		#endregion Methods

		#region Commands

		public RelayCommand ChangeDarkLightCommand { get; private set; }
		public RelayCommand<CancelEventArgs> ClosingCommand { get; private set; }
		public RelayCommand LoadedCommand { get; private set; }

		#endregion Commands
	}
}
