namespace PB.BZH.ReleaseDashboard.UI.Grids;

public static class ProductGridViewConfigurator {
  public const string ColDisplayName = "colDisplayName";
  public const string ColType = "colType";
  public const string ColVersion = "colVersion";
  public const string ColStatus = "colStatus";
  public const string ColLastCheck = "colLastCheck";
  public const string ColArtifactFile = "colArtifactFile";
  public const string ColLocalCheck = "colLocalCheck";

  public static void Configure(DataGridView grid) {
    if (grid == null)
      throw new ArgumentNullException(nameof(grid));

    grid.SuspendLayout();

    try {
      grid.Columns.Clear();

      grid.AutoGenerateColumns = false;
      grid.AllowUserToAddRows = false;
      grid.AllowUserToDeleteRows = false;
      grid.ReadOnly = true;
      grid.MultiSelect = false;
      grid.RowHeadersVisible = false;
      grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      grid.BackgroundColor = SystemColors.Window;
      grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

      grid.Columns.Add(CreateTextColumn(
          ColDisplayName,
          "Product",
          "DisplayName",
          220,
          DataGridViewAutoSizeColumnMode.Fill));

      grid.Columns.Add(CreateTextColumn(
          ColType,
          "Type",
          "Type",
          80));

      grid.Columns.Add(CreateTextColumn(
          ColVersion,
          "Version",
          "Version",
          90));

      grid.Columns.Add(CreateTextColumn(
          ColStatus,
          "Status",
          "Status",
          90));

      grid.Columns.Add(CreateTextColumn(
          ColLastCheck,
          "Last Check",
          "LastCheck",
          160));

      grid.Columns.Add(CreateTextColumn(
          ColArtifactFile,
          "Artifact",
          "ArtifactFile",
          280));

      grid.Columns.Add(CreateTextColumn(
          ColLocalCheck,
          "Local check",
          "LocalCheck",
          110));
    }
    finally {
      grid.ResumeLayout();
    }
  }

  public static void ApplyStatusColors(DataGridView grid) {
    if (grid == null)
      return;

    foreach (DataGridViewRow row in grid.Rows) {
      if (row.IsNewRow)
        continue;

      string status =
          Convert.ToString(row.Cells[ColStatus].Value) ?? string.Empty;

      ApplyDefaultRowStyle(row);
      ApplyStatusCellStyle(row,status);
    }
  }

  private static DataGridViewTextBoxColumn CreateTextColumn(
      string name,
      string headerText,
      string dataPropertyName,
      int width,
      DataGridViewAutoSizeColumnMode autoSizeMode = DataGridViewAutoSizeColumnMode.None) {
    return new DataGridViewTextBoxColumn {
      Name = name,
      HeaderText = headerText,
      DataPropertyName = dataPropertyName,
      Width = width,
      MinimumWidth = Math.Min(width,120),
      AutoSizeMode = autoSizeMode,
      ReadOnly = true
    };
  }

  private static void ApplyDefaultRowStyle(DataGridViewRow row) {
    row.DefaultCellStyle.BackColor = Color.White;
    row.DefaultCellStyle.ForeColor = Color.Black;
  }

  private static void ApplyStatusCellStyle(
      DataGridViewRow row,
      string status) {
    DataGridViewCell statusCell =
        row.Cells[ColStatus];

    switch (status.ToUpperInvariant()) {
      case "OK":
        statusCell.Style.BackColor = Color.FromArgb(210,245,210);
        statusCell.Style.ForeColor = Color.DarkGreen;
        break;

      case "WARN":
        statusCell.Style.BackColor = Color.FromArgb(255,240,190);
        statusCell.Style.ForeColor = Color.DarkOrange;
        break;

      case "FAIL":
        statusCell.Style.BackColor = Color.FromArgb(255,210,210);
        statusCell.Style.ForeColor = Color.DarkRed;
        break;

      default:
        statusCell.Style.BackColor = Color.White;
        statusCell.Style.ForeColor = Color.Gray;
        break;
    }
  }

  public static void ApplyLightGridTheme(DataGridView grid) {
    if (grid == null)
      throw new ArgumentNullException(nameof(grid));

    grid.EnableHeadersVisualStyles = true;

    grid.BackgroundColor = Color.White;
    grid.GridColor = Color.Silver;
    grid.BorderStyle = BorderStyle.FixedSingle;

    grid.DefaultCellStyle.BackColor = Color.White;
    grid.DefaultCellStyle.ForeColor = Color.Black;
    grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0,122,204);
    grid.DefaultCellStyle.SelectionForeColor = Color.White;

    grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245,248,252);
    grid.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

    grid.CellFormatting -= Grid_CellFormatting;
    grid.CellFormatting += Grid_CellFormatting;
  }

  private static void Grid_CellFormatting(
      object? sender,
      DataGridViewCellFormattingEventArgs e) {
    if (sender is not DataGridView grid)
      return;

    if (e.RowIndex < 0)
      return;

    if (grid.Columns[e.ColumnIndex].Name != ColStatus)
      return;

    string status =
        Convert.ToString(e.Value) ?? string.Empty;

    switch (status.ToUpperInvariant()) {
      case "OK":
        e.CellStyle.BackColor = Color.FromArgb(210,245,210);
        e.CellStyle.ForeColor = Color.DarkGreen;
        break;

      case "WARN":
        e.CellStyle.BackColor = Color.FromArgb(95,75,25);
        e.CellStyle.ForeColor = Color.FromArgb(255,210,90);
        break;

      case "FAIL":
        e.CellStyle.BackColor = Color.FromArgb(95,35,35);
        e.CellStyle.ForeColor = Color.FromArgb(255,120,120);
        break;

      case "NOT CHECKED":
        e.CellStyle.BackColor = Color.FromArgb(230,230,230);
        e.CellStyle.ForeColor = Color.DimGray;
        break;

      default:
        e.CellStyle.BackColor = Color.FromArgb(37,37,38);
        e.CellStyle.ForeColor = Color.Silver;
        break;
    }
  }
}