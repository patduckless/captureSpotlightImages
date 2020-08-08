/*
I know this is not the best way to do this but it was an exercise to learn how to interact with the regisrty. 
I could write this just to move the images from the source folder to the destination folder and it would be
much more elegant but where is the fun in elegance?
*/

using System;
using Microsoft.Win32;
using System.Linq;
using System.DirectoryServices.AccountManagement;
using System.IO;

namespace captureSpotlightBackgrounds
{
    class Program
    {
        static void Main(string[] args)
        {
            // get users SID 
            string mySid = UserPrincipal.Current.Sid.ToString();
            string toAccess = $"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Authentication\\LogonUI\\Creative\\{mySid}";
            // had to use OpenBaseKey due to 32/64 bit incompatibilities. This drove me mad for days before I found it. 
            RegistryKey RegistryBase = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey hKey = RegistryBase.OpenSubKey(toAccess);
            string[] keys = hKey.GetSubKeyNames();
            foreach (string key in keys )
            {
                string keyPath = string.Format(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\Creative\{0}\{1}", mySid, key);
                RegistryKey ImageKey = RegistryBase.OpenSubKey(keyPath);
                string keyVal = ImageKey.GetValue("LandscapeImage").ToString();
                string image = keyVal.Split('\\').Last();
                string dest = $"C:\\Users\\{Environment.UserName}\\Wallpapers\\{image}.jpeg";
                File.Copy(keyVal, dest);
            }
        }
    }
}
