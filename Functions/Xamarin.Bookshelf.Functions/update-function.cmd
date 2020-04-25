@echo off
echo Stoping function %2
az functionapp stop -g %1 -n %2
echo Function %2 stoped
echo Publishing function %2
func azure functionapp publish %2
echo Function %2 published
echo Starting function %2
az functionapp start -g %1 -n %2
echo Function %2 started