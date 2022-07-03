using System.Net.Http;

namespace Extensions.Pack.System.Net.Http
{
    public static class InMemoryFileExtensions
    {
        public static MultipartFormDataContent ToMultipartFormDataContent(this InMemoryFileAsByteArray source, string controllerParameterName)
        {
            var multiPartFormData = new MultipartFormDataContent();
            multiPartFormData.Add(new ByteArrayContent(source.FileContent), controllerParameterName, source.Name);
            return multiPartFormData;
        }

        public static MultipartFormDataContent ToMultipartFormDataContent(this InMemoryFileAsStream source, string controllerParameterName)
        {
            var multiPartFormData = new MultipartFormDataContent();
            multiPartFormData.Add(new StreamContent(source.FileStream), controllerParameterName, source.FileName);
            return multiPartFormData;
        }
    }
}
