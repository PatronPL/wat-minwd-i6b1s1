package pl.krzywyyy.ztmwatcher.model;

import lombok.Data;

@Data
public class Ztm {
    private Float Lat;
    private Float Lon;
    private String Time;
    private String Lines;
    private String Brigade;
}
