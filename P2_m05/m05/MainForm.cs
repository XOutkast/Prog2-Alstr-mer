using m05.Vehicle;
using m05.Biluppgifter;
using Repository;

namespace m05;

// MainForm: UI, läs/spara & hämta fordonsdata via API

public class MainForm : Form
{
    // Databas & API-klienten
    private readonly Repository.Repository _repository;
    private readonly API_C _apiClient;

    // Lagra fordon lokalt
    private readonly List<Vehicle.Vehicle> _vehicles = new();

    // UI-komponenter (labels för fordon)
    private readonly Label lblType = new();
    private readonly Label lblRegNr = new();
    private readonly Label lblMake = new();
    private readonly Label lblModel = new();
    private readonly Label lblYear = new();
    private readonly Label lblForSale = new();
    private readonly Label lblLoad = new();

    // UI-komponenter för inmatning
    private readonly ComboBox cmbType = new();
    private readonly TextBox txtRegNr = new();
    private readonly TextBox txtMake = new();
    private readonly TextBox txtModel = new();
    private readonly TextBox txtYear = new();
    private readonly TextBox txtLoad = new();
    private readonly CheckBox chkForSale = new();

    // UI-knappar
    private readonly Button btnAdd = new();
    private readonly Button btnClear = new();
    private readonly Button btnRemove = new();

    // List som visar fordon
    private readonly ListView lvVehicles = new();

    // API-sökning UI
    private readonly Label lblSearchRegNr = new();
    private readonly TextBox txtSearchRegNr = new();
    private readonly Button btnSearch = new();
    private readonly RichTextBox txtRawJson = new(); // Visar rå JSON från API
    private readonly TableLayoutPanel tlpCarInfo = new(); // Tabell för API-detaljer

    // Labels visar API-data
    private readonly Label lblValueRegNr = new();
    private readonly Label lblValueType = new();
    private readonly Label lblValueMake = new();
    private readonly Label lblValueModel = new();
    private readonly Label lblValueModelYear = new();
    private readonly Label lblValueColor = new();

    public MainForm()
    {
        // En repo för lokalt data hantering
        _repository = new Repository.Repository();
        // Hämta API-nyckel från miljövariabel eller fallback
        var token = Environment.GetEnvironmentVariable("BILUPPGIFTER_API_KEY")
                    ?? "jtBt2fWF8vS0v1B5EcZT9111j9YExcUfiLHq5-4xSWY";
        // Initiera API-klient med token
        _apiClient = new API_C(token);

        // Inställningar för fönstret
        Text = "CarGUI m05";
        Width = 1240;
        Height = 560;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        // Bygg UI: ladda in de befintliga fordon, ladda data & kontrollera knapparna
        InitializeLayout();
        CheckButtons();
        RefreshVehicleList();
    }

    // UI-komponenter & layout
    private void InitializeLayout()
    {
        BackColor = Color.Gainsboro;

        // Fordonets visningsfält (labels)
        lblType.Text = "type";
        lblType.Location = new Point(40, 30);
        lblType.AutoSize = true;

        lblRegNr.Text = "regNr";
        lblRegNr.Location = new Point(40, 62);
        lblRegNr.AutoSize = true;

        lblMake.Text = "make";
        lblMake.Location = new Point(40, 94);
        lblMake.AutoSize = true;

        lblModel.Text = "model";
        lblModel.Location = new Point(40, 126);
        lblModel.AutoSize = true;

        lblYear.Text = "year";
        lblYear.Location = new Point(40, 158);
        lblYear.AutoSize = true;

        lblForSale.Text = "forSale";
        lblForSale.Location = new Point(40, 190);
        lblForSale.AutoSize = true;

        lblLoad.Text = "load(kg)";
        lblLoad.Location = new Point(40, 222);
        lblLoad.AutoSize = true;
        lblLoad.Visible = false;

        // Inmatning: fordontyp
        cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbType.Items.AddRange(new object[] { "Car", "Lorry" });
        cmbType.SelectedIndex = 0;
        cmbType.Location = new Point(110, 26);
        cmbType.Size = new Size(120, 23);
        // Byt valda fält beroende på den valda fordonstyp
        cmbType.SelectedIndexChanged += (_, _) => ToggleLorryFields();

        // Inmatningsfält
        txtRegNr.Location = new Point(110, 58);
        txtRegNr.Size = new Size(120, 23);
        txtRegNr.CharacterCasing = CharacterCasing.Upper;

        txtMake.Location = new Point(110, 90);
        txtMake.Size = new Size(120, 23);

        txtModel.Location = new Point(110, 122);
        txtModel.Size = new Size(120, 23);

        txtYear.Location = new Point(110, 154);
        txtYear.Size = new Size(120, 23);

        txtLoad.Location = new Point(110, 218);
        txtLoad.Size = new Size(120, 23);
        txtLoad.Visible = false;

        chkForSale.Location = new Point(110, 190);
        chkForSale.Size = new Size(15, 14);

        // Knappar: CRUD
        btnAdd.Text = "Add";
        btnAdd.Location = new Point(270, 36);
        btnAdd.Size = new Size(92, 42);

        // Knapp: lägg till fordon i databas
        btnAdd.Click += (_, _) => AddVehicle();
        btnClear.Text = "Clear";
        btnClear.Location = new Point(270, 94);
        btnClear.Size = new Size(92, 42);

        // Knapp: rensa alla fordon
        btnClear.Click += (_, _) => ClearVehicles();
        btnRemove.Text = "Remove";
        btnRemove.Location = new Point(270, 152);
        btnRemove.Size = new Size(92, 42);

        // Knapp: ta bort den valda fordon
        btnRemove.Click += (_, _) => RemoveSelectedVehicle();

        // Listview för fordon
        lvVehicles.Location = new Point(40, 280);
        lvVehicles.Size = new Size(600, 200);
        lvVehicles.FullRowSelect = true;
        lvVehicles.GridLines = true;
        lvVehicles.View = View.Details;
        lvVehicles.MultiSelect = false;
        lvVehicles.HideSelection = false;

        // Uppdatera knappstatus vid val
        lvVehicles.SelectedIndexChanged += (_, _) => CheckButtons();

        // Kolumner
        lvVehicles.Columns.Add("type", 80);
        lvVehicles.Columns.Add("regNr", 90);
        lvVehicles.Columns.Add("make", 90);
        lvVehicles.Columns.Add("model", 90);
        lvVehicles.Columns.Add("year", 80);
        lvVehicles.Columns.Add("forSale", 80);
        lvVehicles.Columns.Add("load", 80);

        // API-sökning
        lblSearchRegNr.Text = "RegNr";
        lblSearchRegNr.Location = new Point(690, 34);
        lblSearchRegNr.AutoSize = true;

        txtSearchRegNr.Location = new Point(742, 30);
        txtSearchRegNr.Size = new Size(120, 23);
        txtSearchRegNr.CharacterCasing = CharacterCasing.Upper;
        txtSearchRegNr.TextChanged += (_, _) => CheckButtons();

        btnSearch.Text = "Sok";
        btnSearch.Location = new Point(690, 65);
        btnSearch.Size = new Size(70, 30);
        // API-sökknapp (async anrop)
        btnSearch.Click += BtnSearch_ClickAsync;

        // API-datavisning (alltså tabellen)
        tlpCarInfo.Location = new Point(690, 120);
        tlpCarInfo.Size = new Size(240, 200);
        tlpCarInfo.ColumnCount = 2;
        tlpCarInfo.RowCount = 6;
        tlpCarInfo.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
        tlpCarInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpCarInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        for (var i = 0; i < 6; i++)
        {
            tlpCarInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        }

        // Fyll tabellen med API-info (label & value)
        AddInfoRow(0, "RegNr", lblValueRegNr);
        AddInfoRow(1, "Typ", lblValueType);
        AddInfoRow(2, "Marke", lblValueMake);
        AddInfoRow(3, "Modell", lblValueModel);
        AddInfoRow(4, "Arsmodell", lblValueModelYear);
        AddInfoRow(5, "Farg", lblValueColor);

        //API-output
        txtRawJson.Location = new Point(950, 30);
        txtRawJson.Size = new Size(250, 450);
        txtRawJson.DetectUrls = true;
        txtRawJson.ScrollBars = RichTextBoxScrollBars.Vertical;
        // Textfält för rå API-data
        txtRawJson.ReadOnly = true;
        txtRawJson.BorderStyle = BorderStyle.FixedSingle;
        txtRawJson.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);

        // Lägg till  UI i formuläret
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

        Controls.Add(lblSearchRegNr);
        Controls.Add(txtSearchRegNr);
        Controls.Add(btnSearch);
        Controls.Add(tlpCarInfo);
        Controls.Add(txtRawJson);

        ToggleLorryFields();
    }

    // API-tabell (titel & värde)
    private void AddInfoRow(int row, string title, Label valueLabel)
    {
        var titleLabel = new Label
        {
            Text = title,
            AutoSize = true,
            Font = new Font(Font, FontStyle.Bold),
            Anchor = AnchorStyles.Left
        };
        valueLabel.Text = string.Empty;
        valueLabel.AutoSize = true;
        valueLabel.Anchor = AnchorStyles.Left;

        tlpCarInfo.Controls.Add(titleLabel, 0, row);
        tlpCarInfo.Controls.Add(valueLabel, 1, row);
    }

    // API-sökning på regNumret
    private async void BtnSearch_ClickAsync(object? sender, EventArgs e)
    {
        // Läs (regNr för sökning)
        // Normalisera (alltså ta bort mellanslag, gör allt till stora bokstäver)
        var regNr = txtSearchRegNr.Text.Trim().ToUpperInvariant();
        // Validera inmatning (så att det är inte tom)
        if (string.IsNullOrWhiteSpace(regNr))
        {
            MessageBox.Show("Du maste ange ett registreringsnummer!", "Inmatning saknas",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // Rensa tidigare resultat
        ClearApiResult();

        try
        {
            var vehicle = await _apiClient.GetVehicleByRegNoAsync(regNr);

            // Fyll UI med API-datan
            lblValueRegNr.Text = vehicle.RegNo;
            lblValueType.Text = vehicle.Type;
            lblValueMake.Text = vehicle.Make;
            lblValueModel.Text = vehicle.Model;
            lblValueModelYear.Text = vehicle.ModelYear;
            lblValueColor.Text = vehicle.Color;
            txtRawJson.Text = vehicle.RawJson;

            // Förifyll formuläret
            txtRegNr.Text = vehicle.RegNo;
            txtMake.Text = vehicle.Make;
            txtModel.Text = vehicle.Model;
            txtYear.Text = vehicle.ModelYear;
            cmbType.SelectedIndex = vehicle.Type.Contains("last", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
            txtRegNr.Focus();
            txtRegNr.SelectionStart = txtRegNr.TextLength;
        }
        // Fel från API-anrop
        catch (HttpRequestException ex)
        {
            MessageBox.Show($"Registreringsnummer {regNr} hittades inte i databasen.\nMeddelande: {ex.Message}",
                "Felaktigt registreringsnummer", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Nagot gick fel vid hamtning av fordonsdata.\nMeddelande: {ex.Message}",
                "Tekniskt fel", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        // Updatera knappstatus
        CheckButtons();
    }

    // Rensa API-visning
    private void ClearApiResult()
    {
        // Rensa visad API-fält
        lblValueRegNr.Text = string.Empty;
        lblValueType.Text = string.Empty;
        lblValueMake.Text = string.Empty;
        lblValueModel.Text = string.Empty;
        lblValueModelYear.Text = string.Empty;
        lblValueColor.Text = string.Empty;
        txtRawJson.Clear();
    }

    // Lägg till nytt fordon (Car eller Lorry)
    private void AddVehicle()
    {
        // Läs & validera inmatning
        // tar bort alla mellanslag & gör till versaler
        var type = cmbType.SelectedItem?.ToString() ?? "Car";
        var regNr = txtRegNr.Text.Trim().ToUpperInvariant();
        var make = txtMake.Text.Trim();
        var model = txtModel.Text.Trim();
        var yearText = txtYear.Text.Trim();
        var loadText = txtLoad.Text.Trim();

        //Validera de obligatoriska fölt
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

        // Skapa rätt typ
        if (string.Equals(type, "Lorry", StringComparison.OrdinalIgnoreCase))
        {
            // Validera lastkapacitet
            if (string.IsNullOrWhiteSpace(loadText) || !int.TryParse(loadText, out var load) || load <= 0)
            {
                // Visa felmeddelande vid ogiltig inmatning
                MessageBox.Show("Lastbil maste ha giltig maxlast", "Felaktig inmatning");
                return;
            }
            // Försök lägga till lastbil i databasen
            if (!_repository.AddLorry(new Lorry(regNr, make, model, year, chkForSale.Checked, load)))
            {
                // Visa felmeddelande när regNr redan finns
                MessageBox.Show("Kunde inte lagga till lastbilen. Regnumret finns kanske redan.", "Databas fel");
                return;
            }
        }
        else
        {
            if (!_repository.AddCar(new Car(regNr, make, model, year, chkForSale.Checked)))
            {
                MessageBox.Show("Kunde inte lagga till bilen. Regnumret finns kanske redan.", "Databas fel");
                return;
            }
        }

        RefreshVehicleList();
        ClearInputs();
        CheckButtons();
    }

    // Ta bort markerat fordon
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

        CheckButtons();
    }

    // Rensa alla fordon efter en bekräftelse
    private void ClearVehicles()
    {
        // Töm alla fordon
        var count = _vehicles.Count;
        if (count == 0)
        {
            return;
        }

        var result = MessageBox.Show(
            $"Ar du saker? Du kommer att ta bort {count} st fordon.",
            "Bekrafta rensning",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result != DialogResult.Yes)
        {
            return;
        }

        // Undvik iteration, så skapa en kopia av listan _vehciles
        var snapshot = new List<Vehicle.Vehicle>(_vehicles);
        foreach (var vehicle in snapshot)
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
        CheckButtons();
    }

    // Uppdatera listView från databasen
    private void RefreshVehicleList()
    {
        lvVehicles.BeginUpdate();
        lvVehicles.Items.Clear();
        _vehicles.Clear();

        _vehicles.AddRange(_repository.GetAllCars());
        _vehicles.AddRange(_repository.GetAllLorries());

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

    // Rensa inmatningsfält & återställ fokus
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

    // Visa/dölj lastfält beroende på fordon
    private void ToggleLorryFields()
    {
        var isLorry = string.Equals(cmbType.SelectedItem?.ToString(), "Lorry", StringComparison.OrdinalIgnoreCase);
        lblLoad.Visible = isLorry;
        txtLoad.Visible = isLorry;
    }

    // Updatera knappstatus beroende på state
    private void CheckButtons()
    {
        var hasItems = lvVehicles.Items.Count > 0;
        btnRemove.Enabled = hasItems;
        btnClear.Enabled = hasItems;
        btnSearch.Enabled = txtSearchRegNr.TextLength > 0;
    }
}