namespace m05;

// Starta WinForms-applikationen
internal static class Program
{
    [STAThread]
    private static void Main()
    {
        // Initiera applikationsinställningar
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm()); // Starta formuläret
    }
}