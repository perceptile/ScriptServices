namespace ScriptServices.hosting
{
    internal class WindowsService
    {
        public void Start()
        {
            AnonymousServiceHost.Create();
        }

        public void Stop()
        {
        }
    }
}
