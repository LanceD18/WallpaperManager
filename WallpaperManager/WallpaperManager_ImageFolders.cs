using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using LanceTools;
using WallpaperManager.ApplicationData;

namespace WallpaperManager
{
    public partial class WallpaperManager : Form
    {
        private void buttonAddFolder_Click(object sender, EventArgs e)
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                // dialog properties
                dialog.Multiselect = true;
                dialog.IsFolderPicker = true;

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    Dictionary<string, bool> imageFolders = new Dictionary<string, bool>();
                    foreach (string file in dialog.FileNames)
                    {
                        imageFolders.Add(file, true);
                    }

                    AddImageFolders(imageFolders);
                }
            }
        }

        public void LoadImageFolders(Dictionary<string, bool> imageFolders) //? this is here solely for WallpaperData so that the class can load in ImageFolder info
        {
            AddImageFolders(imageFolders);
        }

        private void AddImageFolders(Dictionary<string, bool> imageFolders)
        {
            // output properties
            string duplicateFolderPaths = "";
            int initialLength = flowLayoutPanelImageFolders.Controls.Count;
            bool pauseLayout = imageFolders.Count > 1;

            // loops through selected paths and adds the paths that don't already exist
            if (pauseLayout) { flowLayoutPanelImageFolders.SuspendLayout(); } // prevents inefficiency by handling controls all at once instead of one-by-one
            foreach (string path in imageFolders.Keys)
            {
                if (WallpaperData.AddFolder(path))  // if the path does not exist, create a new CheckBox with this path as the text and add it to flowLayoutPanelImageFolders
                {
                    CheckBox newCheckBox = new CheckBox { AutoSize = true, Text = path, ForeColor = Color.White };
                    newCheckBox.Checked = imageFolders[path];
                    //? The below event must be added after declaring Checked since AddFolder() already handles activation
                    newCheckBox.CheckedChanged += imageFolderCheckBox_CheckedChanged;

                    flowLayoutPanelImageFolders.Controls.Add(newCheckBox);
                }
                else
                {
                    duplicateFolderPaths += "\n" + path;
                }
            }
            if (pauseLayout) { flowLayoutPanelImageFolders.ResumeLayout(); } // prevents inefficiency by handling controls all at once instead of one-by-one

            //! Make an Autosave Option | Also this might be hindering performance on load [Tbh thou I don't really notice too big of a difference, test]
            /*
            if (initialLength != flowLayoutPanelImageFolders.Controls.Count) // one or more folders have been added, rewrite data
            {
                WallpaperData.SaveData(PathData.ActiveWallpaperTheme);
            }
            */

            // displays a list of folder paths the user attempted to add that already exist
            if (duplicateFolderPaths != "")
            {
                MessageBox.Show("The following folders already exist: " + duplicateFolderPaths);
            }
        }

        private void buttomRemoveFolder_Click(object sender, EventArgs e)
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                // dialog properties
                dialog.Multiselect = true;
                dialog.IsFolderPicker = true;

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    RemoveImageFolders(dialog.FileNames.ToArray());
                }
            }
        }

        private void RemoveImageFolders(string[] folderPaths)
        {
            // output properties
            string invalidFolderPaths = "";
            int initialLength = flowLayoutPanelImageFolders.Controls.Count;
            bool pauseLayout = folderPaths.Length > 1;

            // loops through selected paths and adds the paths that don't already exist
            if (pauseLayout) { flowLayoutPanelImageFolders.SuspendLayout(); } // prevents inefficiency by handling controls all at once instead of one-by-one
            foreach (string path in folderPaths)
            {
                if (WallpaperData.RemoveFolder(path)) // if the path exists, remove the checkBox at the index of path
                {
                    flowLayoutPanelImageFolders.Controls.RemoveAt(GetImageFolderPaths().IndexOf(path));
                }
                else
                {
                    invalidFolderPaths += "\n" + path;
                }
            }
            if (pauseLayout) { flowLayoutPanelImageFolders.ResumeLayout(); } // prevents inefficiency by handling controls all at once instead of one-by-one

            //! Make an Autosave Option | Also this might be hindering performance on load [Tbh thou I don't really notice too big of a difference, test]
            /*
            if (initialLength != flowLayoutPanelImageFolders.Controls.Count) // one or more folders have been removed, rewrite data
            {
                WallpaperData.SaveData(PathData.ActiveWallpaperTheme);
            }
            */

            // displays a list of folder paths the user attempted to add that already exist
            if (invalidFolderPaths != "")
            {
                MessageBox.Show("The following folders were invalid: " + invalidFolderPaths);
            }
        }

        private void imageFolderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox folderCheckBox)
            {
                string folderPath = folderCheckBox.Text;

                if (Directory.Exists(folderPath)) // activate or deactive images within folder based on option selected
                {
                    if (folderCheckBox.Checked)
                    {
                        ActivateFolderImages(folderPath);
                    }
                    else
                    {
                        DeactivateFolderImages(folderPath);
                    }
                }
                else // folder path does not exist, display error message and as if user wants to remove invalid folder
                {
                    DialogResult result = MessageBox.Show("Invalid folder path encountered: \n" + folderPath + "\n\nRemove folder?", "Invalid Folder Path", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        int folderIndex = GetImageFolderPaths().IndexOf(folderPath);

                        if (folderIndex != -1) // yes this can happen
                        {
                            flowLayoutPanelImageFolders.Controls.RemoveAt(GetImageFolderPaths().IndexOf(folderPath));
                        }
                        else
                        {
                            MessageBox.Show( "Error: This folder was not found in your collection \n (That is, your collection within the application not file explorer)");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("An error occured. Check Box item is invalid");
            }
        }

        private void ActivateFolderImages(string folderPath)
        {
            WallpaperData.ActivateImages(folderPath);
        }

        private void DeactivateFolderImages(string folderPath)
        {
            WallpaperData.DeactivateImages(folderPath);
        }

        private string[] GetImageFolderPaths()
        {
            Control.ControlCollection folderControls = flowLayoutPanelImageFolders.Controls;
            string[] folderPaths = new string[folderControls.Count];

            // creates an array of already existing folder paths
            for (int i = 0; i < folderControls.Count; i++)
            {
                CheckBox curFolderControl = folderControls[i] as CheckBox;

                try
                {
                    folderPaths[i] = curFolderControl.Text; //? don't try fixing the warning here, this is handled in the catch block
                }
                catch (Exception exception)
                {
                    throw new NullReferenceException("Incorrect control type used in flowLayoutPanelImageFolders. All controls should be of type CheckBox", exception);
                }
            }

            return folderPaths;
        }

        private void ClearImageFolders()
        {
            flowLayoutPanelImageFolders.Controls.Clear();
        }
    }
}
