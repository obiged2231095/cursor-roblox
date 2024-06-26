using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

class CursorUpdater
{
    const string Version = "1.1";
    const string VersionUrl = "https://raw.githubusercontent.com/eqate/cursor-roblox/main/version.txt";
    const string CursorFileName = "cursor.png";
    const string BloxstrapPathTemplate = @"C:\Users\{0}\AppData\Local\Bloxstrap\Versions\version-1088f3c8e4a44cc7\content\textures\Cursors\KeyboardMouse";
    const string BloxstrapTexturePathTemplate = @"C:\Users\{0}\AppData\Local\Bloxstrap\Versions\version-1088f3c8e4a44cc7\content\textures";

    static async Task Main(string[] args)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string latestVersion = await client.GetStringAsync(VersionUrl);
                if (Version != latestVersion.Trim())
                {
                    Console.WriteLine("Version Mismatch. [ UPDATE ON GITHUB ]");
                    System.Threading.Thread.Sleep(100000000);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"err: {ex.Message}");
                return;
            }
        }

        string userName = Environment.UserName;
        string bloxstrapPath = string.Format(BloxstrapPathTemplate, userName);
        string cursorPath = Path.Combine(AppContext.BaseDirectory, CursorFileName);
        string bloxstrapTexturePath = string.Format(BloxstrapTexturePathTemplate, userName);

        try
        {
            if (Directory.Exists(bloxstrapPath))
            {
                DirectoryInfo di = new DirectoryInfo(bloxstrapPath);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                Console.WriteLine("cleared kbm directory");
            }
            string[] cursorNames = { "ArrowCursor.png", "ArrowFarCursor.png", "IBeamCursor.png" };
            foreach (string cursorName in cursorNames)
            {
                string destPath = Path.Combine(bloxstrapPath, cursorName);
                File.Copy(cursorPath, destPath, true);
                Console.WriteLine($"copied - {cursorName}.");
            }

            // replace shiftlock
            string mouseLockedCursorPath = Path.Combine(bloxstrapTexturePath, "MouseLockedCursor.png");
            File.Copy(cursorPath, mouseLockedCursorPath, true);
            Console.WriteLine("replaced shiftlock with cursor");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
