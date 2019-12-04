Michał Bryła I6B1S1
Treść zadania:
Zadanie 2. Przedsiębiorstwo produkuje dwa wyroby.
Do ich produkcji zużywa się m.in. dwa limitowane surowce.
Zużycie tych surowców na jednostkę każdego z wyrobów, dopuszczalne limity zużycia oraz zyski jednostkowe ze sprzedaży podano w tabeli poniżej. 

Wyroby	Zużycie surowca na jednostkę	Zysk jednostkowy [zł]
			I	II	
W1			8	7		2
W2			16	4		4
Limit zużycia surowca	96000	56000	

1.	Ile należy wyprodukować wyrobu W1, a ile W2, aby nie przekraczając limitów zmaksymalizować zysk ze sprzedaży wyrobów?
2.	Jak zmieni się rozwiązanie, jeśli proces produkcyjny pozwala na wyprodukowanie maksymalnie 5000 szt. wyrobu W1, oraz maksymalnie 4000 szt. wyrobu W2?
Zbuduj model matematyczny i rozwiąż zadanie metodą geometryczną.


MODEL:
2x + 4y -> max
x + 2y <= 12000
7x + 4y <= 56000
x >= 0
y >= 0

Dla pkt drugiego dodałem ograniczenia:
x <= 5000
y <= 4000


Wynik dla pkt 1:
Liczba zmiennych = 2
Liczba ograniczeń = 2
Rozwiązanie:
Wartość funkcji celu = 24000
Ilość wyrobu pierwszego = 0
Ilość wyrobu drugiego = 6000

wynik dla pkt 2:
Liczba zmiennych = 2
Liczba ograniczeń = 2
Rozwiązanie:
Wartość funkcji celu = 24000
Ilość wyrobu pierwszego = 4000
Ilość wyrobu drugiego = 4000