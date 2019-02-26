@echo off

nuget "install" "xunit.runner.console" "-OutputDirectory" "tools" "-ExcludeVersion"
nuget "install" "FAKE.Core" "-OutputDirectory" "tools" "-ExcludeVersion"
nuget restore
cd ShootingGallery

:Build