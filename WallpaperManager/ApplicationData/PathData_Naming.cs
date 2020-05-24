using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanceTools.IO;
using WallpaperManager.Tagging;

namespace WallpaperManager.ApplicationData
{
    public static partial class PathData
    {
        public static void RenameImage(WallpaperData.ImageData image)
        {
            RenameImages(new WallpaperData.ImageData[] {image});
        }

        public static void RenameAffectedImages(WallpaperData.ImageData[] images)
        {
            if (MessageBox.Show("Rename affected images?", "Choose an option", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                RenameImages(images.ToArray());

            }
        }

        public static void RenameImages(WallpaperData.ImageData[] images)
        {
            /*TODO
            if (MessageBox.Show("hec", "yes", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                AddPotentialNamingExceptions();
            }
            */

            char[] nums = "0123456789".ToCharArray();
            bool renamingMultipleImages = images.Length > 0;

            // Dictionary<Directory, Dictionary<NewName, HashSet<ImageData>>>
            Dictionary<string, Dictionary<string, HashSet<WallpaperData.ImageData>>> namingConflicts = GetRenameData(images, out int failedToNameCount);
            List<string> acceptedNames = new List<string>();

            //bool renamingAllowed = MessageBox.Show("Allow renaming?", "Choose an option", MessageBoxButtons.YesNo) == DialogResult.Yes;
            bool groupRenamedImages = MessageBox.Show("Group renamed images?", "Choose an option", MessageBoxButtons.YesNo) == DialogResult.Yes;

            foreach (string curDirectory in namingConflicts.Keys)
            {
                Debug.WriteLine("\n\n\nDirectory:\n" + curDirectory);
                FileInfo[] directoryFiles = new DirectoryInfo(curDirectory).GetFiles();
                HashSet<string> filePaths = new HashSet<string>();
                foreach (FileInfo curFile in directoryFiles)
                {
                    filePaths.Add(curFile.FullName.Substring(0, curFile.FullName.IndexOf(curFile.Extension)).ToLower());
                }

                foreach (string curName in namingConflicts[curDirectory].Keys)
                {
                    int nameCount = 1;
                    bool canName = false;
                    string directoryPath = curDirectory + "\\";
                    string startingName = directoryPath + curName + nameCount;
                    Debug.WriteLine("\nStarting Name: " + startingName);

                    if (groupRenamedImages)
                    {
                        // Ensures that the group of images renamed can be renamed together
                        while (!canName)
                        {
                            while (filePaths.Contains(startingName.ToLower()))
                            {
                                nameCount++;
                                startingName = directoryPath + curName + nameCount;
                                Debug.WriteLine("Updating Starting Name: " + startingName);
                            }

                            Debug.WriteLine("Checkpoint Starting Name: " + startingName);

                            //TODO Consider making it an option on whether or not the user wishes to group renamed images or have them fill in the next possible location
                            // Checks for the next fully available space
                            canName = true;
                            for (int i = 0; i < namingConflicts[curDirectory][curName].Count; i++)
                            {
                                string testName = directoryPath + curName + nameCount + i;
                                if (filePaths.Contains(testName.ToLower()))
                                {
                                    canName = false;
                                    break;
                                }
                            }
                        }
                    }

                    Debug.WriteLine("Final Starting Name: " + startingName);

                    foreach (WallpaperData.ImageData curImage in namingConflicts[curDirectory][curName])
                    {
                        string oldPath = curImage.Path;
                        string extension = new FileInfo(curImage.Path).Extension;
                        string newPath = directoryPath + curName + nameCount + extension;

                        //DirectoryInfo imageDirectoryInfo = imageFileInfo.Directory;
                        Debug.WriteLine("Setting Name: " + newPath);
                        acceptedNames.Add(newPath);
                        nameCount++;

                        Debug.WriteLine("You disabled renaming entirely with a comment");
                        /*
                        if (renamingAllowed)
                        {
                            UpdateImagePath(oldPath, newPath, null);
                        }
                        */
                    }
                }
            }

            //! Temporary Code
            Debug.WriteLine("\nAccepted Names: ");
            foreach (string name in acceptedNames)
            {
                Debug.WriteLine(name);
            }
            //! Temporary Code

            // Data collected, rename the images
            /*
            if (failedToNameCount != images.Length)
            {
                foreach (Tuple<WallpaperData.ImageData, string> renameInfo in renameData)
                {
                    WallpaperData.ImageData imageData = renameInfo.Item1;
                    string newName = renameInfo.Item2;

                    FileInfo imageFileInfo = new FileInfo(imageData.Path);
                    DirectoryInfo imageDirectoryInfo = imageFileInfo.Directory;
                }
            }
            */

            //! DON'T FORGET TO UPDATE THE PATHS OF THE IMAGES USED!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        private static Dictionary<string, Dictionary<string, HashSet<WallpaperData.ImageData>>> GetRenameData(WallpaperData.ImageData[] images, out int failedToNameCount)
        {
            HashSet<WallpaperData.ImageData> failedToName = new HashSet<WallpaperData.ImageData>();
            string failedToNameException = "No name could be created for the following images." +
                                           "\nThese images either have no tags or all of their tags cannot be used for naming:\n";

            //xHashSet<Tuple<WallpaperData.ImageData, string>> renameData = new HashSet<Tuple<WallpaperData.ImageData, string>>();
            Dictionary<string, HashSet<WallpaperData.ImageData>> encounteredNameDictionary = new Dictionary<string, HashSet<WallpaperData.ImageData>>();

            foreach (WallpaperData.ImageData image in images)
            {
                string desiredName = image.GetTaggedName();

                desiredName = desiredName.Replace(' ', '_');

                if (desiredName != "")
                {
                    Debug.WriteLine("desiredName: " + desiredName);
                    //xrenameData.Add(new Tuple<WallpaperData.ImageData, string>(image, desiredName));

                    if (encounteredNameDictionary.ContainsKey(desiredName)) // this name has been encountered more than once
                    {
                        encounteredNameDictionary[desiredName].Add(image);
                    }
                    else // new name encountered
                    {
                        encounteredNameDictionary.Add(desiredName, new HashSet<WallpaperData.ImageData> { image });
                    }
                }
                else // naming for this image failed | No name could be generated
                {
                    failedToName.Add(image);
                    failedToNameException += "\n" + image.Path;
                }
            }

            failedToNameCount = failedToName.Count;
            if (failedToNameCount > 0)
            {
                MessageBox.Show(failedToNameException);
            }

            Dictionary<string, Dictionary<string, HashSet<WallpaperData.ImageData>>> namingConflicts = new Dictionary<string, Dictionary<string, HashSet<WallpaperData.ImageData>>>();
            if (failedToNameCount != images.Length)
            {
                //if (images.Length > 1) // there's a possibility for duplicate names in the same category
                //{
                    // find all conflicts
                    foreach (string potentialName in encounteredNameDictionary.Keys)
                    {
                        foreach (WallpaperData.ImageData image in encounteredNameDictionary[potentialName])
                        {
                            string imageDirectoryInfo = new FileInfo(image.Path).Directory.FullName;
                            if (namingConflicts.ContainsKey(imageDirectoryInfo)) // this directory has already been found
                            {
                                if (namingConflicts[imageDirectoryInfo].ContainsKey(potentialName)) // this name has already been found in this directory | Conflict found!
                                {
                                    namingConflicts[imageDirectoryInfo][potentialName].Add(image);
                                }
                                else // new potential conflict found (although if this conflict maintains a size of 1 it'll no longer be a conflict)
                                {
                                    namingConflicts[imageDirectoryInfo].Add(potentialName, new HashSet<WallpaperData.ImageData> { image });
                                }
                            }
                            else // new directory found
                            {
                                Dictionary<string, HashSet<WallpaperData.ImageData>> namingConflictsInDirectory = new Dictionary<string, HashSet<WallpaperData.ImageData>>
                                {
                                    { potentialName, new HashSet<WallpaperData.ImageData> { image } }
                                };

                                namingConflicts.Add(imageDirectoryInfo, namingConflictsInDirectory);
                            }
                        }
                    }
                //}
                //else // there's only one image, no need to do anything major
                //{
                    
                //}
            }

            return namingConflicts;
        }

        public static void UpdateImagePath(string oldPath, string newPath, WallpaperData.ImageData image)
        {
            if (image == null) return;

            if (File.Exists(oldPath))
            {
                if (!File.Exists(newPath))
                {
                    try
                    {
                        //? Since File.Move is case insensitive, first we need to check if oldPath and newPath has the same letters when cases are ignored
                        if (String.Equals(oldPath, newPath, StringComparison.CurrentCultureIgnoreCase))
                        {
                            Debug.WriteLine("Cases ignored");
                            // If oldPath and newPath have the same letters, move the file to a temporary location then move it back to the intended location
                            File.Move(oldPath, TempImageLocation);
                            File.Move(TempImageLocation, newPath);
                        }
                        else // otherwise, if the cases do not matter, move the file normally
                        {
                            File.Move(oldPath, newPath);
                        }

                        WallpaperData.GetImageData(oldPath).UpdatePath(newPath);
                    }
                    catch (Exception e)
                    {
                        // Most likely cause of an error is that the file was being used by another process
                        List<Process> processes = FileUtil.WhoIsLocking(oldPath);
                        if (processes.Count > 0)
                        {
                            string processOutput = oldPath + "\nis being used by the following processes: ";
                            for (int i = 0; i < processes.Count; i++)
                            {
                                processOutput += "\n" + processes[i].ProcessName;
                            }

                            MessageBox.Show(processOutput);
                        }
                        else
                        {
                            MessageBox.Show("Image not changed: \n[" + oldPath + "] " +
                                            "\n\nIntended new path: \n[" + newPath + "] " +
                                            "\n\nFile.Move() may have had a duplicate path error | Error: " + e.Message);
                        }
                        throw;
                    }

                }
                else // This shouldn't happen, if it does then there was either an issue with the code or the user renamed an image in the middle of this process
                {
                    throw new Exception("Attempted to rename an image to the name of an existing image: \n" + newPath);
                }
            }
            else // This shouldn't happen, if it does then likely the user attempted to rename the image in the middle of this process
            {
                throw new FileNotFoundException("The following file you attempted to rename was not found: \n" + oldPath);
            }
        }

        public struct RenameData
        {
            public DirectoryInfo Directory { get; }
            public string NewName { get; }
            public WallpaperData.ImageData Image { get; }

            public RenameData(DirectoryInfo directory, string newName, WallpaperData.ImageData image)
            {
                Directory = directory;
                NewName = newName;
                Image = image;
            }
        }

        //! Using this implies that all images have been tagged
        // This will go through each image and find naming exception in their already existing names then applying this to the actual image data
        public static void AddPotentialNamingExceptions()
        {
            char[] nums = "0123456789".ToCharArray();

            foreach (WallpaperData.ImageData image in WallpaperData.GetAllRankedImageData())
            {
                string imageFileName = new FileInfo(image.Path).Name;
                string imageName = imageFileName.Substring(0, imageFileName.IndexOfAny(nums));
            }
        }
    }
}
