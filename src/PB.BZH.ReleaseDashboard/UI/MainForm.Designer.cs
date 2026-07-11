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
    tlpMain = new TableLayoutPanel();
    lblTitle = new Label();
    pnlHeader = new Panel();
    flpSummary = new FlowLayoutPanel();
    lblLastCheck = new Label();
    lblSummaryInfo = new Label();
    lblSummaryOk = new Label();
    lblSummaryWarnings = new Label();
    lblSummaryErrors = new Label();
    lblRoot = new Label();
    flpToolbar = new FlowLayoutPanel();
    btnRunReleaseCheck = new Button();
    btnOpenLastReport = new Button();
    btnOpenReportsFolder = new Button();
    btnOpenDownloadUrl = new Button();
    btnOpenArtifactUrl = new Button();
    btnOpenUpdateJsonUrl = new Button();
    dgvProducts = new DataGridView();
    txtConsole = new RichTextBox();
    tlpMain.SuspendLayout();
    pnlHeader.SuspendLayout();
    flpSummary.SuspendLayout();
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
    tlpMain.RowStyles.Add(new RowStyle());
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
    lblTitle.Font = new Font("Segoe UI",16F,FontStyle.Bold);
    lblTitle.Location = new Point(12,12);
    lblTitle.Margin = new Padding(0,0,0,8);
    lblTitle.Name = "lblTitle";
    lblTitle.Size = new Size(1156,30);
    lblTitle.TabIndex = 0;
    lblTitle.Text = "PB BZH Release Dashboard";
    // 
    // pnlHeader
    // 
    pnlHeader.Controls.Add(flpSummary);
    pnlHeader.Controls.Add(lblRoot);
    pnlHeader.Controls.Add(flpToolbar);
    pnlHeader.Dock = DockStyle.Fill;
    pnlHeader.Location = new Point(12,50);
    pnlHeader.Margin = new Padding(0);
    pnlHeader.Name = "pnlHeader";
    pnlHeader.Size = new Size(1156,90);
    pnlHeader.TabIndex = 1;
    // 
    // flpSummary
    // 
    flpSummary.AutoSize = true;
    flpSummary.Controls.Add(lblLastCheck);
    flpSummary.Controls.Add(lblSummaryOk);
    flpSummary.Controls.Add(lblSummaryInfo);
    flpSummary.Controls.Add(lblSummaryWarnings);
    flpSummary.Controls.Add(lblSummaryErrors);
    flpSummary.Location = new Point(0,70);
    flpSummary.Name = "flpSummary";
    flpSummary.Size = new Size(1156,24);
    flpSummary.TabIndex = 2;
    flpSummary.WrapContents = false;
    // 
    // lblLastCheck
    // 
    lblLastCheck.AutoSize = true;
    lblLastCheck.Location = new Point(3,0);
    lblLastCheck.Name = "lblLastCheck";
    lblLastCheck.Padding = new Padding(0,0,26,0);
    lblLastCheck.Size = new Size(94,15);
    lblLastCheck.TabIndex = 0;
    lblLastCheck.Text = "Last check :";
    // 
    // lblSummaryInfo
    // 
    lblSummaryInfo.AutoSize = true;
    lblSummaryInfo.Location = new Point(170,0);
    lblSummaryInfo.Name = "lblSummaryInfo";
    lblSummaryInfo.Padding = new Padding(0,0,24,0);
    lblSummaryInfo.Size = new Size(71,15);
    lblSummaryInfo.TabIndex = 3;
    lblSummaryInfo.Text = "Infos : -";
    // 
    // lblSummaryOk
    // 
    lblSummaryOk.AutoSize = true;
    lblSummaryOk.Location = new Point(103,0);
    lblSummaryOk.Name = "lblSummaryOk";
    lblSummaryOk.Padding = new Padding(0,0,24,0);
    lblSummaryOk.Size = new Size(61,15);
    lblSummaryOk.TabIndex = 1;
    lblSummaryOk.Text = "OK : -";
    // 
    // lblSummaryWarnings
    // 
    lblSummaryWarnings.AutoSize = true;
    lblSummaryWarnings.Location = new Point(247,0);
    lblSummaryWarnings.Name = "lblSummaryWarnings";
    lblSummaryWarnings.Padding = new Padding(0,0,24,0);
    lblSummaryWarnings.Size = new Size(95,15);
    lblSummaryWarnings.TabIndex = 2;
    lblSummaryWarnings.Text = "Warnings : -";
    // 
    // lblSummaryErrors
    // 
    lblSummaryErrors.AutoSize = true;
    lblSummaryErrors.Location = new Point(345,0);
    lblSummaryErrors.Margin = new Padding(0);
    lblSummaryErrors.Name = "lblSummaryErrors";
    lblSummaryErrors.Size = new Size(51,15);
    lblSummaryErrors.TabIndex = 3;
    lblSummaryErrors.Text = "Errors : -";
    // 
    // lblRoot
    // 
    lblRoot.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    lblRoot.AutoSize = true;
    lblRoot.Location = new Point(0,45);
    lblRoot.Margin = new Padding(0);
    lblRoot.Name = "lblRoot";
    lblRoot.Size = new Size(38,15);
    lblRoot.TabIndex = 1;
    lblRoot.Text = "Root :";
    // 
    // flpToolbar
    // 
    flpToolbar.Controls.Add(btnRunReleaseCheck);
    flpToolbar.Controls.Add(btnOpenLastReport);
    flpToolbar.Controls.Add(btnOpenReportsFolder);
    flpToolbar.Controls.Add(btnOpenDownloadUrl);
    flpToolbar.Controls.Add(btnOpenArtifactUrl);
    flpToolbar.Controls.Add(btnOpenUpdateJsonUrl);
    flpToolbar.Dock = DockStyle.Top;
    flpToolbar.Location = new Point(0,0);
    flpToolbar.Name = "flpToolbar";
    flpToolbar.Size = new Size(1156,42);
    flpToolbar.TabIndex = 0;
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
    // btnOpenUpdateJsonUrl
    // 
    btnOpenUpdateJsonUrl.AutoSize = true;
    btnOpenUpdateJsonUrl.Location = new Point(688,0);
    btnOpenUpdateJsonUrl.Margin = new Padding(0,0,8,8);
    btnOpenUpdateJsonUrl.Name = "btnOpenUpdateJsonUrl";
    btnOpenUpdateJsonUrl.Size = new Size(124,34);
    btnOpenUpdateJsonUrl.TabIndex = 4;
    btnOpenUpdateJsonUrl.Text = "Open update.json";
    btnOpenUpdateJsonUrl.UseVisualStyleBackColor = true;
    btnOpenUpdateJsonUrl.Click += btnOpenUpdateJsonUrl_Click;
    // 
    // dgvProducts
    // 
    dgvProducts.AllowUserToAddRows = false;
    dgvProducts.AllowUserToDeleteRows = false;
    dgvProducts.BackgroundColor = SystemColors.Window;
    dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    dgvProducts.Dock = DockStyle.Fill;
    dgvProducts.Location = new Point(12,140);
    dgvProducts.Margin = new Padding(0,0,0,8);
    dgvProducts.MultiSelect = false;
    dgvProducts.Name = "dgvProducts";
    dgvProducts.ReadOnly = true;
    dgvProducts.RowHeadersVisible = false;
    dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
    dgvProducts.Size = new Size(1156,326);
    dgvProducts.TabIndex = 2;
    // 
    // txtConsole
    // 
    txtConsole.BackColor = Color.FromArgb(30,30,30);
    txtConsole.Dock = DockStyle.Fill;
    txtConsole.Font = new Font("Consolas",9F);
    txtConsole.ForeColor = Color.Gainsboro;
    txtConsole.Location = new Point(12,474);
    txtConsole.Margin = new Padding(0);
    txtConsole.Name = "txtConsole";
    txtConsole.ReadOnly = true;
    txtConsole.Size = new Size(1156,274);
    txtConsole.TabIndex = 3;
    txtConsole.Text = "";
    txtConsole.WordWrap = false;
    txtConsole.LinkClicked += txtConsole_LinkClicked;
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
    flpSummary.ResumeLayout(false);
    flpSummary.PerformLayout();
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
  private RichTextBox txtConsole;
  private Button btnOpenUpdateJsonUrl;
  private FlowLayoutPanel flpSummary;
  private Label lblLastCheck;
  private Label lblSummaryOk;
  private Label lblSummaryWarnings;
  private Label lblSummaryErrors;
  private Label lblSummaryInfo;
}