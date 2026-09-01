# Cheat Sheet

## Hard Links & Junctions (CMD)

mklink /h "%APPDATA%\Microsoft\UserSecrets\poc-dentity-blazor\secrets.json" "%USERPROFILE%\OneDrive\Dev\UserSecrets\poc-dentity-blazor\secrets.json"

mklink /J "%APPDATA%\Microsoft\UserSecrets" "%USERPROFILE%\OneDrive\Dev\UserSecrets"

