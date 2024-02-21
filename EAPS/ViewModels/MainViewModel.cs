﻿
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeviceCommunicators.Models;
using DeviceHandler.Models;
using DeviceHandler.Models.DeviceFullDataModels;
using Entities.Enums;
using System;

namespace EAPS.ViewModels
{
	public class MainViewModel : ObservableObject
	{
		#region Properties

		public DeviceParameterData ActualVoltage { get; set; }//
		public DeviceParameterData SetVoltage { get; set; }//

		public DeviceParameterData ActualCurrent { get; set; }//
		public DeviceParameterData ELSetCurrent { get; set; }//
		public DeviceParameterData PSSetCurrent { get; set; }//

		public DeviceParameterData ActualPower { get; set; }//
		public DeviceParameterData ELSetPower { get; set; }//
		public DeviceParameterData PSSetPower { get; set; }//

		public DeviceParameterData ResistenceState { get; set; }



		public DeviceParameterData Mode { get; set; }
		public DeviceParameterData OPMode { get; set; }//
		public DeviceParameterData MSMode { get; set; }//
		public DeviceParameterData Access { get; set; }
		public DeviceParameterData Alarm { get; set; }//

		public string PartNumber { get; set; }
		public string SerialNumber { get; set; }

		public DeviceParameterData MaxVoltage { get; set; }//
		public DeviceParameterData MaxCurrent { get; set; }//
		public DeviceParameterData MaxPower { get; set; }//

		public string HMIVersion { get; set; }
		public string KEVersion { get; set; }
		public string DRVersion { get; set; }

		public DeviceParameterData OVP { get; set; }//
		public DeviceParameterData ELOCP { get; set; }//
		public DeviceParameterData ELOPP { get; set; }//
		public DeviceParameterData PSOCP { get; set; }//
		public DeviceParameterData PSOPP { get; set; }//

		#endregion Properties


		#region Fileds

		private DevicesContainer _devicesContainer;

		#endregion Fileds


		#region Constructor

		public MainViewModel(DevicesContainer devicesContainer)
		{
			_devicesContainer = devicesContainer;

			DeviceFullData deviceFullData =
				_devicesContainer.TypeToDevicesFullData[DeviceTypesEnum.PowerSupplyEA];
			deviceFullData.ConnectionEvent += DeviceFullData_ConnectionEvent;

			


			//ResistenceState = 1;

			RemoteControlCommand = new RelayCommand(RemoteControl);

			InitParameters();
		}



		#endregion Constructor

		#region Methods

		private void InitParameters()
		{
			DeviceData eaDevice = _devicesContainer.DevicesList[0];
			foreach(DeviceParameterData param in eaDevice.ParemetersList) 
			{
				if(param.Name == "Actual Voltage")
					ActualVoltage = param;
				if (param.Name == "Voltage Limit")
					SetVoltage = param;
				if (param.Name == "Actual Current")
					ActualCurrent = param;
				if (param.Name == "(EL) Current Limit")
					ELSetCurrent = param;
				if (param.Name == "(PS) Current Limit")
					PSSetCurrent = param;
				if (param.Name == "Actual Power")
					ActualPower = param;
				if (param.Name == "(EL) Power Limit")
					ELSetPower = param;
				if (param.Name == "(PS) Power Limit")
					PSSetPower = param;


				//Mode = "CV";
				if (param.Name == "OP Mode")
					OPMode = param;
				if (param.Name == "MS Mode")
					MSMode = param;
				//Access = "Rem USB";
				if (param.Name == "Alarm")
					Alarm = param;

				if (param.Name == "Identification")
				{
					//PartNumber = "9873496872840928437";
					//SerialNumber = "089476087049872049385";
					//HMIVersion = "00.00";
					//KEVersion = "00.00";
					//DRVersion = "00.00";
				}

				if (param.Name == "Max Voltage")
					MaxVoltage = param;
				if (param.Name == "Max Current")
					MaxCurrent = param;
				if (param.Name == "Max Power")
					MaxPower = param;



				if (param.Name == "Voltage Protection")
					OVP = param;
				if (param.Name == "(EL) Current Protection")
					ELOCP = param;
				if (param.Name == "(EL) Power Protection")
					ELOPP = param;
				if (param.Name == "(PS) Current Protection")
					PSOCP = param;
				if (param.Name == "(PS) Power Protection")
					PSOPP = param;
			}
		}

		private void DeviceFullData_ConnectionEvent()
		{
			OneTimeReadParams();
		}

		private void OneTimeReadParams()
		{
			

		}

		private void RemoteControl()
		{

		}

		#endregion Methods

		#region Commands

		public RelayCommand RemoteControlCommand { get; set; }

		#endregion Commands
	}
}
