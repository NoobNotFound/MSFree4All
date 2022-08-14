using Microsoft.Win32;
using MSFree4All.Core.Office.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFree4All.Core.Office
{
    public static class Extensions
    {
        /// <summary>
        /// Convets <paramref name="iDs"/> to an Office version (<see cref="int"/>) based on what it contains.
        /// </summary>
        /// <returns>Office version (<see cref="int"/>)</returns>
        public static int ConverToVer(this OfficeProductIDs? iDs)
        {
            if (iDs == null) { return 0; }
            if (iDs.ToString().Contains("365"))
            {
                return 365;
            }
            else if (iDs.ToString().Contains("2021"))
            {
                return 2021;
            }
            else if (iDs.ToString().Contains("2019"))
            {
                return 2019;
            }
            else
            {
                return 2016;
            }
        }

        /// <summary>
        /// Converts <paramref name="iDs"/> to a readable <see cref="string"/>
        /// </summary> 
        /// <returns>Firendly (<see cref="string"/>)</returns>
        public static string ConvertToString(this OfficeProductIDs? iD)
        {
            string r = "";
            if (iD == null) { return r; }
            switch ((int)iD)
            {
                case 1:
                    r = "Apps For Enterprise";
                    break;
                case 2:
                    r = "Apps For Business";
                    break;
                case 3:
                    r = "Small Business Premium";
                    break;
                case 4:
                    r = "Family And Personal";
                    break;
                case 5:
                    r = "Microsft 365";
                    break;

                //

                case 6:
                    r = "Professional Plus";
                    break;
                case 7:
                    r = "Professional Plus - Volume";
                    break;
                case 8:
                    r = "Home And Business";
                    break;
                case 9:
                    r = "Home And Student";
                    break;
                case 10:
                    r = "Personal";
                    break;
                case 11:
                    r = "Professional";
                    break;
                case 12:
                    r = "Standard";
                    break;
                //
                case 13:
                    r = "Professional Plus";
                    break;
                case 14:
                    r = "Professional Plus - Volume";
                    break;
                case 15:
                    r = "Home And Business";
                    break;
                case 16:
                    r = "Home And Student";
                    break;
                case 17:
                    r = "Personal";
                    break;
                case 18:
                    r = "Professional";
                    break;
                case 19:
                    r = "Standard";
                    break;
                //

                case 20:
                    r = "Professional Plus";
                    break;
                case 21:
                    r = "Home And Business";
                    break;
                case 22:
                    r = "Home And Student";
                    break;
                case 23:
                    r = "Personal";
                    break;
                case 24:
                    r = "Professional";
                    break;
                case 25:
                    r = "Standard";
                    break;
                case 26:
                    r = "Mondo";
                    break;

            }
            return r;
        }

        /// <summary>
        /// Converts the <paramref name="error"/> to an readable <see cref="string"/>
        /// </summary>
        /// <param name="error"></param>
        /// <returns><see cref="string"/></returns>
        public static string ToReadableString(this Errors error)
        {
            switch (error)
            {
                case Errors.AlreadyExists:
                    return "Product already exists";
                case Errors.NoLangaugae:
                    return "Language(s) is not selected";
                case Errors.NoProduct:
                    return "Product is not selected";
                case Errors.VolumeError:
                    return "Only one product can be deployed as a volume-license product";
                case Errors.InvalidLangaugae:
                    return "Invalid language(s) detected";
                case Errors.InvalidChannelVolume:
                    return "You can't select that channel with a volume-license product";
            }
            return "";
        }

        /// <summary>
        /// Converts the <paramref name="arch"/> to an Office Architecture <see cref="string"/>
        /// </summary>
        /// <param name="arch"></param>
        /// <returns><see cref="string"/></returns>
        public static string ToRealArch(this Architecture arch)
        {
            switch (arch)
            {
                case Architecture.x64:
                    return "64";
                case Architecture.x86:
                    return "32";
                case Architecture.AutoDetect:
                    var reg = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment").GetValue("PROCESSOR_ARCHITECTURE").ToString();
                    if (reg == "AMD64")
                    {
                        return "64";
                    }
                    else
                    {
                        return "32";
                    }
            }
            return "32";
        }

        /// <summary>
        /// Converts the <paramref name="b"/> to an <see cref="int"/>
        /// </summary>
        /// <param name="b"></param>
        /// <returns>1 or 0</returns>
        public static int ToInt(this bool b)
        {
            return b ? 1 : 0;
        }

        /// <summary>
        /// Converts the <paramref name="b"/> to an <see cref="int"/>
        /// </summary>
        /// <param name="b"></param>
        /// <returns>1, 0 or 2</returns>
        public static int ToInt(this bool? b)
        {
            switch (b)
            {
                case true:
                    return 1;
                case false:
                    return 0;
                case null:
                    return 2;
            }
        }

        /// <summary>
        /// Converts the <paramref name="b"/> to an <see cref="int"/>
        /// </summary>
        /// <param name="b"></param>
        /// <returns>true, false or null</returns>
        public static bool? ToBool(this int i)
        {
            switch (i)
            {
                case 1:
                    return true;
                case 0:
                    return false;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Converts the <paramref name="b"/> to an uper case <see cref="string"/>
        /// </summary>
        /// <param name="b"></param>
        /// <returns>Upper case <see cref="string"/></returns>
        public static string ToUpperCase(this bool b)
        {
            return b.ToString().ToUpper();
        }
    }
}
