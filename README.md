Zadanie 7

lP1 – liczba zakupionych kilogramów paszy P1
lP2 – liczba zakupionych kilogramów paszy P2
lP3 – liczba zakupionych kilogramów paszy P3
lP4 – liczba zakupionych kilogramów paszy P4
A1 – zawartość składnika A w paszy P1
A2 – zawartość składnika A w paszy P2
A3 – zawartość składnika A w paszy P3
A4 – zawartość składnika A w paszy P4
B1 – zawartość składnika B w paszy P1
B2 – zawartość składnika B w paszy P2
B3 – zawartość składnika B w paszy P3
B4 – zawartość składnika B w paszy P4
MinA – minimalna ilość składnika A
MinB – minimalna ilość składnika B
kP1 – koszt jednego kilograma paszy P1
kP2 – koszt jednego kilograma paszy P2
kP3 – koszt jednego kilograma paszy P3
kP4 – koszt jednego kilograma paszy P4

kCal – koszt całkowity = lP1 * kP1 + lP2 * kP2 + lP3 * kP3 + lP4 * lP4
Acal – calkowita zawartość składnika A = lP1 * A1 + lP2 * A2 + lP3 * A3 + lP4 * A4
Bcal – calkowita zawartość składnika B = lP1 * B1 + lP2 * B2 + lP3 * B3 + lP4 * B4

a = < A1, A2, A3, A4, B1, B2, B3, B4, MinA, MinB, kP1, kP2, kP3, kP4>
A = {< A1, A2, A3, A4, B1, B2, B3, B4, MinA, MinB, kP1, kP2, kP3, kP4> ∈ N14  }
x = < lP1, lP2, lP3, lP4 >
Ω(a) = { < lP1, lP2, lP3, lP4 > ∈ N4 : Acal >= MinA, Bcal >= MinB }
w = kCal
W(a,x) = { kCal ∈ R: kCal = lP1 * kP1 + lP2 * kP2 + lP3 * kP3 + lP4 * lP4 }
W(a) = {kCal ∈ W(a,x) : x ∈ Ω(a)}
Ea(Z(x*)) = { 1 gdy Z(x*) = min(W(a)) = max(Z(x)) po omedze ; 0 w p.p.
Z(x) = f(a,x) = lP1 * kP1 + lP2 * kP2 + lP3 * kP3 + lP4 * lP4

Zadanie optymalizacyjne:
Dla danych a ∈ A
Wyznaczyć takie x* ∈ Ω(a)
Aby Ea(Z(x*)) =1

Odp:

Liczba zmiennych: 4
Liczba ograniczeń: 2
Rozwiązanie:
Wartość funkcji celu = 10800
Ilość paszy 1: 750
Ilość paszy 2: 250
Ilość paszy 3: 0
Ilość paszy 4: 0