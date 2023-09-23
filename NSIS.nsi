;NSIS Modern User Interface

;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"

;--------------------------------
;General

  ;Name and file
  Name "MixStore"
  OutFile "MixStore.exe"
  Unicode True

  ;Default installation folder
  InstallDir "$LOCALAPPDATA\MixStore"
  
  ;Get installation folder from registry if available
  ;InstallDirRegKey HKCU "Software\Modern UI Test" ""

  ;Request application privileges for Windows Vista
  RequestExecutionLevel user

;--------------------------------
;Interface Settings

  !define MUI_HEADERIMAGE
  !define MUI_HEADERIMAGE_BITMAP "MixApp.Client\header.bmp" ; optional
  !define MUI_ABORTWARNING

;--------------------------------
;Variables

  Var StartMenuFolder

;--------------------------------
;Pages

  ;!insertmacro MUI_PAGE_LICENSE "${NSISDIR}\Docs\Modern UI\License.txt"
  ;!insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  
  ;Start Menu Folder Page Configuration
  !define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKCU" 
  !define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\MixStore" 
  !define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "MixStore"
  
  !insertmacro MUI_PAGE_STARTMENU Application $StartMenuFolder
  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  
;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Installer Sections

Section "Dummy Section" SecDummy

  SetOutPath "$INSTDIR"
  
  ;ADD YOUR OWN FILES HERE...
  File /nonfatal /r "MixApp.Client\bin\Release\net8.0\win-x64\publish\*.*"
  
  ;Store installation folder
  ;WriteRegStr HKCU "Software\MixStore" "" $INSTDIR
  
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"
  
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\MixStore" "DisplayName" "MixStore"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\MixStore" "UninstallString" "$INSTDIR\Uninstall.exe"

  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    
    ;Create shortcuts
    CreateDirectory "$SMPROGRAMS\$StartMenuFolder\MixStore"
    CreateShortcut "$SMPROGRAMS\$StartMenuFolder\MixStore\MixStore.lnk" "$INSTDIR\MixStore.exe"
    CreateShortcut "$SMPROGRAMS\$StartMenuFolder\MixStore\Uninstall.lnk" "$INSTDIR\Uninstall.exe"
  
  !insertmacro MUI_STARTMENU_WRITE_END

SectionEnd

;--------------------------------
;Uninstaller Section

Section "Uninstall"

  ;ADD YOUR OWN FILES HERE...

  RMDir /r "$INSTDIR"
  
  !insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuFolder
  
  RMDir "$SMPROGRAMS\$StartMenuFolder\MixStore"
  ;Delete "$SMPROGRAMS\$StartMenuFolder\MixStore.lnk"
  ;Delete "$SMPROGRAMS\$StartMenuFolder\Uninstall.lnk"
  
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\MixStore"
  ;DeleteRegKey /ifempty HKCU "Software\MixStore"

SectionEnd
