package com.cz.rideshare;

import android.net.Uri;

import com.cz.rideshare.model.Gender;
import com.cz.rideshare.model.Rating;
import com.cz.rideshare.model.RideSnapshot;
import com.cz.rideshare.model.User;
import com.cz.rideshare.model.Verification;
import com.google.firebase.auth.FirebaseUser;
import com.google.firebase.database.DataSnapshot;
import com.google.firebase.firestore.CollectionReference;
import com.google.firebase.firestore.FirebaseFirestore;

import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Date;
import java.util.Map;

/**
 * Created by Home on 28-12-2017.
 */

class RideShareController {
    private static final RideShareController ourInstance = new RideShareController();

    static RideShareController getInstance() {
        return ourInstance;
    }

    public User user = null;
    private RideSnapshot rideSnapshot = null;
    private CollectionReference rideSnapshots;
    private CollectionReference users;
    private FirebaseFirestore db;

    public void setRideSnapshot(RideSnapshot rideSnapshot){
        this.rideSnapshot = rideSnapshot;
    }
    public RideSnapshot getRideSnapshot(){
        return rideSnapshot;
    }

    public void setUser(FirebaseUser fUser) {

        //TODO parse JSON from Firebase
        user = new User(fUser.getUid(), fUser.getDisplayName(), fUser.getEmail(), new Rating(45, 34), Gender.MALE, new ArrayList<RideSnapshot>(), fUser.getPhoneNumber(), new Date(), "", fUser.getPhotoUrl().toString(), new Date(), new ArrayList<Verification>());

    }

    private RideShareController() {
        //get settings from local store

        //initializing with initial values, so it's never null
        db = FirebaseFirestore.getInstance();
        rideSnapshots = db.collection("snapshots");
        users = db.collection("users");


        ArrayList<Verification> verifications = new ArrayList<Verification>();
        verifications.add(new Verification("Mobile Number Verified", true));
        verifications.add(new Verification("Email Verified", true));
        verifications.add(new Verification("Licence Verified", false));
        verifications.add(new Verification("208 Facebook Friends", true));
        user = new User("0", "User", "", new Rating(0, 0), Gender.MALE, null, "", new Date(), "", null, new Date(), verifications);
    }

    public CollectionReference getRideSnapshotsRef(){
        return rideSnapshots;
    }


    public CollectionReference getUsersRef() {
        return users;
    }
}
