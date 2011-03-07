using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace Regedit
{
    class RegistryUtils
    {

        public static RegistryKey OpenKeyFromPath(string keyPath, bool writableKey)
        {
            if (string.IsNullOrEmpty(keyPath))
                return null;
            string[] pathElements = keyPath.Split(new[] { '\\' });

            // Getting the first element
            string rootKeyName = pathElements[0];
            string subPath = keyPath.IndexOf('\\') == -1 ? keyPath : keyPath.Substring(keyPath.IndexOf('\\'));
            try
            {
                if (pathElements.Length == 1) // Root nodes have been selected.
                {
                    if (rootKeyName == Registry.ClassesRoot.Name)
                        return Registry.ClassesRoot;

                    else if (rootKeyName == Registry.CurrentUser.Name)
                        return Registry.CurrentUser;

                    else if (rootKeyName == Registry.LocalMachine.Name)
                        return Registry.LocalMachine;

                    else if (rootKeyName == Registry.Users.Name)
                        return Registry.Users;
                }
                else
                {
                    if (rootKeyName == Registry.ClassesRoot.Name)
                        return Registry.ClassesRoot.OpenSubKey(subPath, writableKey);

                    else if (rootKeyName == Registry.CurrentUser.Name)
                        return Registry.CurrentUser.OpenSubKey(subPath, writableKey);

                    else if (rootKeyName == Registry.LocalMachine.Name)
                        return Registry.LocalMachine.OpenSubKey(subPath, writableKey);

                    else if (rootKeyName == Registry.Users.Name)
                        return Registry.Users.OpenSubKey(subPath, writableKey);
                }

                    return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Renames a subkey of the passed in registry key since 
        /// the Framework totally forgot to include such a handy feature.
        /// </summary>
        /// <param name="parentKey">The RegistryKey that contains the subkey 
        /// you want to rename (must be writeable)</param>
        /// <param name="subKeyName">The name of the subkey that you want to rename
        /// </param>
        /// <param name="newSubKeyName">The new name of the RegistryKey</param>
        /// <returns>True if succeeds</returns>
        public static bool RenameSubKey(RegistryKey parentKey,
            string subKeyName, string newSubKeyName)
        {
            CopyKey(parentKey, subKeyName, newSubKeyName);
            parentKey.DeleteSubKeyTree(subKeyName);
            parentKey.Flush();
            return true;
        }

        /// <summary>
        /// Copy a registry key.  The parentKey must be writeable.
        /// </summary>
        /// <param name="parentKey"></param>
        /// <param name="keyNameToCopy"></param>
        /// <param name="newKeyName"></param>
        /// <returns></returns>
        public static bool CopyKey(RegistryKey parentKey,
            string keyNameToCopy, string newKeyName)
        {
            //Create new key
            RegistryKey destinationKey = parentKey.CreateSubKey(newKeyName);

            //Open the sourceKey we are copying from
            RegistryKey sourceKey = parentKey.OpenSubKey(keyNameToCopy, true);

            RecurseCopyKey(sourceKey, destinationKey);
            return true;
        }

        private static void RecurseCopyKey(RegistryKey sourceKey, RegistryKey destinationKey)
        {
            //copy all the values
            foreach (string valueName in sourceKey.GetValueNames())
            {
                object objValue = sourceKey.GetValue(valueName);
                RegistryValueKind valKind = sourceKey.GetValueKind(valueName);
                destinationKey.SetValue(valueName, objValue, valKind);
            }

            //For Each subKey 
            //Create a new subKey in destinationKey 
            //Call myself 
            foreach (string sourceSubKeyName in sourceKey.GetSubKeyNames())
            {
                RegistryKey sourceSubKey = sourceKey.OpenSubKey(sourceSubKeyName, true);
                RegistryKey destSubKey = destinationKey.CreateSubKey(sourceSubKeyName);
                RecurseCopyKey(sourceSubKey, destSubKey);
            }

            // Close it when finished so it can be deleted later in the final step
            sourceKey.Close();
            destinationKey.Close();
        }
    }
}
