package pl.krzywyyy.ztmwatcher.api;

import pl.krzywyyy.ztmwatcher.model.Result;
import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Query;

public interface ApiService {
    @GET("busestrams_get/")
    Call<Result> findAll(@Query("resource_id") String resourceId,
                         @Query("apikey") String apiKey,
                         @Query("type") String type,
                         @Query("line") String line);
}
