del /s /q "..\Assets\Res\Config\Conf*.json"
del /s /q "..\Assets\Scripts\Game\Config\Conf*.cs"

..\Tools\excel-translator\ExcelTranslator.exe -e "../Config" -j "../Assets/Res/Config" -c "../Assets/Scripts/Game/Config" -C "C" -E "E" -n "Game.Config"

pause