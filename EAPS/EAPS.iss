[Setup]
AppName=EAPS
AppVersion=1
WizardStyle=modern
DefaultDirName={autopf}\EAPS
DefaultGroupName=EAPS
SourceDir=C:\Projects\EAPS\EAPS\bin\Release\net6.0-windows
OutputDir=C:\Projects\EAPS\EAPS\Output
OutputBaseFilename=EAPSSetup

[Files]
Source: "*.*"; DestDir: "{app}"; Flags: recursesubdirs ignoreversion

[Icons]
Name: "{group}\EAPS"; Filename: "{app}\EAPS.exe"
Name: "{commondesktop}\EAPS" ; Filename: "{app}\EAPS.exe"

[Code]

procedure InitializeWizard;
 Begin
 DelTree(ExpandConstant('{autopf}') + '\EAPS\Data', True, True, True) ;
 
End;