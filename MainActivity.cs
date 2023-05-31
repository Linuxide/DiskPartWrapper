using System;
using System.IO;
namespace DiskPartWrapper
{

    // it contains the required functionallity because im lazy
    public class DiskPart
    {



        // maybe lots of changes might be added, and why do it once and not waste time in txt creation/deletion
        public void ApplyChanges(string scriptFileLocation)
        {
            return;
        }

        public void ejectPartition()
        {
            return; // later
        }


        /// <summary>
        /// Shrinks a partition, but it doesn't go kids to school. 
        /// </summary>
        /// <param name="diskNumber">Disk number (Disk 0/1/2/3...)</param>
        /// <param name="partitionNumber">Partition number (par 1/2/3...)</param>
        /// <param name="size">Size (in MB) to be shrinked</param>
        public bool ShrinkPartition(int diskNumber, int partitionNumber, int size)
        {

            if (diskNumber == null) { return false; }
            if (partitionNumber == null) { return false; }
            if (size == null) { return false; }
            try
            {
                string scriptFilePath = System.IO.Path.GetTempPath() + @"\dpScript.txt";

                if (File.Exists(scriptFilePath)) { File.Delete(scriptFilePath); }


                File.AppendAllText(scriptFilePath,
                string.Format(
                    "SELECT DISK={0}\n" +
                "SELECT VOLUME={1}\n" +
                "SHRINK DESIRED={1}\n" +
                "CREATE PARTITION EFI\n" +
                "ASSIGN LETTER=T:\n" +
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
