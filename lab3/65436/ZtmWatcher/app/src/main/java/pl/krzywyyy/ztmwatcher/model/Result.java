package pl.krzywyyy.ztmwatcher.model;

import java.util.List;

import lombok.Data;
import pl.krzywyyy.ztmwatcher.model.Ztm;

@Data
public class Result {
    private List<Ztm> result;
}
