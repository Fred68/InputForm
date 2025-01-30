using Fred68.GenDictionary;
using InputForms;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

#pragma warning disable CS8602        // Dereferenziamento di un riferimento eventualmente Null.                                                                                                                                                                                        
#pragma warning disable CS8603        // Possibile riferimento Null restituito.                                               
#pragma warning disable CS8622        // Il supporto dei valori Null dei tipi riferimento nel tipo di parametro non corrisponde al delegato di destinazione                                                                                                                           

namespace InputForms
{
	public class InputForm:Form
	{
		static string tbPrefixName = "_tbValue";
		static Icon? _icon;		// File di icona

		FormData _fd;
		//FormData _fdOrigin;
		int _maxTxtLength;
		Label[]? _lblNames;
		Label[]? _lblTypes;
		List<Control>? _tbValues;
		bool _confirmData;

		private Button? btOK;
		private Label? label1;
		private TextBox? textBox1;
		private Button? btCancel;


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
			btOK.Location = new Point(127,36);
			btOK.Name = "btOK";
			btOK.Size = new Size(75,23);
			btOK.TabIndex = 0;
			btOK.Text = "OK";
			btOK.UseVisualStyleBackColor = true;
			// 
			// btCancel
			// 
			btCancel.DialogResult = DialogResult.Cancel;
			btCancel.Location = new Point(46,36);
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
			ClientSize = new Size(218,77);
			Controls.Add(textBox1);
			Controls.Add(label1);
			Controls.Add(btCancel);
			Controls.Add(btOK);
			Name = "InputForm";
			ResumeLayout(false);
			PerformLayout();
		}

		public static void SetIcon(Icon icon)
		{
			_icon = icon;
		}

		public InputForm(FormData fd, bool confirmData = false, int maxTxtLength = 50)
		{
			//_fdOrigin = fd;
			//_fd = new FormData(fd);

			_fd = fd;
			_maxTxtLength = maxTxtLength;
			_confirmData = confirmData;

			StringBuilder sb = new StringBuilder();
			foreach(InputInfo info in _fd.InputInfo())
			{
				sb.AppendLine($"{info.Name}:{info.Dt.Get()}");
			}
			if(_icon != null)
			{
				this.Icon = _icon;
			}
			InitializeComponent();
			SetupForm();
		}
		
		void SetupForm()
		{
			int _xNames, _xSpace, _ySpace;
			int _yName, _yValue, _yStep;

			int lTxt = 1;									// Lunghezza del testo
			for(int i = 0;i < _fd.Count;i++)
			{
				int lFdTxt;
				bool isbool = _fd.Info(i).Dt.Get().GetType().ToString() == "System.Boolean";
				lFdTxt = isbool ? 1 : ((_fd.Info(i).Dt.Get()).ToString()).Length;
				if(lFdTxt > lTxt)	lTxt = lFdTxt;
			}
			Font fontTxtbox = textBox1.Font;                // Legge il font della textbox e calcola la larghezza massima
			int txtbWidt = TextRenderer.MeasureText(new string('8',int.Min(lTxt,_maxTxtLength)),fontTxtbox).Width;
			if(txtbWidt < textBox1.Width)
				txtbWidt = textBox1.Width;					// Corregge se troppo corto
				
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
			
			for(int i = 0;i < _fd.Count;i++)				// Crea le etichette
			{
				_lblNames[i] = new Label();
				_lblNames[i].Location = new Point(_xNames,_yName + i * _yStep);
				_lblNames[i].AutoSize = true;
				_lblNames[i].Text = _fd.Info(i).Name;
				this.Controls.Add(_lblNames[i]);
				if(_lblNames[i].Right > lblxmax)
					lblxmax = _lblNames[i].Right;
			}

			for(int i = 0;i < _fd.Count;i++)				// Crea i controlli di input (textbox o checkbox)
			{
				bool isbool = _fd.Info(i).Dt.Get().GetType().ToString() == "System.Boolean";
				if(!isbool)
				{
					_tbValues.Add(new TextBox());
				}
				else
				{
					_tbValues.Add(new CheckBox());
				}
				_tbValues[i].Location = new Point(lblxmax + _xSpace,_yValue + i * _yStep);
				_tbValues[i].Name = $"{tbPrefixName}{i.ToString()}";
				if(!isbool)
					((TextBox)_tbValues[i]).TextAlign = HorizontalAlignment.Left;
				_tbValues[i].Size = new Size(txtbWidt,int.Max(label1.Height,textBox1.Height));
				if(isbool)
					_tbValues[i].Enabled = !_fd.Info(i).isReadonly;		// Enabled
				else
					((TextBox)_tbValues[i]).ReadOnly = _fd.Info(i).isReadonly;		// Readonly
				this.Controls.Add(_tbValues[i]);
				if(_tbValues[i].Right > tbxmax)
					tbxmax = _tbValues[i].Right;
			}

			for(int i = 0;i < _fd.Count;i++)				// Crea le etichette con i tipo di dato	
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

			UpdateInputData();								// Aggiorna i contenuti

			for(int i = 0;i < _fd.Count;i++)				// Imposta handler per text e checked changed (DOPO aver aggiornato i contenuti)
			{
				bool isbool = _fd.Info(i).Dt.Get().GetType().ToString() == "System.Boolean";
				if(!_fd.Info(i).isReadonly)
				{
					if(!isbool)
						((TextBox)_tbValues[i]).TextChanged += txtChanged_Handler;
					else
						((CheckBox)_tbValues[i]).CheckedChanged += txtChanged_Handler;
				}
			}
							
			SetDialogButtonHandlers();						// Imposta i pulsanti
			btOK.Location = new Point(tbxmax - btOK.Width,_yValue + _fd.Count * _yStep + _ySpace);
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
				//_isOk = false;
			}

			ResumeLayout(true);

		}

		private void SetDialogButtonHandlers()
		{
			btOK.Click += btOK_Click;
			btOK.DialogResult = _confirmData ? DialogResult.None : DialogResult.OK;
			btCancel.Click += btCancel_Click;
			btCancel.DialogResult = DialogResult.Cancel;
		}

		private void txtChanged_Handler(object sender,EventArgs e)
		{
			int i = -1;
			string sname = ((Control)sender).Name;
			if(int.TryParse(sname.Substring(InputForm.tbPrefixName.Length),out i))
			{
				_fd.Info(i).isModified = true;
				_fd.isModified = true;
				if(_confirmData)
				{
					btOK.Text = "Set";
				}
			}
			else
			{
				throw new Exception($"Wrong textBox id in txtChanged_Handler() for '{sname}'.");
			}

		}

		/// <summary>
		/// Update FormData with form content
		/// </summary>
		private void UpdateFormData()
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

		/// <summary>
		/// Update form content with FormData
		/// </summary>
		private void UpdateInputData()
		{
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
			}
		}

		private void btOK_Click(object sender,EventArgs e)
		{
			UpdateFormData();
			if(_confirmData)
			{
				if(_fd.isModified)
				{
					UpdateInputData();
					_fd.isModified = false;	
					_fd.isValid = true;
					this.DialogResult = DialogResult.None;
					btOK.Text = "OK";
				}
				else
				{
					//_fdOrigin = _fd;			// ?????

					this.DialogResult = DialogResult.OK;
					return;
				}
			}
			else
			{
				_fd.isValid = true;
			}
		}

		private void btCancel_Click(object sender,EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			_fd.isValid = false;
			MessageBox.Show("Annullato");
		}
	}
}


#pragma warning restore CS8602	// Dereferenziamento di un riferimento eventualmente Null.                                                            
#pragma warning restore CS8603	// Possibile riferimento Null restituito.
#pragma warning restore CS8622	// Il supporto dei valori Null dei tipi riferimento nel tipo di parametro non corrisponde al delegato di destinazione
