<Query Kind="Program">
  <Connection>
    <ID>8b348f37-a266-4b19-be69-10e2ee787c74</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>(localdb)\MSSQLLocalDB</Server>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <AttachFile>true</AttachFile>
    <AttachFileName>&lt;MyDocuments&gt;\LINQPad Queries\linqpad\git\Kerbal Space Program UI sample\ksp.mdf</AttachFileName>
  </Connection>
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
</Query>

TextBox _nameInput;
SelectBox _specialisationsInput;
RangeControl _minimalCourageInput;
CheckBox _enableSpecialisationsInput;
DumpContainer _results;

void Main()
{
	_nameInput = new TextBox();
	_specialisationsInput = new SelectBox(SelectBoxKind.MultiSelectListBox, Specialisations.Select(n => n.Name).ToArray());
	_minimalCourageInput = new RangeControl(0, 100, 0);
	_enableSpecialisationsInput = new CheckBox();
	_results = new DumpContainer();

	_nameInput.TextInput += (s, e) => RefreshSearchResults();
	_minimalCourageInput.ValueInput += (s, e) => RefreshSearchResults();
	_specialisationsInput.SelectionChanged += (s, e) => RefreshSearchResults();
	_enableSpecialisationsInput.Click += (s, e) => RefreshSearchResults();

	var table = new Table(noBorders: true, cellVerticalAlign: "middle");
	table.Rows.Add(new Label("Name"), _nameInput);
	table.Rows.Add(new Label("Search By Specialisations"), _enableSpecialisationsInput);
	table.Rows.Add(new Label("Specialisations"), _specialisationsInput);
	table.Rows.Add(new Label("Minimal Courage Level"), _minimalCourageInput);
	table.Dump();

	_results.Dump();

	RefreshSearchResults();
}

void RefreshSearchResults()
{
	_specialisationsInput.Enabled = _enableSpecialisationsInput.Checked;
	_results.Content = SearchWorkers(
		_nameInput.Text, 
		_minimalCourageInput.Value, 
		_enableSpecialisationsInput.Checked, 
		_specialisationsInput.SelectedOptions.Cast<string>());
}

IEnumerable<object> SearchWorkers(string name, int minimalCourageLevel, bool searchBySpecialisation, IEnumerable<string> specialisations)
{
	return Workers
		.Where(n => string.IsNullOrEmpty(name) || n.FirstName.StartsWith(name) || n.SecondName.StartsWith(name))
		.Where(n => n.Courage > minimalCourageLevel)
		.Where(n => !searchBySpecialisation || specialisations.Contains(n.SpecialisationEntity.Name))
	    .AsEnumerable()
	    .Select(n => new
	    {
		    n.ID,
		    n.FirstName,
		    n.SecondName,
		    Occupation = n.OccupationEntity.Name,
		    Specialisation = n.SpecialisationEntity?.Name,
		    Courage = new Util.ProgressBar() { Percent = n.Courage },
		    Stupidity = new Util.ProgressBar() { Percent = n.Stupidity },
			Shot = n.WorkerMugshots.Select(s => Util.Image(s.Data)).FirstOrDefault(),
		    Rename = new Hyperlinq(() => ChangeName(n), "Rename"),
	    });
}

void ChangeName(Workers w)
{
	var snapshot = Util.Snapshot(w);
	w.FirstName = Util.ReadLine("New firstname?", w.FirstName);
	base.SubmitChanges();
	RefreshSearchResults();
	
	var difference = Util.Dif(snapshot, w);
	_results.AppendContent(difference);
}




