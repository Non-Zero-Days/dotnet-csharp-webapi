namespace dotnet_csharp_webapi
{
    public interface IMessageRepository
    {
        string GetMessage();
        string SetMessage(string input);
    }
}