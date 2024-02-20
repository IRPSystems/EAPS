

using Controls.ViewModels;
using DeviceHandler.ViewModels;
using DeviceHandler.Views;
using DeviceSimulators.ViewModels;
using DeviceSimulators.Views;
using Syncfusion.Windows.Tools.Controls;
using System.Windows.Controls;

namespace EAPS.ViewModels
{
	public class DocingViewModel: DocingBaseViewModel
	{
		#region Fields

		private ContentControl _communicationSettings;
		private ContentControl _deviceSimulatorsViewModel;

		#endregion Fields

		#region Constructor

		public DocingViewModel(
			CommunicationViewModel communicationSettings,
			DeviceSimulatorsViewModel deviceSimulatorsViewModel) :
			base("DockingMain")
		{

			CreateWindows(
				communicationSettings,
				deviceSimulatorsViewModel);
		}

		#endregion Constructor

		#region Methods

		private void CreateWindows(
			CommunicationViewModel communicationSettings,
			DeviceSimulatorsViewModel deviceSimulatorsViewModel)
		{
			DockFill = true;

			_communicationSettings = new ContentControl();
			CommunicationView communication = new CommunicationView() { DataContext = communicationSettings };
			_communicationSettings.Content = communication;
			SetHeader(_communicationSettings, "Communication Settings");
			SetFloatParams(_communicationSettings);
			Children.Add(_communicationSettings);

			_deviceSimulatorsViewModel = new ContentControl();
			DeviceSimulatorsView deviceSimulators = new DeviceSimulatorsView() { DataContext = deviceSimulatorsViewModel };
			_deviceSimulatorsViewModel.Content = deviceSimulators;
			SetHeader(_deviceSimulatorsViewModel, "Device Simulators");
			SetFloatParams(_deviceSimulatorsViewModel);
			Children.Add(_deviceSimulatorsViewModel);
		}

		private void SetFloatParams(ContentControl control)
		{
			SetSizetoContentInDock(control, true);
			SetSizetoContentInFloat(control, true);
			SetState(control, DockState.Hidden);
		}

		public void OpenCommSettings()
		{
			SetState(_communicationSettings, DockState.Float);
		}

		public void OpenDeviceSimulator()
		{
			SetState(_deviceSimulatorsViewModel, DockState.Float);
		}

		#endregion Methods
	}
}
