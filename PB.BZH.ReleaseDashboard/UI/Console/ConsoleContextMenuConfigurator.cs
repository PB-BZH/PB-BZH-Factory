using System.Diagnostics;
using System.Text;

namespace PB.BZH.ReleaseDashboard.UI.Console;

public static class ConsoleContextMenuConfigurator {
  public static void Configure(RichTextBox console) {
    if (console == null)
      throw new ArgumentNullException(nameof(console));

    ContextMenuStrip menu = new();

    ToolStripMenuItem selectAllItem = new("Tout sélectionner");
    ToolStripMenuItem copyItem = new("Copier");
    ToolStripSeparator separator1 = new();
    ToolStripMenuItem editInNotepadItem = new("Éditer dans Notepad++");
    ToolStripSeparator separator2 = new();
    ToolStripMenuItem clearItem = new("Effacer");

    selectAllItem.Click += (_,_) => {
      console.Focus();
      console.SelectAll();
    };

    copyItem.Click += (_,_) => {
      if (!string.IsNullOrEmpty(console.SelectedText)) {
        Clipboard.SetText(console.SelectedText);
        return;
      }

      if (!string.IsNullOrEmpty(console.Text)) {
        Clipboard.SetText(console.Text);
      }
    };

    editInNotepadItem.Click += (_,_) => {
      OpenConsoleContentInNotepadPlusPlus(console.Text);
    };

    clearItem.Click += (_,_) => {
      console.Clear();
    };

    menu.Items.Add(selectAllItem);
    menu.Items.Add(copyItem);
    menu.Items.Add(separator1);
    menu.Items.Add(editInNotepadItem);
    menu.Items.Add(separator2);
    menu.Items.Add(clearItem);

    menu.Opening += (_,_) => {
      bool hasText =
          !string.IsNullOrWhiteSpace(console.Text);

      selectAllItem.Enabled = hasText;
      copyItem.Enabled = hasText;
      editInNotepadItem.Enabled = hasText;
      clearItem.Enabled = hasText;
    };

    console.ContextMenuStrip = menu;
  }

  private static void OpenConsoleContentInNotepadPlusPlus(string content) {
    if (string.IsNullOrWhiteSpace(content))
      return;

    string tempDirectory =
        Path.Combine(
            Path.GetTempPath(),
            "PB-BZH-Factory",
            "ReleaseDashboard");

    Directory.CreateDirectory(tempDirectory);

    string logFilePath =
        Path.Combine(
            tempDirectory,
            "release-dashboard-console_" +
            DateTime.Now.ToString("yyyyMMdd_HHmmss") +
            ".log");

    File.WriteAllText(
        logFilePath,
        content,
        new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));

    string editorPath =
        ResolveNotepadPlusPlusPath();

    Process.Start(new ProcessStartInfo {
      FileName = editorPath,
      Arguments = "\"" + logFilePath + "\"",
      UseShellExecute = false
    });
  }

  private static string ResolveNotepadPlusPlusPath() {
    string programFiles =
        Environment.GetFolderPath(
            Environment.SpecialFolder.ProgramFiles);

    string programFilesX86 =
        Environment.GetFolderPath(
            Environment.SpecialFolder.ProgramFilesX86);

    string[] candidates = [
        Path.Combine(programFiles,"Notepad++","notepad++.exe"),
        Path.Combine(programFilesX86,"Notepad++","notepad++.exe")
    ];

    foreach (string candidate in candidates) {
      if (File.Exists(candidate))
        return candidate;
    }

    string? pathEnvironment =
        Environment.GetEnvironmentVariable("PATH");

    if (!string.IsNullOrWhiteSpace(pathEnvironment)) {
      foreach (string directory in pathEnvironment.Split(Path.PathSeparator)) {
        if (string.IsNullOrWhiteSpace(directory))
          continue;

        string candidate =
            Path.Combine(directory.Trim(),"notepad++.exe");

        if (File.Exists(candidate))
          return candidate;
      }
    }

    return "notepad.exe";
  }
}