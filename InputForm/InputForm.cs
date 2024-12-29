
using Fred68.GenDictionary;
using Fred68.InputForm;
using System.Text;

namespace Fred68.InputForm
{
	public class InputForm : Form
	{

		

		List<InputInfo> _datList;           // Alternativa: Dictionary<string,Info> _dict;


		private void InitializeComponent()
		{
			button1 = new Button();
			button2 = new Button();
			SuspendLayout();
			// 
			// button1
			// 
			button1.DialogResult = DialogResult.OK;
			button1.Location = new Point(207,236);
			button1.Name = "button1";
			button1.Size = new Size(75,23);
			button1.TabIndex = 0;
			button1.Text = "OK";
			button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			button2.DialogResult = DialogResult.Cancel;
			button2.Location = new Point(126,236);
			button2.Name = "button2";
			button2.Size = new Size(75,23);
			button2.TabIndex = 1;
			button2.Text = "Cancel";
			button2.UseVisualStyleBackColor = true;
			// 
			// InputForm
			// 
			ClientSize = new Size(284,261);
			Controls.Add(button2);
			Controls.Add(button1);
			Name = "InputForm";
			ResumeLayout(false);
		}

		public InputForm(List<InputInfo> datList)
		{
			_datList = datList;

			StringBuilder sb = new StringBuilder();
			foreach (InputInfo info in _datList)
			{
				sb.AppendLine($"{info.Name}:{info.Dt.Get()}");	
			}
			MessageBox.Show(sb.ToString());

			InitializeComponent();
		}

		private Button button1;
		private Button button2;
	}
}
