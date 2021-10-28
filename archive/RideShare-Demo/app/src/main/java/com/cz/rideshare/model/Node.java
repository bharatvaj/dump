package com.cz.rideshare.model;

import java.util.Date;
import android.media.Image;

import com.google.android.gms.maps.model.LatLng;
import com.google.firebase.firestore.GeoPoint;

/**
* @generated
*/
public class Node {
    
    /**
    * @generated
    */
    private String locationName;
    
    /**
    * @generated
    */
    private GeoPoint location;
    
    /**
    * @generated
    */
    private Date timeDelta;
    
    
    public Node(){

    }
    /**
     * @generated
     */
    public Node(String locationName, Date timeDelta, GeoPoint location) {
        this.locationName = locationName;
        this.timeDelta = timeDelta;
        this.location = location;
    }

    /**
    * @generated
    */
    public String getLocationName() {
        return this.locationName;
    }
    
    /**
    * @generated
    */
    public void setLocationName(String locationName) {
        this.locationName = locationName;
    }
    
    
    /**
    * @generated
    */
    public GeoPoint getLocation() {
        return this.location;
    }
    
    /**
    * @generated
    */
    public void setLocation(GeoPoint location) {
        this.location = location;
    }
    
    
    /**
    * @generated
    */
    public Date getTimeDelta() {
        return this.timeDelta;
    }
    
    /**
    * @generated
    */
    public void setTimeDelta(Date timeDelta) {
        this.timeDelta = timeDelta;
    }
    
    
    
    
}
