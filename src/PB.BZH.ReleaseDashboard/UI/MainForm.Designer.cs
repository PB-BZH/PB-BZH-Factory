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
    lblSummaryOk = new Label();
    lblSummaryInfo = new Label();
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
    splitProducts = new SplitContainer();
    dgvProducts = new DataGridView();
    grpProductDetails = new GroupBox();
    tlpProductDetails = new TableLayoutPanel();
    lblDetailProduct = new Label();
    lblDetailType = new Label();
    lblDetailVersion = new Label();
    lblDetailStatus = new Label();
    lblDetailDownloadUrl = new ();
    txtDetailDownloadUrl = new ();
    lblDetailArtifactUrl = new ();
    txtDetailArtifactUrl = new ();
    lblDetailSha256Url = new ();
    txtDetailSha256Url = new ();
    lblDetailUpdateJsonUrl = new Label();
    txtDetailUpdateJsonUrl = new();
    txtConsole = new RichTextBox();
    tlpMain.SuspendLayout();
    pnlHeader.SuspendLayout();
    flpSummary.SuspendLayout();
    flpToolbar.SuspendLayout();
    ((System.ComponentModel.ISupportInitialize)splitProducts).BeginInit();
    splitProducts.Panel1.SuspendLayout();
    splitProducts.Panel2.SuspendLayout();
    splitProducts.SuspendLayout();
    ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
    grpProductDetails.SuspendLayout();
    tlpProductDetails.SuspendLayout();
    SuspendLayout();
    // 
    // tlpMain
    // 
    tlpMain.ColumnCount = 1;
    tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100F));
    tlpMain.Controls.Add(lblTitle,0,0);
    tlpMain.Controls.Add(pnlHeader,0,1);
    tlpMain.Controls.Add(splitProducts,0,2);
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
    // lblSummaryInfo
    // 
    lblSummaryInfo.AutoSize = true;
    lblSummaryInfo.Location = new Point(170,0);
    lblSummaryInfo.Name = "lblSummaryInfo";
    lblSummaryInfo.Padding = new Padding(0,0,24,0);
    lblSummaryInfo.Size = new Size(71,15);
    lblSummaryInfo.TabIndex = 2;
    lblSummaryInfo.Text = "Infos : -";
    // 
    // lblSummaryWarnings
    // 
    lblSummaryWarnings.AutoSize = true;
    lblSummaryWarnings.Location = new Point(247,0);
    lblSummaryWarnings.Name = "lblSummaryWarnings";
    lblSummaryWarnings.Padding = new Padding(0,0,24,0);
    lblSummaryWarnings.Size = new Size(95,15);
    lblSummaryWarnings.TabIndex = 3;
    lblSummaryWarnings.Text = "Warnings : -";
    // 
    // lblSummaryErrors
    // 
    lblSummaryErrors.AutoSize = true;
    lblSummaryErrors.Location = new Point(345,0);
    lblSummaryErrors.Margin = new Padding(0);
    lblSummaryErrors.Name = "lblSummaryErrors";
    lblSummaryErrors.Size = new Size(51,15);
    lblSummaryErrors.TabIndex = 4;
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
    btnOpenUpdateJsonUrl.TabIndex = 5;
    btnOpenUpdateJsonUrl.Text = "Open update.json";
    btnOpenUpdateJsonUrl.UseVisualStyleBackColor = true;
    btnOpenUpdateJsonUrl.Click += btnOpenUpdateJsonUrl_Click;
    // 
    // splitProducts
    // 
    splitProducts.Dock = DockStyle.Fill;
    splitProducts.Location = new Point(12,140);
    splitProducts.Margin = new Padding(0,0,0,8);
    splitProducts.Name = "splitProducts";
    // 
    // splitProducts.Panel1
    // 
    splitProducts.Panel1.Controls.Add(dgvProducts);
    // 
    // splitProducts.Panel2
    // 
    splitProducts.Panel2.Controls.Add(grpProductDetails);
    splitProducts.Size = new Size(1156,326);
    splitProducts.SplitterDistance = 780;
    splitProducts.TabIndex = 2;
    // 
    // dgvProducts
    // 
    dgvProducts.AllowUserToAddRows = false;
    dgvProducts.AllowUserToDeleteRows = false;
    dgvProducts.BackgroundColor = SystemColors.Window;
    dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
    dgvProducts.Dock = DockStyle.Fill;
    dgvProducts.Location = new Point(0,0);
    dgvProducts.Margin = new Padding(0);
    dgvProducts.MultiSelect = false;
    dgvProducts.Name = "dgvProducts";
    dgvProducts.ReadOnly = true;
    dgvProducts.RowHeadersVisible = false;
    dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
    dgvProducts.Size = new Size(780,326);
    dgvProducts.TabIndex = 0;
    dgvProducts.SelectionChanged += dgvProducts_SelectionChanged;
    // 
    // grpProductDetails
    // 
    grpProductDetails.Controls.Add(tlpProductDetails);
    grpProductDetails.Dock = DockStyle.Fill;
    grpProductDetails.Location = new Point(0,0);
    grpProductDetails.Name = "grpProductDetails";
    grpProductDetails.Padding = new Padding(8);
    grpProductDetails.Size = new Size(372,326);
    grpProductDetails.TabIndex = 0;
    grpProductDetails.TabStop = false;
    grpProductDetails.Text = "Product details";
    // 
    // tlpProductDetails
    // 
    tlpProductDetails.ColumnCount = 1;
    tlpProductDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100F));
    tlpProductDetails.Controls.Add(lblDetailProduct,0,0);
    tlpProductDetails.Controls.Add(lblDetailType,0,1);
    tlpProductDetails.Controls.Add(lblDetailVersion,0,2);
    tlpProductDetails.Controls.Add(lblDetailStatus,0,3);
    tlpProductDetails.Controls.Add(lblDetailDownloadUrl,0,4);
    tlpProductDetails.Controls.Add(txtDetailDownloadUrl,0,5);
    tlpProductDetails.Controls.Add(lblDetailArtifactUrl,0,6);
    tlpProductDetails.Controls.Add(txtDetailArtifactUrl,0,7);
    tlpProductDetails.Controls.Add(lblDetailSha256Url,0,8);
    tlpProductDetails.Controls.Add(txtDetailSha256Url,0,9);
    tlpProductDetails.Controls.Add(lblDetailUpdateJsonUrl,0,10);
    tlpProductDetails.Controls.Add(txtDetailUpdateJsonUrl,0,11);
    tlpProductDetails.Dock = DockStyle.Fill;
    tlpProductDetails.Location = new Point(8,24);
    tlpProductDetails.Name = "tlpProductDetails";
    tlpProductDetails.RowCount = 12;
    tlpProductDetails.RowStyles.Add(new RowStyle());
    tlpProductDetails.RowStyles.Add(new RowStyle());
    tlpProductDetails.RowStyles.Add(new RowStyle());
    tlpProductDetails.RowStyles.Add(new RowStyle());
    tlpProductDetails.RowStyles.Add(new RowStyle());
    tlpProductDetails.RowStyles.Add(new RowStyle(SizeType.Absolute,42F));
    tlpProductDetails.RowStyles.Add(new RowStyle());
    tlpProductDetails.RowStyles.Add(new RowStyle(SizeType.Absolute,42F));
    tlpProductDetails.RowStyles.Add(new RowStyle());
    tlpProductDetails.RowStyles.Add(new RowStyle(SizeType.Absolute,42F));
    tlpProductDetails.RowStyles.Add(new RowStyle());
    tlpProductDetails.RowStyles.Add(new RowStyle(SizeType.Absolute,42F));
    tlpProductDetails.Size = new Size(356,294);
    tlpProductDetails.TabIndex = 0;
    // 
    // lblDetailProduct
    // 
    lblDetailProduct.AutoSize = true;
    lblDetailProduct.Dock = DockStyle.Fill;
    lblDetailProduct.Font = new Font("Segoe UI",9F,FontStyle.Bold);
    lblDetailProduct.Location = new Point(3,0);
    lblDetailProduct.Name = "lblDetailProduct";
    lblDetailProduct.Size = new Size(350,15);
    lblDetailProduct.TabIndex = 0;
    lblDetailProduct.Text = "Product : -";
    // 
    // lblDetailType
    // 
    lblDetailType.AutoSize = true;
    lblDetailType.Dock = DockStyle.Fill;
    lblDetailType.Location = new Point(3,15);
    lblDetailType.Name = "lblDetailType";
    lblDetailType.Size = new Size(350,15);
    lblDetailType.TabIndex = 1;
    lblDetailType.Text = "Type : -";
    // 
    // lblDetailVersion
    // 
    lblDetailVersion.AutoSize = true;
    lblDetailVersion.Dock = DockStyle.Fill;
    lblDetailVersion.Location = new Point(3,30);
    lblDetailVersion.Name = "lblDetailVersion";
    lblDetailVersion.Size = new Size(350,15);
    lblDetailVersion.TabIndex = 2;
    lblDetailVersion.Text = "Version : -";
    // 
    // lblDetailStatus
    // 
    lblDetailStatus.AutoSize = true;
    lblDetailStatus.Dock = DockStyle.Fill;
    lblDetailStatus.Location = new Point(3,45);
    lblDetailStatus.Margin = new Padding(3,0,3,8);
    lblDetailStatus.Name = "lblDetailStatus";
    lblDetailStatus.Size = new Size(350,15);
    lblDetailStatus.TabIndex = 3;
    lblDetailStatus.Text = "Status : -";
    // 
    // lblDetailDownloadUrl
    // 
    lblDetailDownloadUrl.AutoSize = true;
    lblDetailDownloadUrl.Dock = DockStyle.Fill;
    lblDetailDownloadUrl.Location = new Point(3,68);
    lblDetailDownloadUrl.Name = "lblDetailDownloadUrl";
    lblDetailDownloadUrl.Size = new Size(350,15);
    lblDetailDownloadUrl.TabIndex = 4;
    lblDetailDownloadUrl.Text = "Download URL";
    // 
    // txtDetailDownloadUrl
    // 
    txtDetailDownloadUrl.BorderStyle = BorderStyle.FixedSingle;
    txtDetailDownloadUrl.DetectUrls = true;
    txtDetailDownloadUrl.Dock = DockStyle.Fill;
    txtDetailDownloadUrl.Font = new Font("Consolas",8.5F);
    txtDetailDownloadUrl.Location = new Point(3,86);
    txtDetailDownloadUrl.Name = "txtDetailDownloadUrl";
    txtDetailDownloadUrl.ReadOnly = true;
    txtDetailDownloadUrl.ScrollBars = RichTextBoxScrollBars.Horizontal;
    txtDetailDownloadUrl.Size = new Size(350,36);
    txtDetailDownloadUrl.TabIndex = 5;
    txtDetailDownloadUrl.Text = "";
    txtDetailDownloadUrl.WordWrap = false;
    txtDetailDownloadUrl.LinkClicked += detailUrl_LinkClicked;    
    
    // 
    // lblDetailArtifactUrl
    // 
    lblDetailArtifactUrl.AutoSize = true;
    lblDetailArtifactUrl.Dock = DockStyle.Fill;
    lblDetailArtifactUrl.Location = new Point(3,125);
    lblDetailArtifactUrl.Name = "lblDetailArtifactUrl";
    lblDetailArtifactUrl.Size = new Size(350,15);
    lblDetailArtifactUrl.TabIndex = 6;
    lblDetailArtifactUrl.Text = "Artifact URL";
    // 
    // txtDetailArtifactUrl
    // 
    txtDetailArtifactUrl.BorderStyle = BorderStyle.FixedSingle;
    txtDetailArtifactUrl.DetectUrls = true;
    txtDetailArtifactUrl.Dock = DockStyle.Fill;
    txtDetailArtifactUrl.Font = new Font("Consolas",8.5F);
    txtDetailArtifactUrl.Location = new Point(3,146);
    txtDetailArtifactUrl.Name = "txtDetailArtifactUrl";
    txtDetailArtifactUrl.ReadOnly = true;
    txtDetailArtifactUrl.ScrollBars = RichTextBoxScrollBars.Horizontal;
    txtDetailArtifactUrl.Size = new Size(350,36);
    txtDetailArtifactUrl.TabIndex = 7;
    txtDetailArtifactUrl.Text = "";
    txtDetailArtifactUrl.WordWrap = false;
    txtDetailArtifactUrl.LinkClicked += detailUrl_LinkClicked;
    
    // 
    // lblDetailSha256Url
    // 
    lblDetailSha256Url.AutoSize = true;
    lblDetailSha256Url.Dock = DockStyle.Fill;
    lblDetailSha256Url.Location = new Point(3,182);
    lblDetailSha256Url.Name = "lblDetailSha256Url";
    lblDetailSha256Url.Size = new Size(350,15);
    lblDetailSha256Url.TabIndex = 8;
    lblDetailSha256Url.Text = "SHA256 URL";
    // 
    // txtDetailSha256Url
    // 
    txtDetailSha256Url.BorderStyle = BorderStyle.FixedSingle;
    txtDetailSha256Url.DetectUrls = true;
    txtDetailSha256Url.Dock = DockStyle.Fill;
    txtDetailSha256Url.Font = new Font("Consolas",8.5F);
    txtDetailSha256Url.Location = new Point(3,206);
    txtDetailSha256Url.Name = "txtDetailSha256Url";
    txtDetailSha256Url.ReadOnly = true;
    txtDetailSha256Url.ScrollBars = RichTextBoxScrollBars.Horizontal;
    txtDetailSha256Url.Size = new Size(350,36);
    txtDetailSha256Url.TabIndex = 9;
    txtDetailSha256Url.Text = "";
    txtDetailSha256Url.WordWrap = false;
    txtDetailSha256Url.LinkClicked += detailUrl_LinkClicked;
    
    // 
    // lblDetailUpdateJsonUrl
    // 
    lblDetailUpdateJsonUrl.AutoSize = true;
    lblDetailUpdateJsonUrl.Dock = DockStyle.Fill;
    lblDetailUpdateJsonUrl.Location = new Point(3,239);
    lblDetailUpdateJsonUrl.Name = "lblDetailUpdateJsonUrl";
    lblDetailUpdateJsonUrl.Size = new Size(350,15);
    lblDetailUpdateJsonUrl.TabIndex = 10;
    lblDetailUpdateJsonUrl.Text = "Update JSON URL";
    // 
    // txtDetailUpdateJsonUrl
    // 
    txtDetailUpdateJsonUrl.BorderStyle = BorderStyle.FixedSingle;
    txtDetailUpdateJsonUrl.DetectUrls = true;
    txtDetailUpdateJsonUrl.Dock = DockStyle.Fill;
    txtDetailUpdateJsonUrl.Font = new Font("Consolas",8.5F);
    txtDetailUpdateJsonUrl.Location = new Point(3,266);
    txtDetailUpdateJsonUrl.Name = "txtDetailUpdateJsonUrl";
    txtDetailUpdateJsonUrl.ReadOnly = true;
    txtDetailUpdateJsonUrl.ScrollBars = RichTextBoxScrollBars.Horizontal;
    txtDetailUpdateJsonUrl.Size = new Size(350,36);
    txtDetailUpdateJsonUrl.TabIndex = 11;
    txtDetailUpdateJsonUrl.Text = "";
    txtDetailUpdateJsonUrl.WordWrap = false;
    txtDetailUpdateJsonUrl.LinkClicked += detailUrl_LinkClicked;
    
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
    splitProducts.Panel1.ResumeLayout(false);
    splitProducts.Panel2.ResumeLayout(false);
    ((System.ComponentModel.ISupportInitialize)splitProducts).EndInit();
    splitProducts.ResumeLayout(false);
    ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
    grpProductDetails.ResumeLayout(false);
    tlpProductDetails.ResumeLayout(false);
    tlpProductDetails.PerformLayout();
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
  private Button btnOpenUpdateJsonUrl;
  private Label lblRoot;
  private FlowLayoutPanel flpSummary;
  private Label lblLastCheck;
  private Label lblSummaryOk;
  private Label lblSummaryInfo;
  private Label lblSummaryWarnings;
  private Label lblSummaryErrors;
  private SplitContainer splitProducts;
  private DataGridView dgvProducts;
  private GroupBox grpProductDetails;
  private TableLayoutPanel tlpProductDetails;
  private Label lblDetailProduct;
  private Label lblDetailType;
  private Label lblDetailVersion;
  private Label lblDetailStatus;
  private Label lblDetailDownloadUrl;
  private RichTextBox txtDetailDownloadUrl;
  private Label lblDetailArtifactUrl;
  private RichTextBox txtDetailArtifactUrl;
  private Label lblDetailSha256Url;
  private RichTextBox txtDetailSha256Url;
  private Label lblDetailUpdateJsonUrl;
  private RichTextBox txtDetailUpdateJsonUrl;
  private RichTextBox txtConsole;
}