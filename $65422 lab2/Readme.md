# Opis programu
Skrypt zawierający scrapper, który pobiera dane ze strony otodom.pl

# Autor: Michał Bryła

# Wymagania
Python 3.7

# Instrukcja uruchomieniowa
Uruchomić konsolę w folderze, który zawiera plik Scrapper.py
Instalacja bibliotek: pip install -r requirements.txt
Aby uruchomić skrypt należy wpisać py Scrapper.py param1 param2 param3 param4 param5 param6 param7 param8 param9 gdzie:

param 1: typ budynku (przyjmuje wartości dom/mieszkanie)
param 2: rodzaj oferty (przyjmuje wartości sprzedaz/wynajem)
param 3: cena minimalna (wartość int)
param 4: cena maksymalna (wartość int)
param 5: minimalna powierzchnia domu/mieszkania (wartość int)
param 6: maksymalna powierzchnia domu/mieszkania (wartość int)
param 7: ilość pokoi (wartość int)
param 8: minimalny rok budowy (wartość int)
param 9: maksymalny rok budowy (wartość int)

# Wynik zapisany zostaje w pliku flats.json w postaci:
"Title": - tytuł ogłoszenia
"Location": - lokalizacja
"Price": - cena
"Price/m2": - cena za metr
"Rooms": - liczba pokoi
"Area": - powierzchnia
"Image":  - obraz w formie Base64
