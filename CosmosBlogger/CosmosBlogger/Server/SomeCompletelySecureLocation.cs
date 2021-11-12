namespace CosmosBlogger.Server
{
    public static class SomeCompletelySecureLocation
    {
        private const string EndPoint = "https://localhost:8081";
        private const string Key =
            "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        
        public static (string EndPoint, string Key) Keys => (EndPoint, Key);
    }
}
