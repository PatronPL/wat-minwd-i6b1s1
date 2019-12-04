package pl.naru;

import org.json.JSONArray;
import org.json.JSONObject;
import org.jsoup.Connection;
import org.jsoup.Jsoup;
import org.jsoup.nodes.Element;
import org.jsoup.select.Elements;

import java.io.FileWriter;
import java.io.IOException;
import java.util.Base64;
import java.util.Scanner;

public class JsoupRun {
    static Scanner scanner = new Scanner(System.in);
    static Connection connection;
    static Elements links;
    static String url;
    static final String FIRST_PAGE = "1";
    static JSONObject jsonObject;
    static JSONArray jsonArray;
    static final String FILE_NAME = "results.json";

    public static void main(String[] args) throws IOException {


        url = createURL();

        connection = Jsoup.connect(url + FIRST_PAGE);
        String offersCount = connection.get().getElementsByClass("offers-index pull-left text-nowrap").text();



        //System.out.println(links.get(0));
        int tmpFor = calculateTotalPages(offersCount);
        try (FileWriter fw = new FileWriter(FILE_NAME)) {


            for (int j = 1; j <= tmpFor; j++) {


                System.out.println("-------------------------loading page...-------------------------");
                connection = Jsoup.connect(url + j).timeout(15000);
                System.out.println(url + j);
                links = connection.get().getElementsByTag("article");
                System.out.println("-------------------------PAGE " + j + "-------------------------");
                for (Element link : links) {
                    jsonObject = new JSONObject();
                    jsonArray = new JSONArray();
                    jsonObject.put("image", Base64.getUrlEncoder().encodeToString((link.getElementsByClass("img-cover lazy").tagName("background-image").attr("data-src")).getBytes()));
                    System.out.println("image: " + link.getElementsByClass("img-cover lazy").tagName("background-image").attr("data-src"));
                    if (link.attr("data-featured-name").contains("listing_no_promo")) {
                        jsonObject.put("URL", link.getElementsByClass("img-cover lazy").tagName("background-image").attr("data-src"));
                        System.out.println("link to a website: " + link.getElementsByTag("a").attr("href"));

                        //System.out.println(link.getElementsByClass("params").tagName("li").text());
                        for (Element e : link.getElementsByClass("params")) {

                            for (int i = 0; i <= e.getElementsByTag("li").size() - 1; i++) {
                                jsonArray.put(e.getElementsByTag("li").get(i).text());
                                System.out.println(e.getElementsByTag("li").get(i).text());
                            }

                        }

                    }
                    jsonObject.put("details", jsonArray);
                    fw.write(jsonObject.toString());
                    fw.write("\n");
                    //fw.flush();

                }

            }
        } catch (IOException e) {
            e.printStackTrace();
        }


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

    public static String createURL() {
        String sellOrRent;
        String houseOrAccommodation;
        String cityName;
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

        return sb.toString();


    }
}
