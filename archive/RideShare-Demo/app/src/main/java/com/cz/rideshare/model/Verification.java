package com.cz.rideshare.model;

/**
 * Created by Home on 31-12-2017.
 */

public class Verification {
    private String verificationName;

    private boolean isVerified = false;


    public Verification() {

    }

    public Verification(String verificationName, boolean isVerified) {
        this.verificationName = verificationName;
        this.isVerified = isVerified;
    }


    public String getVerificationName() {
        return verificationName;
    }

    public boolean getIsVerified() {
        return isVerified;
    }


    public void setVerificationName(String verificationName) {
        this.verificationName = verificationName;
    }

    public void setVerified(boolean verified) {
        isVerified = verified;
    }
}
