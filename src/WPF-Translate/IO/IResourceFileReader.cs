namespace de.LandauSoftware.WPFTranslate.IO
{
    public interface IResourceFileReader
    {
        string FileExtension { get; }

        string GetLanguageKey(ResourceDictionaryFile file);

        IResourceFileWriter GetWriter();

        ResourceDictionaryFile Read(string file);
    }
}