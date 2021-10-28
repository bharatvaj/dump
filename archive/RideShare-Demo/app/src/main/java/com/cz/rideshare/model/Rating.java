package com.cz.rideshare.model;

import java.util.Date;
import android.media.Image;

/**
* @generated
*/
public class Rating {
    
    /**
    * @generated
    */
    private float rating;
    
    /**
    * @generated
    */
    private int usersRated;
    
    
    public Rating(){

    }
    /**
     * @generated
     */
    public Rating(int usersRated, float rating) {
        this.usersRated = usersRated;
        this.rating = rating;
    }
    

    /**
    * @generated
    */
    public float getRating() {
        return this.rating;
    }
    
    /**
    * @generated
    */
    public void setRating(float rating) {
        this.rating = rating;
    }
    
    
    /**
    * @generated
    */
    public int getUsersRated() {
        return this.usersRated;
    }
    
    /**
    * @generated
    */
    public void setUsersRated(Integer usersRated) {
        this.usersRated = usersRated;
    }
    
    
    
    
}
