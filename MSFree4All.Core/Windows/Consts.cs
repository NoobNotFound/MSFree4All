using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFree4All.Core.Windows
{
    public static class Consts
    {
        public const string DLiCheck = "@echo off" +
            "\ncscript //nologo %systemroot%\\System32\\slmgr.vbs /dli" +
            "\ncscript //nologo %systemroot%\\System32\\slmgr.vbs /xpr" +
            "\nstart /b \"\" cmd /c del \"%~f0\"&exit /b";
        public const string DLiGetDefaultKey = "@echo off" +
            "\necho { \"Message\": \"Starting script.\", \"Severity\": 0}" +
            "\nfor /F \"TOKENS=3 DELIMS=: \" %%A in ('DISM /English /Online /Get-CurrentEdition 2^>NUL ^| FIND /I \"Current Edition :\"') do set osedition=%%A\nif [%osedition%] == [Cloud] (\n\tset \"edition=Cloud\"\n\tset \"key=V3WVW-N2PV2-CGWC3-34QGF-VMJ2C\"\n\tset \"sku=178\"\n\tset \"editionId=X21-32983\"\n\tgoto :Insertkey\n)\nif [%osedition%] == [CloudN] (\n\tset \"edition=CloudN\"\n\tset \"key=NH9J3-68WK7-6FB93-4K3DF-DJ4F6\"\n\tset \"sku=179\"\n\tset \"editionId=X21-32987\"\n\tgoto :Insertkey\n)\nif [%osedition%] == [Core] (\n\tset \"edition=Core\"\n\tset \"key=YTMG3-N6DKC-DKB77-7M9GH-8HVX7\"\n\tset \"sku=101\"\n\tset \"editionId=X19-98868\"\n\tgoto :Insertkey\n)\nif [%osedition%] == [CoreCountrySpecific] (\n\tset \"edition=CoreCountrySpecific\"\n\tset \"key=N2434-X9D7W-8PF6X-8DV9T-8TYMD\"\n\tset \"sku=99\"\n\tset \"editionId=X19-99652\"\n\tgoto :Insertkey\n)\nif [%osedition%] == [CoreN] (\n\tset \"edition=CoreN\"\n\tset \"key=4CPRK-NM3K3-X6XXQ-RXX86-WXCHW\"\n\tset \"sku=98\"\n\tset \"editionId=X19-98877\"\n\tgoto :Insertkey\n)\nif [%osedition%] == [CoreSingleLanguage] (\n\tset \"edition=CoreSingleLanguage\"\n\tset \"key=BT79Q-G7N6G-PGBYW-4YWX6-6F4BT\"\n\tset \"sku=100\"\n\tset \"editionId=X19-99661\"\n\tgoto :Insertkey\n)\nif [%osedition%] == [Education] (\n\tset \"edition=Education\"\n\tset \"key=YNMGQ-8RYV3-4PGQ3-C8XTP-7CFBY\"\n\tset \"sku=121\"\n\tset \"editionId=X19-98886\"\n\tgoto :Insertkey\n)\nif [%osedition%] == [EducationN] (\n\tset \"edition=EducationN\"\n\tset \"key=84NGF-MHBT6-FXBX8-QWJK7-DRR8H\"\n\tset \"sku=122\"\n\tset \"editionId=X19-98892\"\n\tgoto :Insertkey\n)\nif [%osedition%] == [Enterprise] (\n\tset \"edition=Enterprise\"\n\tset \"key=XGVPP-NMH47-7TTHJ-W3FW7-8HV2C\"\n\tset \"sku=4\"\n\tset \"editionId=X19-99683\"\n\tgoto :Insertkey\n)\nif [%osedition%] == [EnterpriseN] (\n\tset \"edition=EnterpriseN\"\n        set \"key=3V6Q6-NQXCX-V8YXR-9QCYV-QPFCT\"\n\tset \"sku=27\"\n\tset \"editionId=X19-98746\"\n\tgoto :Insertkey\n)\nif [%osedition%] == [EnterpriseS] (\n\tset \"edition=EnterpriseS\"\n\tset \"key=NK96Y-D9CD8-W44CQ-R8YTK-DYJWX\"\n\tset \"sku=125\"\n\tset \"editionId=X21-05035\"\n\tgoto :Insertkey\n)\nif [%osedition%] == [EnterpriseSN] (\n\tset \"edition=EnterpriseSN\"\n\tset \"key=2DBW3-N2PJG-MVHW3-G7TDK-9HKR4\"\n\tset \"sku=126\"\n\tset \"editionId=X21-04921\"\n\tgoto :Insertkey\n)\nif [%osedition%] == [Professional] (\n\tset \"edition=Professional\"\n\tset \"key=VK7JG-NPHTM-C97JM-9MPGT-3V66T\"\n\tset \"sku=48\"\n\tset \"editionId=X19-98841\"\n\tgoto :Insertkey\n)\nif [%osedition%] == [ProfessionalEducation] (\n\tset \"edition=ProfessionalEducation\"\n\tset \"key=8PTT6-RNW4C-6V7J2-C2D3X-MHBPB\"\n\tset \"sku=164\"\n\tset \"editionId=X21-04955\"\n\tgoto :Insertkey\n)\nif [%osedition%] == [ProfessionalEducationN] (\n\tset \"edition=ProfessionalEducationN\"\n\tset \"key=GJTYN-HDMQY-FRR76-HVGC7-QPF8P\"\n\tset \"sku=165\"\n\tset \"editionId=X21-04956\"\n\tgoto :Insertkey\n)\nif [%osedition%] == [ProfessionalN] (\n\tset \"edition=ProfessionalN\"\n\tset \"key=2B87N-8KFHP-DKV6R-Y2C8J-PKCKT\"\n\tset \"sku=49\"\n\tset \"editionId=X19-98859\"\n\tgoto :Insertkey\n)\nif [%osedition%] == [ProfessionalWorkstation] (\n\tset \"edition=ProfessionalWorkstation\"\n\tset \"key=DXG7C-N36C4-C4HTG-X4T3X-2YV77\"\n\tset \"sku=161\"\n\tset \"editionId=X21-43626\"\n\tgoto :Insertkey\n)\nif [%osedition%] == [ProfessionalWorkstationN] (\n\tset \"edition=ProfessionalWorkstationN\"\n\tset \"key=WYPNQ-8C467-V2W6J-TX4WX-WT2RQ\"\n\tset \"sku=162\"\n\tset \"editionId=X21-43644\"\n\tgoto :Insertkey\n)\nif [%osedition%] == [ServerRdsh] (\n\tset \"edition=ServerRdsh\"\n\tset \"key=NJCF7-PW8QT-3324D-688JX-2YV66\"\n\tset \"sku=175\"\n\tset \"editionId=X21-41295\"\n\tgoto :Insertkey\n)" +
            "\n:Insertkey" +
            "\necho { \"Message\": \"Edition: %osedition%\\nKey: %key%\", \"Severity\": 1, \"PopUp\": true, \"PopUpTitle\": \"Windows Digital license key\"}" +
            "\nstart /b \"\" cmd /c del \"%~f0\"";
        public const string ActKMS = "@echo off" +
            "\necho { \"Message\": \"Starting script.\", \"Severity\": 0}" +
            "\necho { \"Message\": \"Trying to install keys.\", \"Severity\": 0}" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk FJ82H-XT6CR-J8D7P-XQJJ2-GPDD4 >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk MRPKT-YTG23-K7D7T-X2JMM-QY7MG >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk W82YF-2Q76Y-63HXB-FGJG9-GF7QX >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk 33PXH-7Y6KF-2VJC9-XBBR8-HVTHH >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk YDRBP-3D83W-TY26F-D46B2-XCKRJ >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk C29WB-22CC8-VJ326-GHFJW-H9DH4 >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk BN3D2-R7TKB-3YPBD-8DRP2-27GG4 >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk 2WN2H-YGCQR-KFX6K-CD6TF-84YXQ >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk NG4HW-VH26C-733KW-K6F98-J8CK4 >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk XCVCF-2NXM9-723PB-MHCB7-2RYQQ >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk GNBB8-YVD74-QJHX6-27H4K-8QHDG >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk 32JNW-9KQ84-P47T8-D8GGY-CWCK7 >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk JMNMF-RHW7P-DMY6X-RF3DR-X2BQT >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk M9Q9P-WNJJT-6PXPY-DWX8H-6XWKK >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk 7B9N3-D94CG-YTVHR-QBPX3-RJP64 >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk BB6NG-PQ82V-VRDPW-8XVD2-V8P66 >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk GCRJD-8NW9H-F2CDX-CCM8D-9D6T9 >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk HMCNV-VVBFX-7HMBH-CTY9B-B4FXY >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk 789NJ-TQK6T-6XTH8-J39CJ-J8D3P >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk TX9XD-98N7V-6WMQ6-BX7FG-H8Q99 >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk 3KHY7-WNT83-DGQKR-F7HPR-844BM >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk 7HNRX-D7KGG-3K4RQ-4WPJ4-YTDFH >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk PVMJN-6DFY6-9CCP6-7BKTT-D3WVR >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk W269N-WFGWX-YVC9B-4J6C9-T83GX >nul" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ipk MH37W-N47XK-V7XM9-C7227-GCQG9 >nul" +
            "\nset i=1" +
            "\n:server" +
            "\nif %i%==1 set KMS=kms7.MSGuides.com" +
            "\nif %i%==2 set KMS=s8.uk.to" +
            "\nif %i%==3 set KMS=s9.us.to" +
            "\nif %i%==4 goto notsupported" +
            "\necho { \"Message\": \"Trying to set KMS server %i%\", \"Severity\": 0}" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /skms %KMS% >nul" +
            "\necho { \"Message\": \"Trying to activate\", \"Severity\": 0}" +
            "\ncscript //nologo c:\\windows\\system32\\slmgr.vbs /ato | find /i \"successfully\" && (goto success) || ( echo { \"Message\": \"The connection to the KMS server %i% failed!\", \"Severity\": 2} & set /a i+=1 & goto server)" +
            "\n:notsupported" +
            "\necho { \"Message\": \"Activation Failed (Connecting to KMS servers Failed).\", \"Severity\": 3, \"PopUp\": true, \"PopUpTitle\": \"Windows activator (KMS)\"}" +
            "\ngoto halt" +
            "\n:success" +
            "\necho { \"Message\": \"Activation Done.\", \"Severity\": 1, \"PopUp\": true, \"PopUpTitle\": \"Windows activator (KMS)\"}" +
            "\n:halt" +
            "\necho { \"Message\": \"exit.\", \"Severity\": 0}" +
            "\nstart /b \"\" cmd /c del \"%~f0\"&exit /b";
    }
}
