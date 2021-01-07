using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanceTools.IO;
using WallpaperManager.ApplicationData;

namespace WallpaperManager.Pathing
{
    public static class ImagePathing
    {
        private static readonly string TempImageLocation = Path.GetDirectoryName(Application.ExecutablePath) + @"\WallpaperData\TempImageLocation.file";

        public static string[] RenameImage(string image, DirectoryInfo moveDirectory, bool allowTagBasedNaming, ImageType[] filter)
        {
            return RenameImages(new string[] {image}, moveDirectory, allowTagBasedNaming, filter);
        }

        public static string[] RenameImages(string[] images, DirectoryInfo moveDirectory, bool allowTagBasedNaming, ImageType[] filter)
        {
            List<WallpaperData.ImageData> imagesData = new List<WallpaperData.ImageData>();
            foreach (string image in images)
            {
                bool filterBreached = false;
                foreach (ImageType imageType in filter) // allows for the filtering of an image type, i.e. Static, GIF, & Video
                {
                    if (WallpaperData.GetAllImagesOfType(imageType).Contains(image))
                    {
                        filterBreached = true;
                        break;
                    }
                }

                if (filterBreached) continue;

                imagesData.Add(WallpaperData.GetImageData(image));
            }

            return RenameImages(imagesData.ToArray(), moveDirectory, allowTagBasedNaming);
        }

        private static string[] RenameImages(WallpaperData.ImageData[] imagesToRename, DirectoryInfo moveDirectory, bool allowTagBaseNaming)
        {
            if (imagesToRename.Length == 0)
            {
                MessageBox.Show("None of the given images were able to be renamed");
                return null;
            }

            ;
            if (allowTagBaseNaming)
            {
                return TagBasedNaming(imagesToRename, moveDirectory);
            }
            else
            {
                // regardless of if an image is tagged are not, all images in this code block will be directly MOVED (this only applies to moved images)
                // read the TODOs below for some related comments, although there should still be yet another renaming segment below for directly renamed images
            }

            // TODO for all images that won't be renamed based on tags, you should gather a collection of all tag-based renamed images so that they won't be processed a second time
            // TODO See if you can use hashSet's union()/unionWith()/intersectWith() system for this
            // TODO Considering that the filter doesn't highlight the possibility of moving naming-disabled images, the user should be warned of this option with a prompt

            return null;
        }

        private static string[] TagBasedNaming(WallpaperData.ImageData[] imagesToRename, DirectoryInfo moveDirectory)
        {
            //! NOTE: THIS ONLY HANDLES TAGGED IMAGES | UNTAGGED OR UNNAMABLE IMAGES WILL HAVE TO BE HANDLED IN A SECOND SET
            Dictionary<string, Dictionary<string, HashSet<WallpaperData.ImageData>>> desiredNames = GetDesiredNames(imagesToRename, moveDirectory);

            //! Get extensions and names of images that aren't being touched using FileInfo, not
            //! Images that already exist, regardless of whether or not they are in the process of being renamed, cannot be touched
            //! If you were to rename an entire group of images with the same name, they'd have to be renamed twice to get the proper numbering in place
            //? Note that if a move directory was present, only one directory will be scanned

            // no need to group if there's only 1 image
            // not keeping this as a static option allows for more flexibility in when you want to keep a set of images together or not worry about that at all
            bool groupRenamedImages = imagesToRename.Length > 1 && MessageBox.Show("Group renamed images?", "Choose an option", MessageBoxButtons.YesNo) == DialogResult.Yes;
            Debug.WriteLine("Grouping: " + groupRenamedImages);

            List<string> finalNames = new List<string>();
            foreach (string directory in desiredNames.Keys)
            {
                Debug.WriteLine("\n\n\nDirectory:\n" + directory);
                FileInfo[] directoryFiles = new DirectoryInfo(directory).GetFiles();
                HashSet<string> filePaths = new HashSet<string>();
                foreach (FileInfo file in directoryFiles)
                {
                    string fileName = file.FullName;
                    filePaths.Add(fileName.Substring(0, fileName.IndexOf(file.Extension, StringComparison.Ordinal)).ToLower());
                }

                foreach (string name in desiredNames[directory].Keys)
                {
                    int nameCount = 1; // image counts don't start at 0
                    bool canName = false;
                    string directoryPath = directory + "\\";
                    string countlessName = directoryPath + name;
                    string startingName = countlessName + nameCount;
                    Debug.WriteLine("\nStarting Name: " + startingName);

                    // This process will repeat until a group-able section is found (or none at all if groupRenamedImages is set to false)
                    while (!canName)
                    {
                        // Finds the next possible starting name
                        // There is no way to skip counting this 1 by 1 without *assuming* that all concurrent values of this name have been filled
                        while (filePaths.Contains(startingName.ToLower()))
                        {
                            nameCount++;
                            startingName = countlessName + nameCount;
                            Debug.WriteLine("Updating Starting Name: " + startingName);
                        }

                        Debug.WriteLine("Checkpoint Starting Name: " + startingName);

                        // Checks for the next fully available space
                        // Ensures that a group of images can be renamed together
                        canName = true;
                        if (groupRenamedImages)
                        {
                            for (int i = 0; i < desiredNames[directory][name].Count; i++)
                            {
                                string testName = countlessName + (nameCount + i);
                                if (filePaths.Contains(testName.ToLower())) //! Note: nameCount should only change if the process fails to update the position
                                {
                                    Debug.WriteLine("Grouping Failed At: " + testName);
                                    nameCount += i + 1; // sets the count to the next possibly valid position
                                    canName = false;
                                    break;
                                }
                            }
                        }
                    }

                    Debug.WriteLine("Final Starting Name: " + startingName);

                    foreach (WallpaperData.ImageData image in desiredNames[directory][name])
                    {
                        string oldPath = image.Path;
                        string newPathWithoutExtension = countlessName + nameCount;

                        // last call for conflicts | if the images aren't grouped this is expected, if not then there was an unexpected issue
                        while (filePaths.Contains(newPathWithoutExtension.ToLower()))
                        {
                            nameCount++;
                            newPathWithoutExtension = countlessName + nameCount;
                        }

                        string extension = new FileInfo(image.Path).Extension;
                        string newPath = newPathWithoutExtension + extension;
                        Debug.WriteLine("Setting Name: " + newPath);
                        nameCount++;

                        if (UpdateImagePath(oldPath, newPath, image))
                        {
                            finalNames.Add(newPath);
                        }
                        else
                        {
                            finalNames.Add(oldPath);
                        }
                    }
                }
            }

            return finalNames.ToArray();
        }

        private static Dictionary<string, Dictionary<string, HashSet<WallpaperData.ImageData>>> GetDesiredNames(WallpaperData.ImageData[] imagesToRename, DirectoryInfo moveDirectory)
        {
            string failedToNameException = "No name could be created for the following images." +
                               "\nThese images either have no tags or all of their tags cannot be used for naming:\n";
            bool failedToNameAnImage = false;

            // Generates a collection of directories filled with
            Dictionary<string, Dictionary<string, HashSet<WallpaperData.ImageData>>> desiredNames = new Dictionary<string, Dictionary<string, HashSet<WallpaperData.ImageData>>>();
            foreach (var image in imagesToRename)
            {
                string desiredName = image.GetTaggedName();

                if (desiredName != "")
                {
                    //? If a moveDirectory is present, only one directory should be used
                    string directory = moveDirectory == null ? new FileInfo(image.Path).Directory.FullName : moveDirectory.FullName;
                    Debug.WriteLine("Directory: " + directory + " | DesiredName: " + desiredName + " | Image: " + new FileInfo(image.Path).Name);

                    if (desiredNames.ContainsKey(directory))
                    {
                        if (desiredNames[directory].ContainsKey(desiredName)) // this name has been encountered more than once
                        {
                            desiredNames[directory][desiredName].Add(image);
                        }
                        else
                        {
                            Debug.WriteLine("New Name");
                            desiredNames[directory].Add(desiredName, new HashSet<WallpaperData.ImageData> { image });
                        }
                    }
                    else // initializes the newly found directory with the desired name and the image
                    {
                        Debug.WriteLine("New Directory");
                        desiredNames.Add(directory, new Dictionary<string, HashSet<WallpaperData.ImageData>>
                        {
                            {desiredName, new HashSet<WallpaperData.ImageData> {image}}
                        });
                    }
                }
                else // naming for this image failed | No name could be generated
                {
                    failedToNameException += "\n" + image.Path;
                    failedToNameAnImage = true;
                }
            }

            // TODO THIS SHOULD LATER EXCLUDE IMAGES THAT CANNOT BE RENAMED (But want to be moved), THEY WILL BE ADDED TO A SEPARATE SET
            if (failedToNameAnImage)
            {
                MessageBox.Show(failedToNameException);
            }

            return desiredNames;
        }

        private static bool UpdateImagePath(string oldPath, string newPath, WallpaperData.ImageData image)
        {
            if (image == null) return false;

            if (File.Exists(oldPath))
            {
                if (!File.Exists(newPath))
                {
                    if (new FileInfo(newPath).Name.IndexOfAny(Path.GetInvalidFileNameChars()) == -1)
                    {
                        return FinalizeImagePathUpdate(oldPath, newPath);
                    }
                    else
                    {
                        MessageBox.Show("The image could not be renamed as its name contains invalid characters");
                    }
                }
                else
                {
                    MessageBox.Show("The following image could not be renamed as its intended path has already been taken: " +
                                    "\nOld Path: " + oldPath +
                                    "\nNew Path: " + newPath);
                }
            }
            else
            {
                MessageBox.Show("The image you attempted to rename was not found: \n" + oldPath);
            }

            return false;
        }

        private static bool FinalizeImagePathUpdate(string oldPath, string newPath)
        {
            try
            {
                //? Since File.Move is case insensitive, first we need to check if oldPath and newPath has the same letters when cases are ignored
                if (string.Equals(oldPath, newPath, StringComparison.CurrentCultureIgnoreCase))
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
                return true;
            }
            catch (Exception e)
            {
                // Most likely cause of an error is that the file was being used by another process
                List<Process> processes = FileUtil.WhoIsLocking(oldPath);
                if (processes.Count > 0)
                {
                    string processOutput = oldPath + "\nThe above image is being used by the following process: ";
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
                                    "\n\nError: " + e.Message);
                }
            }

            return false;
        }
    }
}
