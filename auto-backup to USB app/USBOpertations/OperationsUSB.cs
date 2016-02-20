using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace auto_backup_to_USB_app.USBOpertations
{
    class OperationsUSB
    {
        static DriveInfo[] drives;
        static List<String> foldersToBackUp;

        static OperationsUSB()
        {
            drives = DriveInfo.GetDrives();
            foldersToBackUp = new List<String>();
        }

        static private string getFullPathToUSB()
        {
            string fullPath = string.Empty;
            foreach (DriveInfo d in drives)
            {
                if (d.IsReady && d.DriveType == DriveType.Removable)
                    fullPath = d.RootDirectory.FullName;
            }
            return fullPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toUSB">ture to backup, false to restore</param>
        static private void backUpData(bool toUSB)
        {
            string fileName, sourcePath = "", targetPath = "", destFile;

            if (toUSB)
            {
                targetPath = getFullPathToUSB();
            }
            else
            {
                sourcePath = getFullPathToUSB();
            }

            foreach (String path in foldersToBackUp)
            {
                if (toUSB)
                {
                    sourcePath = path;
                }
                else
                {
                    targetPath = path;
                }
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                string[] files = Directory.GetFiles(sourcePath);
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    fileName = Path.GetFileName(s);
                    destFile =Path.Combine(targetPath, fileName);
                    File.Copy(s, destFile, true);
                }
            }

        }

        static public void backupToUSB()
        {
            backUpData(true);
        }

        static public void storeFromUSB()
        {
            backUpData(false);
        }

        static public bool addFolderToList(String path)
        {
            bool existFolder = Directory.Exists(path);
            if (existFolder) foldersToBackUp.Add(path);
            return existFolder;
        }

        //TODO: learn how to start ur app in OS start up, and end it with it :D
        //TODO: save list to txt file, and load from it in start up
    }
}
