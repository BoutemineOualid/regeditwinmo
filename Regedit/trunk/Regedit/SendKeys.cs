using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Regedit
{
    /// <summary>
    /// Provides methods for sending keystrokes to an application.
    /// </summary>
    public class SendKeys
    {
        static SendKeys()
        {
            keywords.Add("ENTER", 13);
            keywords.Add("TAB", 9);
            keywords.Add("ESC", 0x1b);
            keywords.Add("ESCAPE", 0x1b);
			keywords.Add("HOME", 0x24);
            keywords.Add("END", 0x23);
			keywords.Add("LEFT", 0x25);
            keywords.Add("RIGHT", 0x27);
			keywords.Add("UP", 0x26);
            keywords.Add("DOWN", 40);
			keywords.Add("PGUP", 0x21);
            keywords.Add("PGDN", 0x22);
			keywords.Add("NUMLOCK", 0x90);
            keywords.Add("SCROLLLOCK", 0x91);
			keywords.Add("PRTSC", 0x2c);
            keywords.Add("BREAK", 3);
			keywords.Add("BACKSPACE", 8);
            keywords.Add("BKSP", 8);
			keywords.Add("BS", 8);
            keywords.Add("CLEAR", 12);
			keywords.Add("CAPSLOCK", 20);
            keywords.Add("INS", 0x2d);
			keywords.Add("INSERT", 0x2d); 
            keywords.Add("DEL", 0x2e);
			keywords.Add("DELETE", 0x2e); 
            keywords.Add("HELP", 0x2f); 
			keywords.Add("F1", 0x70); 
            keywords.Add("F2", 0x71); 
			keywords.Add("F3", 0x72); 
            keywords.Add("F4", 0x73); 
			keywords.Add("F5", 0x74); 
            keywords.Add("F6", 0x75); 
			keywords.Add("F7", 0x76); 
            keywords.Add("F8", 0x77); 
			keywords.Add("F9", 120); 
            keywords.Add("F10", 0x79); 
			keywords.Add("F11", 0x7a); 
            keywords.Add("F12", 0x7b); 
			keywords.Add("F13", 0x7c); 
            keywords.Add("F14", 0x7d); 
			keywords.Add("F15", 0x7e); 
            keywords.Add("F16", 0x7f); 
			keywords.Add("MULTIPLY", 0x6a); 
            keywords.Add("ADD", 0x6b); 
			keywords.Add("SUBTRACT", 0x6d); 
            keywords.Add("DIVIDE", 0x6f);
            keywords.Add("+", 0x6b);

        }

        private static int MatchKeyword(string key)
        {
            foreach (var keyword in keywords)
            {
                if (String.Compare(keyword.Key, key, true, CultureInfo.InvariantCulture) == 0)
                    return keyword.Value;
            }

            return -1;
        }

        private static void CancelMods(byte[] keys, int level)
        {
            SetMods(keys, level, 0);
        }

        private static void SetMods(byte[] keys, int level, byte value)
        {
            if (keys[SHIFTPOS] == level)
            {
                if (value == 0) SendKey(SHIFTVK, KEYEVENTF_KEYUP);
                keys[SHIFTPOS] = value;
            }
            if (keys[CONTROLPOS] == level)
            {
                if (value == 0) SendKey(CONTROLVK, KEYEVENTF_KEYUP);
                keys[CONTROLPOS] = value;
            }
            if (keys[ALTPOS] == level)
            {
                if (value == 0) SendKey(ALTVK, KEYEVENTF_KEYUP);
                keys[ALTPOS] = value;
            }
        }


        /// <summary>
        /// Sends keystrokes to the active application.
        /// </summary>
        /// <param name="keys">The string of keystrokes to send.</param>
        public static void Send(string keys)
        {
            byte[] mods = new byte[3];

            char ch;
            int pos;
            int index = 0;
            byte group = 0;
            while (index < keys.Length)
            {
                ch = keys[index];
                switch (ch)
                {
                    case '(': // start group
                        if (group > 3) throw new ArgumentException("Nesting error.");
                        group++;
                        SetMods(mods, 4, group);
                        break;

                    case ')': // end group
                        if (group < 1) throw new ArgumentException("'(' is missing.");
                        CancelMods(mods, group);
                        group--;
                        break;

                    case '+':
                        if (mods[SHIFTPOS] != 0) throw new ArgumentException("Invalid SendKey string.");
                        mods[SHIFTPOS] = 4;
                        SendKey(SHIFTVK, KEYEVENTF_KEYDOWN);
                        break;

                    case '^':
                        if (mods[CONTROLPOS] != 0) throw new ArgumentException("Invalid SendKey string.");
                        mods[CONTROLPOS] = 4;
                        SendKey(CONTROLVK, KEYEVENTF_KEYDOWN);
                        break;

                    case '%':
                        if (mods[ALTPOS] != 0) throw new ArgumentException("Invalid SendKey string.");
                        mods[ALTPOS] = 4;
                        SendKey(ALTVK, KEYEVENTF_KEYDOWN);
                        break;

                    case '~': // ENTER key 
                        SendChar(13, mods,false);
                        break;

                    case '{':
                        {
                            index++;
                            if ((pos = keys.IndexOf('}', index)) < 0) throw new ArgumentException("'}' is missing.");
                            string t = keys.Substring(index, pos - index);
                            if (t.Length == 0) // {}}?
                            {
                                index++;
                                if (keys.Length > index && keys[index] == '}')
                                    SendChar((byte)'}', mods,true);
                                else
                                    throw new ArgumentException("'{}' is invalid sequence.");
                            }
                            else
                            {
                                int count;
                                index += t.Length;
                                string[] s = t.Split(new char[] { ' ' });
                                if (s.Length > 2) throw new ArgumentException("Incorrect string in the {...}");
                                if (s.Length == 2)
                                {
                                    t = s[0];
                                    count = Convert.ToInt32(s[1]);
                                }
                                else
                                    count = 1;

                                int vk = MatchKeyword(t);
                                if (vk < 0) // key has not been found
                                {
                                    if (t.Length == 1) // is it {+}, {~}, {^}, etc.
                                        vk = (byte)t[0];
                                    else
                                        throw new ArgumentException("Keyword '" + t + "' has not been found.");
                                }

                                for (int i = 0; i < count; i++)
                                    SendChar((byte)vk, mods,true);
                            }
                            break;
                        }

                    default:
                        SendChar((byte)ch, mods,false);
                        break;

                }

                index++;
            }

            CancelMods(mods, 4);
        }

        private static void SendKey(byte k, int flags)
        {
            keybd_event(k, 0, flags, 0);
        }

        private static void SendChar(byte k, byte[] mods, bool isSpeacialChar)
        {
            // We use the PostKeybdMessage() api instead of keybd_event
            // as it correctly manages upper case, lower case
            // and special characters
            //keybd_event(k, 0, 0, 0);
            //keybd_event(k, 0, KEYEVENTF_KEYUP, 0);
            //IntPtr hwnd = IntPtr.Zero;

            if (isSpeacialChar)
            {
                keybd_event(k, 0, KEYEVENTF_KEYDOWN, 0);
                keybd_event(k, 0, KEYEVENTF_KEYUP, 0);
            }
            else
            {
                uint KeyStateDownFlag = 0x0080;
                uint KeyShiftDeadFlag = 0x20000;

                uint[] buf1 = new uint[1];
                uint[] DownStates = new uint[1];
                DownStates[0] = KeyStateDownFlag;
                buf1[0] = (uint)k;

                uint[] DeadStates = { KeyShiftDeadFlag };

                int hwnd = -1;
                PostKeybdMessage(hwnd, 0, KeyStateDownFlag, (uint)buf1.Length, DownStates, buf1);
                buf1[0] = 0;
                PostKeybdMessage(hwnd, 0, KeyShiftDeadFlag, 1, DeadStates, buf1);
                CancelMods(mods, 4);
            }
        }


        private static Dictionary<string, int> keywords = new Dictionary<string, int>(); 

        const int SHIFTVK = 0x10;
        const int CONTROLVK = 0x11;
        const int ALTVK = 0x12;

        const int SHIFTPOS = 0;
        const int CONTROLPOS = 1;
        const int ALTPOS = 2;

        const int KEYEVENTF_KEYDOWN = 0;
        const int KEYEVENTF_KEYUP = 2;

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("coredll.dll")]
        private static extern bool PostKeybdMessage(int hwnd, uint vKey, uint KeyStateFlags, uint cCharacters, uint[] pShiftStateBuffer, uint[] pCharacterBuffer);

    }
}
