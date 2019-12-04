package pl.krzywyyy;

import lombok.Builder;
import lombok.Data;

@Data
@Builder
class Advertisement {
    private String title;
    private String year;
    private String mileage;
    private String engineCapacity;
    private String fuelType;
    private String price;
    private String location;
    private String url;
    private String image;

    @Override
    public String toString() {
        return "\t{\n" +
                "\t\t\"Advertisement\": {\n" +
                "\t\t\t\"title\":\"" + title + "\",\n" +
                "\t\t\t\"year=\":" + year + ",\n" +
                "\t\t\t\"mileage\":\"" + mileage + "\",\n" +
                "\t\t\t\"engineCapacity\":" + (engineCapacity != null ? "\"" + engineCapacity + "\"" : "null") + ",\n" +
                "\t\t\t\"fuelType\":\"" + fuelType + "\",\n" +
                "\t\t\t\"price\":\"" + price + "\",\n" +
                "\t\t\t\"location\":\"" + location + "\",\n" +
                "\t\t\t\"url\":\"" + url + "\",\n" +
                "\t\t\t\"image\":\"" + image + "\"\n" +
                "\t\t}\n" +
                "\t},\n";
    }
}
