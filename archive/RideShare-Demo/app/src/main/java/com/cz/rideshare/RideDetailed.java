package com.cz.rideshare;

import android.content.Context;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.View;
import android.widget.Button;
import android.widget.ImageButton;
import android.widget.Toast;

import com.cz.rideshare.adapter.PathListAdapter;
import com.cz.rideshare.adapter.PermissionListAdapter;
import com.cz.rideshare.adapter.VerificationListAdapter;
import com.cz.rideshare.model.Node;
import com.cz.rideshare.model.Permission;
import com.cz.rideshare.model.RideSnapshot;

import java.util.ArrayList;

public class RideDetailed extends AppCompatActivity implements View.OnClickListener {

    Context context = this;
    RecyclerView carPermissionRecyclerView = null;
    RecyclerView verificationRecyclerView = null;
    RecyclerView pathRecyclerView = null;
    Button bookRide = null;
    ImageButton shareButton = null;

    RideSnapshot rideSnapshot = null;

    void initialise(){
        carPermissionRecyclerView = findViewById(R.id.carPermissionRecyclerView);
        verificationRecyclerView = findViewById(R.id.detailedVerificationRecyclerView);
        pathRecyclerView = findViewById(R.id.detailedPathRecyclerView);
        bookRide = findViewById(R.id.detailedBookButton);
        shareButton = findViewById(R.id.detailedShareButton);
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_ride_detailed);
        rideSnapshot = RideShareController.getInstance().getRideSnapshot();
        initialise();

        bookRide.setOnClickListener(this);
        shareButton.setOnClickListener(this);

        ArrayList<Permission> permissions = new ArrayList<Permission>();

        ///////////////////////////////////////TODO FIREBASE INTEGRATION/////////////////////////////////
        permissions.add(new Permission("Chat", "https://cdn1.iconfinder.com/data/icons/freeline/32/account_friend_human_man_member_person_profile_user_users-256.png"));
        permissions.add(new Permission("Chat", "https://cdn1.iconfinder.com/data/icons/freeline/32/account_friend_human_man_member_person_profile_user_users-256.png"));
        permissions.add(new Permission("Chat", "https://cdn1.iconfinder.com/data/icons/freeline/32/account_friend_human_man_member_person_profile_user_users-256.png"));
        permissions.add(new Permission("Chat", "https://cdn1.iconfinder.com/data/icons/freeline/32/account_friend_human_man_member_person_profile_user_users-256.png"));

        //////////////////////////////////////////////////////////////////////////////////////////////////


        carPermissionRecyclerView.setAdapter(new PermissionListAdapter(permissions));
        carPermissionRecyclerView.setLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.HORIZONTAL, false));

        verificationRecyclerView.setAdapter(new VerificationListAdapter(RideShareController.getInstance().user.getVerifications()));
        verificationRecyclerView.setLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false));

        ArrayList<Node> nodes = new ArrayList<Node>();
        nodes.add(RideShareController.getInstance().getRideSnapshot().getStart());
        if(RideShareController.getInstance().getRideSnapshot().getIntermediates() != null)
            nodes.addAll(RideShareController.getInstance().getRideSnapshot().getIntermediates());
        nodes.add(RideShareController.getInstance().getRideSnapshot().getEnd());
        pathRecyclerView.setAdapter(new PathListAdapter(nodes));
        pathRecyclerView.setLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.VERTICAL, false));
    }

    @Override
    public void onClick(View v) {
        switch (v.getId()){
            case R.id.detailedBookButton:
                Intent i = new Intent(context, RideActivity.class);
                RideShareController.getInstance().setRideSnapshot(rideSnapshot);
                startActivity(i);
                break;
            case R.id.detailedShareButton:
                Toast.makeText(this, "Sharing is not supported as of this build", Toast.LENGTH_SHORT).show();
                break;
        }
    }
}
