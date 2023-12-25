namespace Extensions.Pack
{
    public static class InMemoryFileExtensions
    {
        /// <summary>
        /// Creates a <see cref="MultipartFormDataContent"/> from an <see cref="InMemoryFileAsByteArray"/>. 
        /// </summary>
        /// <param name="source">The <see cref="MultipartFormDataContent"/></param>
        /// <param name="controllerParameterName">This has to be the parameter name of your controller method !!!</param>
        /// <returns></returns>
        public static MultipartFormDataContent ToMultipartFormDataContent(this InMemoryFileAsByteArray source, string controllerParameterName)
        {
            var multiPartFormData = new MultipartFormDataContent();
            multiPartFormData.Add(new ByteArrayContent(source.FileContent), controllerParameterName, source.Name);
            return multiPartFormData;
        }

        /// <summary>
        /// Creates a <see cref="MultipartFormDataContent"/> from an <see cref="InMemoryFileAsStream"/>. 
        /// </summary>
        /// <param name="source">The <see cref="MultipartFormDataContent"/></param>
        /// <param name="controllerParameterName">This has to be the parameter name of your controller method !!!</param>
        /// <returns></returns>
        public static MultipartFormDataContent ToMultipartFormDataContent(this InMemoryFileAsStream source, string controllerParameterName)
        {
            var multiPartFormData = new MultipartFormDataContent();
            multiPartFormData.Add(new StreamContent(source.FileStream), controllerParameterName, source.FileName);
            return multiPartFormData;
        }
    }
}
