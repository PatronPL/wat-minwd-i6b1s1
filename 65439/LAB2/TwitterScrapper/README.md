Michał Kozłowski I6B1S1

W cmd:
1. pip install -r requirements.txt
2. python NazwaSkryptu.py "C:\Ścieżka\Do\Chromedriver.exe" "Argumenty dla metod"

Przykłady wywołań skryptów:
1. python GetUserInfo.py "C:\Users\ppmic\Desktop\chromedriver.exe" "Snowden"
2. python GetUserHashtags.py "C:\Users\ppmic\Desktop\chromedriver.exe" "realDonaldTrump" "2019-10-27" "2019-11-19" - tutaj jednymi z argumetow, oprócz nazwy użytkownika, są daty, między którymi będą przeszukiwane tweety pod kątem tagów.
3. python GetUserFollowers.py "C:\Users\ppmic\Desktop\chromedriver.exe" "Marvel" "login" "haslo" 50 - tutaj pojawiają się argumenty "login" i "hasło" - w które należy wpisać nazwę użytkownika/email oraz hasło do konta na Twitterze - powodem jest to, że bez zalogowania, nie da się przejrzeć followersów wybranego użytkownika. Ostatni argument - w tym przypadku 50 - to ilość użytkowników jaką skrypt ma zeskrapować ze strony /followers na twitterze.

Każdy skrypt zapisuje wyniki w formie pliku .csv w miejscu "przebywania" skryptu.