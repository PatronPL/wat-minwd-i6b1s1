Paweł Wałęga

# Instalacja

pip install -r requirements.txt w folderze z tym plikiem

python Lab1.py

# Treść 

Zadanie 8. Przedsiębiorstwo produkuje trzy wyroby. Do ich produkcji zużywa się m.in. dwa limitowane surowce. Zużycie tych surowców na jednostkę każdego z wyrobów, dopuszczalne limity zużycia oraz zyski jednostkowe ze sprzedaży podano w tabeli poniżej. Należy wyznaczyć takie ilości poszczególnych wyrobów, aby zysk był maksymalny.


I:

Dla W1 - 3/2	

Dla W2 - 3 

Dla W3 - 4

II:

Dla W1 - 3

Dla W2 - 2

Dla W3 - 1

Zysk jednostkowy [zł]:

Dla W1 - 12	

Dla W2 - 18

Dla W3 - 12

Limit zużycia surowca:

I - 1500 

II - 1200

Zbuduj model matematyczny i rozwiąż zadanie metodą geometryczną.

## Model

1,5W1 + 3W2 + 4W3 < 1500

3W1   + 2W2 + W3 < 1200

W1, W2, W3 - należą do liczb naturalnych

## Treść zadania optymalizacyjnego 

12W1 + 18W2 + 12W3 -> max

Dla danych W1, W2, W3 należących do liczb nautralnych

## Rozwiązanie

Status: Optimal

w1  =  100.0

w2  =  450.0

w3  =  0.0

objective =  9300.0
