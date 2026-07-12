namespace PB.BZH.ReleaseDashboard.UI.Console;

public static class ConsoleLogHighlighter {
  public static void AppendLine(
      RichTextBox console,
      string message) {
    if (console == null)
      throw new ArgumentNullException(nameof(console));

    message ??= string.Empty;

    Color color =
        GetLineColor(message);

    FontStyle style =
        GetLineStyle(message);

    int start =
        console.TextLength;

    console.SelectionStart = start;
    console.SelectionLength = 0;
    console.SelectionColor = color;
    console.SelectionFont = new Font(console.Font,style);

    console.AppendText(message + Environment.NewLine);

    console.SelectionColor = console.ForeColor;
    console.SelectionFont = console.Font;

    console.SelectionStart = console.TextLength;
    console.ScrollToCaret();
  }

  public static void Clear(RichTextBox console) {
    if (console == null)
      throw new ArgumentNullException(nameof(console));

    console.Clear();
  }

  private static Color GetLineColor(string message) {
    string value =
        message.Trim();

    if (value.StartsWith("[OK]",StringComparison.OrdinalIgnoreCase) ||
        value.Contains("OK       :",StringComparison.OrdinalIgnoreCase) ||
        value.Contains("Exit code : 0",StringComparison.OrdinalIgnoreCase)) {
      return Color.FromArgb(120,220,120);
    }

    if (value.StartsWith("[INFO]",StringComparison.OrdinalIgnoreCase) ||
        value.Contains("Infos    :",StringComparison.OrdinalIgnoreCase)) {
      return Color.FromArgb(120,200,255);
    }

    if (value.StartsWith("[WARN]",StringComparison.OrdinalIgnoreCase) ||
        value.Contains("Warnings :",StringComparison.OrdinalIgnoreCase)) {
      return Color.FromArgb(255,210,90);
    }

    if (value.StartsWith("[FAIL]",StringComparison.OrdinalIgnoreCase) ||
        value.StartsWith("[ERROR]",StringComparison.OrdinalIgnoreCase) ||
        value.Contains("Errors   :",StringComparison.OrdinalIgnoreCase) ||
        value.Contains("Exit code : 1",StringComparison.OrdinalIgnoreCase)) {
      return Color.FromArgb(255,110,110);
    }

    if (value.StartsWith("http",StringComparison.OrdinalIgnoreCase) ||
        value.Contains("https://",StringComparison.OrdinalIgnoreCase)) {
      return Color.FromArgb(150,200,255);
    }

    if (value.StartsWith("===") ||
        value.Equals("Summary",StringComparison.OrdinalIgnoreCase)) {
      return Color.FromArgb(230,230,230);
    }

    return Color.Gainsboro;
  }

  private static FontStyle GetLineStyle(string message) {
    string value =
        message.Trim();

    if (value.StartsWith("===") ||
        value.Equals("Summary",StringComparison.OrdinalIgnoreCase)) {
      return FontStyle.Bold;
    }

    if (value.StartsWith("[OK]",StringComparison.OrdinalIgnoreCase) ||
        value.StartsWith("[INFO]",StringComparison.OrdinalIgnoreCase) ||
        value.StartsWith("[WARN]",StringComparison.OrdinalIgnoreCase) ||
        value.StartsWith("[FAIL]",StringComparison.OrdinalIgnoreCase) ||
        value.StartsWith("[ERROR]",StringComparison.OrdinalIgnoreCase)) {
      return FontStyle.Bold;
    }

    return FontStyle.Regular;
  }
}