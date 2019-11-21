package pl.krzywyyy;

import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;
import org.jsoup.Jsoup;
import org.jsoup.nodes.Document;
import org.jsoup.nodes.Element;
import org.jsoup.select.Elements;

import java.io.FileWriter;
import java.io.IOException;
import java.util.Base64;

public class Main {

    public static void main(String[] args) {
        try {
            String path = buildPath(args);
            Document document = Jsoup.connect(path).get();
            int numberOfPages = getNumberOfPages(document);

            OkHttpClient httpClient = new OkHttpClient();
            StringBuilder result = new StringBuilder().append("[\n");

            for (int i = 1; i <= numberOfPages; i++) {
                Document page = Jsoup.connect(path + "?page=" + i).get();
                Elements repos = page.getElementsByClass("offer-item");

                for (Element element : repos) {
                    addAdvertisementToResultString(httpClient, result, element);
                }
                result.delete(result.length() - 2, result.length());
                result.append("\n]");
            }

            try(FileWriter fileWriter = new FileWriter("MyDreamCars.json")){
                fileWriter.write(result.toString());
                fileWriter.flush();
            }
            System.out.println("Ogłoszenia zostały zapisane do pliku MyDreamCars.json");
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private static String buildPath(String[] args) {
        String brand = args[0];
        String model = args[1];
        String productionYearFrom = args[2];
        String productionYearTo = args[3];
        String priceFrom = args[4];
        String priceTo = args[5];
        String mileageFrom = args[6];
        String mileageTo = args[7];

        return "https://www.otomoto.pl/osobowe/" +
                brand + "/" + model + "/od-" + productionYearFrom +
                "/?search%5Bfilter_float_price%3Afrom%5D=" + priceFrom +
                "&search%5Bfilter_float_price%3Ato%5D=" + priceTo +
                "&search%5Bfilter_float_year%3Ato%5D=" + productionYearTo +
                "&search%5Bfilter_float_mileage%3Afrom%5D=" + mileageFrom +
                "&search%5Bfilter_float_mileage%3Ato%5D=" + mileageTo;
    }

    private static int getNumberOfPages(Document document) {
        Elements pages = document.getElementsByClass("page");
        return pages.size() > 1 ?  Integer.parseInt(pages.get(pages.size() - 1).text()) : 1;
    }

    private static void addAdvertisementToResultString(OkHttpClient httpClient, StringBuilder result, Element element) throws IOException {
        Advertisement.AdvertisementBuilder advertisementBuilder = Advertisement.builder()
                .title(element.getElementsByClass("offer-title ds-title").text())
                .price(element.getElementsByClass("offer-item__price").text())
                .location(element.getElementsByClass("ds-location-city").text())
                .url(element.attr("data-href"))
                .image(getBase64FromImageUrl(httpClient, element.getElementsByTag("img").get(0).attr("data-src")));

        addParams(advertisementBuilder, element);
        Advertisement advertisement = advertisementBuilder.build();
        result.append(advertisement.toString());
    }

    private static String getBase64FromImageUrl(OkHttpClient httpClient, String imageURL) throws IOException {
        Request request = new Request.Builder()
                .url(imageURL)
                .build();

        try (Response response = httpClient.newCall(request).execute()) {
            if (response.isSuccessful() && response.body() != null) {
                return Base64.getEncoder().encodeToString(response.body().bytes());
            }
        }
        return null;
    }

    private static void addParams(Advertisement.AdvertisementBuilder advertisement, Element element) {
        Elements parameters = element.getElementsByClass("ds-param");
        if (parameters.size() == 4) {
            advertisement
                    .year(parameters.get(0).text())
                    .mileage(parameters.get(1).text())
                    .engineCapacity(parameters.get(2).text())
                    .fuelType(parameters.get(3).text());
        } else {
            advertisement
                    .year(parameters.get(0).text())
                    .mileage(parameters.get(1).text())
                    .fuelType(parameters.get(2).text());
        }
    }
}
