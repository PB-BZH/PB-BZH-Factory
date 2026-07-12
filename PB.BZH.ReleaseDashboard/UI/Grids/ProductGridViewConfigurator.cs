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

    grid.AutoGenerateColumns = false;
    grid.Columns.Clear();

    grid.Columns.Add(new DataGridViewTextBoxColumn {
      Name = ColDisplayName,
      HeaderText = "Product",
      DataPropertyName = "DisplayName",
      Width = 260
    });

    grid.Columns.Add(new DataGridViewTextBoxColumn {
      Name = ColType,
      HeaderText = "Type",
      DataPropertyName = "Type",
      Width = 80
    });

    grid.Columns.Add(new DataGridViewTextBoxColumn {
      Name = ColVersion,
      HeaderText = "Version",
      DataPropertyName = "Version",
      Width = 90
    });

    grid.Columns.Add(new DataGridViewTextBoxColumn {
      Name = ColStatus,
      HeaderText = "Status",
      DataPropertyName = "Status",
      Width = 90
    });

    grid.Columns.Add(new DataGridViewTextBoxColumn {
      Name = ColLastCheck,
      HeaderText = "Last Check",
      DataPropertyName = "LastCheck",
      Width = 160
    });

    grid.Columns.Add(new DataGridViewTextBoxColumn {
      Name = ColArtifactFile,
      HeaderText = "Artifact",
      DataPropertyName = "ArtifactFile",
      Width = 260
    });

    grid.Columns.Add(new DataGridViewTextBoxColumn {
      Name = ColLocalCheck,
      HeaderText = "Local check",
      DataPropertyName = "LocalCheck",
      Width = 120
    });

    ApplyLightGridTheme(grid);
    AttachStatusFormatting(grid);
  }

  public static void AttachStatusFormatting(DataGridView grid) {
    if (grid == null)
      throw new ArgumentNullException(nameof(grid));

    grid.CellFormatting -= Grid_CellFormatting;
    grid.CellFormatting += Grid_CellFormatting;
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

    Color gridBackColor = Color.Black;
    Color alternateBackColor = Color.Black;
    Color headerBackColor = Color.Black;
    Color textColor = Color.White;
    Color selectionBackColor = Color.FromArgb(0,120,215);

    grid.EnableHeadersVisualStyles = false;

    grid.BackgroundColor = gridBackColor;
    grid.GridColor = Color.FromArgb(120,120,120);
    grid.BorderStyle = BorderStyle.FixedSingle;

    grid.DefaultCellStyle.BackColor = gridBackColor;
    grid.DefaultCellStyle.ForeColor = textColor;
    grid.DefaultCellStyle.SelectionBackColor = selectionBackColor;
    grid.DefaultCellStyle.SelectionForeColor = Color.White;

    grid.RowsDefaultCellStyle.BackColor = gridBackColor;
    grid.RowsDefaultCellStyle.ForeColor = textColor;
    grid.RowsDefaultCellStyle.SelectionBackColor = selectionBackColor;
    grid.RowsDefaultCellStyle.SelectionForeColor = Color.White;

    grid.AlternatingRowsDefaultCellStyle.BackColor = alternateBackColor;
    grid.AlternatingRowsDefaultCellStyle.ForeColor = textColor;
    grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = selectionBackColor;
    grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

    grid.ColumnHeadersDefaultCellStyle.BackColor = headerBackColor;
    grid.ColumnHeadersDefaultCellStyle.ForeColor = textColor;
    grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = headerBackColor;
    grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = textColor;

    grid.RowHeadersDefaultCellStyle.BackColor = headerBackColor;
    grid.RowHeadersDefaultCellStyle.ForeColor = textColor;
    grid.RowHeadersDefaultCellStyle.SelectionBackColor = headerBackColor;
    grid.RowHeadersDefaultCellStyle.SelectionForeColor = textColor;

    foreach (DataGridViewColumn column in grid.Columns) {
      column.HeaderCell.Style.BackColor = headerBackColor;
      column.HeaderCell.Style.ForeColor = textColor;
      column.HeaderCell.Style.SelectionBackColor = headerBackColor;
      column.HeaderCell.Style.SelectionForeColor = textColor;
    }
  }

  private static void Grid_CellFormatting(object? sender,DataGridViewCellFormattingEventArgs e) {
    if (sender is not DataGridView grid)
      return;

    if (e.RowIndex < 0 || e.ColumnIndex < 0)
      return;

    DataGridViewColumn column =
        grid.Columns[e.ColumnIndex];

    bool isStatusColumn =
        string.Equals(column.Name,ColStatus,StringComparison.OrdinalIgnoreCase) ||
        string.Equals(column.DataPropertyName,"Status",StringComparison.OrdinalIgnoreCase) ||
        string.Equals(column.HeaderText,"Status",StringComparison.OrdinalIgnoreCase);

    if (!isStatusColumn)
      return;

    string status =
        Convert.ToString(e.Value)?.Trim().ToUpperInvariant() ?? string.Empty;

    switch (status) {
      case "OK":
        e.CellStyle.BackColor = Color.FromArgb(200,245,200);
        e.CellStyle.ForeColor = Color.DarkGreen;
        e.CellStyle.SelectionBackColor = Color.FromArgb(200,245,200);
        e.CellStyle.SelectionForeColor = Color.DarkGreen;
        break;

      case "WARN":
      case "WARNING":
        e.CellStyle.BackColor = Color.FromArgb(255,235,170);
        e.CellStyle.ForeColor = Color.DarkOrange;
        e.CellStyle.SelectionBackColor = Color.FromArgb(255,235,170);
        e.CellStyle.SelectionForeColor = Color.DarkOrange;
        break;

      case "FAIL":
      case "ERROR":
        e.CellStyle.BackColor = Color.FromArgb(255,200,200);
        e.CellStyle.ForeColor = Color.DarkRed;
        e.CellStyle.SelectionBackColor = Color.FromArgb(255,200,200);
        e.CellStyle.SelectionForeColor = Color.DarkRed;
        break;

      case "NOT CHECKED":
      case "UNKNOWN":
        e.CellStyle.BackColor = Color.FromArgb(230,230,230);
        e.CellStyle.ForeColor = Color.DimGray;
        e.CellStyle.SelectionBackColor = Color.FromArgb(230,230,230);
        e.CellStyle.SelectionForeColor = Color.DimGray;
        break;
    }

    e.FormattingApplied = true;
  }
}