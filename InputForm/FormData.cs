﻿using InputForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Dynamic;
using System.Collections;
using System.Data;

namespace InputForms
{
	public class FormData
	{
		List<InputInfo> data;
		bool _isValid;
		bool _isModified;

		/// <summary>
		/// Is name (key) free ?
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		bool isFree(string name)
		{
			bool found = false;
			foreach(InputInfo info in data)
			{
				if(info.Name == name)
				{
					found = true;
					break;
				}
			}
			return !found;
		}

		/// <summary>
		/// Element Count
		/// </summary>
		public int Count
		{
			get { return data.Count; }
		}
		
		/// <summary>
		/// True is any element has benn modified
		/// </summary>
		/// <returns></returns>
		public bool isModified
		{
			get
			{
				return _isModified;
			}
			set
			{
				_isModified = value;
			}
			
		}

		public bool isAnyModified
		{
			get
			{
				bool mod = false;
				foreach (InputInfo info in data)
				{
					if(info.isModified)
					{
						mod = true;
						break;
					}
				}
				return mod;
			}
			set
			{
				foreach (InputInfo info in data)
				{
					info.isModified = value;
				}
			}
		}

		/// <summary>
		/// IsValid
		/// </summary>
		public bool isValid
		{
			get {return _isValid; }
			set {_isValid = value;}
		}

		/// <summary>
		/// Ctor
		/// </summary>
		public FormData()
		{
			data = new List<InputInfo>();
			_isValid = false;
			_isModified = false;
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="prev"></param>
		public FormData(FormData prev) : this()
		{
			foreach(InputInfo iif in prev.data)
			{
				data.Add(new InputInfo(iif));
			}
		}


		/// <summary>
		/// Add dynamic
		/// </summary>
		/// <param name="name"></param>
		/// <param name="x"></param>
		/// <param name="read_only"></param>
		/// <param name="dropdown"></param>
		/// <returns></returns>
		public bool Add(string name, dynamic x, bool read_only = false, bool dropdown = false)
		{
			bool ret = isFree(name);
			if(ret)	data.Add(new InputInfo(name, x, read_only, dropdown));
			return ret;

		}
		/// <summary>
		/// Add integer
		/// </summary>
		/// <param name="name"></param>
		/// <param name="x"></param>
		/// <param name="read_only"></param>
		/// <param name="dropdown"></param>
		/// <returns></returns>
		public bool Add(string name, int x, bool read_only = false, bool dropdown = false)
		{
			bool ret = isFree(name);
			if(ret)	data.Add(new InputInfo(name, x, read_only, dropdown));
			return ret;
		}
		/// <summary>
		/// Add boolean
		/// </summary>
		/// <param name="name"></param>
		/// <param name="x"></param>
		/// <param name="read_only"></param>
		/// <param name="dropdown"></param>
		/// <returns></returns>
		public bool Add(string name, bool x, bool read_only = false, bool dropdown = false)
		{
			bool ret = isFree(name);
			data.Add(new InputInfo(name, x, read_only, dropdown));
			return ret;
		}
		/// <summary>
		/// Add string
		/// </summary>
		/// <param name="name"></param>
		/// <param name="x"></param>
		/// <param name="read_only"></param>
		/// <param name="dropdown"></param>
		/// <returns></returns>
		public bool Add(string name, string x, bool read_only = false, bool dropdown = false)
		{
			bool ret = isFree(name);
			data.Add(new InputInfo(name, x, read_only, dropdown));
			return ret;
		}
		/// <summary>
		/// Add float
		/// </summary>
		/// <param name="name"></param>
		/// <param name="x"></param>
		/// <param name="read_only"></param>
		/// <param name="dropdown"></param>
		/// <returns></returns>
		public bool Add(string name, float x, bool read_only = false, bool dropdown = false)
		{
			bool ret = isFree(name);
			data.Add(new InputInfo(name, x, read_only, dropdown));
			return ret;
		}
		/// <summary>
		/// Add double
		/// </summary>
		/// <param name="name"></param>
		/// <param name="x"></param>
		/// <param name="read_only"></param>
		/// <param name="dropdown"></param>
		/// <returns></returns>
		public bool Add(string name, double x, bool read_only = false, bool dropdown = false)
		{	
			bool ret = isFree(name);
			data.Add(new InputInfo(name, x, read_only, dropdown));
			return ret;
		}
		/// <summary>
		/// Add DateTime
		/// </summary>
		/// <param name="name"></param>
		/// <param name="x"></param>
		/// <param name="read_only"></param>
		/// <param name="dropdown"></param>
		/// <returns></returns>
		public bool Add(string name, DateTime x, bool read_only = false, bool dropdown = false)
		{
			bool ret = isFree(name);
			data.Add(new InputInfo(name, x, read_only, dropdown));
			return ret;
		}

		/// <summary>
		/// Get value from name (key)
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		/// <exception cref="KeyNotFoundException"></exception>
		public dynamic this[string key]
		{
			get
			{
				foreach(InputInfo info in data)
				{
					if(info.Name == key)
					{
						return (dynamic)info.Dt.Get();
					}
				}
				throw new KeyNotFoundException();		
			}
		}

		/// <summary>
		/// Name (key) esists 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool Contains(string name)
		{
			return !isFree(name);
		}

		/// <summary>
		/// Names enumeration
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> Names()
		{
			foreach(InputInfo info in data)
			{
				yield return info.Name;
			}
			yield break;
		}

		public IEnumerable<InputInfo> InputInfo()
		{
			foreach(InputInfo info in data)
			{
				yield return info;
			}
			yield break;
		}
		/// <summary>
		/// Dump all values into string
		/// </summary>
		/// <returns></returns>

		public InputInfo Info(int index)
		{
			return data[index];
		}

		public string Dump(bool onlyModified = false)
		{
			StringBuilder sb = new StringBuilder();
			for(int i=0; i<data.Count; i++)
			{
				if(!onlyModified || data[i].isModified)
				{
					sb.AppendLine($"{data[i].Name} : {data[i].Dt.Get().ToString()}");	
				}
			}
			return sb.ToString();
		}
		
	}
}
