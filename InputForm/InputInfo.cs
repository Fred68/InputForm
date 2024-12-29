using Fred68.GenDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred68.InputForm
{
	public class InputInfo
	{
			string	_name;
			bool	_readonly;
			bool	_dropdown;		// Valore Dat string, ma on click apre nuova dialog con la lista
			Dat		_dat;
			bool	_isSet;

			public string Name {get {return _name;}}
			public bool isReadonly {get {return _readonly;}}
			public bool isDropdown {get {return _dropdown;}}

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

			public InputInfo(string name, bool read_only = false, bool dropdown = false)
			{
				_name = name;
				_readonly = read_only;
				_dropdown = dropdown;
				_isSet = false;
			}

		}


}
