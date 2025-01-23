using TT.WebClient.Application;

internal static class EntryPoint 
{
    public static void Main(string[] args) 
    {
        var app = new TaskTrainWebClientApp();
        app.Build(args);
        app.Run();
    }

}

