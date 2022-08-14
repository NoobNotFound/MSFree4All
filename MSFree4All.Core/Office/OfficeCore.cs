using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using MSFree4All.Core.Office.Models;
using MSFree4All.Core.Office.Enums;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

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
    
    public class Display
    {
        public DisplayLevel Level { get; set; }
        public bool AcceptEULA { get; set; }
        public Display(DisplayLevel level, bool acceptEULA)
        {
            Level = level;
            AcceptEULA = acceptEULA;
        }

        public XML.Display GetElement()
        {
            if(Level == DisplayLevel.Full || AcceptEULA)
            {
                var d = new XML.Display();
                d.AcceptEULA = AcceptEULA;
                d.Level = Level.ToString();
                return d;
            }
            return null;
        }
    }
    public class RemoveMSI
    {
        public bool Value { get; set; }
        public List<RemoveMSIApps> Apps { get; set; } = new();
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

        public XML.RemoveMSI GetElement()
        {
            var rmsi = new XML.RemoveMSI();
            if (Value)
            {
                rmsi.IgnoreProduct = new();
                foreach (var item in Apps)
                {
                    rmsi.IgnoreProduct.Add(new XML.IgnoreProduct() { ID = item.ToString() });
                }
                return rmsi;
            }
            else
            {
                return null;
            }
        }
    }
    public class Properties
    {
        public bool FORCEAPPSHUTDOWN { get; set; }
        public bool PinIconsToTaskbar { get; set; }
        public bool AUTOACTIVATE { get; set; }
        public Licensing LicensingProperties { get; } = new Licensing();
        public List<XML.Property> GetElements()
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
    public class Updates
    {
        #region Properties
        /// <summary>
        /// Define whether Update element is used
        /// </summary>
        public bool? Enabled { get; set; } = null;

        /// <summary>
        /// The UpdatePath attribute of the Update element
        /// </summary>
        public string UpdatePath { get; set; } = "";

        /// <summary>
        /// The TargetVersion attribute of the Update element
        /// </summary>
        public string TargetVersion { get; set; } = "";

        /// <summary>
        /// The DeadLine attribute of the Update element
        /// </summary>
        public string DeadLine { get; set; } = "";

        /// <summary>
        /// The Channel attribute of the Update element
        /// </summary>
        public Channel? Channel { get; set; } = null;
        #endregion

        public XML.Updates GetElement()
        {
            if (Enabled != null)
            {
                var u = new XML.Updates();
                u.Enabled = Enabled.Value;
                u.UpdatePath = string.IsNullOrEmpty(UpdatePath) ? null : UpdatePath;
                u.TargetVersion = string.IsNullOrEmpty(TargetVersion) ? null : TargetVersion;
                u.DeadLine = string.IsNullOrEmpty(DeadLine) ? null : DeadLine;
                u.Channel = Channel == null ? null : Channel.Value.ToString();
                return u;
            }
            else
            {
                return null;
            }
        }
    }
    public class Add
    {
        #region Properties
        /// <summary>
        /// ForceUpgrade attribute in the Add element of the configuration
        /// </summary>
        public bool ForceUpgrade { get; set; }
        /// <summary>
        /// CDNFallback attribute in the Add element of the configuration
        /// </summary>
        public bool AllowCdnFallback { get; set; }
        /// <summary>
        /// OfficeClientEdition attribute in the Add element of the configuration
        /// </summary>
        public Architecture Architecture { get; set; } = Architecture.x86;
        /// <summary>
        /// Channel attribute in the Add element of the configuration
        /// </summary>
        public Channel Channel { get; set; } = Channel.Current;
        /// <summary>
        /// DownloadPath attribute in the Add element of the configuration
        /// </summary>
        public string DownloadPath { get; set; } = "";
        /// <summary>
        /// SourcePath attribute in the Add element of the configuration
        /// </summary>
        public string SourcePath { get; set; } = "";
        /// <summary>
        /// Channel attribute in the Add element of the configuration
        /// </summary>
        public string Version { get; set; } = "";
        /// <summary>
        /// All products of the configuration
        /// </summary>
        public ObservableCollection<OfficeProduct> Products { get; set; } = new();
        #endregion

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

        public (ErrorsList[], XML.Add) GetElementOrErrors()
        {
            var errors = new List<ErrorsList>();
            foreach (var item in Products)
            {
                var er = CheckProductErrors(item.Count).ToList();
                var l = new ErrorsList($"Office {item.Version} {item.DisplayName}");

                if ((item.ID == OfficeProductIDs.ProPlus2021Volume && Channel != Channel.PerpetualVL2021) || (item.ID == OfficeProductIDs.ProPlus2019Volume && Channel != Channel.PerpetualVL2019))
                {
                    er.Add(Errors.InvalidChannelVolume);
                }
                

                foreach (var e in er)
                {
                    l.Add(e);
                }
                if (er.Count() > 0)
                {
                    errors.Add(l);
                }

            }
            if (errors.Count() > 0)
            {
                return (errors.ToArray(), null);
            }
            else
            {
                var add = new XML.Add()
                {
                    AllowCdnFallback = AllowCdnFallback,
                    Channel = Channel.ToString(),
                    OfficeClientEdition = int.Parse(Architecture.ToRealArch()),
                    DownloadPath = string.IsNullOrEmpty(DownloadPath) ? null : DownloadPath,
                    SourcePath = string.IsNullOrEmpty(SourcePath) ? null : SourcePath,
                    Version = string.IsNullOrEmpty(Version) ? null : Version,
                    ForceUpgrade = ForceUpgrade,
                    Product = new List<XML.Product>()
                };


                foreach (var item in Products)
                {
                    var p = new XML.Product()
                    {
                        ID = item.ID.ToString(),
                        PIDKEY = item.PIDKEY,
                        Language = new List<XML.Language>(),
                        ExcludeApp = new List<XML.ExcludeApp>()
                    };
                    foreach (var lang in item.Languages)
                    {
                        p.Language.Add(new XML.Language() { ID = lang.Culture });
                    }
                    foreach (var app in item.ExcludeApps)
                    {
                        p.ExcludeApp.Add(new XML.ExcludeApp() { ID = app.ToString() });
                    }
                    add.Product.Add(p);
                }
                return (errors.ToArray(), add);
            }
        }
    }
    public class ErrorsList : List<Errors>
    {
        public string Title { get; set; } = "";
        public ErrorsList(string title)
        {
            Title = title;
        }
    }
    public class OfficeCore
    {
        #region Properties

        /// <summary>
        /// The main XML Class of the configuration
        /// </summary>
        public XML.Configuration ConfigurationXML { get; set; } = XML.Configuration.CreateNew();
        /// <summary>
        /// RemoveMSI element of the configuration
        /// </summary>
        public RemoveMSI RemoveMSI { get; } = new RemoveMSI(false, new List<RemoveMSIApps>(), false);
        /// <summary>
        /// Updates element of the configuration
        /// </summary>
        public Updates Updates { get; } = new();
        /// <summary>
        /// Add element of the configuration
        /// </summary>
        public Add Add { get; } = new();
        /// <summary>
        /// Display element of the configuration
        /// </summary>
        public Display Display { get; } = new Display(DisplayLevel.Full, false);
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

        public ErrorsList[] Compile()
        {
            var cfg = new XML.Configuration()
            {
                Updates = Updates.GetElement(),
                Property = PropertyElements.GetElements(),
                RemoveMSI = RemoveMSI.GetElement(),
                Display = Display.GetElement(),
                Info = string.IsNullOrEmpty(Description) ? null : new XML.Info() { Description = Description },
                AppSettings = string.IsNullOrEmpty(CompanyName) ? null : new XML.AppSettings() { Setup = new List<XML.Setup>() { new XML.Setup() { Name = "Company", Value = CompanyName } } }
            };

            var add = Add.GetElementOrErrors();
            if(add.Item1.Count() > 0 || add.Item2 == null)
            {
                return add.Item1;
            }
            else
            {
                cfg.Add = add.Item2;
                ConfigurationXML = cfg;
                return add.Item1;
            }
        }
        
        public string SerializeLastCompiled()
        {
            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(typeof(XML.Configuration));
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true,
            };

            using var stream = new StringWriter();
            using var writer = XmlWriter.Create(stream, settings);
            serializer.Serialize(writer, ConfigurationXML, emptyNamespaces);
            return stream.ToString();
        }

    }
}
