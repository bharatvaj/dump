import { Room } from './room'
import { Device } from './device'
import * as firebase from 'firebase/app';
import 'firebase/firestore';
import { Observable, of } from 'rxjs';
export class Informer {
    private static instance: Informer;
    private _temperature: number;
    private _observableRooms: Observable<Array<Room>>;
    private _rooms = Array<Room>()
    private constructor() {
        // Initialize Firebase
        var config = {
            apiKey: "AIzaSyCdWLtBoFdlno9ZMABmF9CjqHwDN23qpJo",
            authDomain: "homate-aacb0.firebaseapp.com",
            projectId: "homate-aacb0",
        };
        firebase.initializeApp(config);
    }
    static getInstance() {
        if (!Informer.instance) {
            Informer.instance = new Informer();
        }
        return Informer.instance;
    }
    get rooms(): Observable<Array<Room>> {
        //todo fetch from firebase?
        this._observableRooms = of(this._rooms);
        firebase.firestore().collection("rooms").onSnapshot(function (querySnapshot) {
            querySnapshot.docChanges().forEach(function (change) {
                if (change.type == "added") {
                    this._rooms.push(change.doc.data() as Room)
                } else if (change.type == "modified") {

                } else if (change.type == "removed") {
                }
            }.bind(this));
        }.bind(this));
        return this._observableRooms;
    }
    devices(room: Room) : Observable<Array<Device>>  {
        let devices = new Array<Device>();
        let observableDevices = of(devices);
        //fetch data from firebase

        console.log(room);
        if(room.id == ""){
            return observableDevices;
        }

        firebase.firestore().collection('rooms').doc(room.id).collection('devices').onSnapshot(function (querySnapshot) {
            querySnapshot.docChanges().forEach(function (change) {
                if (change.type == "added") {
                    console.log(change.doc.data() as Device);
                    devices.push(change.doc.data() as Device)
                } 
            }.bind(this));
        }.bind(this));
        return observableDevices;
    }
}
