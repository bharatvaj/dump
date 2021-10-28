package com.cz.rideshare.model;

import java.util.Date;
import android.media.Image;

/**
* @generated
*/
public class Permission {

    /**
    * @generated
    */
    private String permissionName;

    /**
    * @generated
    */
    private String permissionImage;

    private boolean isAllowed = false;

    public Permission(){

    }
    /**
     * @generated
     */
    public Permission(String permissionName, String permissionImage) {
        this.permissionName = permissionName;
        this.permissionImage = permissionImage;
    }

    public boolean getIsAllowed() {
        return isAllowed;
    }

    public void setIsAllowed(boolean isAllowed) {
        this.isAllowed = isAllowed;
    }

    /**
    * @generated
    */
    public String getPermissionName() {
        return this.permissionName;
    }

    /**
    * @generated
    */
    public void setPermissionName(String permissionName) {
        this.permissionName = permissionName;
    }


    /**
    * @generated
    */
    public String getPermissionImage() {
        return this.permissionImage;
    }

    /**
    * @generated
    */
    public void setPermissionImage(String permissionImage) {
        this.permissionImage = permissionImage;
    }
    
    
    
    
}
