using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using MSFree4All.Core.Office.DataTemplates;
using MSFree4All.Core.Office.Enums;
namespace MSFree4All.Core.Office
{
    public static class Consts
    {
        #region Langs
        public static List<(string, string)> Languages
        {
            get => new()
            {
        ("Same as OS Language", "MatchOS"),
        ("Afrikaans", "af-ZA"),
        ("Albanian", "sq-AL"),
        ("Arabic", "ar-SA"),
        ("Armenian", "hy-AM"),
        ("Assamese", "as-IN"),
        ("Azerbaijani (Latin)", "az-Latn-AZ"),
        ("Bangla (Bangladesh)", "bn-BD"),
        ("Bangla (Bengali India)", "bn-IN"),
        ("Basque (Basque)", "eu-ES"),
        ("Bosnian (Latin)", "bs-latn-BA"),
        ("Bulgarian", "bg-BG"),
        ("Catalan", "ca-ES"),
        ("Catalan (Valencia)", "ca-ES-valencia"),
        ("Chinese(Simplified)", "zh-CN"),
        ("Chinese(Traditional)", "zh-TW"),
        ("Croatian", "hr-HR"),
        ("Czech", "cs-CZ"),
        ("Danish", "da-DK"),
        ("Dutch", "nl-NL"),
        ("English", "en-US"),
        ("English UK", "en-GB"),
        ("Estonian", "et-EE"),
        ("Finnish", "fi-FI "),
        ("French", "fr-FR"),
        ("French Canada", "fr-CA"),
        ("Galician", "gl-ES"),
        ("Georgian", "ka-GE"),
        ("German", "de-DE"),
        ("Greek", "el-GR"),
        ("Gujarati", "gu-IN"),
        ("Hausa", "ha-Latn-NG"),
        ("Hebrew", "he-IL"),
        ("Hindi", "hi-IN"),
        ("Hungarian", "hu-HU"),
        ("Icelandic", "is-IS"),
        ("Igbo", "ig-NG"),
        ("Indonesian", "id-ID "),
        ("Irish", "ga-IE"),
        ("isiXhosa", "xh-ZA"),
        ("isiZulu", "zu-ZA"),
        ("Italian", "it-IT"),
        ("Japanese", "ja-JP"),
        ("Kannada", "kn-IN"),
        ("Kazakh", "kk-KZ"),
        ("Kinyarwanda", "rw-RW"),
        ("KiSwahili", "sw-KE"),
        ("Konkani", "kok-IN"),
        ("Korean", "ko-KR"),
        ("Kyrgyz", "ky-KG"),
        ("Latvian", "lv-LV"),
        ("Lithuanian", "lt-LT"),
        ("Luxembourgish", "lb-LU"),
        ("Macedonian (North Macedonia)", "mk-MK"),
        ("Malay (Latin)", "ms-MY"),
        ("Malayalam", "ml-IN"),
        ("Maltese", "mt-MT"),
        ("Maori", "mi-NZ"),
        ("Marathi", "mr-IN"),
        ("Nepali", "ne-NP"),
        ("Norwegian Bokmål", "nb-NO"),
        ("Norwegian Nynorsk", "nn-NO"),
        ("Odia", "or-IN"),
        ("Pashto", "ps-AF"),
        ("Persian (Farsi)", "fa-IR"),
        ("Polish", "pl-PL"),
        ("Portuguese(Portugal)", "pt-PT"),
        ("Portuguese(Brazil) ", "pt-BR"),
        ("Punjabi (Gurmukhi)", "pa-IN"),
        ("Romanian", "ro-RO"),
        ("Romansh", "rm-CH"),
        ("Russian", "ru-RU"),
        ("Scottish Gaelic", "gd-GB"),
        ("Serbian (Cyrillic, Serbia)", "sr-cyrl-RS"),
        ("Serbian (Latin, Serbia)", "sr-latn-RS"),
        ("Serbian (Cyrillic, Bosnia & Herzegovina)", "sr-cyrl-BA"),
        ("Sesotho sa Leboa", "nso-ZA"),
        ("Setswana", "tn-ZA"),
        ("Sinhala", "si-LK"),
        ("Slovak", "sk-SK"),
        ("Slovenian", "sl-SI"),
        ("Spanish", "es-ES"),
        ("Spanish Mexico", "es-MX"),
        ("Swedish", "sv-SE"),
        ("Tamil", "ta-IN"),
        ("Tatar (Cyrillic)", "tt-RU"),
        ("Telugu", "te-IN"),
        ("Thai", "th-TH"),
        ("Turkish", "tr-TR"),
        ("Ukrainian", "uk-UA"),
        ("Urdu", "ur-PK"),
        ("Uzbek (Latin)", "uz-Latn-UZ"),
        ("Vietnamese", "vi-VN"),
        ("Welsh", "cy-GB"),
        ("Wolof", "wo-SN"),
        ("Wolof", "yo-NG")
            };
        }
        #endregion
    }
    
    public class DisplayElement
    {
        public DisplayLevel Level { get; set; }
        public bool AcceptEULA { get; set; }
        public DisplayElement(DisplayLevel level, bool acceptEULA)
        {
            Level = level;
            AcceptEULA = acceptEULA;
        }
    }
    public class RemoveMSI
    {
        public bool Value { get; set; }
        public List<RemoveMSIApps> Apps { get; set; }
        public bool IsSameLang { get; set; }
        public RemoveMSI(bool value, List<RemoveMSIApps> apps, bool isSameLang)
        {
            Value = value;
            Apps = apps;
            IsSameLang = isSameLang;
        }
        public void AddRemoveApp(RemoveMSIApps app)
        {
            Apps.Add(app);
        }
        public void RemoveRemoveApp(RemoveMSIApps app)
        {
            Apps.Remove(app);
        }
        public void SetValues(bool value,List<RemoveMSIApps> apps = null,bool? isSameLang = null)
        {
            this.Value = value;
            if(apps != null)
            {
                Apps = apps;
            }
            if(isSameLang != null)
            {
                this.IsSameLang = (bool)isSameLang;
            }
        }
    }
    public class Properties
    {
        public bool FORCEAPPSHUTDOWN { get; set; }
        public bool PinIconsToTaskbar { get; set; }
        public bool AUTOACTIVATE { get; set; }
        public Licensing LicensingProperties { get; } = new Licensing();
        public List<XML.Property> GetProperties()
        {
            List<XML.Property> properties = LicensingProperties.GetProperties();
            properties.Add(new XML.Property { Name = nameof(FORCEAPPSHUTDOWN), Value = FORCEAPPSHUTDOWN.ToUpperCase() });
            properties.Add(new XML.Property { Name = nameof(PinIconsToTaskbar), Value = PinIconsToTaskbar.ToUpperCase() });
            properties.Add(new XML.Property { Name = nameof(AUTOACTIVATE), Value = AUTOACTIVATE.ToInt().ToString() });
            return properties;
        }

        public class Licensing
        {
            public LicensingType Type { get; set; } = LicensingType.UserBased;
            public bool SCLCacheOverride { get; set; }
            public string SCLCacheOverrideDirectory { get; set; } = "";

            public List<XML.Property> GetProperties()
            {
                List<XML.Property> properties = new();
                if (Type == LicensingType.DeviceBased)
                {
                    properties.Add(new XML.Property { Name = "DeviceBasedLicensing", Value = "1" });
                }
                else if(Type == LicensingType.SharedComputer)
                {
                    properties.Add(new XML.Property { Name = "SharedComputerLicensing", Value = "1" });
                    if (SCLCacheOverride)
                    {
                        properties.Add(new XML.Property { Name = "SCLCacheOverride", Value = "1" });
                        if (!string.IsNullOrEmpty(SCLCacheOverrideDirectory))
                        {
                            properties.Add(new XML.Property { Name = "SCLCacheOverrideDirectory", Value = SCLCacheOverrideDirectory });
                        }
                    }
                }
                return properties;
            }
        }
    }
    public class Add
    {
        #region Properties
        /// <summary>
        /// ForceUpgrade arguement in the Add element of the configuration
        /// </summary>
        public bool ForceUpgrade { get; set; }
        /// <summary>
        /// CDNFallback arguement in the Add element of the configuration
        /// </summary>
        public bool AllowCdnFallback { get; set; }
        /// <summary>
        /// OfficeClientEdition arguement in the Add element of the configuration
        /// </summary>
        public Architecture Architecture { get; set; } = Architecture.x86;
        /// <summary>
        /// Channel arguement in the Add element of the configuration
        /// </summary>
        public Channel Channel { get; set; } = Channel.Current;
        /// <summary>
        /// DownloadPath arguement in the Add element of the configuration
        /// </summary>
        public string DownloadPath { get; set; } = "";
        /// <summary>
        /// SourcePath arguement in the Add element of the configuration
        /// </summary>
        public string SourcePath { get; set; } = "";
        /// <summary>
        /// Channel arguement in the Add element of the configuration
        /// </summary>
        public string Version { get; set; } = "";
        #endregion
    }
    public class OfficeCore
    {
        #region Properties

        /// <summary>
        /// The main XML Class of the configuration
        /// </summary>
        public XML.Configuration ConfigurationXML { get; set; } = XML.Configuration.CreateNew();
        /// <summary>
        /// All products of the configuration
        /// </summary>
        public ObservableCollection<OfficeProduct> Products { get; set; } = new();
        /// <summary>
        /// RemoveMSI element of the configuration
        /// </summary>
        public RemoveMSI RemoveMSI { get; } = new RemoveMSI(false, new List<RemoveMSIApps>(), false);
        /// <summary>
        /// Add element of the configuration
        /// </summary>
        public Add Add { get; } = new();
        /// <summary>
        /// Display element of the configuration
        /// </summary>
        public DisplayElement Display { get; } = new DisplayElement(DisplayLevel.Full, false);
        /// <summary>
        /// Property elements of the configuration
        /// </summary>
        public Properties PropertyElements { get; } = new Properties();
        /// <summary>
        /// organization name to set the Company property on office documents
        /// </summary>
        public string CompanyName { get; set; } = "";
        /// <summary>
        /// The description of the configuration
        /// </summary>
        public string Description { get; set; } = "";

        #endregion


        /// <summary>
        /// Creates an empty office core with default values
        /// </summary>
        public OfficeCore() { }

        /// <summary>
        /// Check the known errors of the given product ID
        /// </summary>
        /// <returns>An array of <see cref="Errors[]"/></returns>
        /// <exception cref="NullReferenceException"/>
        public Errors[] CheckProductErrors(int productCount)
        {
            try
            {
                var item = Products.Where(x => x.Count == productCount).FirstOrDefault();

                List<Errors> errors = new();

                #region No Product Error
                if (item.ID == null)
                {
                    errors.Add(Errors.NoProduct);
                }
                #endregion

                #region Already Exists Error
                else
                {
                    bool exists = false;
                    foreach (var p in Products)
                    {
                        if (p.Count != productCount)
                        {
                            if (p.ID == item.ID)
                            {
                                exists = true;
                            }
                        }
                    }
                    if (exists)
                    {
                        errors.Add(Errors.AlreadyExists);
                    }
                }
                #endregion

                #region No Language Error
                if (item.Languages.Count == 0)
                {
                    errors.Add(Errors.NoLangaugae);
                }
                #endregion

                #region Inavalid Language Error
                else
                {
                    bool isntInvald = true;
                    foreach (var lang in item.Languages)
                    {
                        bool Real = false;
                        foreach (var rl in Consts.Languages)
                        {
                            if (rl.Item2 == lang.Culture)
                            {
                                Real = true;
                            }
                        }
                        if (!Real)
                        {
                            isntInvald = false;
                        }

                    }
                    if (!isntInvald)
                    {
                        errors.Add(Errors.InvalidLangaugae);
                    }
                }
                #endregion

                #region Volime Licnese Error
                foreach (var p in Products)
                {
                    if (p.ID.ToString().Contains("Volume"))
                    {
                        if (Products.Count > 1)
                        {
                            errors.Add(Errors.VolumeError);
                        }
                    }
                }
                #endregion

                return errors.ToArray();
            }
            catch
            {

                throw new NullReferenceException("Cannot find a product with that Count");
            }
        }
    }
}
