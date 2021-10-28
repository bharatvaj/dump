package com.cz.rideshare;


import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.design.widget.Snackbar;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentStatePagerAdapter;
import android.support.v7.app.AppCompatActivity;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.Toast;

import com.cz.rideshare.model.Gender;
import com.cz.rideshare.model.User;
import com.cz.rideshare.model.Verification;
import com.cz.rideshare.view.NonSwipeableViewPager;
import com.google.android.gms.tasks.OnCompleteListener;
import com.google.android.gms.tasks.Task;
import com.google.firebase.FirebaseException;
import com.google.firebase.FirebaseTooManyRequestsException;
import com.google.firebase.auth.AuthResult;
import com.google.firebase.auth.FirebaseAuth;
import com.google.firebase.auth.FirebaseAuthInvalidCredentialsException;
import com.google.firebase.auth.FirebaseUser;
import com.google.firebase.auth.PhoneAuthCredential;
import com.google.firebase.auth.PhoneAuthProvider;

import java.util.ArrayList;
import java.util.Date;
import java.util.concurrent.TimeUnit;


public class LoginActivity extends AppCompatActivity implements
        View.OnClickListener {

    static EditText mPhoneNumberField, mVerificationField;
    static ImageButton mStartButton, mVerifyButton;
    static LinearLayout mResendButton;
    Button skipLoginBtn = null;
    NonSwipeableViewPager loginViewPager = null;


    private FirebaseAuth mAuth;
    private PhoneAuthProvider.ForceResendingToken mResendToken;
    private PhoneAuthProvider.OnVerificationStateChangedCallbacks mCallbacks;
    static LoginActivity context;

    private static final String TAG = "PhoneAuthActivity";
    static String phoneNumber = "";
    static String verifyCode = "";

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
        context = this;


        loginViewPager = findViewById(R.id.loginViewPager);
        loginViewPager.setAdapter(new AuthPagerAdapter(getSupportFragmentManager()));

        skipLoginBtn = findViewById(R.id.skip_login);


        skipLoginBtn.setOnClickListener(this);

        mAuth = FirebaseAuth.getInstance();
        mCallbacks = new PhoneAuthProvider.OnVerificationStateChangedCallbacks() {
            @Override
            public void onVerificationCompleted(PhoneAuthCredential credential) {
                Log.d(TAG, "onVerificationCompleted:" + credential);
                signInWithPhoneAuthCredential(credential);
            }

            @Override
            public void onVerificationFailed(FirebaseException e) {
                Log.w(TAG, "onVerificationFailed", e);
                if (e instanceof FirebaseAuthInvalidCredentialsException) {
                    mPhoneNumberField.setError("Invalid phone number.");
                } else if (e instanceof FirebaseTooManyRequestsException) {
                    Snackbar.make(findViewById(android.R.id.content), "Quota exceeded.",
                            Snackbar.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onCodeSent(String verificationId,
                                   PhoneAuthProvider.ForceResendingToken token) {
                Log.d(TAG, "onCodeSent:" + verificationId);
                verifyCode = verificationId;
                mResendToken = token;
            }
        };
    }

    private void signInWithPhoneAuthCredential(PhoneAuthCredential credential) {
        mAuth.signInWithCredential(credential)
                .addOnCompleteListener(this, new OnCompleteListener<AuthResult>() {
                    @Override
                    public void onComplete(@NonNull Task<AuthResult> task) {
                        if (task.isSuccessful()) {
                            Log.d(TAG, "signInWithCredential:success");
                            FirebaseUser user = task.getResult().getUser();
                            startActivity(new Intent(LoginActivity.this, MainActivity.class));
                            finish();
                        } else {
                            Log.w(TAG, "signInWithCredential:failure", task.getException());
                            if (task.getException() instanceof FirebaseAuthInvalidCredentialsException) {
                                mVerificationField.setError("Invalid code.");
                            }
                        }
                    }
                });
    }


    private void startPhoneNumberVerification(String phoneNumber) {
        PhoneAuthProvider.getInstance().verifyPhoneNumber(
                phoneNumber,        // Phone number to verify
                60,                 // Timeout duration
                TimeUnit.SECONDS,   // Unit of timeout
                this,               // Activity (for callback binding)
                mCallbacks);        // OnVerificationStateChangedCallbacks
    }

    private void verifyPhoneNumberWithCode(String verificationId, String code) {
        PhoneAuthCredential credential = PhoneAuthProvider.getCredential(verificationId, code);
        signInWithPhoneAuthCredential(credential);
    }

    private void resendVerificationCode(String phoneNumber,
                                        PhoneAuthProvider.ForceResendingToken token) {
        PhoneAuthProvider.getInstance().verifyPhoneNumber(
                phoneNumber,        // Phone number to verify
                60,                 // Timeout duration
                TimeUnit.SECONDS,   // Unit of timeout
                this,               // Activity (for callback binding)
                mCallbacks,         // OnVerificationStateChangedCallbacks
                token);             // ForceResendingToken from callbacks
    }


    @Override
    public void onStart() {
        super.onStart();
        FirebaseUser user = mAuth.getCurrentUser();
        if (user != null) {
            RideShareController.getInstance().setUser(user);
            Intent intent = new Intent(LoginActivity.this, MainActivity.class);
            startActivity(intent);
            finish();
            return;
        }
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.button_start_verification:
                //if (!validatePhoneNumber()) {
                //   return;
                //}
                loginViewPager.setCurrentItem(1); //1 - verify page
                startPhoneNumberVerification(phoneNumber);
                break;
            case R.id.button_verify_phone:
                String code = verifyCode;
                if (TextUtils.isEmpty(code)) {
                    mVerificationField.setError("Cannot be empty.");
                    return;
                }
                verifyPhoneNumberWithCode(verifyCode, code);
                break;
            case R.id.button_resend:
                resendVerificationCode(phoneNumber, mResendToken);
                break;
            case R.id.button_call_user:
                Toast.makeText(this, "Calling user is not supported in this build", Toast.LENGTH_LONG).show();
                break;
            case R.id.skip_login:
                Intent intent = new Intent(LoginActivity.this, MainActivity.class);
                startActivity(intent);
                break;
        }
    }


    public static class AuthFragment extends Fragment {

        private View.OnClickListener listener = null;
        public ImageButton loginButton = null;
        public EditText loginCountryCode = null;
        public EditText loginPhoneNumber = null;

        public AuthFragment() {
        }

        static Fragment getInstance() {
            return new AuthFragment();
        }

        @Override
        public View onCreateView(LayoutInflater inflater, ViewGroup container,
                                 Bundle savedInstanceState) {
            ViewGroup rootView = (ViewGroup) inflater.inflate(R.layout.login_page_phone, container, false);
            loginCountryCode = rootView.findViewById(R.id.loginCountryCode);
            loginPhoneNumber = rootView.findViewById(R.id.field_phone_number);
            loginButton = rootView.findViewById(R.id.button_start_verification);
            LoginActivity.mPhoneNumberField = loginPhoneNumber;
            loginButton.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    phoneNumber = loginCountryCode.getText().toString() + " " + loginPhoneNumber.getText().toString(); //conforming to E.164 standard
                    Log.i("AuthFragment", phoneNumber);
                    context.onClick(v);
                }
            });
            return rootView;
        }

    }


    public static class VerifyFragment extends Fragment {

        ImageButton verifyBtn = null;
        LinearLayout resendBtn = null;
        LinearLayout callBtn = null;
        EditText verificationField = null;

        public VerifyFragment() {

        }

        static Fragment getInstance() {
            return new VerifyFragment();
        }

        View.OnClickListener listener = new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                LoginActivity.verifyCode = verificationField.getText().toString();
                context.onClick(v);
            }
        };

        @Override
        public View onCreateView(LayoutInflater inflater, ViewGroup container,
                                 Bundle savedInstanceState) {
            ViewGroup rootView = (ViewGroup) inflater.inflate(R.layout.login_page_verify, container, false);
            verifyBtn = rootView.findViewById(R.id.button_verify_phone);
            resendBtn = rootView.findViewById(R.id.button_resend);
            callBtn = rootView.findViewById(R.id.button_call_user);
            verificationField = rootView.findViewById(R.id.field_verification_code);
            LoginActivity.mVerificationField = verificationField;
            verifyBtn.setOnClickListener(listener);
            resendBtn.setOnClickListener(listener);
            callBtn.setOnClickListener(listener);
            return rootView;
        }

    }


    public class AuthPagerAdapter extends FragmentStatePagerAdapter {
        public AuthPagerAdapter(FragmentManager fm) {
            super(fm);
        }


        @Override
        public Fragment getItem(int position) {
            if (position == 0)
                return AuthFragment.getInstance();
            else
                return VerifyFragment.getInstance();

        }

        @Override
        public int getCount() {
            // 0 - phone number page
            // 1 - verify page
            return 2;
        }
    }

}