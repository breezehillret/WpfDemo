
$dllDirectory = "C:\Tritech\Dll"
$ocxDirectory = "C:\TriTech\Ocx\"
$binDirectory = "C:\TriTech\Visicad\Bin\"

function KillCad()
{
    Write-Host "Stopping any running CAD processes."
    $processes = @(
        "VisiCad.exe",
        "Roster.exe",
        "CommonFunction.exe",
        "NetworkManager.exe",
        "MsgSwitchOleSrvr.exe",
        "IOMDDE.exe",
        "TSSIntMessagePipe.exe",
        "TSSIntNotificationUti.exe",
        "MailRoom.exe",
        "MessageProcessor.exe",
        "TSSIntRecordsCheck.exe"
    )

    foreach($process in $processes)
    {
        $processData = (Get-Process -Name $process -ErrorAction "SilentlyContinue")
        if($null -ne $processData)
        {
            Write-Host "Stopping $process"
            Stop-Process -Name $process -Force
        }        
    }

    Write-Host "All CAD processes are stopped."
}

function Show-Dialog
{
    param([string] $Message)
    $Shell = New-Object -ComObject "WScript.Shell"
    $Button = $Shell.Popup($Message, 0, "Build Paused", 0)
}

function Build-NetProject
{
    param([string] $Solution, [string] $Configuration = "Debug")

    & "$($msbuild)" "$($Solution)" /t:Rebuild /p:Configuration=$Configuration
}

function Build-VBProject
{
    param([string] $Solution, [string] $OutputDirectory)

    & "$($vb6)" /make "$($Solution)" /outdir "$($OutputDirectory)"

}

KillCad


## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Dll\CADManagerRCW\CADManagerRCW.vbp" -OutputDirectory $dllDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Dll\AppManagerRCW\AppManagerRCW.vbp" -OutputDirectory $dllDirectory

## Build-NetProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Common\Build Shared Library.sln" -Configuration "Debug-Server"
Build-NetProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Data\Build Data Layer.sln"  -Configuration "Debug-Server"
## Build-NetProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Common\Build Shared Library.sln" 
## Build-NetProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Data\Build Data Layer.sln" 
Build-NetProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Business\Build Business Layer.sln"  
## Build-NetProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\App\Build Launch.sln"
## Build-NetProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\App\VisiNetMap\VisiNetMap.sln"
## Build-NetProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\App\Build Wrapper API.sln"
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Dll\MSOS\MSOS.vbp" -OutputDirectory $dllDirectory

## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Ocx\keyhook\keyhook.vbp" -OutputDirectory $ocxDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Dll\CADManagerRCW\CADManagerRCW.vbp" -OutputDirectory $dllDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Dll\AppManagerRCW\AppManagerRCW.vbp" -OutputDirectory $dllDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Dll\CCWFactory\CCWFactory.vbp" -OutputDirectory $dllDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Dll\cautionnoteAPI\CautionNote.vbp" -OutputDirectory $dllDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Dll\SFZipCodeLookup\ZipCodeLookup.vbp" -OutputDirectory $dllDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Dll\VCPeople\VCPeople.vbp" -OutputDirectory $dllDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Dll\shiftcontroller\shiftcontroller.vbp" -OutputDirectory $dllDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Dll\shifteditor\shifteditor.vbp" -OutputDirectory $dllDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\CTConfiguratorUtility\caltaks.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\grid builder\gridbuilder.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\ihsetup\ihsetup.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\gisutil\gisutil.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\prescheduledcalltaking\preschedule.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\polygon lookup manager\polygonlookupmanager.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\incIDE\nteditor\editmgr.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\gis\gis.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\Convert_Form_IH\langcnvt.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\Convert_Text_IH\textcnvt.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\Convert_Manager_IH\cnvtmgr.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\Convert_Message_IH\msgconv.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\streetfinder setup utility\streetfindersetup.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\Reverification Utility\PremiseReVerify.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\OffLineIncIDE\ntEntry\IncIDE\ntEntry.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\rosterremote\roster_remote.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\rostersystem\roster.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\mapbook builder\mapbook.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\interagencycommentsharing\interagencycomment.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\sound manager\soundmgr.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\Flat File Maker\flatfilemaker.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\time reports\timereports.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\text_conversion_manager\textcvtmgr.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\explorer setup utility\explorersetuputility.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\StandardInterface\Classic\Record Check\App\TSSIntRecordsCheck\TSSIntRecordsCheck.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\VisiNETTDD\VisiNETTDD.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Bin\visicad\visicad.vbp" -OutputDirectory $binDirectory
## Build-VBProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\Classic\Dll\MSOS\MSOS.vbp" -OutputDirectory $dllDirectory

## Show-Dialog -Message "Open VB6 manually and compile CommonFunction.vbp.  This is to preserve binary compatibility.  Press OK once compilation is complete to resume build."

##Build-NetProject -Solution "C:\ADO\PSJ-Ent-CADMOB\Dev-Cic\src\CAD\App\Build Application Layer.sln"

Read-Host -Prompt "Press Enter to exit"