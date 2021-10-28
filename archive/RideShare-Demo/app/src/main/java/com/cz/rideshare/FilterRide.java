package com.cz.rideshare;

import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.View;

import com.cz.rideshare.adapter.RideSnapshotListAdpater;
import com.cz.rideshare.model.Gender;
import com.cz.rideshare.model.Node;
import com.cz.rideshare.model.Rating;
import com.cz.rideshare.listener.RecyclerViewItemClickListener;
import com.cz.rideshare.model.RideSnapshot;
import com.cz.rideshare.model.User;
import com.cz.rideshare.model.Vehicle;
import com.cz.rideshare.model.VehicleType;
import com.cz.rideshare.view.RideshareToolbar;
import com.google.android.gms.maps.model.LatLng;
import com.google.firebase.firestore.GeoPoint;

import java.util.ArrayList;
import java.util.Date;

public class FilterRide extends AppCompatActivity {

    private final Context context = this;

    private RecyclerView recyclerView = null;
    private RideshareToolbar rideshareToolbar = null;
    //private TextView headerTextView = null;

    private void initialise(){
        recyclerView = findViewById(R.id.rideFilterRecyclerView);
        rideshareToolbar = findViewById(R.id.rideFilterToolbar);
    }
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_filter_ride);
        initialise();


        final ArrayList<RideSnapshot> rideSnapshots = new ArrayList<>();

        ///////////////////////////////////TODO Firebase Integration/////////////////////////////////////

        User user = new User("78", "John Doe", "someone@someone.com", new Rating(5, 10f),
                Gender.MALE,
                null, "+91 99999 99999",
                new Date(), "dsfs", "http://hardikmanktala.com/projects/themes/signe/demo/assets/images/people/people1.jpg", new Date(), null);
        final RideSnapshot rideSnapshot = new RideSnapshot(20, 365f,
                new Node("RMK College", new Date(), new GeoPoint(13.357659, 80.142839)), null,
                new Vehicle("Suzuki Honda",
                        2,
                        "TN-XX XXXX",
                        564,
                        new VehicleType("https://images.hgmsites.net/lrg/2013-bmw-6-series-4-door-sedan-640i-gran-coupe-front-exterior-view_100411916_l.jpg", "Van"),
                        null),
                null,
                null, new Date(),
                new Node("CZ Smart Mobility", new Date(), new GeoPoint(12.899488, 80.235022)), new Rating(5,10f), user);
        rideSnapshots.add(rideSnapshot);
        rideSnapshots.add(rideSnapshot);
        rideSnapshots.add(rideSnapshot);
        rideSnapshots.add(rideSnapshot);
        rideSnapshots.add(rideSnapshot);
        rideSnapshots.add(rideSnapshot);
        rideSnapshots.add(rideSnapshot);

        ////////////////////////////////////TODO END////////////////////////////////////////////////////////////////

        rideshareToolbar.setHeaderText(rideSnapshot.getStart().getLocationName() + " - " + rideSnapshot.getEnd().getLocationName());
        recyclerView.setLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false));
        recyclerView.setAdapter(new RideSnapshotListAdpater(this, rideSnapshots, new RecyclerViewItemClickListener() {
            @Override
            public void onCLick(View v, int position) {
                Intent i = new Intent(context, RideDetailed.class);
                RideShareController.getInstance().setRideSnapshot(rideSnapshot);
                startActivity(i);
            }
        }));
    }
}
