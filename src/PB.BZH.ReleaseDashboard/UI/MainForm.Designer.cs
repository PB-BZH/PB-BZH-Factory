namespace PB.BZH.ReleaseDashboard.UI;

partial class MainForm {
  private System.ComponentModel.IContainer components = null!;

  protected override void Dispose(bool disposing) {
    if (disposing && (components != null)) {
      components.Dispose();
    }

    base.Dispose(disposing);
  }

  #region Windows Form Designer generated code

  private void InitializeComponent() {
    components = new System.ComponentModel.Container();

    tlpMain = new TableLayoutPanel();
    lblTitle = new Label();
    pnlHeader = new Panel();
    flpToolbar = new FlowLayoutPanel();
    btnRunReleaseCheck = new Button();
    btnOpenLastReport = new Button();
    btnOpenReportsFolder = new Button();
    btnOpenDownloadUrl = new Button();
    btnOpenArtifactUrl = new Button();
    lblRoot = new Label();
    dgvProducts = new DataGridView();
    colDisplayName = new DataGridViewTextBoxColumn();
    colLastCheck = new DataGridViewTextBoxColumn();
    colType = new DataGridViewTextBoxColumn();
    colVersion = new DataGridViewTextBoxColumn();
    colArtifactFile = new DataGridViewTextBoxColumn();
    colLocalCheck = new DataGridViewTextBoxColumn();
    colStatus = new DataGridViewTextBoxColumn();
    txtConsole = new TextBox();

    tlpMain.SuspendLayout();
    pnlHeader.SuspendLayout();
    flpToolbar.SuspendLayout();
    ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
    SuspendLayout();

    // 
    // tlpMain
    // 
    tlpMain.ColumnCount = 1;
    tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100F));
    tlpMain.Controls.Add(lblTitle,0,0);
    tlpMain.Controls.Add(pnlHeader,0,1);
    tlpMain.Controls.Add(dgvProducts,0,2);
    tlpMain.Controls.Add(txtConsole,0,3);
    tlpMain.Dock = DockStyle.Fill;
    tlpMain.Location = new Point(0,0);
    tlpMain.Name = "tlpMain";
    tlpMain.Padding = new Padding(12);
    tlpMain.RowCount = 4;
    tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
    tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute,90F));
    tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent,55F));
    tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent,45F));
    tlpMain.Size = new Size(1180,760);
    tlpMain.TabIndex = 0;

    // 
    // lblTitle
    // 
    lblTitle.AutoSize = true;
    lblTitle.Dock = DockStyle.Fill;
    lblTitle.Font = new Font("Segoe UI",16F,FontStyle.Bold,GraphicsUnit.Point);
    lblTitle.Location = new Point(12,12);
    lblTitle.Margin = new Padding(0,0,0,8);
    lblTitle.Name = "lblTitle";
    lblTitle.Size = new Size(1156,30);
    lblTitle.TabIndex = 0;
    lblTitle.Text = "PB BZH Release Dashboard";

    // 
    // pnlHeader
    // 
    pnlHeader.Controls.Add(lblRoot);
    pnlHeader.Controls.Add(flpToolbar);
    pnlHeader.Dock = DockStyle.Fill;
    pnlHeader.Location = new Point(12,50);
    pnlHeader.Margin = new Padding(0);
    pnlHeader.Name = "pnlHeader";
    pnlHeader.Size = new Size(1156,90);
    pnlHeader.TabIndex = 1;

    // 
    // flpToolbar
    // 
    flpToolbar.AutoSize = false;
    flpToolbar.Controls.Add(btnRunReleaseCheck);
    flpToolbar.Controls.Add(btnOpenLastReport);
    flpToolbar.Controls.Add(btnOpenReportsFolder);
    flpToolbar.Controls.Add(btnOpenDownloadUrl);
    flpToolbar.Controls.Add(btnOpenArtifactUrl);
    flpToolbar.Dock = DockStyle.Top;
    flpToolbar.FlowDirection = FlowDirection.LeftToRight;
    flpToolbar.Location = new Point(0,0);
    flpToolbar.Name = "flpToolbar";
    flpToolbar.Size = new Size(1156,42);
    flpToolbar.TabIndex = 0;
    flpToolbar.WrapContents = true;

    // 
    // btnRunReleaseCheck
    // 
    btnRunReleaseCheck.AutoSize = true;
    btnRunReleaseCheck.Location = new Point(0,0);
    btnRunReleaseCheck.Margin = new Padding(0,0,8,8);
    btnRunReleaseCheck.Name = "btnRunReleaseCheck";
    btnRunReleaseCheck.Size = new Size(129,34);
    btnRunReleaseCheck.TabIndex = 0;
    btnRunReleaseCheck.Text = "Run release check";
    btnRunReleaseCheck.UseVisualStyleBackColor = true;
    btnRunReleaseCheck.Click += btnRunReleaseCheck_Click;

    // 
    // btnOpenLastReport
    // 
    btnOpenLastReport.AutoSize = true;
    btnOpenLastReport.Location = new Point(137,0);
    btnOpenLastReport.Margin = new Padding(0,0,8,8);
    btnOpenLastReport.Name = "btnOpenLastReport";
    btnOpenLastReport.Size = new Size(119,34);
    btnOpenLastReport.TabIndex = 1;
    btnOpenLastReport.Text = "Open last report";
    btnOpenLastReport.UseVisualStyleBackColor = true;
    btnOpenLastReport.Click += btnOpenLastReport_Click;

    // 
    // btnOpenReportsFolder
    // 
    btnOpenReportsFolder.AutoSize = true;
    btnOpenReportsFolder.Location = new Point(264,0);
    btnOpenReportsFolder.Margin = new Padding(0,0,8,8);
    btnOpenReportsFolder.Name = "btnOpenReportsFolder";
    btnOpenReportsFolder.Size = new Size(139,34);
    btnOpenReportsFolder.TabIndex = 2;
    btnOpenReportsFolder.Text = "Open reports folder";
    btnOpenReportsFolder.UseVisualStyleBackColor = true;
    btnOpenReportsFolder.Click += btnOpenReportsFolder_Click;

    // 
    // btnOpenDownloadUrl
    // 
    btnOpenDownloadUrl.AutoSize = true;
    btnOpenDownloadUrl.Location = new Point(411,0);
    btnOpenDownloadUrl.Margin = new Padding(0,0,8,8);
    btnOpenDownloadUrl.Name = "btnOpenDownloadUrl";
    btnOpenDownloadUrl.Size = new Size(137,34);
    btnOpenDownloadUrl.TabIndex = 3;
    btnOpenDownloadUrl.Text = "Open download URL";
    btnOpenDownloadUrl.UseVisualStyleBackColor = true;
    btnOpenDownloadUrl.Click += btnOpenDownloadUrl_Click;

    // 
    // btnOpenArtifactUrl
    // 
    btnOpenArtifactUrl.AutoSize = true;
    btnOpenArtifactUrl.Location = new Point(556,0);
    btnOpenArtifactUrl.Margin = new Padding(0,0,8,8);
    btnOpenArtifactUrl.Name = "btnOpenArtifactUrl";
    btnOpenArtifactUrl.Size = new Size(124,34);
    btnOpenArtifactUrl.TabIndex = 4;
    btnOpenArtifactUrl.Text = "Open artifact URL";
    btnOpenArtifactUrl.UseVisualStyleBackColor = true;
    btnOpenArtifactUrl.Click += btnOpenArtifactUrl_Click;

    // 
    // lblRoot
    // 
    lblRoot.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    lblRoot.AutoSize = true;
    lblRoot.Location = new Point(0,54);
    lblRoot.Margin = new Padding(0);
    lblRoot.Name = "lblRoot";
    lblRoot.Size = new Size(40,15);
    lblRoot.TabIndex = 1;
    lblRoot.Text = "Root :";

    // 
    // dgvProducts
    // 
    dgvProducts.AllowUserToAddRows = false;
    dgvProducts.AllowUserToDeleteRows = false;
    dgvProducts.AutoGenerateColumns = false;
    dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
    dgvProducts.BackgroundColor = SystemColors.Window;
    dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    dgvProducts.Columns.AddRange(
      new DataGridViewColumn[] {
        colDisplayName,
        colType,
        colVersion,
        colStatus,
        colLastCheck,
        colArtifactFile,
        colLocalCheck
      });
    dgvProducts.Dock = DockStyle.Fill;
    dgvProducts.Location = new Point(12,140);
    dgvProducts.Margin = new Padding(0,0,0,8);
    dgvProducts.MultiSelect = false;
    dgvProducts.Name = "dgvProducts";
    dgvProducts.ReadOnly = true;
    dgvProducts.RowHeadersVisible = false;
    dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
    dgvProducts.Size = new Size(1156,328);
    dgvProducts.TabIndex = 2;

    // 
    // colDisplayName
    // 
    colDisplayName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    colDisplayName.DataPropertyName = "DisplayName";
    colDisplayName.HeaderText = "Product";
    colDisplayName.MinimumWidth = 180;
    colDisplayName.Name = "colDisplayName";
    colDisplayName.ReadOnly = true;

    // 
    // colType
    // 
    colType.DataPropertyName = "Type";
    colType.HeaderText = "Type";
    colType.Name = "colType";
    colType.ReadOnly = true;
    colType.Width = 80;

    // 
    // colVersion
    // 
    colVersion.DataPropertyName = "Version";
    colVersion.HeaderText = "Version";
    colVersion.Name = "colVersion";
    colVersion.ReadOnly = true;
    colVersion.Width = 100;

    // 
    // colStatus
    // 
    colStatus.DataPropertyName = "Status";
    colStatus.HeaderText = "Status";
    colStatus.Name = "colStatus";
    colStatus.ReadOnly = true;
    colStatus.Width = 90;

    // 
    // colLastCheck
    // 
    colLastCheck.DataPropertyName = "LastCheck";
    colLastCheck.HeaderText = "Last Check";
    colLastCheck.Name = "colLastCheck";
    colLastCheck.ReadOnly = true;
    colLastCheck.Width = 160;
    // 
    // colArtifactFile
    // 
    colArtifactFile.DataPropertyName = "ArtifactFile";
    colArtifactFile.HeaderText = "Artifact";
    colArtifactFile.Name = "colArtifactFile";
    colArtifactFile.ReadOnly = true;
    colArtifactFile.Width = 280;

    // 
    // colLocalCheck
    // 
    colLocalCheck.DataPropertyName = "LocalCheck";
    colLocalCheck.HeaderText = "Local check";
    colLocalCheck.Name = "colLocalCheck";
    colLocalCheck.ReadOnly = true;
    colLocalCheck.Width = 120;

    // 
    // txtConsole
    // 
    txtConsole.BackColor = Color.FromArgb(30,30,30);
    txtConsole.Dock = DockStyle.Fill;
    txtConsole.Font = new Font("Consolas",9F,FontStyle.Regular,GraphicsUnit.Point);
    txtConsole.ForeColor = Color.Gainsboro;
    txtConsole.Location = new Point(12,476);
    txtConsole.Margin = new Padding(0);
    txtConsole.Multiline = true;
    txtConsole.Name = "txtConsole";
    txtConsole.ReadOnly = true;
    txtConsole.ScrollBars = ScrollBars.Both;
    txtConsole.Size = new Size(1156,272);
    txtConsole.TabIndex = 3;
    txtConsole.WordWrap = false;

    // 
    // MainForm
    // 
    AutoScaleDimensions = new SizeF(7F,15F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(1180,760);
    Controls.Add(tlpMain);
    MinimumSize = new Size(950,600);
    Name = "MainForm";
    StartPosition = FormStartPosition.CenterScreen;
    Text = "PB BZH Release Dashboard";

    tlpMain.ResumeLayout(false);
    tlpMain.PerformLayout();
    pnlHeader.ResumeLayout(false);
    pnlHeader.PerformLayout();
    flpToolbar.ResumeLayout(false);
    flpToolbar.PerformLayout();
    ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
    ResumeLayout(false);
  }

  #endregion

  private TableLayoutPanel tlpMain;
  private Label lblTitle;
  private Panel pnlHeader;
  private FlowLayoutPanel flpToolbar;
  private Button btnRunReleaseCheck;
  private Button btnOpenLastReport;
  private Button btnOpenReportsFolder;
  private Button btnOpenDownloadUrl;
  private Button btnOpenArtifactUrl;
  private Label lblRoot;
  private DataGridView dgvProducts;
  private DataGridViewTextBoxColumn colDisplayName;
  private DataGridViewTextBoxColumn colType;
  private DataGridViewTextBoxColumn colVersion;
  private DataGridViewTextBoxColumn colStatus;
  private DataGridViewTextBoxColumn colLastCheck;
  private DataGridViewTextBoxColumn colArtifactFile;
  private DataGridViewTextBoxColumn colLocalCheck;
  private TextBox txtConsole;
}