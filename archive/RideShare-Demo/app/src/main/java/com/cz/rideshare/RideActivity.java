package com.cz.rideshare;

import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.net.Uri;
import android.support.v4.app.ActivityCompat;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.View;
import android.widget.ImageButton;
import android.widget.TextView;
import android.widget.Toast;

import com.bumptech.glide.Glide;
import com.cz.rideshare.adapter.PathListAdapter;
import com.cz.rideshare.model.Node;
import com.cz.rideshare.model.RideSnapshot;
import com.directions.route.Route;
import com.directions.route.RouteException;
import com.directions.route.Routing;
import com.directions.route.RoutingListener;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.LatLngBounds;
import com.google.android.gms.maps.model.MarkerOptions;
import com.google.firebase.firestore.GeoPoint;

import java.util.ArrayList;

import de.hdodenhof.circleimageview.CircleImageView;

public class RideActivity extends AppCompatActivity implements OnMapReadyCallback, View.OnClickListener {

    private GoogleMap rideMap;
    LocationListener locationListener;
    LocationManager locationManager;

    private TextView rideInfo = null;
    private ImageButton callBtn, msgBtn, cancelBtn;
    private RecyclerView nodeRecyclerView = null;

    private CircleImageView driverImage = null;
    private TextView driverName = null;
    private TextView driverRating = null;

    private RideSnapshot rideSnapshot = null;

    void initialize() {
        SupportMapFragment mapFragment = (SupportMapFragment) getSupportFragmentManager()
                .findFragmentById(R.id.rideMap);
        mapFragment.getMapAsync(this);
        rideInfo = findViewById(R.id.rideInfoText);
        callBtn = findViewById(R.id.rideCallDriver);
        msgBtn = findViewById(R.id.rideMsgDriver);
        cancelBtn = findViewById(R.id.rideCancel);
        nodeRecyclerView = findViewById(R.id.rideNodeRecyclerView);

        driverImage = findViewById(R.id.snapshotDriverImage);
        driverName = findViewById(R.id.snapshotDriverName);
        driverRating = findViewById(R.id.snapshotDriverRating);
    }

    LatLng getLatLng(GeoPoint geoPoint){
        return new LatLng(geoPoint.getLatitude(), geoPoint.getLongitude());
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ride);
        rideSnapshot = RideShareController.getInstance().getRideSnapshot();
        initialize();
        callBtn.setOnClickListener(this);
        msgBtn.setOnClickListener(this);
        cancelBtn.setOnClickListener(this);
        ArrayList<Node> nodes = new ArrayList<Node>();
        nodes.add(rideSnapshot.getStart());
        nodes.add(rideSnapshot.getEnd());
        nodeRecyclerView.setAdapter(new PathListAdapter(nodes));
        nodeRecyclerView.setLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false));
        //TODO pass node information to rideMap : rideMap.addMarker(new MarkerOptions().position(nodes.get(0).getLocation()).title(nodes.get(0).getLocationName()));
        rideInfo.setText("6mins to your destination"); //TODO periodically calculate time difference between currentTime and nodes[1]
        driverName.setText(rideSnapshot.getDriver().getName());
        Glide.with(this)
                .load(rideSnapshot.getDriver().getDisplayPicture())
                .into(driverImage);
        driverRating.setText(String.valueOf(rideSnapshot.getDriver().getRating().getRating()));
    }

    @Override
    public void onMapReady(GoogleMap googleMap) {
        rideMap = googleMap;
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
        rideMap.clear();
        rideMap.addMarker(new MarkerOptions().position(getLatLng(rideSnapshot.getStart().getLocation())).title(rideSnapshot.getStart().getLocationName()));
        rideMap.addMarker(new MarkerOptions().position(getLatLng(rideSnapshot.getEnd().getLocation())).title(rideSnapshot.getEnd().getLocationName()));

        Routing routing = new Routing.Builder()
                .travelMode(Routing.TravelMode.DRIVING)
                .withListener(new RoutingListener() {
                    @Override
                    public void onRoutingFailure(RouteException e) {

                    }

                    @Override
                    public void onRoutingStart() {

                    }

                    @Override
                    public void onRoutingSuccess(ArrayList<Route> arrayList, int i) {
                        rideMap.moveCamera(CameraUpdateFactory.newLatLngZoom(getLatLng(rideSnapshot.getStart().getLocation()), 17.0f));
                        rideMap.moveCamera(CameraUpdateFactory.newLatLng(arrayList.get(0).getLatLgnBounds().getCenter()));
                        LatLngBounds.Builder builder = new LatLngBounds.Builder();
                        builder.include(getLatLng(rideSnapshot.getEnd().getLocation()));
                        builder.include(getLatLng(rideSnapshot.getStart().getLocation()));
                        LatLngBounds bounds = builder.build();
                        rideMap.animateCamera(CameraUpdateFactory.newLatLngBounds(bounds, 50));
                        rideMap.addPolyline(arrayList.get(0).getPolyOptions().color(getResources().getColor(R.color.colorRideshare)));
                    }

                    @Override
                    public void onRoutingCancelled() {

                    }
                })
                .waypoints(getLatLng(rideSnapshot.getStart().getLocation()), getLatLng(rideSnapshot.getEnd().getLocation()))
                .build();
        routing.execute();
    }

    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.rideCallDriver:
                Intent intent = new Intent(Intent.ACTION_CALL, Uri.parse("tel:" + rideSnapshot.getDriver().getPhoneNumber()));
                if (ActivityCompat.checkSelfPermission(this, android.Manifest.permission.CALL_PHONE) != PackageManager.PERMISSION_GRANTED) {
                    Toast.makeText(this, "Please grant call permissions", Toast.LENGTH_SHORT).show();
                    //TODO ask permission here
                    return;
                }
                startActivity(intent);
                break;
            case R.id.rideMsgDriver:
                Toast.makeText(this, "Messaging not supported yet", Toast.LENGTH_SHORT).show();
                break;
            case R.id.rideCancel:
                //TODO add confirmation and necessary steps to cancel the ride
                Intent i = new Intent(this, MainActivity.class);
                startActivity(i);
                break;
        }
    }
}
