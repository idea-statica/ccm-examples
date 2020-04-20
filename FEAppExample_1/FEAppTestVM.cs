using IdeaStatiCa.Plugin;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Input;

namespace FEAppExample_1
{
	public class FEAppExample_1VM : INotifyPropertyChanged, IHistoryLog
	{
		private IBIMPluginHosting feaAppHosting;
		private string modelFeaXml;
		private string workingDirectory;
		private string projectName;
		private string projectDir;

		public FEAppExample_1VM()
		{
			Actions = new ObservableCollection<string>();
			IdeaStatiCaStatus = AppStatus.Finished;
			RunCmd = new CustomCommand(this.CanRun, this.Run);
			LoadCmd = new CustomCommand(this.CanLoad, this.Load);
			ProjectName = string.Empty;
			WorkingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Process.GetCurrentProcess().ProcessName);
			if (!Directory.Exists(WorkingDirectory))
			{
				Directory.CreateDirectory(WorkingDirectory);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<string> Actions { get; set; }
		public AppStatus IdeaStatiCaStatus { get; set; }
		public CustomCommand LoadCmd { get; set; }

		public CustomCommand RunCmd { get; set; }
		public string ModelFeaXml { get => modelFeaXml; set => modelFeaXml = value; }

		public string WorkingDirectory
		{
			get => workingDirectory;
			set
			{
				workingDirectory = value;
				NotifyPropertyChanged("WorkingDirectory");
			}
		}

		public string ProjectName
		{
			get => projectName;
			set
			{
				projectName = value;
				NotifyPropertyChanged("ProjectName");
			}
		}

		public string ProjectDir
		{
			get => projectDir;
			set
			{
				projectDir = value;
				NotifyPropertyChanged("ProjectDir");
			}
		}

		public void Add(string action)
		{
			System.Windows.Application.Current.Dispatcher.BeginInvoke(
				System.Windows.Threading.DispatcherPriority.Normal,
				(Action)(() =>
				{
					Actions.Add(action);
				}));
		}

		public bool CanLoad(object param)
		{
			return (IdeaStatiCaStatus != AppStatus.Started);
		}

		public bool CanRun(object param)
		{
			return ((IdeaStatiCaStatus == AppStatus.Finished) && !string.IsNullOrEmpty(ProjectName));
		}

		public void Load(object param)
		{
			var filePath = param == null
				? GetFilePath()
				: Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), param.ToString());

			CreateProjectDirectory(filePath);
		}

		private void CreateProjectDirectory(string filePath)
		{
			ProjectName = Path.GetFileNameWithoutExtension(filePath);
			ModelFeaXml = File.ReadAllText(filePath);

			ProjectDir = Path.Combine(WorkingDirectory, ProjectName);
			if(!Directory.Exists(ProjectDir))
			{
				Directory.CreateDirectory(ProjectDir);
				File.Copy(filePath, Path.Combine(ProjectDir, Path.GetFileName(filePath)));
			}

			Add($"Model {filePath} loaded.");
		}

		public void Run(object param)
		{
			var factory = new PluginFactory(this);
			feaAppHosting = new BIMPluginHosting(factory);
			feaAppHosting.AppStatusChanged += new ISEventHandler(IdeaStaticAppStatusChanged);
			var id = Process.GetCurrentProcess().Id.ToString();

			ProjectDir = Path.Combine(WorkingDirectory, ProjectName);
			if (!Directory.Exists(ProjectDir))
			{
				Directory.CreateDirectory(ProjectDir);
			}

			var ideaStatiCaProjectDir = Path.Combine(ProjectDir, "IdeaStatiCa-" + ProjectName);
			if (!Directory.Exists(ideaStatiCaProjectDir))
			{
				Directory.CreateDirectory(ideaStatiCaProjectDir);
			}

			Add(string.Format("Starting FEAPluginHosting clientTd = {0}", id));
			feaAppHosting.RunAsync(id, ideaStatiCaProjectDir);
		}

		private static string GetFilePath()
		{
			var openFileDialog = new OpenFileDialog { Filter = "XML Files | *.xml" };
			if (openFileDialog.ShowDialog() == true)
			{
				return openFileDialog.FileName;
			}

			return null;
		}

		private void IdeaStaticAppStatusChanged(object sender, ISEventArgs e)
		{
			if (e.Status == AppStatus.Finished)
			{
				feaAppHosting.AppStatusChanged -= new ISEventHandler(IdeaStaticAppStatusChanged);
				feaAppHosting = null;
			}

			if (e.Status == AppStatus.Started)
			{
				if (string.IsNullOrEmpty(modelFeaXml))
				{
					string fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Path.ChangeExtension(ProjectName, ".xml"));
					ModelFeaXml = File.ReadAllText(fileName);
					Add($"Model {fileName} loaded.");
				}

				var model = Tools.ModelFromXml(ModelFeaXml);
				((FakeFEA)(feaAppHosting.Service)).FeaModel = model;
			}

			System.Windows.Application.Current.Dispatcher.BeginInvoke(
				System.Windows.Threading.DispatcherPriority.Normal,
				(Action)(() =>
				{
					Add(string.Format("IdeaStatiCa current status is = {0}", e.Status));
					IdeaStatiCaStatus = e.Status;
					CommandManager.InvalidateRequerySuggested();
				}));
		}

		// This method is called by the Set accessor of each property.
		// The CallerMemberName attribute that is applied to the optional propertyName
		// parameter causes the property name of the caller to be substituted as an argument.
		private void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}