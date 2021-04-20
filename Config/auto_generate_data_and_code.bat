del /s /q "..\Assets\Res\Config\*.json"
del /s /q "..\Assets\Scripts\Game\Config\*.cs"

..\Tools\excel-translator\ExcelTranslator.exe -e "../Config" -j "../Assets/Res/Config" -c "../Assets/Scripts/Game/Config" -p "Conf" -n "Game.Config"

pause