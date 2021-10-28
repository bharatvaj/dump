package com.cz.rideshare.model;

import java.util.ArrayList;
import java.util.Date;

/**
* @generated
*/
public class RideSnapshot {
    
    /**
    * @generated
    */
    private Node start;
    
    /**
    * @generated
    */
    private ArrayList<Node> intermediates;
    
    /**
    * @generated
    */
    private Node end;
    
    /**
    * @generated
    */
    private User driver;
    
    /**
    * @generated
    */
    private Rating rating;
    
    /**
    * @generated
    */
    private Date timeStarted;
    
    /**
    * @generated
    */
    private Vehicle vehicle;
    
    /**
    * @generated
    */
    private ArrayList<User> collaborators;
    
    /**
    * @generated
    */
    private int distance;
    
    /**
    * @generated
    */
    private Date duration;

    private float price;
    
    public RideSnapshot(){

    }
    
    /**
     * @generated
     */
    public RideSnapshot(int distance, float price, Node start, ArrayList<Node> intermediates, Vehicle vehicle, ArrayList<User> collaborators, Date duration, Date timeStarted, Node end, Rating rating, User driver) {
        this.distance = distance;
        this.price = price;
        this.start = start;
        this.intermediates = intermediates;
        this.vehicle = vehicle;
        this.collaborators = collaborators;
        this.duration = duration;
        this.timeStarted = timeStarted;
        this.end = end;
        this.rating = rating;
        this.driver = driver;
    }
    

    /**
    * @generated
    */
    public Node getStart() {
        return this.start;
    }
    
    /**
    * @generated
    */
    public void setStart(Node start) {
        this.start = start;
    }
    
    
    /**
    * @generated
    */
    public ArrayList<Node> getIntermediates() {
        return this.intermediates;
    }
    
    /**
    * @generated
    */
    public void setIntermediates(ArrayList<Node> intermediates) {
        this.intermediates = intermediates;
    }
    
    
    /**
    * @generated
    */
    public Node getEnd() {
        return this.end;
    }
    
    /**
    * @generated
    */
    public void setEnd(Node end) {
        this.end = end;
    }
    
    
    /**
    * @generated
    */
    public User getDriver() {
        return this.driver;
    }
    
    /**
    * @generated
    */
    public void setDriver(User driver) {
        this.driver = driver;
    }
    
    
    /**
    * @generated
    */
    public Rating getRating() {
        return this.rating;
    }
    
    /**
    * @generated
    */
    public void setRating(Rating rating) {
        this.rating = rating;
    }
    
    
    /**
    * @generated
    */
    public Date getTimeStarted() {
        return this.timeStarted;
    }
    
    /**
    * @generated
    */
    public void setTimeStarted(Date timeStarted) {
        this.timeStarted = timeStarted;
    }
    
    
    /**
    * @generated
    */
    public Vehicle getVehicle() {
        return this.vehicle;
    }
    
    /**
    * @generated
    */
    public void setVehicle(Vehicle vehicle) {
        this.vehicle = vehicle;
    }
    
    
    /**
    * @generated
    */
    public ArrayList<User> getCollaborators() {
        return this.collaborators;
    }
    
    /**
    * @generated
     * @param collaborators
    */
    public void setCollaborators(ArrayList<User> collaborators) {
        this.collaborators = collaborators;
    }
    
    
    /**
    * @generated
    */
    public int getDistance() {
        return this.distance;
    }
    
    /**
    * @generated
    */
    public void setDistance(Integer distance) {
        this.distance = distance;
    }
    
    
    /**
    * @generated
    */
    public Date getDuration() {
        return this.duration;
    }
    
    /**
    * @generated
    */
    public void setDuration(Date duration) {
        this.duration = duration;
    }
    
    public void setPrice(float price){
        this.price = price;
    }

    public float getPrice(){
        return this.price;
    }
    
    
}
