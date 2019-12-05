package main

import (
    "os"
	"fmt"
    "log"
	"bufio"
    "net/http"
	"strings"
    "github.com/PuerkitoBio/goquery"
	"strconv"
    "encoding/json"
	"github.com/polds/imgbase64"
)

var elements = []Offer{}
var offersList = Offers{elements}
var counter int =0

type Offers struct {
        Elements []Offer `json:"offers"`
}

type Offer struct {
		Title string `json:"title"`
		Link string `json:"link"`
		Year string `json:"year"`
		Mileage string `json:"mileage"`
		Engine_capacity string `json:"engine_capacity"`
		Fuel_type string `json:"fuel_type"`
		City string `json:"city"`
		Price string `json:"price"`
		Img string `json:"img"`
}

func processElement(index int, element *goquery.Selection) {

	_title, v2:= element.Find("a").Attr("title")

	_link, v1 := element.Attr("data-href")

	details := element.Find("div").Find("ul").Find("li").Find("span").Contents().Text()

	_year := details[0:4]

	_mileage := "Brak podanego przebiegu w ofercie"
	if len(strings.SplitAfter(details[5:len(details)], "km")) > 1{
		_mileage = strings.SplitAfter(details[5:len(details)], "km")[0]
	}

	_engine_capacity := "Brak podanej pojemnosci silnika w ofercie"
	if len(strings.SplitAfter(details, "cm3")) > 1{
		_engine_capacity = strings.SplitAfter(details, "cm3")[0][len(strings.SplitAfter(details, "cm3")[0])-9:len(strings.SplitAfter(details, "cm3")[0])] 
	}

	
	_fuel_type := "Brak podanego paliwa w ofercie"
	if len(strings.SplitAfter(details, "Benzyna")) < 2{
		_fuel_type = strings.SplitAfter(details, "Diesel")[0][len(strings.SplitAfter(details, "Diesel")[0])-6:len(strings.SplitAfter(details, "Diesel")[0])]
	}else{
		_fuel_type = strings.SplitAfter(details, "Benzyna")[0][len(strings.SplitAfter(details, "Benzyna")[0])-7:len(strings.SplitAfter(details, "Benzyna")[0])] + strings.SplitAfter(details, "Benzyna")[1]
	}
	
	_city := element.Find("div").Find("h4").Find("span").Contents().Text()
	
	_price := element.Find("div").Find("div").Find("span").Find("span").Contents().Text()
	
	_img, v3 := element.Find("div").Find("a").Find("img").Attr("data-src")
	

	if v1 && v2 && v3{
		element := Offer{_title, _link, _year, _mileage, _engine_capacity, _fuel_type, _city, _price, strings.SplitAfter(imgbase64.FromRemote(_img), ",")[1]}	
		offersList.Elements = append(offersList.Elements, element)
		counter++
		fmt.Printf("Pobieranie ofert, aktualna liczba: %d\n", counter)
	}
}

func scanner() string {
  scanner := bufio.NewScanner(os.Stdin)
  scanner.Scan()
  return scanner.Text()
}

func main() {
	fmt.Print("Marka: ")
	brand := scanner()
	fmt.Print("Model: ")
	model := scanner()
	fmt.Print("Cena(zł) od: ")
	minPrice := scanner()
	fmt.Print("Cena(zł) do: ")
	maxPrice := scanner()
	fmt.Print("Rok produkcji od: ")
	minYear := scanner()
	fmt.Print("Rok produkcji do: ")
	maxYear := scanner()
	fmt.Print("Przebieg(km) od: ")
	minMileage := scanner()
	fmt.Print("Przebieg(km) do: ")
	maxMileage := scanner()
	fmt.Print("Lokalizacja(miasto): ")
	city := scanner()
	
	url := "https://www.otomoto.pl/osobowe/" + brand + "/" + model + "/od-" + minYear + "/" + city + "/?search%5Bfilter_float_price%3Afrom%5D=" + minPrice + "&search%5Bfilter_float_price%3Ato%5D=" + maxPrice + "&search%5Bfilter_float_year%3Ato%5D=" + maxYear + "&search%5Bfilter_float_mileage%3Afrom%5D=" + minMileage + "&search%5Bfilter_float_mileage%3Ato%5D=" + maxMileage + "&search%5Border%5D=created_at%3Adesc&search%5Bbrand_program_id%5D%5B0%5D=&search%5Bcountry%5D=&page="

	page := 1
	oldpage := 1
	
	for{
		urlpage := url + strconv.Itoa(page)
		response, err := http.Get(urlpage)
		if err != nil {
			log.Fatal(err)
		}
		defer response.Body.Close()

		document, err := goquery.NewDocumentFromReader(response.Body)
		if err != nil {
			log.Fatal("Error loading HTTP response body. ", err)
		}
	
		document.Find("div").Find("div").Find("section").Find("div").Find("div").Find("div").Find("div").Find("div").Find("article").Each(processElement)
		document.Find("div").Find("div").Find("section").Find("div").Find("div").Find("ul").Find("li").Each(func(index int, element *goquery.Selection) {
				next, v1 := element.Attr("class")
				if v1{
					if next == "next abs"{
						page++
					}		
				}
		})
		if (page == oldpage){
			break
		}else{
			oldpage++
		}
	}

	offersToJson := Offers{
        Elements: offersList.Elements,
    }

	jsonData, _ := json.MarshalIndent(&offersToJson, "", "  ")
	
	jsonFile, err := os.Create("otomoto.json")

	if err != nil {
		panic(err)
	}
	defer jsonFile.Close()

	jsonFile.Write(jsonData)
	jsonFile.Close()
	
	fmt.Println("Koniec dzialania programu, wyniki znajduja sie w pliku otomoto.json")

}

	