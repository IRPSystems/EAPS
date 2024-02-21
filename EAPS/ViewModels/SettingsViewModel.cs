
using CommunityToolkit.Mvvm.ComponentModel;
using DeviceHandler.Models;
using DeviceHandler.ViewModels;

namespace EAPS.ViewModels
{
	public class SettingsViewModel: ObservableObject
	{
		public CommunicationViewModel CommunicationSettings { get; set; }

		public SettingsViewModel(DevicesContainer devicesContainer)
		{
			CommunicationSettings = new CommunicationViewModel(devicesContainer);
		}
	}
}
