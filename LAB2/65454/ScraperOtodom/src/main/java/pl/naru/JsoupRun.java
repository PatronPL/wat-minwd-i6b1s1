package pl.naru;

import org.jsoup.Connection;
import org.jsoup.Jsoup;
import org.jsoup.nodes.Document;
import org.jsoup.nodes.Element;
import org.jsoup.select.Elements;

import java.io.IOException;
import java.sql.DataTruncation;
import java.util.ArrayList;
import java.util.Scanner;

public class JsoupRun {
    static Scanner scanner = new Scanner(System.in);

    public static void main(String[] args) throws IOException {
        String sellOrRent;
        String houseOrAccommodation;
        String cityName;
        int page;
        System.out.println("Hello, would you like to sell or rent(sprzedaz/wynajem)?");
        sellOrRent = scanner.nextLine().toLowerCase();
        System.out.println("Okay and say me, would You like to live in a house or accommodation(dom/mieszkanie)?");
        houseOrAccommodation = scanner.nextLine().toLowerCase();
        System.out.println("almost done! The last thing is the city you want to live in(nazwa miasta): ");
        cityName = scanner.nextLine().toLowerCase();
        StringBuilder sb = new StringBuilder("https://www.otodom.pl/");
        sb.append(sellOrRent).append("/");
        sb.append(houseOrAccommodation).append("/");
        sb.append(cityName).append("/?search&nrAdsPerPage=72&page=");


        Connection connection = Jsoup.connect(sb.toString()+"1");
        Elements links; //= connection.get().getElementsByTag("article");
        String offersCount = connection.get().getElementsByClass("offers-index pull-left text-nowrap").text();
        ArrayList<String> linkToTheOffer = new ArrayList<>();

        int x = 0;
        ArrayList<String> offerLink = new ArrayList<>();

        //System.out.println(links.get(0));
        int tmpFor = calculateTotalPages(offersCount);

        for (int j = 1; j <= tmpFor; j++) {
            System.out.println("-------------------------loading page...-------------------------");
            connection = Jsoup.connect(sb.toString()+ j).timeout(15000);
            System.out.println(sb.toString()+j);
            links = connection.get().getElementsByTag("article");
            System.out.println("-------------------------PAGE " + j + "-------------------------");
            for (Element link : links) {
                x++;
                System.out.println("image: " + link.getElementsByClass("img-cover lazy").tagName("background-image").attr("data-src"));
                if (link.attr("data-featured-name").contains("listing_no_promo")) {
                    System.out.println("link to a website: " + link.getElementsByTag("a").attr("href"));
                    linkToTheOffer.add(link.getElementsByTag("a").attr("href"));

                    //System.out.println(link.getElementsByClass("params").tagName("li").text());
                    for (Element e : link.getElementsByClass("params")) {

                        for (int i = 0; i <= e.getElementsByTag("li").size() - 1; i++) {
                            System.out.println(e.getElementsByTag("li").get(i).text());
                        }

                    }

                }

            }

        }
        System.out.println(x);





    }

    static String totalPagesLongerWay(Connection connection) throws IOException {
        System.out.println("please wait calculating total pages. ETA strongly depends from your network connection speed");

        String total = null;
        Elements elements;
        String tmp = null;


        do {
            elements = connection.get().head().getElementsByAttributeValue("rel", "next");
            if (!elements.isEmpty()) {
                tmp = elements.get(0).attr("href");
                total = tmp.substring(tmp.lastIndexOf("=") + 1);
                connection = Jsoup.connect(tmp);
            }
            System.out.println("still calculating..." + total);
        }
        while (tmp != null);

        System.out.println("done!");
        return total;
    }

    public static int calculateTotalPages(String offersCount) {
        int totalPages = (int) Math.ceil(Double.parseDouble(offersCount.substring(offersCount.lastIndexOf(":") + 1).replace(" ", "")) / 72);
        return totalPages;
    }
}
