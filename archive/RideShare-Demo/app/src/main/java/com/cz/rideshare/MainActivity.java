package com.cz.rideshare;

import android.Manifest;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.net.Uri;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.design.widget.NavigationView;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.AppCompatImageButton;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.bumptech.glide.request.RequestOptions;
import com.cz.rideshare.model.RideSnapshot;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.MarkerOptions;

import de.hdodenhof.circleimageview.CircleImageView;

public class MainActivity extends AppCompatActivity
        implements NavigationView.OnNavigationItemSelectedListener, OnMapReadyCallback, SelectDate.OnFragmentInteractionListener {

    private final Context context = this;
    private DrawerLayout drawer = null;
    private NavigationView navigationView = null;
    private ImageButton centerLocBtn = null;

    private EditText editTextFrom = null;
    private EditText editTextTo = null;

    private Button rideShareButton = null;
    private AppCompatImageButton sidebarToggleOnMap = null;
    private AppCompatImageButton sidebarToggleOnNavBar = null;
    private AppCompatImageButton centerLocationButton = null;


    private GoogleMap mMap;
    LocationListener locationListener;
    LocationManager locationManager;

    void initialize() {
        drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        navigationView = (NavigationView) findViewById(R.id.nav_view);
        centerLocBtn = (ImageButton) findViewById(R.id.centerLocationButton);

        rideShareButton = findViewById(R.id.rideShareButton);
        centerLocationButton = findViewById(R.id.centerLocationButton);
        sidebarToggleOnMap = findViewById(R.id.sidebarToggleOnMap);
        sidebarToggleOnNavBar = findViewById(R.id.sidebarToggleOnNavBar);

        editTextFrom = findViewById(R.id.editTextFrom);
        editTextTo = findViewById(R.id.editTextTo);
    }


    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        if (requestCode == 1) {
            if (grantResults[0] == PackageManager.PERMISSION_GRANTED && grantResults.length > 0) {
                if (ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION) == PackageManager.PERMISSION_GRANTED) {
                    locationManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, 0, 0, locationListener);

                }
            }
        }
    }

    private void loadFromController() {
        RequestOptions options = new RequestOptions();
        options.centerCrop();

        TextView user_txt = (TextView) navigationView.getHeaderView(0).findViewById(R.id.user_name);
        CircleImageView user_image = (CircleImageView) navigationView.getHeaderView(0).findViewById(R.id.userImageView);

        user_txt.setText(RideShareController.getInstance().user.getName());
        Glide.with(navigationView.getHeaderView(0))
                .load(RideShareController.getInstance().user.getDisplayPicture())
                .apply(options)
                .into(user_image);

    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        initialize();
        loadFromController();

        rideShareButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Bundle b = new Bundle();
                if(rideShareButton.getText() == getResources().getString(R.string.menu_book_rides)) {
                    b.putString("travel_type", getResources().getString(R.string.menu_book_rides));
                } else {
                    b.putString("travel_type", getResources().getString(R.string.menu_offer_ride));
                }
                SelectDate sd = SelectDate.newInstance();
                sd.setArguments(b);
                sd.show(getSupportFragmentManager(), "fragment_select_date");
            }
        });

        sidebarToggleOnMap.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                //TODO Center Map
            }
        });

        navigationView.getHeaderView(0).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent i = new Intent(context, ProfileActivity.class);
                startActivity(i);
            }
        });
        // Obtain the SupportMapFragment and get notified when the map is ready to be used.
        SupportMapFragment mapFragment = (SupportMapFragment) getSupportFragmentManager()
                .findFragmentById(R.id.map);
        mapFragment.getMapAsync(this);
        centerLocBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                //TODO Center Map
            }
        });
        sidebarToggleOnMap.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                drawer.openDrawer(GravityCompat.START);
            }
        });
        sidebarToggleOnNavBar.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                drawer.closeDrawer(GravityCompat.START);
            }
        });

        navigationView.setNavigationItemSelectedListener(this);
    }

    void sidebarToggle(View view) {
        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        if (drawer.isDrawerOpen(GravityCompat.START)) {
            drawer.closeDrawer(GravityCompat.START);
        } else {
            super.onBackPressed();
        }
    }

    void bookRides(){
        rideShareButton.setText("Book Ride");
    }

    void offerRides(){
        rideShareButton.setText("Offer Ride");
    }

    @Override
    public void onBackPressed() {
        sidebarToggle(null);
    }

    @Override
    public boolean onNavigationItemSelected(MenuItem item) {
        // Handle navigation view item clicks here.
        int id = item.getItemId();

        Intent i = null;
        if (id == R.id.nav_book_rides) {
            rideShareButton.setText(getResources().getString(R.string.menu_book_rides));
        } else if (id == R.id.nav_ride_history) {
            i = new Intent(this, RideHistory.class);
        } else if (id == R.id.nav_emergency_contacts) {
            i = new Intent(this, EmergencyContactsActivity.class);
        } else if (id == R.id.nav_offer_ride) {
            rideShareButton.setText(getResources().getString(R.string.menu_offer_ride));
        } else if (id == R.id.nav_support) {
            i = new Intent(this, SupportActivity.class);
        }
        if (i != null) {
            startActivity(i);
        }
        drawer.closeDrawer(GravityCompat.START);
        return true;
    }


    @Override
    public void onMapReady(GoogleMap googleMap) {
        mMap = googleMap;
        locationManager = (LocationManager) this.getSystemService(Context.LOCATION_SERVICE);
        locationListener = new LocationListener() {
            @Override
            public void onLocationChanged(Location location) {
            }

            @Override
            public void onStatusChanged(String s, int i, Bundle bundle) {

            }

            @Override
            public void onProviderEnabled(String s) {

            }

            @Override
            public void onProviderDisabled(String s) {

            }
        };

//        if(Build.VERSION.SDK_INT<=23)
//        {
//            if(ContextCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION)== PackageManager.PERMISSION_GRANTED) {
//                locationManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, 0, 0, locationListener);
//            }
//        }
//        else {
        if (ContextCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
            ActivityCompat.requestPermissions(this, new String[]{Manifest.permission.ACCESS_FINE_LOCATION}, 1);
        } else {
            locationManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, 0, 0, locationListener);
            Location lastLocation = locationManager.getLastKnownLocation(LocationManager.GPS_PROVIDER);
            if (lastLocation != null) {
                LatLng sydney = new LatLng(lastLocation.getLatitude(), lastLocation.getLongitude());
                mMap.clear();
                mMap.addMarker(new MarkerOptions().position(sydney).title("Your Location"));
                mMap.moveCamera(CameraUpdateFactory.newLatLngZoom(sydney, 17.0f));
            }
        }
//        }

        // Add a marker in Sydney and move the camera

    }

    @Override
    public void onFragmentInteraction(Uri uri) {

    }
}
