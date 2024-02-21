

using Controls.ViewModels;
using DeviceHandler.ViewModels;
using DeviceHandler.Views;
using DeviceSimulators.ViewModels;
using DeviceSimulators.Views;
using EAPS.Views;
using Syncfusion.Windows.Tools.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace EAPS.ViewModels
{
	public class DocingViewModel: DocingBaseViewModel
	{
		#region Fields

		private ContentControl _settingsViewModel;
		private ContentControl _deviceSimulatorsViewModel;
		private ContentControl _mainViewModel;

		#endregion Fields

		#region Constructor

		public DocingViewModel(
			//CommunicationViewModel communicationSettings,
			SettingsViewModel settingsViewModel,
			DeviceSimulatorsViewModel deviceSimulatorsViewModel,
			MainViewModel mainVM) :
			base("DockingMain")
		{

			CreateWindows(
				//communicationSettings,
				settingsViewModel,
				deviceSimulatorsViewModel,
				mainVM);
		}

		#endregion Constructor

		#region Methods

		private void CreateWindows(
			//CommunicationViewModel communicationSettings,
			SettingsViewModel settingsViewModel,
			DeviceSimulatorsViewModel deviceSimulatorsViewModel,
			MainViewModel mainVM)
		{
			DockFill = true;

			_settingsViewModel = new ContentControl();
			SettingsView settingsView = new SettingsView() { DataContext = settingsViewModel };
			_settingsViewModel.Content = settingsView;
			SetHeader(_settingsViewModel, "Settings");
			SetFloatParams(_settingsViewModel);
			Children.Add(_settingsViewModel);

			_deviceSimulatorsViewModel = new ContentControl();
			DeviceSimulatorsView deviceSimulators = new DeviceSimulatorsView() { DataContext = deviceSimulatorsViewModel };
			_deviceSimulatorsViewModel.Content = deviceSimulators;
			SetHeader(_deviceSimulatorsViewModel, "Device Simulators");
			SetFloatParams(_deviceSimulatorsViewModel);
			Children.Add(_deviceSimulatorsViewModel);


			//_mainViewModel = new ContentControl();
			//MainView mainView = new MainView() { DataContext = mainVM };
			//_mainViewModel.Content = mainView;
			//SetState(_mainViewModel, DockState.Dock);
			//SetSideInDockedMode(_mainViewModel, DockSide.Left); 
			//SetHeader(_mainViewModel, "");
			//Children.Add(_mainViewModel);
		}

		private void SetFloatParams(ContentControl control)
		{
			SetSizetoContentInDock(control, true);
			SetSizetoContentInFloat(control, true);
			SetState(control, DockState.Hidden);
		}

		public void OpenSettings()
		{
			SetState(_settingsViewModel, DockState.Float);
		}

		public void OpenDeviceSimulator()
		{
			SetState(_deviceSimulatorsViewModel, DockState.Float);
		}

		#endregion Methods
	}
}
