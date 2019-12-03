Zad 10
Damian Żyłka I6B1S1
Python biblioteka PuLp

Treść:

Jednostkowe nakłady środków na produkcje wyrobów przedstawiono w tabeli. Znając zysk ze
sprzedaży jednostki każdego z wyrobów (ostatni wiersz) wyznaczyć optymalne z punktu widzenia
zysków rozmiary produkcji.

|      | 1  | 2  | 3  | 4  | Środki produkcja |
|---   |--- |--- |--- |--- |---               |
| A    | 15 | 10 | 20 | 19 | 26000            |
| B    | 9  | 3  | 5  | 10 | 100000           |
| Zysk | 6  | 3  | 5  | 2  |                  |

Zbuduj model matematyczny i rozwiąż zadanie metodą geometryczną.

Opis zmiennych:

x1 - zbiór produktów nr 1
x2 - zbiór produktów nr 2
x3 - zbiór produktów nr 3
x4 - zbiór produktów nr 4

Funkcja celu:

6x1 + 3x2 + 5x3 + 2x4 -> MAX

Ograniczenia:

A: 15x1 + 10x2 + 20x3 + 19x4 <= 26000
B: 9x1 + 3x2 + 5x3 + 10x4 <= 100000

Rozwiązanie:

x1 = 1733
x2 = 0
x3 = 0
x4 = 0

Cel = 10398

Instalacja:
W folderze uruchomić w terminal następnie wpisać następujące komendy: 
1) pip install -r requirements.txt
2) python Code.py

