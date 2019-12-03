package pl.krzywyyy.ztmwatcher.ui.main;

import android.content.Intent;
import android.os.Bundle;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.google.android.gms.common.util.Strings;

import pl.krzywyyy.ztmwatcher.R;
import pl.krzywyyy.ztmwatcher.ui.maps.MapsActivity;

public class MainActivity extends AppCompatActivity {

    private EditText lineNumber;
    private Spinner conveyanceType;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        lineNumber = findViewById(R.id.line_number);
        conveyanceType = findViewById(R.id.conveance_type);

        findViewById(R.id.main_button).setOnClickListener(e -> checkDataAndShowMap());
    }

    private void checkDataAndShowMap() {
        if (!Strings.isEmptyOrWhitespace(lineNumber.getText().toString())) {
            String type = String.valueOf(conveyanceType.getSelectedItemPosition() + 1);
            String number = String.valueOf(lineNumber.getText());

            Intent maps = new Intent(this, MapsActivity.class);
            maps.putExtra("type", type);
            maps.putExtra("number", number);
            startActivity(maps);
        } else {
            Toast.makeText(this, getString(R.string.line_number_cannot_be_empty), Toast.LENGTH_SHORT).show();
        }
    }
}
