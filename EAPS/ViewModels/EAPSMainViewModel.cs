
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DeviceCommunicators.Models;
using DeviceCommunicators.Services;
using DeviceHandler.Models;
using DeviceHandler.Models.DeviceFullDataModels;
using EAPS.Models;
using Entities.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace EAPS.ViewModels
{
	public class EAPSMainViewModel: ObservableObject
	{
		#region Properties

		public string Version { get; set; }

		public EAPSUserData EAPSUserData { get; set; }

		public DevicesContainer DevicesContainter { get; set; }

		#endregion Properties

		#region Fields
		#endregion Fields

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

			if (DevicesContainter != null)
			{
				foreach (DeviceFullData deviceFullData in DevicesContainter.DevicesFullDataList)
				{
					deviceFullData.Disconnect();

					if (deviceFullData.CheckCommunication == null)
						continue;

					deviceFullData.CheckCommunication.Dispose();
				}
			}
		}

		private void Loaded()
		{			
			UpdateSetup();
		}

		private void UpdateSetup()
		{
			DevicesContainter = new DevicesContainer();
			DevicesContainter.DevicesFullDataList = new ObservableCollection<DeviceFullData>();
			DevicesContainter.DevicesList = new ObservableCollection<DeviceData>();
			DevicesContainter.TypeToDevicesFullData = new Dictionary<DeviceTypesEnum, DeviceFullData>();

			ReadDevicesFileService readDevicesFile = new ReadDevicesFileService();
			ObservableCollection<DeviceData> deviceList = readDevicesFile.ReadAllFiles(
				@"Data\Device Communications\",
				null,
				null,
				null,
				null,
				false);


			List<DeviceData> newDevices = new List<DeviceData>();
			foreach (DeviceData deviceData in deviceList)
			{
				DeviceData existingDevice =
					DevicesContainter.DevicesList.ToList().Find((d) => d.DeviceType == deviceData.DeviceType);
				if (existingDevice == null)
					newDevices.Add(deviceData);
			}

			List<DeviceData> removedDevices = new List<DeviceData>();
			foreach (DeviceData deviceData in DevicesContainter.DevicesList)
			{
				DeviceData existingDevice =
					deviceList.ToList().Find((d) => d.DeviceType == deviceData.DeviceType);
				if (existingDevice == null)
					removedDevices.Add(deviceData);
			}




			foreach (DeviceData device in removedDevices)
			{
				DeviceFullData deviceFullData =
					DevicesContainter.DevicesFullDataList.ToList().Find((d) => d.Device.DeviceType == device.DeviceType);
				deviceFullData.Disconnect();

				DevicesContainter.DevicesFullDataList.Remove(deviceFullData);
				DevicesContainter.DevicesList.Remove(deviceFullData.Device);
				DevicesContainter.TypeToDevicesFullData.Remove(deviceFullData.Device.DeviceType);
			}



			foreach (DeviceData device in newDevices)
			{
				DeviceFullData deviceFullData = DeviceFullData.Factory(device);

				deviceFullData.Init("EAPS");

				DevicesContainter.DevicesFullDataList.Add(deviceFullData);
				DevicesContainter.DevicesList.Add(device as DeviceData);
				if (DevicesContainter.TypeToDevicesFullData.ContainsKey(device.DeviceType) == false)
					DevicesContainter.TypeToDevicesFullData.Add(device.DeviceType, deviceFullData);

				deviceFullData.Connect();
			}


			

			WeakReferenceMessenger.Default.Send(new SETUP_UPDATEDMessage());
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
