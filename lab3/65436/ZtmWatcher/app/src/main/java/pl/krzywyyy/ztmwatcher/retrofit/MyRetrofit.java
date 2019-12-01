package pl.krzywyyy.ztmwatcher.retrofit;

import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class MyRetrofit {

    private static Retrofit retrofit;
    private static String ztmApi = "https://api.um.warszawa.pl/api/action/";

    private MyRetrofit() {

    }

    public static Retrofit getRetrofit() {
        if (retrofit == null) {
            retrofit = new Retrofit.Builder()
                    .baseUrl(ztmApi)
                    .addConverterFactory(GsonConverterFactory.create())
                    .build();
        }
        return retrofit;
    }
}
