using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Commanding
{
	public class DemoViewModel : INotifyPropertyChanged
	{
		bool canDownload = true;
		string simulatedDownloadResult;

		public double SquareRootResult { get; private set; }

		public string SimulatedDownloadResult {
			get { return simulatedDownloadResult; }
			private set {
				if (simulatedDownloadResult != value) {
					simulatedDownloadResult = value;
					OnPropertyChanged ("SimulatedDownloadResult");
				}
			}
		}

		public ICommand SquareRootCommand { get; private set; }

		public ICommand SimulateDownloadCommand { get; private set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public DemoViewModel ()
		{
			SquareRootCommand = new Command<string> (CalculateSquareRoot);
			SimulateDownloadCommand = new Command (async () => await SimulateDownloadAsync (), () => canDownload);
		}

		void CalculateSquareRoot (string value)
		{
			double num = Convert.ToDouble (value);
			SquareRootResult = Math.Sqrt ((double)num);
			OnPropertyChanged ("SquareRootResult");			
		}

		async Task SimulateDownloadAsync ()
		{
			CanInitiateNewDownload (false);
			SimulatedDownloadResult = string.Empty;
			await Task.Run (() => SimulateDownload ());
			SimulatedDownloadResult = "Simulated download complete";
			CanInitiateNewDownload (true);
		}

		void CanInitiateNewDownload (bool value)
		{
			canDownload = value;
			((Command)SimulateDownloadCommand).ChangeCanExecute ();
		}

		void SimulateDownload ()
		{
			// Simulate a 5 second pause
			var endTime = DateTime.Now.AddSeconds (5);
			while (true) {
				if (DateTime.Now >= endTime) {
					break;
				}
			}
		}

		protected virtual void OnPropertyChanged (string propertyName)
		{
			var changed = PropertyChanged;
			if (changed != null) {
				PropertyChanged (this, new PropertyChangedEventArgs (propertyName));
			}
		}
	}
}

