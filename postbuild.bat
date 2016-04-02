set Configuration=%1
set TargetPath=%2
set ReleaseDir=%3
set ProjectPath=%4

if %Configuration%==Release (
	echo Copying file from %TargetPath% to %ReleaseDir%
	xcopy %TargetPath% %ReleaseDir% /i /d /y
)
if %Configuration%==Publish (
	echo Generating package from %ProjectPath% to %NugetPackageSourceLocalPath%
	NuGet.exe pack %ProjectPath% -OutputDirectory "%NugetPackageSourceLocalPath%" -Prop Configuration=Release
)