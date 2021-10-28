const express = require('express');
app = express();
const firebase = require('firebase');
const mqtt = require('mqtt');
port = process.env.PORT || 4000;

app.use(express.static(__dirname + '/public'));
app.listen(port);
// Initialize Firebase
let config = {
  apiKey: "AIzaSyCdWLtBoFdlno9ZMABmF9CjqHwDN23qpJo",
  authDomain: "homate-aacb0.firebaseapp.com",
  databaseURL: "https://homate-aacb0.firebaseio.com",
  projectId: "homate-aacb0",
  storageBucket: "homate-aacb0.appspot.com",
  messagingSenderId: "24651950498"
};
firebase.initializeApp(config);

let db = firebase.firestore();
let room1 = db.collection('rooms').doc('jPixAsGaVenBRrCbV9FA').collection('devices');

var client = mqtt.connect("mqtt://localhost").on('connect', () => {
  client.subscribe('device1', () => {
    client.on('message', (topic, message, packet) => {
      console.log("" + message);
      room1.doc('device1').set({
        id: topic,
        topic: topic,
        cmd: ""+message
      });
    });
  });
});