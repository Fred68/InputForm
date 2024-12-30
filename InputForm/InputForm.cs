
using Fred68.GenDictionary;
using InputForms;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Fred68.InputForms
{
	public class InputForm:Form
	{
		static string tbPrefixName = "_tbValue";
		
		public class InputInfo
		{
			string	_name;
			bool	_readonly;
			bool	_dropdown;		// Valore Dat string, ma on click apre nuova dialog con la lista
			Dat		_dat;
			bool	_isSet;
			bool	_isModified;

			public string Name {get {return _name;}}
			public bool isReadonly {get {return _readonly;}}
			public bool isDropdown {get {return _dropdown;}}
			public bool isModified {get {return _isModified;} set {_isModified = value;}}

			/// <summary>
			/// Set and access to data with:
			/// dynamic Get()
			/// new Dat(...)
			/// static Type GetEqType(dynamic x)
			/// </summary>
			public Dat Dt {
							get {
								if(_isSet)
									return _dat;
								else
									throw new Exception("Not initialized value");
								}
							set {
								_dat = value;
								_isSet = true;
								}
							}

			void _Set(string name, bool read_only = false, bool dropdown = false)
			{
				_name = name;
				_readonly = read_only;
				_dropdown = dropdown;
				_isSet = false;
			}

			public InputInfo(string name, int x, bool read_only = false, bool dropdown = false)
			{
				_Set(name, read_only, dropdown);
				Dt = new Dat(x);
			}
			public InputInfo(string name, bool x, bool read_only = false, bool dropdown = false)
			{
				_Set(name, read_only, dropdown);
				Dt = new Dat(x);
			}
			public InputInfo(string name, string x, bool read_only = false, bool dropdown = false)
			{
				_Set(name, read_only, dropdown);
				Dt = new Dat(x);
			}
			public InputInfo(string name, float x, bool read_only = false, bool dropdown = false)
			{
				_Set(name, read_only, dropdown);
				Dt = new Dat(x);
			}
			public InputInfo(string name, double x, bool read_only = false, bool dropdown = false)
			{
				_Set(name, read_only, dropdown);
				Dt = new Dat(x);
			}
			public InputInfo(string name, DateTime x, bool read_only = false, bool dropdown = false)
			{
				_Set(name, read_only, dropdown);
				Dt = new Dat(x);
			}


		}

		FormData _fd;
		int _maxTxtLength;
		Label[] _lblNames;
		Label[] _lblTypes;
		List<Control> _tbValues;
		bool _isOk;

		private void InitializeComponent()
		{
			btOK = new Button();
			btCancel = new Button();
			label1 = new Label();
			textBox1 = new TextBox();
			SuspendLayout();
			// 
			// btOK
			// 
			btOK.DialogResult = DialogResult.OK;
			btOK.Location = new Point(85,36);
			btOK.Name = "btOK";
			btOK.Size = new Size(75,23);
			btOK.TabIndex = 0;
			btOK.Text = "OK";
			btOK.UseVisualStyleBackColor = true;
			// 
			// btCancel
			// 
			btCancel.DialogResult = DialogResult.Cancel;
			btCancel.Location = new Point(4,36);
			btCancel.Name = "btCancel";
			btCancel.Size = new Size(75,23);
			btCancel.TabIndex = 1;
			btCancel.Text = "Cancel";
			btCancel.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.BorderStyle = BorderStyle.FixedSingle;
			label1.Location = new Point(12,9);
			label1.Name = "label1";
			label1.Size = new Size(40,17);
			label1.TabIndex = 2;
			label1.Text = "label1";
			label1.TextAlign = ContentAlignment.MiddleLeft;
			// 
			// textBox1
			// 
			textBox1.BorderStyle = BorderStyle.FixedSingle;
			textBox1.Location = new Point(58,7);
			textBox1.Name = "textBox1";
			textBox1.Size = new Size(144,23);
			textBox1.TabIndex = 3;
			// 
			// InputForm
			// 
			ClientSize = new Size(365,229);
			Controls.Add(textBox1);
			Controls.Add(label1);
			Controls.Add(btCancel);
			Controls.Add(btOK);
			Name = "InputForm";
			ResumeLayout(false);
			PerformLayout();
		}

		public InputForm(FormData fd,int maxTxtLength = 20)
		{
			_isOk = true;
			_fd = fd;
			_maxTxtLength = maxTxtLength;

			StringBuilder sb = new StringBuilder();
			foreach(InputForm.InputInfo info in _fd.Info())
			{
				sb.AppendLine($"{info.Name}:{info.Dt.Get()}");
			}

			InitializeComponent();
			SetupForm();
		}

		void SetupForm()
		{
			int _xNames, _xSpace, _ySpace;
			int _yName, _yValue, _yStep;

			Font fontTxtbox = textBox1.Font;                // Legge il font della textbox e calcola la larghezza massima
			int txtbWidt = TextRenderer.MeasureText(new string('8',_maxTxtLength),fontTxtbox).Width;

			_xNames = label1.Location.X;                    // Imposta le origini ed il passo
			_yName = label1.Location.Y;
			_yValue = textBox1.Location.Y;
			_xSpace = textBox1.Left - label1.Right;
			_ySpace = textBox1.Top;

			_yStep = int.Max(label1.Height,textBox1.Height);

			_lblNames = new Label[_fd.Count];
			_lblTypes = new Label[_fd.Count];
			_tbValues = new List<Control>(_fd.Count);

			int lblxmax, tbxmax, lbvmax;
			lblxmax = tbxmax = lbvmax = -1;

			SuspendLayout();
			for(int i = 0;i < _fd.Count;i++)
			{
				_lblNames[i] = new Label();
				_lblNames[i].Location = new Point(_xNames,_yName + i * _yStep);
				_lblNames[i].AutoSize = true;
				_lblNames[i].Text = _fd.Info(i).Name;
				this.Controls.Add(_lblNames[i]);
				if(_lblNames[i].Right > lblxmax)
					lblxmax = _lblNames[i].Right;
			}

			for(int i = 0;i < _fd.Count;i++)
			{
				bool isbool = _fd.Info(i).Dt.Get().GetType().ToString() == "System.Boolean";
				if(!isbool)
				{
					_tbValues.Add(new TextBox());
					((TextBox)_tbValues[i]).Text = (_fd.Info(i).Dt.Get()).ToString();
				}
				else
				{
					_tbValues.Add(new CheckBox());
					((CheckBox)_tbValues[i]).Checked = _fd.Info(i).Dt.Get();
				}
				_tbValues[i].Location = new Point(lblxmax + _xSpace,_yValue + i * _yStep);
				_tbValues[i].Name = $"{tbPrefixName}{i.ToString()}";
				if(!isbool)
					((TextBox)_tbValues[i]).TextAlign = HorizontalAlignment.Left;
				_tbValues[i].Size = new Size(txtbWidt,int.Max(label1.Height,textBox1.Height));
				_tbValues[i].Enabled = !_fd.Info(i).isReadonly;		// Usa Enabled, valido per tutti i controlli, invece che Readonly
				if(!_fd.Info(i).isReadonly)
				{
					if(!isbool)
						((TextBox)_tbValues[i]).TextChanged += txtChanged_Handler;
					else
						((CheckBox)_tbValues[i]).CheckedChanged += txtChanged_Handler;
				}
				this.Controls.Add(_tbValues[i]);
				if(_tbValues[i].Right > tbxmax)
					tbxmax = _tbValues[i].Right;
			}
			for(int i = 0;i < _fd.Count;i++)
			{
				_lblTypes[i] = new Label();
				_lblTypes[i].Location = new Point(tbxmax + _xSpace,_yName + i * _yStep);
				_lblTypes[i].AutoSize = true;
				string txt = (_fd.Info(i).Dt.Get().GetType()).ToString();
				_lblTypes[i].Text = txt.Substring(txt.LastIndexOf('.') + 1);
				if(_tbValues[i] is TextBox)
				{
					this.Controls.Add(_lblTypes[i]);		// Aggiunge il tipo di dato solo se è una textbox
				}
				if(_lblTypes[i].Right > lbvmax)
					lbvmax = _lblTypes[i].Right;
			}

			this.Controls.Remove(label1);                   // Elimina label e textbox
			this.Controls.Remove(textBox1);

			btOK.Location = new Point(tbxmax - btOK.Width,_yValue + _fd.Count * _yStep + _ySpace);
			btOK.Click += btOK_Click;
			btCancel.Location = btOK.Location - new Size(btCancel.Width + _xSpace,0);

			int xMax, yMax;                                 // Ridimensiona la dialog
			xMax = lbvmax + _xSpace;
			yMax = btOK.Bottom + _ySpace;

			this.ClientSize = new Size(xMax,yMax);
			this.StartPosition = FormStartPosition.CenterScreen;

			Size maxSize = new Size((int)(Screen.FromControl(this).WorkingArea.Width * 0.9),(int)(Screen.FromControl(this).WorkingArea.Height * 0.9));

			bool bx = this.Size.Width > maxSize.Width;
			bool by = this.Size.Height > maxSize.Height;

			this.MaximumSize = new Size((int)(Screen.FromControl(this).WorkingArea.Width * 0.9),(int)(Screen.FromControl(this).WorkingArea.Height * 0.9));


			if(bx || by)
			{
				StringBuilder sb = new StringBuilder();
				if(bx)
					sb.AppendLine("Dimensioni eccessive X");
				if(by)
					sb.AppendLine("Dimensioni eccessive Y");
				MessageBox.Show(sb.ToString(),"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				_isOk = false;
			}

			ResumeLayout(true);

		}
		private Button btOK;
		private Label label1;
		private TextBox textBox1;
		private Button btCancel;

		private void txtChanged_Handler(object sender,EventArgs e)
		{
			int i = -1;
			string sname = ((Control)sender).Name;
			if(int.TryParse(sname.Substring(InputForm.tbPrefixName.Length),out i))
			{
				_fd.Info(i).isModified = true;
			}
			else
			{
				throw new Exception($"Wrong textBox id in txtChanged_Handler() for '{sname}'.");
			}

		}

		private void btOK_Click(object sender,EventArgs e)
		{
			for(int i = 0;i < _fd.Count;i++)
			{
				if(_fd.Info(i).isModified)
				{
					string t = _fd.Info(i).Dt.Get().GetType().ToString();

					switch(t)
					{
						case "System.Int32":
						{
							int x;
							if(int.TryParse(_tbValues[i].Text,out x))
							{
								_fd.Info(i).Dt = new Dat(x);
							}
						}
						break;
						case "System.String":
						{
							_fd.Info(i).Dt = new Dat(_tbValues[i].Text);
						}
						break;
						case "System.DateTime":
						{
							DateTime x;
							if(DateTime.TryParse(_tbValues[i].Text,out x))
							{
								_fd.Info(i).Dt = new Dat(x);
							}
						}
						break;
						case "System.Single":
						{
							float x;
							if(float.TryParse(_tbValues[i].Text,out x))
							{
								_fd.Info(i).Dt = new Dat(x);
							}
						}
						break;
						case "System.Double":
						{
							double x;
							if(double.TryParse(_tbValues[i].Text,out x))
							{
								_fd.Info(i).Dt = new Dat(x);
							}
						}
						break;
						case "System.Boolean":
						{
							bool x;
							x = ((CheckBox)_tbValues[i]).Checked;
							_fd.Info(i).Dt = new Dat(x);
						}
						break;
						default:
						{
							MessageBox.Show($"{t} non gestito");
						}
						break;
					}

				}

			}
		}
	}
}
