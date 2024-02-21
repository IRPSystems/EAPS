
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DeviceCommunicators.Enums;
using DeviceCommunicators.Models;
using DeviceCommunicators.PowerSupplayEA;
using DeviceHandler.Models;
using DeviceHandler.Models.DeviceFullDataModels;
using Entities.Enums;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EAPS.ViewModels
{
	public class MainViewModel : ObservableObject
	{
		public enum OPModesEnum { UIR, UIP }

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

		public DeviceParameterData RemoteState { get; set; }
		public DeviceParameterData OutputState { get; set; }

		public DeviceParameterData Mode { get; set; }
		public OPModesEnum OPMode { get; set; }//
		public DeviceParameterData MSMode { get; set; }//
		public string Access { get; set; }
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


			RemoteControlCommand = new RelayCommand(RemoteControl);
			OutputOnOffCommand = new RelayCommand(OutputOnOff);
			TextBox_KeyUpCommand = new RelayCommand<KeyEventArgs>(TextBox_KeyUp);

			InitParameters();
			ChangeTheme();
		}



		#endregion Constructor

		#region Methods

		private void InitParameters()
		{
			Access = "USB";

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
				
				if (param.Name == "MS Mode")
					MSMode = param;
				if (param.Name == "Alarm")
					Alarm = param;

				if (param.Name == "Remote state")
					RemoteState = param;
				if (param.Name == "Output state")
					OutputState = param;

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
			if (_devicesContainer.DevicesFullDataList[0].DeviceCommunicator.IsInitialized)
			{
				OneTimeReadParams();
				InitMonitoring();
			}
			else
			{
				StopMonitoring();
			}

		}

		private void InitMonitoring()
		{
			DeviceFullData deviceFullData = _devicesContainer.DevicesFullDataList[0];


			foreach (DeviceParameterData param in deviceFullData.Device.ParemetersList)
			{
				if (!(param is PowerSupplayEA_ParamData paramData))
					continue;

				if (paramData.ParamType != ParamTypeEnum.Monitor)
					continue;

				deviceFullData.ParametersRepository.Add(
					param, 
					DeviceHandler.Enums.RepositoryPriorityEnum.Medium, 
					Callback);
			}
		}

		private void StopMonitoring()
		{
			DeviceFullData deviceFullData = _devicesContainer.DevicesFullDataList[0];


			foreach (DeviceParameterData param in deviceFullData.Device.ParemetersList)
			{
				if (!(param is PowerSupplayEA_ParamData paramData))
					continue;

				if (paramData.ParamType != ParamTypeEnum.Monitor)
					continue;

				deviceFullData.ParametersRepository.Remove(
					param,
					Callback);
			}
		}

		private void OneTimeReadParams()
		{
			DeviceFullData deviceFullData = _devicesContainer.DevicesFullDataList[0];
			

			foreach (DeviceParameterData param in deviceFullData.Device.ParemetersList)
			{
				
				if (!(param is PowerSupplayEA_ParamData paramData))
					continue;

				if (paramData.ParamType != ParamTypeEnum.ReadOnce &&
					paramData.ParamType != ParamTypeEnum.Setpoint)
				{
					continue;
				}

				deviceFullData.DeviceCommunicator.GetParamValue(param, Callback);
			}

		}

		private void Callback(DeviceParameterData param, CommunicatorResultEnum result, string errDescription)
		{
			if (param.Name == "Identification")
			{
				if (!(param.Value is string idn))
					return;

				string[] idnParts = idn.Split(',');
				PartNumber = idnParts[1];
				SerialNumber = idnParts[2];

				string versions = idnParts[3];
				string[] versionsList = versions.Split('V');

				string[] hmiVersions = versionsList[1].Split(' ');
				HMIVersion = hmiVersions[0];

				string[] keVersions = versionsList[1].Split(' ');
				KEVersion = keVersions[0];

				string[] dvVersions = versionsList[1].Split(' ');
				DRVersion = dvVersions[0];
			}

			else if(param.Name == "Remote state")
			{
				if(param.Value is string state && state == "REMOTE") 
				{
					param.Background = Brushes.Green;
				}
				else
				{
					param.Background = Brushes.Gray;
				}

			}
			else if (param.Name == "Output state")
			{
				if (param.Value is string state && state == "ON")
				{
					param.Background = Brushes.Green;
				}
				else
				{
					param.Background = Brushes.Red;
				}

			}
			else if (param.Name == "OP Mode")
			{
				OPMode = (OPModesEnum)param.Value;

			}
		}

		private void RemoteControl()
		{
			DeviceFullData deviceFullData = _devicesContainer.DevicesFullDataList[0];

			DeviceParameterData paramRemoveOnOff = 
				deviceFullData.Device.ParemetersList.ToList().Find((p) => p.Name == "Remote On/Off");
			DeviceParameterData paramRemoteState =
				deviceFullData.Device.ParemetersList.ToList().Find((p) => p.Name == "Remote state");

			int iVal = 0;
			if(paramRemoteState.Value is string str &&
				str == "REMOTE")
			{
				iVal = 1;
			}

			paramRemoteState.Background = Brushes.Yellow;

			deviceFullData.DeviceCommunicator.SetParamValue(paramRemoveOnOff, iVal, Callback);

		}

		private void OutputOnOff()
		{
			DeviceFullData deviceFullData = _devicesContainer.DevicesFullDataList[0];

			DeviceParameterData paramOutputOnOff =
				deviceFullData.Device.ParemetersList.ToList().Find((p) => p.Name == "Output On/Off");
			DeviceParameterData paramOutputState =
				deviceFullData.Device.ParemetersList.ToList().Find((p) => p.Name == "Output state");

			int iVal = 0;
			if (paramOutputState.Value is string str &&
				str.ToUpper() == "ON")
			{
				iVal = 1;
			}

			paramOutputState.Background = Brushes.Orange;

			deviceFullData.DeviceCommunicator.SetParamValue(paramOutputOnOff, iVal, Callback);
		}

		private void TextBox_KeyUp(KeyEventArgs e)
		{
			if(!(e.Source is TextBox textBox)) 
				return;

			if(!(textBox.DataContext is DeviceParameterData param)) 
				return;

			if(e.Key == Key.Enter) 
			{
				DeviceFullData deviceFullData = _devicesContainer.DevicesFullDataList[0];

				double dVal;
				bool res = double.TryParse(param.Value.ToString(), out dVal);
				if (res == false)
					return;

				deviceFullData.DeviceCommunicator.SetParamValue(param, dVal, Callback);
				deviceFullData.DeviceCommunicator.GetParamValue(param, Callback);
				param.Background = Application.Current.MainWindow.Background;
				return;
			}

			param.Background = Brushes.LightBlue;
		}


		public void ChangeTheme()
		{
			DeviceFullData deviceFullData = _devicesContainer.DevicesFullDataList[0];

			foreach (DeviceParameterData param in deviceFullData.Device.ParemetersList)
				param.Background = Application.Current.MainWindow.Background;
		}

		#endregion Methods

		#region Commands

		public RelayCommand RemoteControlCommand { get; set; }
		public RelayCommand OutputOnOffCommand { get; set; }
		public RelayCommand<KeyEventArgs> TextBox_KeyUpCommand { get; private set; }

		#endregion Commands
	}
}
