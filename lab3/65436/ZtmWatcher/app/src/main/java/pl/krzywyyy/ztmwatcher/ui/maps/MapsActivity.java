package pl.krzywyyy.ztmwatcher.ui.maps;

import android.Manifest;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AlertDialog;
import androidx.core.app.ActivityCompat;
import androidx.fragment.app.FragmentActivity;

import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.CameraPosition;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.Marker;
import com.google.android.gms.maps.model.MarkerOptions;

import java.util.HashMap;
import java.util.List;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;

import pl.krzywyyy.ztmwatcher.R;
import pl.krzywyyy.ztmwatcher.api.ApiService;
import pl.krzywyyy.ztmwatcher.model.Result;
import pl.krzywyyy.ztmwatcher.model.Ztm;
import pl.krzywyyy.ztmwatcher.properties.Properties;
import pl.krzywyyy.ztmwatcher.retrofit.MyRetrofit;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.internal.EverythingIsNonNull;

public class MapsActivity extends FragmentActivity implements OnMapReadyCallback, GoogleMap.OnMarkerClickListener {

    private static final int LOCATION_PERMISSION_REQUEST_CODE = 1;
    private GoogleMap mMap;
    private Retrofit retrofit;
    private HashMap<String, Marker> markers;
    private String ztmNumber;
    private String type;
    private List<Ztm> conveyances;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_maps);

        retrofit = MyRetrofit.getRetrofit();
        markers = new HashMap<>();

        ztmNumber = getIntent().getStringExtra("number");
        type = getIntent().getStringExtra("type");

        SupportMapFragment mapFragment = (SupportMapFragment) getSupportFragmentManager()
                .findFragmentById(R.id.map);

        if (mapFragment != null) {
            mapFragment.getMapAsync(this);
        }
    }

    @Override
    public void onMapReady(GoogleMap googleMap) {
        mMap = googleMap;
        enableMyLocation();

        ApiService apiService = retrofit.create(ApiService.class);

        setMarkersLocations(apiService);

        mMap.setOnMarkerClickListener(this);
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions,
                                           @NonNull int[] grantResults) {
        if (requestCode == LOCATION_PERMISSION_REQUEST_CODE) {
            enableMyLocation();
        }
    }

    @Override
    public boolean onMarkerClick(Marker marker) {
        String brigade = marker.getTitle();
        Ztm ztm = findZtm(brigade);

        if (ztm != null) {
            String title = "Szczegóły";
            String message = "Numer linii: " + ztm.getLines() + "\n"
                    + "Numer brygady: " + ztm.getBrigade() + "\n"
                    + "Aktualne położenie: \n" + ztm.getLat() + ", " + ztm.getLon() + "\n"
                    + "Czas ostatniej aktualizacji: \n" + ztm.getTime();

            new AlertDialog.Builder(this)
                    .setTitle(title)
                    .setMessage(message)
                    .setPositiveButton(R.string.ok, null)
                    .show();
        } else {
            Toast.makeText(this, getString(R.string.cannot_read_marker_data), Toast.LENGTH_SHORT).show();
        }
        return true;
    }

    private void setMarkersLocations(final ApiService service) {
        Call<Result> getBusesOrTrams =
                service.findAll(Properties.resourceId, Properties.apiKey, type, ztmNumber);
        getBusesOrTrams.enqueue(new Callback<Result>() {
            @EverythingIsNonNull
            @Override
            public void onResponse(final Call<Result> call, final Response<Result> response) {
                if (response.isSuccessful() && response.body() != null) {
                    if (dataIsPresent(response.body().getResult())) {
                        conveyances = response.body().getResult();
                        initMarkers(conveyances);
                        runLocationUpdater(service);
                    }
                } else {
                    runOnUiThread(() ->
                            Toast.makeText(MapsActivity.this,
                                    R.string.cannot_get_buses_or_trams,
                                    Toast.LENGTH_SHORT).show());
                }
            }

            @EverythingIsNonNull
            @Override
            public void onFailure(Call<Result> call, final Throwable t) {
                runOnUiThread(() ->
                        Toast.makeText(MapsActivity.this,
                                R.string.connection_error,
                                Toast.LENGTH_SHORT).show());
            }
        });
    }

    private boolean dataIsPresent(List<Ztm> result) {
        if (result.size() == 0) {
            Toast.makeText(this, getString(R.string.choosed_line_does_not_drive), Toast.LENGTH_SHORT).show();
            return false;
        }
        return true;
    }

    private void initMarkers(final List<Ztm> models) {
        for (final Ztm model : models) {
            LatLng location = new LatLng(model.getLat(), model.getLon());
            final MarkerOptions options = new MarkerOptions().position(location).title(model.getBrigade());
            runOnUiThread(() -> markers.put(model.getBrigade(), mMap.addMarker(options)));
        }
        runOnUiThread(() -> initCamera(models));
    }

    private void initCamera(List<Ztm> models) {
        LatLng position = new LatLng(models.get(0).getLat(), models.get(0).getLon());
        CameraPosition cameraPosition = new CameraPosition.Builder()
                .target(position)
                .zoom(15)
                .build();
        mMap.moveCamera(CameraUpdateFactory.newCameraPosition(cameraPosition));
    }

    private void runLocationUpdater(ApiService service) {
        Runnable updateTask = getUpdateTask(service);
        ScheduledExecutorService executorService = Executors.newSingleThreadScheduledExecutor();
        executorService.scheduleAtFixedRate(updateTask, 0, 5, TimeUnit.SECONDS);
    }

    private Runnable getUpdateTask(final ApiService service) {
        return () -> {
            Call<Result> getBusesOrTrams =
                    service.findAll(Properties.resourceId, Properties.apiKey, type, ztmNumber);
            getBusesOrTrams.enqueue(new Callback<Result>() {
                @EverythingIsNonNull
                @Override
                public void onResponse(final Call<Result> call, final Response<Result> response) {
                    if (response.isSuccessful() && response.body() != null) {
                        conveyances = response.body().getResult();
                        for (final Ztm model : conveyances) {
                            final LatLng newLocation = new LatLng(model.getLat(), model.getLon());
                            runOnUiThread(() -> {
                                Marker marker = markers.get(model.getBrigade());
                                if (marker != null) {
                                    marker.setPosition(newLocation);
                                }
                            });
                        }
                    } else {
                        runOnUiThread(() ->
                                Toast.makeText(MapsActivity.this,
                                        R.string.cannot_get_buses_or_trams,
                                        Toast.LENGTH_SHORT).show());
                    }
                }

                @EverythingIsNonNull
                @Override
                public void onFailure(Call<Result> call, final Throwable t) {
                    runOnUiThread(() ->
                            Toast.makeText(MapsActivity.this,
                                    R.string.connection_error,
                                    Toast.LENGTH_SHORT).show());
                }
            });
        };
    }

    private void enableMyLocation() {
        if (ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION)
                != PackageManager.PERMISSION_GRANTED) {
            ActivityCompat.requestPermissions(this,
                    new String[]{Manifest.permission.ACCESS_FINE_LOCATION}, LOCATION_PERMISSION_REQUEST_CODE);
        } else if (mMap != null) {
            mMap.setMyLocationEnabled(true);
        }
    }

    private Ztm findZtm(String brigade) {
        for (Ztm ztm : conveyances) {
            if (ztm.getBrigade().equals(brigade)) {
                return ztm;
            }
        }
        return null;
    }
}
