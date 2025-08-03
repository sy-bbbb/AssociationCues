using System.IO;
using System.Threading.Tasks;
#if WINDOWS_UWP
using Windows.Storage;
#else
using System;
#endif

public class FileWriter
{
    private StreamWriter writer;
    private string path;

    // The constructor starts the asynchronous process of creating the file.
    public FileWriter(string folderName, string fileName, string header)
    {
        Task.Run(() => Initialize(folderName, fileName, header));
    }

    // Creates the directory and file, then writes the header row.
    private async Task Initialize(string folderName, string fileName, string header)
    {
#if WINDOWS_UWP
        // For HoloLens, use the PicturesLibrary known folder.
        StorageFolder rootFolder = KnownFolders.PicturesLibrary;
        StorageFolder sessionFolder = await rootFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
        StorageFile file = await sessionFolder.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);
        path = file.Path;
#else
        string rootPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LogFiles");
        string sessionFolderPath = Path.Combine(rootPath, folderName);
        Directory.CreateDirectory(sessionFolderPath); // Creates the directory if it doesn't exist.
        path = Path.Combine(sessionFolderPath, fileName);
#endif

        writer = new StreamWriter(path, append: true); // 'append: false' will overwrite old files with the same name.
        writer.WriteLine(header);
        writer.AutoFlush = true;
    }

    public void WriteLine(string line)
    {
        writer?.WriteLine(line);
    }

    public void Close()
    {
        writer?.Flush();
        writer?.Close();
        writer?.Dispose();
        writer = null;
    }
}