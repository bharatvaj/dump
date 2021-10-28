package com.cz.rideshare.model;

import java.util.Date;
import android.media.Image;

import com.google.android.gms.maps.model.LatLng;

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
    private LatLng location;
    
    /**
    * @generated
    */
    private Date timeDelta;
    
    
    
    /**
     * @generated
     */
    public Node(String locationName, Date timeDelta, LatLng location) {
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
    public LatLng getLocation() {
        return this.location;
    }
    
    /**
    * @generated
    */
    public void setLocation(LatLng location) {
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
