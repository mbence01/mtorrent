namespace Torrent.Client.Model.Exception
{
    /// <summary>
    /// Exception class for situations when a conversion has started from a dictionary that contains object properties into an equivalent entity.
    /// </summary>
    public class DictionaryToModelConversionFailedException : System.Exception
    {
        /// <summary>
        /// Initializes an exception class for situations when a conversion has started from a dictionary that contains object properties into an equivalent entity.
        /// </summary>
        public DictionaryToModelConversionFailedException(object obj, System.Exception innerException)
            : base($"Failed to convert property collection to type '{obj.GetType()}'. See the inner exception for more information", innerException) { }
    }
}
