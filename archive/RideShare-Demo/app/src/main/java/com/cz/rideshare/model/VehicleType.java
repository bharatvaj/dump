package com.cz.rideshare.model;

import java.util.Date;
import android.media.Image;

/**
* @generated
*/
public class VehicleType {
    
    /**
    * @generated
    */
    private String typeName;
    
    /**
    * @generated
    */
    private String typeImage;
    
    
    public VehicleType(){

    }
    /**
     * @generated
     */
    public VehicleType(String typeImage, String typeName) {
        this.typeImage = typeImage;
        this.typeName = typeName;
    }
    

    /**
    * @generated
    */
    public String getTypeName() {
        return this.typeName;
    }
    
    /**
    * @generated
    */
    public void setTypeName(String typeName) {
        this.typeName = typeName;
    }
    
    
    /**
    * @generated
    */
    public String getTypeImage() {
        return this.typeImage;
    }
    
    /**
    * @generated
    */
    public void setTypeImage(String typeImage) {
        this.typeImage = typeImage;
    }
    
    
    
    
}
