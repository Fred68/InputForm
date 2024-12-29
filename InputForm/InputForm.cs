
using Fred68.GenDictionary;

namespace Fred68.InputForm
{
    public class InputForm
    {
        public class Info
		{
			public string	_name;
			public bool		_readonly;
			public Dat		_dat;
		}

		Info[] x;						// Soluzione 1
		Dictionary<string,Info> _dict;  // Soluzione 2	(Info, ma senza _name)


		public InputForm()
		{
			x = new Info[2];
			x[0] = new Info();
			x[1] = new Info();

			x[0]._name = "Pippo";
			x[0]._readonly = false;
			x[0]._dat = new Dat(123.5f);

			x[1]._name = "Pluto";
			x[1]._readonly = false;
			x[1]._dat = new Dat("Antani");

			MessageBox.Show($"{x[0]._name}: {x[0]._dat.Get()} {Environment.NewLine} {x[1]._name}: {x[1]._dat.Get()} ");

		}

	}
}
