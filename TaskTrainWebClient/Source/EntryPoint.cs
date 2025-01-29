using TT.WebClient.Application;

internal static class EntryPoint
{
    public static void Main(string[] args)
    {
        var taskTrainWebClient = new TaskTrainWebClientApp();
        taskTrainWebClient.Build(args);
        taskTrainWebClient.Run();
    }

}