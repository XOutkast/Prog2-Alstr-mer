using DB.Data;
using DB.Models;

namespace DB;
public class MainForm : Form
{
    private readonly CarRepository _repository;

    private readonly List<Vehicle> _vehicles = new();

    private readonly Label lblType = new();
    private readonly Label lblRegNr = new();
    private readonly Label lblMake = new();
    private readonly Label lblModel = new();
    private readonly Label lblYear = new();
    private readonly Label lblForSale = new();
    private readonly Label lblLoad = new();

    private readonly ComboBox cmbType = new();
    private readonly TextBox txtRegNr = new();
    private readonly TextBox txtMake = new();
    private readonly TextBox txtModel = new();
    private readonly TextBox txtYear = new();
    private readonly TextBox txtLoad = new();
    private readonly CheckBox chkForSale = new();

    private readonly Button btnAdd = new();
    private readonly Button btnClear = new();
    private readonly Button btnRemove = new();

    private readonly ListView lvVehicles = new();

    public MainForm()
    {
        _repository = new CarRepository();
        
        Text = "CarGUI - Database";
        Width = 860;
        Height = 520;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        InitializeLayout();
        CheckDeleteButton();
        RefreshVehicleList();
    }

    private void InitializeLayout()
    {
        BackColor = Color.Gainsboro;

        lblType.Text = "type";
        lblType.Location = new Point(62, 32);
        lblType.AutoSize = true;

        lblRegNr.Text = "regNr";
        lblRegNr.Location = new Point(62, 64);
        lblRegNr.AutoSize = true;

        lblMake.Text = "make";
        lblMake.Location = new Point(62, 96);
        lblMake.AutoSize = true;

        lblModel.Text = "model";
        lblModel.Location = new Point(62, 128);
        lblModel.AutoSize = true;

        lblYear.Text = "year";
        lblYear.Location = new Point(62, 160);
        lblYear.AutoSize = true;

        lblForSale.Text = "forSale";
        lblForSale.Location = new Point(62, 192);
        lblForSale.AutoSize = true;

        lblLoad.Text = "load(kg)";
        lblLoad.Location = new Point(62, 224);
        lblLoad.AutoSize = true;
        lblLoad.Visible = false;

        cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbType.Items.AddRange(new object[] { "Car", "Lorry" });
        cmbType.SelectedIndex = 0;
        cmbType.Location = new Point(132, 28);
        cmbType.Size = new Size(120, 23);
        cmbType.SelectedIndexChanged += (_, _) => ToggleLorryFields();

        txtRegNr.Location = new Point(132, 60);
        txtRegNr.Size = new Size(120, 23);

        txtMake.Location = new Point(132, 92);
        txtMake.Size = new Size(120, 23);

        txtModel.Location = new Point(132, 124);
        txtModel.Size = new Size(120, 23);

        txtYear.Location = new Point(132, 156);
        txtYear.Size = new Size(120, 23);

        txtLoad.Location = new Point(132, 220);
        txtLoad.Size = new Size(120, 23);
        txtLoad.Visible = false;

        chkForSale.Location = new Point(132, 192);
        chkForSale.Size = new Size(15, 14);

        btnAdd.Text = "Add";
        btnAdd.Location = new Point(294, 38);
        btnAdd.Size = new Size(92, 42);
        btnAdd.Click += (_, _) => AddVehicle();

        btnClear.Text = "Clear";
        btnClear.Location = new Point(656, 38);
        btnClear.Size = new Size(92, 42);
        btnClear.Click += (_, _) => ClearVehicles();

        btnRemove.Text = "Remove";
        btnRemove.Location = new Point(656, 140);
        btnRemove.Size = new Size(92, 42);
        btnRemove.Click += (_, _) => RemoveSelectedVehicle();

        lvVehicles.Location = new Point(46, 270);
        lvVehicles.Size = new Size(730, 170);
        lvVehicles.FullRowSelect = true;
        lvVehicles.GridLines = true;
        lvVehicles.View = View.Details;
        lvVehicles.MultiSelect = false;
        lvVehicles.HideSelection = false;
        lvVehicles.SelectedIndexChanged += (_, _) => CheckDeleteButton();

        lvVehicles.Columns.Add("type", 90);
        lvVehicles.Columns.Add("regNr", 120);
        lvVehicles.Columns.Add("make", 120);
        lvVehicles.Columns.Add("model", 120);
        lvVehicles.Columns.Add("year", 100);
        lvVehicles.Columns.Add("forSale", 100);
        lvVehicles.Columns.Add("load", 90);

        Controls.Add(lblType);
        Controls.Add(lblRegNr);
        Controls.Add(lblMake);
        Controls.Add(lblModel);
        Controls.Add(lblYear);
        Controls.Add(lblForSale);
        Controls.Add(lblLoad);
        Controls.Add(cmbType);
        Controls.Add(txtRegNr);
        Controls.Add(txtMake);
        Controls.Add(txtModel);
        Controls.Add(txtYear);
        Controls.Add(txtLoad);
        Controls.Add(chkForSale);
        Controls.Add(btnAdd);
        Controls.Add(btnClear);
        Controls.Add(btnRemove);
        Controls.Add(lvVehicles);

        ToggleLorryFields();
    }

    private void AddVehicle()
    {
        var type = cmbType.SelectedItem?.ToString() ?? "Car";
        var regNr = txtRegNr.Text.Trim();
        var make = txtMake.Text.Trim();
        var model = txtModel.Text.Trim();
        var yearText = txtYear.Text.Trim();
        var loadText = txtLoad.Text.Trim();

        if (string.IsNullOrWhiteSpace(regNr) || string.IsNullOrWhiteSpace(make) ||
            string.IsNullOrWhiteSpace(model) || string.IsNullOrWhiteSpace(yearText))
        {
            MessageBox.Show("Du maste fylla i alla rutorna", "Felaktig inmatning");
            return;
        }

        if (!int.TryParse(yearText, out var year))
        {
            MessageBox.Show("Ar maste vara ett heltal", "Felaktig inmatning");
            return;
        }

        Vehicle vehicle;
        if (string.Equals(type, "Lorry", StringComparison.OrdinalIgnoreCase))
        {
            if (string.IsNullOrWhiteSpace(loadText) || !int.TryParse(loadText, out var load) || load <= 0)
            {
                MessageBox.Show("Lastbil maste ha giltig maxlast", "Felaktig inmatning");
                return;
            }

            vehicle = new Lorry(regNr, make, model, year, chkForSale.Checked, load);

            if (!_repository.AddLorry((Lorry)vehicle))
            {
                MessageBox.Show("Kunde inte lägga till lastbilen. Regnumret finns kanske redan.", "Databas fel");
                return;
            }
        }
        else
        {
            vehicle = new Car(regNr, make, model, year, chkForSale.Checked);

            if (!_repository.AddCar((Car)vehicle))
            {
                MessageBox.Show("Kunde inte lägga till bilen. Regnumret finns kanske redan.", "Databas fel");
                return;
            }
        }

        RefreshVehicleList();
        ClearInputs();
        CheckDeleteButton();
    }

    private void RemoveSelectedVehicle()
    {
        if (lvVehicles.SelectedItems.Count == 0)
        {
            return;
        }

        var selectedIndex = lvVehicles.SelectedIndices[0];
        if (selectedIndex < 0 || selectedIndex >= _vehicles.Count)
        {
            return;
        }

        var vehicle = _vehicles[selectedIndex];
        
        var deleted = vehicle is Lorry ? _repository.DeleteLorry(vehicle.RegNr) : _repository.DeleteCar(vehicle.RegNr);
        if (deleted)
        {
            RefreshVehicleList();
            MessageBox.Show($"Fordonet med regnr: {vehicle.RegNr} togs bort", "Fordon borttaget");
        }
        else
        {
            MessageBox.Show("Kunde inte ta bort fordonet", "Databas fel");
        }
        
        CheckDeleteButton();
    }

    private void ClearVehicles()
    {
        var count = _vehicles.Count;
        if (count == 0)
        {
            return;
        }

        var result = MessageBox.Show(
            $"Är du säker? Du kommer att ta bort {count} st fordon.",
            "Bekräfta rensning",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result != DialogResult.Yes)
        {
            return;
        }

        foreach (var vehicle in _vehicles.ToList())
        {
            if (vehicle is Lorry)
            {
                _repository.DeleteLorry(vehicle.RegNr);
            }
            else
            {
                _repository.DeleteCar(vehicle.RegNr);
            }
        }

        RefreshVehicleList();
        MessageBox.Show($"Du har rensat databasen med {count} st fordon", "Rensa listan");
        CheckDeleteButton();
    }

    private void RefreshVehicleList()
    {
        lvVehicles.BeginUpdate();
        lvVehicles.Items.Clear();
        _vehicles.Clear();

        var cars = _repository.GetAllCars();
        var lorries = _repository.GetAllLorries();
        _vehicles.AddRange(cars);
        _vehicles.AddRange(lorries);

        foreach (var vehicle in _vehicles)
        {
            var item = new ListViewItem(vehicle.VehicleType);
            item.SubItems.Add(vehicle.RegNr);
            item.SubItems.Add(vehicle.Make);
            item.SubItems.Add(vehicle.Model);
            item.SubItems.Add(vehicle.YearText);
            item.SubItems.Add(vehicle.ForSale ? "Yes" : "No");
            item.SubItems.Add(vehicle is Lorry lorry ? lorry.Load.ToString() : "-");

            lvVehicles.Items.Add(item);
        }

        lvVehicles.EndUpdate();
    }

    private void ClearInputs()
    {
        txtRegNr.Clear();
        txtMake.Clear();
        txtModel.Clear();
        txtYear.Clear();
        txtLoad.Clear();
        chkForSale.Checked = false;
        cmbType.SelectedIndex = 0;
        txtRegNr.Focus();
    }

    private void ToggleLorryFields()
    {
        var isLorry = string.Equals(cmbType.SelectedItem?.ToString(), "Lorry", StringComparison.OrdinalIgnoreCase);
        lblLoad.Visible = isLorry;
        txtLoad.Visible = isLorry;
    }

    private void CheckDeleteButton()
    {
        var hasItems = lvVehicles.Items.Count > 0;
        btnRemove.Enabled = hasItems;
        btnClear.Enabled = hasItems;
    }
}
