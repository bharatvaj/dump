package com.cz.rideshare.model;

import java.util.Date;
import android.media.Image;

/**
* @generated
*/
public class Vehicle {
    
    /**
    * @generated
    */
    private VehicleType vehicleType;
    
    /**
    * @generated
    */
    private String vehicleName;
    
    /**
    * @generated
    */
    private int availableSeats;
    
    /**
    * @generated
    */
    private Permission permissions;
    
    /**
    * @generated
    */
    private int pricePerSeat;
    
    /**
    * @generated
    */
    private String registrationNumber;
    
    
    public Vehicle(){

    }
    /**
     * @generated
     */
    public Vehicle(String vehicleName, int availableSeats, String registrationNumber, int pricePerSeat, VehicleType vehicleType, Permission permissions) {
        this.vehicleName = vehicleName;
        this.availableSeats = availableSeats;
        this.registrationNumber = registrationNumber;
        this.pricePerSeat = pricePerSeat;
        this.vehicleType = vehicleType;
        this.permissions = permissions;
    }
    

    /**
    * @generated
    */
    public VehicleType getVehicleType() {
        return this.vehicleType;
    }
    
    /**
    * @generated
    */
    public void setVehicleType(VehicleType vehicleType) {
        this.vehicleType = vehicleType;
    }
    
    
    /**
    * @generated
    */
    public String getVehicleName() {
        return this.vehicleName;
    }
    
    /**
    * @generated
    */
    public void setVehicleName(String vehicleName) {
        this.vehicleName = vehicleName;
    }
    
    
    /**
    * @generated
    */
    public int getAvailableSeats() {
        return this.availableSeats;
    }
    
    /**
    * @generated
    */
    public void setAvailableSeats(Integer availableSeats) {
        this.availableSeats = availableSeats;
    }
    
    
    /**
    * @generated
    */
    public Permission getPermissions() {
        return this.permissions;
    }
    
    /**
    * @generated
    */
    public void setPermissions(Permission permissions) {
        this.permissions = permissions;
    }
    
    
    /**
    * @generated
    */
    public int getPricePerSeat() {
        return this.pricePerSeat;
    }
    
    /**
    * @generated
    */
    public void setPricePerSeat(Integer pricePerSeat) {
        this.pricePerSeat = pricePerSeat;
    }
    
    
    /**
    * @generated
    */
    public String getRegistrationNumber() {
        return this.registrationNumber;
    }
    
    /**
    * @generated
    */
    public void setRegistrationNumber(String registrationNumber) {
        this.registrationNumber = registrationNumber;
    }
    
    
    
    
}
