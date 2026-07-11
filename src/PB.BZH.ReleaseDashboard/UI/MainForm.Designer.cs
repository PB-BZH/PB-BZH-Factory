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
    lblRoot = new Label();
    flpToolbar = new FlowLayoutPanel();
    btnRunReleaseCheck = new Button();
    btnOpenLastReport = new Button();
    btnReloadCatalog = new Button();
    flpSummary = new FlowLayoutPanel();
    lblLastCheck = new Label();
    lblSummaryOk = new Label();
    lblSummaryInfo = new Label();
    lblSummaryWarnings = new Label();
    lblSummaryErrors = new Label();
    splitProducts = new SplitContainer();
    dgvProducts = new DataGridView();
    grpProductDetails = new GroupBox();
    tlpProductDetails = new TableLayoutPanel();
    lblDetailProduct = new Label();
    lblDetailType = new Label();
    lblDetailVersion = new Label();
    lblDetailStatus = new Label();
    lblDetailDownloadUrl = new Label();
    txtDetailDownloadUrl = new RichTextBox();
    lblDetailArtifactUrl = new Label();
    txtDetailArtifactUrl = new RichTextBox();
    lblDetailSha256Url = new Label();
    txtDetailSha256Url = new RichTextBox();
    lblDetailUpdateJsonUrl = new Label();
    txtDetailUpdateJsonUrl = new RichTextBox();
    txtConsole = new RichTextBox();
    menuStrip1 = new MenuStrip();
    mnuRelease = new ToolStripMenuItem();
    mnuRunReleaseCheck = new ToolStripMenuItem();
    mnuOpenLastReport = new ToolStripMenuItem();
    mnuOpenReportFolder = new ToolStripMenuItem();
    mnuProduct = new ToolStripMenuItem();
    mnuOpenDownloadURL = new ToolStripMenuItem();
    mnuOpenArtifactURL = new ToolStripMenuItem();
    mnuViewUpdateJson = new ToolStripMenuItem();
    mnuViewSHA256 = new ToolStripMenuItem();
    mnuVerifySHA256 = new ToolStripMenuItem();
    mnuViewProductChecks = new ToolStripMenuItem();
    mnuCatalog = new ToolStripMenuItem();
    mnuViewProductjson = new ToolStripMenuItem();
    mnuEditProductsjson = new ToolStripMenuItem();
    mnuReloadCatalog = new ToolStripMenuItem();
    mnuRebuildProductsjson = new ToolStripMenuItem();
    mnuApplyRebuiltProductsjson = new ToolStripMenuItem();
    mnuHelp = new ToolStripMenuItem();
    mnuAbout = new ToolStripMenuItem();
    mnuCheckForUpdate = new ToolStripMenuItem();
    tlpMain.SuspendLayout();
    pnlHeader.SuspendLayout();
    flpToolbar.SuspendLayout();
    flpSummary.SuspendLayout();
    ((System.ComponentModel.ISupportInitialize)splitProducts).BeginInit();
    splitProducts.Panel1.SuspendLayout();
    splitProducts.Panel2.SuspendLayout();
    splitProducts.SuspendLayout();
    ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
    grpProductDetails.SuspendLayout();
    tlpProductDetails.SuspendLayout();
    menuStrip1.SuspendLayout();
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
    tlpMain.Location = new Point(0,24);
    tlpMain.Name = "tlpMain";
    tlpMain.Padding = new Padding(12);
    tlpMain.RowCount = 4;
    tlpMain.RowStyles.Add(new RowStyle());
    tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute,125F));
    tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent,55F));
    tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent,45F));
    tlpMain.Size = new Size(1213,736);
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
    lblTitle.Size = new Size(1189,30);
    lblTitle.TabIndex = 0;
    lblTitle.Text = "PB BZH Release Dashboard";
    // 
    // pnlHeader
    // 
    pnlHeader.Controls.Add(lblRoot);
    pnlHeader.Controls.Add(flpToolbar);
    pnlHeader.Controls.Add(flpSummary);
    pnlHeader.Dock = DockStyle.Fill;
    pnlHeader.Location = new Point(12,50);
    pnlHeader.Margin = new Padding(0);
    pnlHeader.Name = "pnlHeader";
    pnlHeader.Size = new Size(1189,125);
    pnlHeader.TabIndex = 1;
    // 
    // lblRoot
    // 
    lblRoot.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    lblRoot.AutoSize = true;
    lblRoot.Location = new Point(12,60);
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
    flpToolbar.Controls.Add(btnReloadCatalog);
    flpToolbar.Dock = DockStyle.Top;
    flpToolbar.Location = new Point(0,0);
    flpToolbar.Name = "flpToolbar";
    flpToolbar.Size = new Size(1189,46);
    flpToolbar.TabIndex = 0;
    // 
    // btnRunReleaseCheck
    // 
    btnRunReleaseCheck.AutoSize = true;
    btnRunReleaseCheck.Location = new Point(0,0);
    btnRunReleaseCheck.Margin = new Padding(0,0,8,8);
    btnRunReleaseCheck.Name = "btnRunReleaseCheck";
    btnRunReleaseCheck.Size = new Size(111,34);
    btnRunReleaseCheck.TabIndex = 0;
    btnRunReleaseCheck.Text = "Run release check";
    btnRunReleaseCheck.UseVisualStyleBackColor = true;
    btnRunReleaseCheck.Click += btnRunReleaseCheck_Click;
    // 
    // btnOpenLastReport
    // 
    btnOpenLastReport.AutoSize = true;
    btnOpenLastReport.Location = new Point(119,0);
    btnOpenLastReport.Margin = new Padding(0,0,8,8);
    btnOpenLastReport.Name = "btnOpenLastReport";
    btnOpenLastReport.Size = new Size(102,34);
    btnOpenLastReport.TabIndex = 1;
    btnOpenLastReport.Text = "Open last report";
    btnOpenLastReport.UseVisualStyleBackColor = true;
    btnOpenLastReport.Click += btnOpenLastReport_Click;
    // 
    // btnReloadCatalog
    // 
    btnReloadCatalog.AutoSize = true;
    btnReloadCatalog.Location = new Point(229,0);
    btnReloadCatalog.Margin = new Padding(0,0,8,8);
    btnReloadCatalog.Name = "btnReloadCatalog";
    btnReloadCatalog.Size = new Size(95,34);
    btnReloadCatalog.TabIndex = 5;
    btnReloadCatalog.Text = "Reload catalog";
    btnReloadCatalog.UseVisualStyleBackColor = true;
    btnReloadCatalog.Click += btnReloadCatalog_Click;
    // 
    // flpSummary
    // 
    flpSummary.AutoSize = true;
    flpSummary.Controls.Add(lblLastCheck);
    flpSummary.Controls.Add(lblSummaryOk);
    flpSummary.Controls.Add(lblSummaryInfo);
    flpSummary.Controls.Add(lblSummaryWarnings);
    flpSummary.Controls.Add(lblSummaryErrors);
    flpSummary.Location = new Point(9,87);
    flpSummary.Name = "flpSummary";
    flpSummary.Size = new Size(396,15);
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
    // splitProducts
    // 
    splitProducts.Dock = DockStyle.Fill;
    splitProducts.Location = new Point(12,175);
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
    splitProducts.Size = new Size(1189,293);
    splitProducts.SplitterDistance = 797;
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
    dgvProducts.Size = new Size(797,293);
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
    grpProductDetails.Size = new Size(388,293);
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
    tlpProductDetails.Size = new Size(372,261);
    tlpProductDetails.TabIndex = 0;
    // 
    // lblDetailProduct
    // 
    lblDetailProduct.AutoSize = true;
    lblDetailProduct.Dock = DockStyle.Fill;
    lblDetailProduct.Font = new Font("Segoe UI",9F,FontStyle.Bold);
    lblDetailProduct.Location = new Point(3,0);
    lblDetailProduct.Name = "lblDetailProduct";
    lblDetailProduct.Size = new Size(366,15);
    lblDetailProduct.TabIndex = 0;
    lblDetailProduct.Text = "Product : -";
    // 
    // lblDetailType
    // 
    lblDetailType.AutoSize = true;
    lblDetailType.Dock = DockStyle.Fill;
    lblDetailType.Location = new Point(3,15);
    lblDetailType.Name = "lblDetailType";
    lblDetailType.Size = new Size(366,15);
    lblDetailType.TabIndex = 1;
    lblDetailType.Text = "Type : -";
    // 
    // lblDetailVersion
    // 
    lblDetailVersion.AutoSize = true;
    lblDetailVersion.Dock = DockStyle.Fill;
    lblDetailVersion.Location = new Point(3,30);
    lblDetailVersion.Name = "lblDetailVersion";
    lblDetailVersion.Size = new Size(366,15);
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
    lblDetailStatus.Size = new Size(366,15);
    lblDetailStatus.TabIndex = 3;
    lblDetailStatus.Text = "Status : -";
    // 
    // lblDetailDownloadUrl
    // 
    lblDetailDownloadUrl.AutoSize = true;
    lblDetailDownloadUrl.Dock = DockStyle.Fill;
    lblDetailDownloadUrl.Location = new Point(3,68);
    lblDetailDownloadUrl.Name = "lblDetailDownloadUrl";
    lblDetailDownloadUrl.Size = new Size(366,15);
    lblDetailDownloadUrl.TabIndex = 4;
    lblDetailDownloadUrl.Text = "Download URL";
    // 
    // txtDetailDownloadUrl
    // 
    txtDetailDownloadUrl.BorderStyle = BorderStyle.FixedSingle;
    txtDetailDownloadUrl.Dock = DockStyle.Fill;
    txtDetailDownloadUrl.Font = new Font("Consolas",8.5F);
    txtDetailDownloadUrl.Location = new Point(3,86);
    txtDetailDownloadUrl.Name = "txtDetailDownloadUrl";
    txtDetailDownloadUrl.ReadOnly = true;
    txtDetailDownloadUrl.ScrollBars = RichTextBoxScrollBars.Horizontal;
    txtDetailDownloadUrl.Size = new Size(366,36);
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
    lblDetailArtifactUrl.Size = new Size(366,15);
    lblDetailArtifactUrl.TabIndex = 6;
    lblDetailArtifactUrl.Text = "Artifact URL";
    // 
    // txtDetailArtifactUrl
    // 
    txtDetailArtifactUrl.BorderStyle = BorderStyle.FixedSingle;
    txtDetailArtifactUrl.Dock = DockStyle.Fill;
    txtDetailArtifactUrl.Font = new Font("Consolas",8.5F);
    txtDetailArtifactUrl.Location = new Point(3,143);
    txtDetailArtifactUrl.Name = "txtDetailArtifactUrl";
    txtDetailArtifactUrl.ReadOnly = true;
    txtDetailArtifactUrl.ScrollBars = RichTextBoxScrollBars.Horizontal;
    txtDetailArtifactUrl.Size = new Size(366,36);
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
    lblDetailSha256Url.Size = new Size(366,15);
    lblDetailSha256Url.TabIndex = 8;
    lblDetailSha256Url.Text = "SHA256 URL";
    // 
    // txtDetailSha256Url
    // 
    txtDetailSha256Url.BorderStyle = BorderStyle.FixedSingle;
    txtDetailSha256Url.Dock = DockStyle.Fill;
    txtDetailSha256Url.Font = new Font("Consolas",8.5F);
    txtDetailSha256Url.Location = new Point(3,200);
    txtDetailSha256Url.Name = "txtDetailSha256Url";
    txtDetailSha256Url.ReadOnly = true;
    txtDetailSha256Url.ScrollBars = RichTextBoxScrollBars.Horizontal;
    txtDetailSha256Url.Size = new Size(366,36);
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
    lblDetailUpdateJsonUrl.Size = new Size(366,15);
    lblDetailUpdateJsonUrl.TabIndex = 10;
    lblDetailUpdateJsonUrl.Text = "Update JSON URL";
    // 
    // txtDetailUpdateJsonUrl
    // 
    txtDetailUpdateJsonUrl.BorderStyle = BorderStyle.FixedSingle;
    txtDetailUpdateJsonUrl.Dock = DockStyle.Fill;
    txtDetailUpdateJsonUrl.Font = new Font("Consolas",8.5F);
    txtDetailUpdateJsonUrl.Location = new Point(3,257);
    txtDetailUpdateJsonUrl.Name = "txtDetailUpdateJsonUrl";
    txtDetailUpdateJsonUrl.ReadOnly = true;
    txtDetailUpdateJsonUrl.ScrollBars = RichTextBoxScrollBars.Horizontal;
    txtDetailUpdateJsonUrl.Size = new Size(366,36);
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
    txtConsole.Location = new Point(12,476);
    txtConsole.Margin = new Padding(0);
    txtConsole.Name = "txtConsole";
    txtConsole.ReadOnly = true;
    txtConsole.Size = new Size(1189,248);
    txtConsole.TabIndex = 3;
    txtConsole.Text = "";
    txtConsole.WordWrap = false;
    txtConsole.LinkClicked += txtConsole_LinkClicked;
    // 
    // menuStrip1
    // 
    menuStrip1.Items.AddRange(new ToolStripItem[] { mnuRelease,mnuProduct,mnuCatalog,mnuHelp });
    menuStrip1.Location = new Point(0,0);
    menuStrip1.Name = "menuStrip1";
    menuStrip1.Size = new Size(1213,24);
    menuStrip1.TabIndex = 1;
    menuStrip1.Text = "menuStrip1";
    // 
    // mnuRelease
    // 
    mnuRelease.DropDownItems.AddRange(new ToolStripItem[] { mnuRunReleaseCheck,mnuOpenLastReport,mnuOpenReportFolder });
    mnuRelease.Name = "mnuRelease";
    mnuRelease.Size = new Size(58,20);
    mnuRelease.Text = "Release";
    // 
    // mnuRunReleaseCheck
    // 
    mnuRunReleaseCheck.Name = "mnuRunReleaseCheck";
    mnuRunReleaseCheck.Size = new Size(180,22);
    mnuRunReleaseCheck.Text = "Run release check";
    mnuRunReleaseCheck.Click += mnuRunReleaseCheck_Click;
    // 
    // mnuOpenLastReport
    // 
    mnuOpenLastReport.Name = "mnuOpenLastReport";
    mnuOpenLastReport.Size = new Size(180,22);
    mnuOpenLastReport.Text = "Open last report";
    mnuOpenLastReport.Click += mnuOpenLastReport_Click;
    // 
    // mnuOpenReportFolder
    // 
    mnuOpenReportFolder.Name = "mnuOpenReportFolder";
    mnuOpenReportFolder.Size = new Size(180,22);
    mnuOpenReportFolder.Text = "Open report folder";
    mnuOpenReportFolder.Click += mnuOpenReportsFolder_Click;
    // 
    // mnuProduct
    // 
    mnuProduct.DropDownItems.AddRange(new ToolStripItem[] { mnuOpenDownloadURL,mnuOpenArtifactURL,mnuViewUpdateJson,mnuViewSHA256,mnuVerifySHA256,mnuViewProductChecks });
    mnuProduct.Name = "mnuProduct";
    mnuProduct.Size = new Size(61,20);
    mnuProduct.Text = "Product";
    // 
    // mnuOpenDownloadURL
    // 
    mnuOpenDownloadURL.Name = "mnuOpenDownloadURL";
    mnuOpenDownloadURL.Size = new Size(183,22);
    mnuOpenDownloadURL.Text = "Open download URL";
    mnuOpenDownloadURL.Click += mnuOpenDownloadUrl_Click;
    // 
    // mnuOpenArtifactURL
    // 
    mnuOpenArtifactURL.Name = "mnuOpenArtifactURL";
    mnuOpenArtifactURL.Size = new Size(183,22);
    mnuOpenArtifactURL.Text = "Open artifact URL";
    mnuOpenArtifactURL.Click += mnuOpenArtifactUrl_Click;
    // 
    // mnuViewUpdateJson
    // 
    mnuViewUpdateJson.Name = "mnuViewUpdateJson";
    mnuViewUpdateJson.Size = new Size(183,22);
    mnuViewUpdateJson.Text = "View update.json";
    mnuViewUpdateJson.Click += mnuViewUpdateJson_Click;
    // 
    // mnuViewSHA256
    // 
    mnuViewSHA256.Name = "mnuViewSHA256";
    mnuViewSHA256.Size = new Size(183,22);
    mnuViewSHA256.Text = "View SHA256";
    mnuViewSHA256.Click += mnuViewSha256_Click;
    // 
    // mnuVerifySHA256
    // 
    mnuVerifySHA256.Name = "mnuVerifySHA256";
    mnuVerifySHA256.Size = new Size(183,22);
    mnuVerifySHA256.Text = "Verify SHA256";
    mnuVerifySHA256.Click += mnuVerifySha256_Click;
    // 
    // mnuViewProductChecks
    // 
    mnuViewProductChecks.Name = "mnuViewProductChecks";
    mnuViewProductChecks.Size = new Size(183,22);
    mnuViewProductChecks.Text = "View product checks";
    mnuViewProductChecks.Click += mnuViewProductChecks_Click;
    // 
    // mnuCatalog
    // 
    mnuCatalog.DropDownItems.AddRange(new ToolStripItem[] { mnuViewProductjson,mnuEditProductsjson,mnuReloadCatalog,mnuRebuildProductsjson,mnuApplyRebuiltProductsjson });
    mnuCatalog.Name = "mnuCatalog";
    mnuCatalog.Size = new Size(60,20);
    mnuCatalog.Text = "Catalog";
    // 
    // mnuViewProductjson
    // 
    mnuViewProductjson.Name = "mnuViewProductjson";
    mnuViewProductjson.Size = new Size(217,22);
    mnuViewProductjson.Text = "View products.json";
    mnuViewProductjson.Click += mnuViewUpdateJson_Click;
    // 
    // mnuEditProductsjson
    // 
    mnuEditProductsjson.Name = "mnuEditProductsjson";
    mnuEditProductsjson.Size = new Size(217,22);
    mnuEditProductsjson.Text = "Edit products.json";
    mnuEditProductsjson.Click += mnuEditProductsJson_Click;
    // 
    // mnuReloadCatalog
    // 
    mnuReloadCatalog.Name = "mnuReloadCatalog";
    mnuReloadCatalog.Size = new Size(217,22);
    mnuReloadCatalog.Text = "Reload catalog";
    mnuReloadCatalog.Click += mnuReloadCatalog_Click;
    // 
    // mnuRebuildProductsjson
    // 
    mnuRebuildProductsjson.Name = "mnuRebuildProductsjson";
    mnuRebuildProductsjson.Size = new Size(217,22);
    mnuRebuildProductsjson.Text = "Rebuild products.json";
    mnuRebuildProductsjson.Click += mnuRebuildProductsJson_Click;
    // 
    // mnuApplyRebuiltProductsjson
    // 
    mnuApplyRebuiltProductsjson.Name = "mnuApplyRebuiltProductsjson";
    mnuApplyRebuiltProductsjson.Size = new Size(217,22);
    mnuApplyRebuiltProductsjson.Text = "Apply rebuilt products.json";
    mnuApplyRebuiltProductsjson.Click += mnuApplyRebuiltProductsJson_Click;
    // 
    // mnuHelp
    // 
    mnuHelp.DropDownItems.AddRange(new ToolStripItem[] { mnuAbout,mnuCheckForUpdate });
    mnuHelp.Name = "mnuHelp";
    mnuHelp.Size = new Size(44,20);
    mnuHelp.Text = "Help";
    // 
    // mnuAbout
    // 
    mnuAbout.Name = "mnuAbout";
    mnuAbout.Size = new Size(174,22);
    mnuAbout.Text = "About";
    // 
    // mnuCheckForUpdate
    // 
    mnuCheckForUpdate.Name = "mnuCheckForUpdate";
    mnuCheckForUpdate.Size = new Size(174,22);
    mnuCheckForUpdate.Text = "Check for update...";
    // 
    // MainForm
    // 
    AutoScaleDimensions = new SizeF(7F,15F);
    AutoScaleMode = AutoScaleMode.Font;
    ClientSize = new Size(1213,760);
    Controls.Add(tlpMain);
    Controls.Add(menuStrip1);
    MainMenuStrip = menuStrip1;
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
    flpSummary.ResumeLayout(false);
    flpSummary.PerformLayout();
    splitProducts.Panel1.ResumeLayout(false);
    splitProducts.Panel2.ResumeLayout(false);
    ((System.ComponentModel.ISupportInitialize)splitProducts).EndInit();
    splitProducts.ResumeLayout(false);
    ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
    grpProductDetails.ResumeLayout(false);
    tlpProductDetails.ResumeLayout(false);
    tlpProductDetails.PerformLayout();
    menuStrip1.ResumeLayout(false);
    menuStrip1.PerformLayout();
    ResumeLayout(false);
    PerformLayout();
  }

  #endregion

  private TableLayoutPanel tlpMain;
  private Label lblTitle;
  private Panel pnlHeader;
  private FlowLayoutPanel flpToolbar;
  private Button btnRunReleaseCheck;
  private Button btnOpenLastReport;
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
  private Button btnReloadCatalog;
  private MenuStrip menuStrip1;
  private ToolStripMenuItem mnuRelease;
  private ToolStripMenuItem mnuRunReleaseCheck;
  private ToolStripMenuItem mnuOpenLastReport;
  private ToolStripMenuItem mnuOpenReportFolder;
  private ToolStripMenuItem mnuProduct;
  private ToolStripMenuItem mnuOpenDownloadURL;
  private ToolStripMenuItem mnuOpenArtifactURL;
  private ToolStripMenuItem mnuViewUpdateJson;
  private ToolStripMenuItem mnuViewSHA256;
  private ToolStripMenuItem mnuVerifySHA256;
  private ToolStripMenuItem mnuViewProductChecks;
  private ToolStripMenuItem mnuCatalog;
  private ToolStripMenuItem mnuViewProductjson;
  private ToolStripMenuItem mnuEditProductsjson;
  private ToolStripMenuItem mnuReloadCatalog;
  private ToolStripMenuItem mnuRebuildProductsjson;
  private ToolStripMenuItem mnuApplyRebuiltProductsjson;
  private ToolStripMenuItem mnuHelp;
  private ToolStripMenuItem mnuAbout;
  private ToolStripMenuItem mnuCheckForUpdate;
}