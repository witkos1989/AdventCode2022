namespace AdventCode2022.Helpers;

public static class Helpers
{
    public static string GetCurrentDirectory(string directoryName, string fileName)
    {
        string path = Path.GetDirectoryName(
            Path.GetDirectoryName(
                Path.GetDirectoryName(
                    Directory.GetCurrentDirectory())))!;
        string archiveFolder = Path.Combine(path, directoryName);
        string file = archiveFolder + "/" + fileName;

        return file;
    }
}