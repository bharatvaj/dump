package com.cz.rideshare;

import android.content.Context;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.TextView;

import com.bumptech.glide.Glide;
import com.google.firebase.auth.FirebaseAuth;

import de.hdodenhof.circleimageview.CircleImageView;

public class ProfileActivity extends AppCompatActivity {
    private final Context context = this;
    private Button logoutButton = null;
    private EditText emailEditText = null;
    private EditText phoneEditText = null;
    private ImageView profileUserImage = null;
    private TextView profileUserName = null;

    void initialize(){
        logoutButton = findViewById(R.id.logoutButton);
        emailEditText = findViewById(R.id.profileUserEmailEdit);
        phoneEditText = findViewById(R.id.profileUserPhoneEdit);
        profileUserImage = findViewById(R.id.profileUserImage);
        profileUserName = findViewById(R.id.profileUserName);
    }
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_profile);
        initialize();
        logoutButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                FirebaseAuth.getInstance().signOut();
                Intent i = new Intent(context, LoginActivity.class);
                startActivity(i);
            }
        });

        emailEditText.setText(RideShareController.getInstance().user.getEmail());
        phoneEditText.setText(RideShareController.getInstance().user.getPhoneNumber());
        profileUserName.setText(RideShareController.getInstance().user.getName());
        Glide.with(this)
                .load(RideShareController.getInstance().user.getDisplayPicture())
                .into(profileUserImage);
    }
}
