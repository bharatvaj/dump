' GrantFullControlToUsers.vbs
Option Explicit

' File to modify permissions for
Const TARGET_FILE = "C:/Program Files/SKGA Automation/PendingAutomation.ods"

' Constants for security permissions
Const ADS_RIGHT_GENERIC_ALL = &H10000000 ' Full Control
Const ADS_ACETYPE_ACCESS_ALLOWED = 0
Const ADS_ACEFLAG_INHERIT_ACE = &H2
Const ADS_FLAG_OBJECT_TYPE_PRESENT = &H1

' Get the file's security descriptor
Dim objSD, objDACL, objACE, objSecurity, objUser
Set objSecurity = CreateObject("ADsSecurityUtility")
Set objSD = objSecurity.GetSecurityDescriptor("file:///" & TARGET_FILE)

If objSD Is Nothing Then
    WScript.Echo "Error: Could not access file security descriptor."
    WScript.Quit 1
End If

' Get the Discretionary ACL (DACL)
Set objDACL = objSD.DiscretionaryAcl

' Create an Access Control Entry (ACE) for "Users"
Set objUser = GetObject("WinNT://./Users,group")
Set objACE = CreateObject("AccessControlEntry")
objACE.Trustee = objUser.ADsPath
objACE.AccessMask = ADS_RIGHT_GENERIC_ALL
objACE.AceType = ADS_ACETYPE_ACCESS_ALLOWED
objACE.AceFlags = ADS_ACEFLAG_INHERIT_ACE

' Add the ACE to the DACL
objDACL.AddAce objACE

' Apply the modified DACL back to the file
objSD.DiscretionaryAcl = objDACL
objSecurity.SetSecurityDescriptor objSD

WScript.Echo "Successfully granted Full Control to 'Users' for: " & TARGET_FILE
