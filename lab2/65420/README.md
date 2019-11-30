#Opis

Scraper pobierający i zapisujący dane ogłoszeń samochodów osobowych z serwisu otomoto.pl

#Wymagania 

Język GO, który można pobrać ze strony https://golang.org/doc/install
Oraz biblioteki które można pobrać w następujący sposób:
Należy uruchomić konsole systemową i wpisać poniższe polecania:
	- go get github.com/PuerkitoBio/goquery
	- go get github.com/polds/imgbase64

# Instrukcja

1. Po pobraniu należy wejść do katalogu z plikami
2. Otworzyć konsole systemową z ścieżką do tego katalogu
3. W celu uruchomienia wpisać do konsoli polecenie: go run otomoto.go
4. Po uruchomieniu należy wpisać parametry samochodu wymagane przez program: 
	- markę
	- model
	- cena minimalna
	- cena maksymalna
	- rok rozpoczęcia produkcji
	- rok zakończenia produkcji
	- przebieg maksymalny
	- przebieg minimalny
	- lokalizacje, czyli miasto
5. Po zakończeniu działania programu wyniki można zobaczyć po otworzeniu pliku otomoto.json, który znajduję się w tym samym katalogu
# Plik otomoto.json

Legenda
      "title": <- nazwa ogłoszenia
      "link":  <- link do ogłoszenia
      "year": <- rok produkcji 
      "mileage": <- przebieg
      "engine_capacity": <- pojemność silnika
      "fuel_type": " <- rodzaj paliwa
      "city": <- miasto (z województwem)
      "price": <- cena
      "img": <- zdjęcie zapisane w formacie base64
