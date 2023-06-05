/*
    DiskPartWrapper.cs
This is a stupid file that its purpouse is to call diskpart, and
to make my ass easier to organize my trashy code :) I think it works
 */

using System;
using System.IO;
using System.Diagnostics;

namespace DiskPartWrapper
{

    // it contains the required functionallity because im lazy
    public class DiskPart
    {
        public string scriptFilePath = string.Empty;


        // maybe lots of changes might be added, and why do it once and not waste time in txt creation/deletion
        public void ApplyChanges(string scriptFileLocation)
        {

            StreamReader myReader = File.OpenText(scriptFilePath);
            myReader.Close();


            ProcessStartInfo diskPartExecutable = new ProcessStartInfo();
            diskPartExecutable.CreateNoWindow = false;
            diskPartExecutable.UseShellExecute = false;
            diskPartExecutable.FileName = @"C:\Windows\System32\diskpart.exe";
            diskPartExecutable.WindowStyle = ProcessWindowStyle.Hidden;
            diskPartExecutable.Arguments = " /s " + scriptFileLocation;
        }



        /// <summary>
        /// Shrinks a partition, but it doesn't go kids to school. 
        /// </summary>
        /// <param name="diskNumber">Disk number (Disk 0/1/2/3...)</param>
        /// <param name="partitionNumber">Partition number (par 1/2/3...)</param>
        /// <param name="size">Size (in MB) to be shrinked</param>
        public bool ShrinkPartition(int diskNumber, int partitionNumber, int size)
        {

            if (diskNumber == null) { return false; } // fuck
            if (partitionNumber == null) { return false; } // fuck
            if (size == null) { return false; } //fuck
            try
            {
                string scriptFilePath = System.IO.Path.GetTempPath() + @"\dpScript.txt";
                FileInfo scriptAttribs = new FileInfo(scriptFilePath);
                scriptAttribs.Attributes = FileAttributes.Temporary;
                if (File.Exists(scriptFilePath)) { File.Delete(scriptFilePath); }


                File.AppendAllText(scriptFilePath,
                string.Format(
                    "SELECT DISK={0}\n" +
                "SELECT PARTITION={1}\n" +
                "SHRINK DESIRED={1}\n" +
                "CREATE PARTITION EFI\n" +
                "ASSIGN LETTER=X:\n" +
                "FORMAT FS=NTFS QUICK\n" +
                "EXIT", diskNumber, partitionNumber, (size * 1000) + 500));
                ApplyChanges("DiskPart.exe" + " /s " + scriptFilePath);
                File.Delete(scriptFilePath); // Delete the script file    
            }
            catch (Exception ex)
            {
                return false;
                throw ex;

            }
            return true;
        }


    }
}
