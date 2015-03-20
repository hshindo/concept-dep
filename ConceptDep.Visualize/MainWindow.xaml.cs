using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConceptDep.Visualize {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow: Window {
		public MainWindow() {
			InitializeComponent();
		}

		Task<string> Compile() {
			return Task.Run(() => {
				//var psi = new System.Diagnostics.ProcessStartInfo("dot.exe", @"-Tpng fsm.dot -o fsm.png");
				//psi.UseShellExecute = false;
				//psi.RedirectStandardOutput = true;
				//var args = String.Format("{0}");
				var p = System.Diagnostics.Process.Start("dot.exe", @"-Tpng resources\fsm.dot -o fsm.png");
				
				//p.SynchronizingObject = this;
				////イベントハンドラの追加
				//p.Exited += new EventHandler(p_Exited);
				////プロセスが終了したときに Exited イベントを発生させる
				//p.EnableRaisingEvents = true;
				//System.Threading.Thread.Sleep(3000);
				return "compile";
            });
		}

		private void heavyLogic() {
			System.Threading.Thread.Sleep(3000);
		}

		async void Update_Click(object sender, RoutedEventArgs e) {
			this.Update.IsEnabled = false;
			var str = await Compile();
			Text.Text = str;
			this.Update.IsEnabled = true;
		}
	}
}
