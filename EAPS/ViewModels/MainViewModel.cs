
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeviceHandler.Models;
using System;

namespace EAPS.ViewModels
{
	public class MainViewModel : ObservableObject
	{
		#region Properties

		public double ActualVoltage { get; set; }
		public double SetVoltage { get; set; }



		public double ActualCurrent { get; set; }
		public double ELSetCurrent { get; set; }
		public double PSSetCurrent { get; set; }



		public double ActualPower { get; set; }
		public double ELSetPower { get; set; }
		public double PSSetPower { get; set; }

		public int ResistenceState { get; set; }



		public string Mode { get; set; }
		public string OPMode { get; set; }
		public string MSMode { get; set; }
		public string Access { get; set; }
		public string Alarm { get; set; }

		public string PartNumber { get; set; }
		public string SerialNumber { get; set; }

		public double MaxVoltage { get; set; }
		public double MaxCurrent { get; set; }
		public double MaxPower { get; set; }

		public string HMIVersion { get; set; }
		public string KEVersion { get; set; }
		public string DRVersion { get; set; }

		public double OVP { get; set; }
		public double ELOCP { get; set; }
		public double ELOPP { get; set; }
		public double PSOCP { get; set; }
		public double PSOPP { get; set; }

		#endregion Properties


		#region Fileds

		private DevicesContainer _devicesContainer;

		#endregion Fileds


		#region Constructor

		public MainViewModel(DevicesContainer devicesContainer)
		{
			_devicesContainer = devicesContainer;

			ActualVoltage = 0;
			SetVoltage = 0;
			ActualCurrent = 0;
			ELSetCurrent = 0;
			PSSetCurrent = 0;
			ActualPower = 0;
			ELSetPower = 0;
			PSSetPower = 0;


			Mode = "CV";
			OPMode = "UIP";
			MSMode = "Off";
			Access = "Rem USB";
			Alarm = "None";

			PartNumber = "9873496872840928437";
			SerialNumber = "089476087049872049385";

			MaxVoltage = 0;
			MaxCurrent = 0;
			MaxPower = 0;

			HMIVersion = "00.00";
			KEVersion = "00.00";
			DRVersion = "00.00";

			OVP = 0;
			ELOCP = 0;
			ELOPP = 0;
			PSOCP = 0;
			PSOPP = 0;


			//ResistenceState = 1;

			RemoteControlCommand = new RelayCommand(RemoteControl);
		}

		#endregion Constructor

		#region Methods

		private void RemoteControl()
		{

		}

		#endregion Methods

		#region Commands

		public RelayCommand RemoteControlCommand { get; set; }

		#endregion Commands
	}
}
