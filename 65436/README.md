Zadanie 7

LP1 - liczba kilogramów paszy P1
LP2 - liczba kilogramów paszy P2
LP3 - liczba kilogramów paszy P3
LP4 - liczba kilogramów paszy P4

A1 - zawartość składnika A w paszy P1
A2 - zawartość składnika A w paszy P2
A3 - zawartość składnika A w paszy P3
A4 - zawartość składnika A w paszy P4

B1 - zawartość składnika B w paszy P1
B2 - zawartość składnika B w paszy P2
B3 - zawartość składnika B w paszy P3
B4 - zawartość składnika B w paszy P4

MINA - Minimalna zawartość składnika A
MINB - Minimalna zawartość składnika B

KP1 - koszt 1 kilograma paszy P1
KP2 - koszt 1 kilograma paszy P2
KP3 - koszt 1 kilograma paszy P3
KP4 - koszt 1 kilograma paszy P4

Koszt całkowity zakupu - KCAL = LP1 * KP1 + LP2 * KP2 + LP3 * KP3 + LP4 * KP4
Całkowita zawartość składnika A - ACAL = LP1 * A1 + LP2 * A2 + LP3 * A3 + LP4 * A4
Całkowita zawartość składnika B - BCAL = LP1 * B1 + LP2 * B2 + LP3 * B3 + LP4 * B4

Model:
9.6 * LP1 + 14.4 * LP2 + 10.8 * LP3 + 7.2 * LP4 -> MIN
LP1 * 0.8 + LP2 * 2.4 + LP3 * 0.9 + LP4 * 0.4 >= 1200
LP1 * 0.6 + LP2 * 0.6 + LP3 * 0.3 + LP4 * 0.3 >= 600

Rozwiązanie optymalne:
750 kg paszy P1 i 250 kg paszy P2.


Jak odpalić (linux):
- instalujemy pipa: sudo apt-get install pip,
- odpalamy plik requirements.txt żeby zaciągnąć pulpa: pip install -r requirements.txt
- odpalamy skrypcik python bydlo.py
