namespace API_;
public class MainForm : Form
{
    private readonly Label lblRegNr = new();
    private readonly TextBox txtRegNr = new();
    private readonly Button btnSearch = new();

    private readonly RichTextBox txtRawJson = new();

    private readonly TableLayoutPanel tlpCarInfo = new();

    private readonly Label lblValueRegNr = new();
    private readonly Label lblValueType = new();
    private readonly Label lblValueMake = new();
    private readonly Label lblValueModel = new();
    private readonly Label lblValueModelYear = new();
    private readonly Label lblValueColor = new();

    private readonly BiluppgifterApiClient apiClient;

    public MainForm()
    {
        Text = "Form1";
        ClientSize = new Size(620, 400);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = SystemColors.Control;

        var token = Environment.GetEnvironmentVariable("BILUPPGIFTER_API_KEY")
                    ?? "jtBt2fWF8vS0v1B5EcZT9111j9YExcUfiLHq5-4xSWY";

        apiClient = new BiluppgifterApiClient(token);

        InitializeLayout();
        UpdateButtons();
    }

    private void InitializeLayout()
    {
        lblRegNr.Text = "RegNr";
        lblRegNr.Location = new Point(56, 44);
        lblRegNr.AutoSize = true;

        txtRegNr.Location = new Point(108, 40);
        txtRegNr.Size = new Size(86, 23);

        txtRegNr.CharacterCasing = CharacterCasing.Upper;

        txtRegNr.TextChanged += (_, _) => UpdateButtons();

        btnSearch.Text = "Sök";
        btnSearch.Location = new Point(56, 75);
        btnSearch.Size = new Size(58, 30);
        btnSearch.Click += BtnSearch_ClickAsync;

        tlpCarInfo.Location = new Point(56, 137);
        tlpCarInfo.Size = new Size(250, 205);
        tlpCarInfo.ColumnCount = 2;
        tlpCarInfo.RowCount = 6;
        tlpCarInfo.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
        tlpCarInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpCarInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        for (var i = 0; i < 6; i++)
        {
            tlpCarInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        }

        AddInfoRow(0, "RegNr", lblValueRegNr);
        AddInfoRow(1, "Typ", lblValueType);
        AddInfoRow(2, "Märke", lblValueMake);
        AddInfoRow(3, "Modell", lblValueModel);
        AddInfoRow(4, "Årsmodell", lblValueModelYear);
        AddInfoRow(5, "Färg", lblValueColor);

        txtRawJson.Location = new Point(330, 24);
        txtRawJson.Size = new Size(260, 340);
        txtRawJson.DetectUrls = true;
        txtRawJson.ScrollBars = RichTextBoxScrollBars.Vertical;
        txtRawJson.ReadOnly = true;
        txtRawJson.BorderStyle = BorderStyle.FixedSingle;
        txtRawJson.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);

        Controls.Add(lblRegNr);
        Controls.Add(txtRegNr);
        Controls.Add(btnSearch);
        Controls.Add(tlpCarInfo);
        Controls.Add(txtRawJson);
    }

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

    private async void BtnSearch_ClickAsync(object? sender, EventArgs e)
    {
        var regNr = txtRegNr.Text.Trim().ToUpperInvariant();

        if (string.IsNullOrWhiteSpace(regNr))
        {
            MessageBox.Show("Du måste ange ett registreringsnummer!", "Inmatning saknas",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        ClearResultTable();

        try
        {
            var vehicle = await apiClient.GetVehicleByRegNoAsync(regNr);

            lblValueRegNr.Text = vehicle.RegNo;
            lblValueType.Text = vehicle.Type;
            lblValueMake.Text = vehicle.Make;
            lblValueModel.Text = vehicle.Model;
            lblValueModelYear.Text = vehicle.ModelYear;
            lblValueColor.Text = vehicle.Color;

            txtRawJson.Text = vehicle.RawJson;
        }
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

        UpdateButtons();
    }

    private void ClearResultTable()
    {
        lblValueRegNr.Text = string.Empty;
        lblValueType.Text = string.Empty;
        lblValueMake.Text = string.Empty;
        lblValueModel.Text = string.Empty;
        lblValueModelYear.Text = string.Empty;
        lblValueColor.Text = string.Empty;
        txtRawJson.Clear();
    }

    private void UpdateButtons()
    {
        btnSearch.Enabled = txtRegNr.TextLength > 0;
    }
}
