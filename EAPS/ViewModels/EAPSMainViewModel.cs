
using CommunityToolkit.Mvvm.ComponentModel;
using System.Reflection;

namespace EAPS.ViewModels
{
	public class EAPSMainViewModel: ObservableObject
	{
		#region Properties

		public string Version { get; set; }

		#endregion Properties

		#region Constructor

		public EAPSMainViewModel()
		{
			Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}

		#endregion Constructor
	}
}
