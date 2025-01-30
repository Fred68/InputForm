using Fred68.GenDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputForms
{
		public class InputInfo
		{
			string?	_name;
			bool	_readonly;
			bool	_dropdown;		// Valore Dat string, ma on click apre nuova dialog con la lista... ANCORA DA FARE
			Dat?	_dat;
			bool	_isSet;
			bool	_isModified;	// Modificato

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

			/***********************************/
			/* CTORs vari                      */
			/***********************************/
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

			/// <summary>
			/// Copy constructor
			/// </summary>
			/// <param name="prev"></param>
			public InputInfo(InputInfo prev)
			{
				_Set(prev.Name, prev._readonly, prev._dropdown);
				TypeVar tp = prev.Dt.Type;
				_dat = new Dat(prev.Dt.Get());
				_isSet = true;
			}

		}

}
