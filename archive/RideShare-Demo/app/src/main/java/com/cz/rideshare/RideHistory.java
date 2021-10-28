package com.cz.rideshare;

import android.databinding.ObservableMap;
import android.net.Uri;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.LinearSnapHelper;
import android.support.v7.widget.RecyclerView;
import android.util.Log;

import com.cz.rideshare.adapter.RideSnapshotListAdpater;
import com.cz.rideshare.model.Gender;
import com.cz.rideshare.model.Node;
import com.cz.rideshare.model.Rating;
import com.cz.rideshare.model.RideSnapshot;
import com.cz.rideshare.model.User;
import com.cz.rideshare.model.Vehicle;
import com.cz.rideshare.model.VehicleType;
import com.google.android.gms.maps.model.LatLng;
import com.google.firebase.firestore.CollectionReference;
import com.google.firebase.firestore.FirebaseFirestore;
import com.google.firebase.firestore.GeoPoint;

import java.util.ArrayList;
import java.util.Date;

public class RideHistory extends AppCompatActivity {

    RecyclerView rideHistoryRecyclerView = null;

    private void initialize() {
        rideHistoryRecyclerView = findViewById(R.id.rideHistoryRecyclerView);
    }
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ride_history);
        initialize();

        ArrayList<RideSnapshot> rideSnapshots = new ArrayList<>();

        ///////////////////////////////////TODO Firebase Integration/////////////////////////////////////

        User user = new User("78", "Someone", "someone@someone.com", new Rating(5, 10f),
                Gender.MALE,
                null, "+91 99999 99999",
                new Date(), "dsfs", "https://www.latfusa.com/media/archive/20130529130146Ben_A__.jpg", new Date(), null);
        RideSnapshot rideSnapshot = new RideSnapshot(20, 365f,
                new Node("RMK College", new Date(), new GeoPoint(45.00, 65.9)), null,
                new Vehicle("Suzuki Honda",
                        2,
                        "TN-XX XXXX",
                        564,
                        new VehicleType("https://images.hgmsites.net/lrg/2013-bmw-6-series-4-door-sedan-640i-gran-coupe-front-exterior-view_100411916_l.jpg", "Van"),
                        null),
                        null,
                null, new Date(),
                new Node("CZ Smart Mobility", new Date(), new GeoPoint(46.00f, 34.65f)), new Rating(5,10f), user);
        //rideSnapshots.add(rideSnapshot);
        FirestoreList<RideSnapshot> firebaseFirestore = new FirestoreList<>(RideSnapshot.class, RideShareController.getInstance().getRideSnapshotsRef());
        RideSnapshotListAdpater rideSnapshotListAdpater = new RideSnapshotListAdpater(this, rideSnapshots, null);
        firebaseFirestore.populate(100);
        firebaseFirestore.addOnMapChangedCallback(new ObservableMap.OnMapChangedCallback<ObservableMap<RideSnapshot, String>, RideSnapshot, String>() {
            @Override
            public void onMapChanged(ObservableMap<RideSnapshot, String> sender, RideSnapshot key) {
                Log.i("MAP", user.getName());
                rideSnapshots.add(key);
                rideSnapshotListAdpater.notifyDataSetChanged();
            }
        });
        //firebaseFirestore.add(rideSnapshot);
        firebaseFirestore.removeOnMapChangedCallback(new ObservableMap.OnMapChangedCallback<ObservableMap<RideSnapshot, String>, RideSnapshot, String>() {
            @Override
            public void onMapChanged(ObservableMap<RideSnapshot, String> sender, RideSnapshot key) {
                Log.e("MAP", "Remove");
            }
        });
        //firebaseFirestore.remove(rideSnapshot);


        //////////////////////////////////////////////////////////////////////////////////////

        rideHistoryRecyclerView.setAdapter(rideSnapshotListAdpater);
        rideHistoryRecyclerView.setLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false));
        LinearSnapHelper sh = new LinearSnapHelper();
        sh.attachToRecyclerView(rideHistoryRecyclerView);

    }

}
