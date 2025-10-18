using Argument.Check;

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
        public static MultipartFormDataContent ToMultipartFormDataContent(this InMemoryFileAsByteArray source,
                                                                          string controllerParameterName)
        {
            Throw.IfNull(source);

            var multiPartFormData = new MultipartFormDataContent();
#pragma warning disable CA2000 // Consumer is responsible for disposing the StreamContent
            var byteArrayContent = new ByteArrayContent(source.FileContent);
#pragma warning restore CA2000 // Consumer is responsible for disposing the StreamContent
            multiPartFormData.Add(byteArrayContent, controllerParameterName, source.Name);

            return multiPartFormData;
        }

        /// <summary>
        /// Creates a <see cref="MultipartFormDataContent"/> from an <see cref="InMemoryFileAsStream"/>. 
        /// </summary>
        /// <param name="source">The <see cref="MultipartFormDataContent"/></param>
        /// <param name="controllerParameterName">This has to be the parameter name of your controller method !!!</param>
        /// <returns></returns>
        public static MultipartFormDataContent ToMultipartFormDataContent(this InMemoryFileAsStream source,
                                                                          string controllerParameterName)
        {
            Throw.IfNull(source);

            var multiPartFormData = new MultipartFormDataContent();
#pragma warning disable CA2000 // Consumer is responsible for disposing the StreamContent
            var streamContent = new StreamContent(source.FileStream);
#pragma warning restore CA2000 // Consumer is responsible for disposing the StreamContent
            multiPartFormData.Add(streamContent, controllerParameterName, source.FileName);

            return multiPartFormData;
        }
    }
}
