numer albumu 65443
Kacper Lenkiewicz I6B1S1


Zadanie 2. Przedsiębiorstwo produkuje dwa wyroby. Do ich produkcji zużywa się m.in. dwa limitowane surowce. Zużycie tych surowców na jednostkę każdego z wyrobów, dopuszczalne limity zużycia oraz zyski jednostkowe ze sprzedaży podano w tabeli poniżej.

Wyroby Zużycie surowca na jednostkę Zysk jednostkowy [zł] I II

W1 8 7 2

W2 16 4 4

Limit zużycia surowca 96000 56000

1. Ile należy wyprodukować wyrobu W1, a ile W2, aby nie przekraczając limitów zmaksymalizować zysk ze sprzedaży wyrobów?

2. Jak zmieni się rozwiązanie, jeśli proces produkcyjny pozwala na wyprodukowanie maksymalnie 5000 szt. wyrobu W1, oraz maksymalnie 4000 szt. wyrobu W2?

Model :
1) 8W1 + 16W2 < 96000
    7W1 + 4W2 < 56000
    2W1 + 4W3 -> MAX

OPTIMAL 24000.0 @ { 0.0, 6000.0 }
############################################
0 <= A: 0 (2)
0 <= B: 6000 (4)
0 <= Ograniczenie 2: 24000.0 <= 56000
0 <= Zuzycie na jednostke I: 12000.0 <= 12000
############################################


2)  8W1 + 16W2 < 5000
        7W1 + 4W2 < 4000
        2W1 + 4W3 -> MAX

OPTIMAL 1250.0 @ { 1.0, 312.0 }
############################################
0 <= A: 1 (2)
0 <= B: 312 (4)
0 <= Ograniczenie 2: 1255.0 <= 4000
0 <= Zuzycie na jednostke I: 625.0 <= 625
############################################