

using Controls.ViewModels;
using DeviceHandler.ViewModels;
using DeviceHandler.Views;
using Syncfusion.Windows.Tools.Controls;
using System.Windows.Controls;

namespace EAPS.ViewModels
{
	public class DocingViewModel: DocingBaseViewModel
	{
		#region Fields

		private ContentControl _communicationSettings;
		

		#endregion Fields

		#region Constructor

		public DocingViewModel(
			CommunicationViewModel communicationSettings) :
			base("DockingMain")
		{

			CreateWindows(
				communicationSettings);
		}

		#endregion Constructor

		#region Methods

		private void CreateWindows(
			CommunicationViewModel communicationSettings)
		{
			DockFill = true;

			_communicationSettings = new ContentControl();
			CommunicationView communication = new CommunicationView() { DataContext = communicationSettings };
			_communicationSettings.Content = communication;
			SetHeader(_communicationSettings, "Communication Settings");
			SetFloatParams(_communicationSettings);
			Children.Add(_communicationSettings);
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

#endregion Methods
	}
}
