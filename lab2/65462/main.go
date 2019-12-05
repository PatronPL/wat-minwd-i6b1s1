package main

import (
	"bufio"
	"context"
	"encoding/base64"
	"encoding/json"
	"fmt"
	"github.com/PuerkitoBio/goquery"
	"github.com/chromedp/chromedp"
	"io/ioutil"
	"net/http"
	"os"
	"strings"
)

type Word struct {
	PartOfSpeech string
	Meaning []string
}

type Result struct {
	Meanings []Word
	Image string
}

func main() {
	siteUrl := "https://pl.wiktionary.org"
	reader := bufio.NewScanner(os.Stdin)
	result := Result{}
	word := Word{}
	ctx, cancel := chromedp.NewContext(context.Background())
	var response string
	done := true
	url := "https://pl.wiktionary.org/wiki/be"
	wordToFind := "be"
	defer cancel()
	for {
		if done {
			print("Wprowadź słowo lub zakończ wpisując q\n")
			reader.Scan()
			wordToFind = reader.Text()
			if wordToFind == "q" {
				os.Exit(0)
			}
			url = siteUrl + "/wiki/" + wordToFind
		}
		if err := chromedp.Run(ctx,
			chromedp.Navigate(url),
			chromedp.OuterHTML("#content", &response),
		); err != nil {
			fmt.Printf("Error: %s\n Trying again. \n", err)
			done = false
			continue
		}
		done = true
		document, err := goquery.NewDocumentFromReader(strings.NewReader(response))
		if err != nil {
			fmt.Printf("Error loading HTTP response body. Trying again. %s\n", err)
			continue
		}

		document.Find("p.lang-en.fldt-znaczenia").Each(func(i int, s *goquery.Selection) {
			ns := s.Next()
			w := s.Text()
			word.PartOfSpeech = w[0 : len(w)-1]
			ns.Find("dd").Each(func(j int, se *goquery.Selection) {
				word.Meaning = append(word.Meaning, se.Text())
			})
			result.Meanings = append(result.Meanings, word)
			word = Word{}
		})

		document.Find("img.thumbimage.lang-en").Each(func(i int, selection *goquery.Selection) {
			imageUrl, e := selection.Attr("src")
			if e {
				imageUrl = "https:" + imageUrl
				client := http.Client{
					CheckRedirect: func(r *http.Request, via []*http.Request) error {
						r.URL.Opaque = r.URL.Path
						return nil
					},
				}
				f, _ := client.Get(imageUrl)
				reader := bufio.NewReader(f.Body)
				content, _ := ioutil.ReadAll(reader)
				encoded := base64.StdEncoding.EncodeToString(content)
				result.Image = encoded
			}
		})

		file, err := json.MarshalIndent(result,"","  ")
		if err != nil {
			fmt.Println(err)
			continue
		}
		filename := wordToFind + ".json"
		_ = ioutil.WriteFile(filename,file,0644)
		fmt.Printf("Plik %s został stworzony\n",filename)
		result = Result{}
	}
}
